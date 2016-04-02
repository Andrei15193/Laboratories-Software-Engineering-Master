require('../../objectExtensions');
const reCaptcha = require('../../reCaptcha');
const bodyParser = require('body-parser');
const crypto = require('crypto');
const azureStorage = require('azure-storage');

module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        response.render('user/register');
    })
    .post('/', bodyParser(), reCaptcha.verify(), validateUser, verifyModelState, registerUser);

const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

function validateUser(request, response, next) {
    response.locals.errors = {};
    response.locals.user =
        {
            username: request.body.username,
            password: request.body.password,
            email: request.body.email
        };

    if (!response.locals.reCaptcha.success)
        response.locals.errors.reCaptcha = 'The reCaptcha validation did not succeed. Please try again, if the problem persists contact the site administrator.';

    if (!/^[^/\\#\?\t\n\r\u0000-\u001F\u007F-\u009F]{4,}$/.test(request.body.username))
        response.locals.errors.username = 'Please pick a username that is at least 4 characters long and does not contain forward slashes (/), backslashes (\\), the number sign (#), the question mark (?), tab or new line characters or any control characters.';

    if (!/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).{7,}$/.test(request.body.password)
        || new RegExp(request.body.username).test(request.body.password))
        response.locals.errors.password = 'The password is too weak! Please pick a password containing letters in both lowercase and uppercase as well as digits and at least one non alphanumeric character. The password must be at least 7 characters long and it may not contain your username.';

    if (request.body.password !== request.body.passwordCheck)
        response.locals.errors.passwordCheck = 'You have not introduced the same password, please try again.';

    if (response.locals.errors.username)
        next();
    else
        storageTable.createTableIfNotExists('usersValidationUsername', function() {
            storageTable.retrieveEntity('usersValidationUsername', request.body.username.toLowerCase(), 'Unique usernames in lowercase', function(error, result) {
                if (!error && result)
                    response.locals.errors.username = 'The username you have picked is already in use. Please use a different one.';

                next();
            });
        });
}

function verifyModelState(request, response, next) {
    if (response.locals.errors && Object.keys(response.locals.errors).length > 0)
        response.render('user/register');
    else
        next();
}

function registerUser(request, response, next) {
    var userCreateBatch = new azureStorage.TableBatch();

    userCreateBatch.insertEntity(
        response.locals.user.toAzureEntity(
            {
                partitionKey: 'username',
                rowKey: 'password',
                rowKeyMap: getHash
            }));
    userCreateBatch.insertEntity(
        response.locals.user.toAzureEntity(
            {
                partitionKey: 'username',
                rowKey: 'password',
                rowKeyMap: function() { return 'check'; }
            }));

    storageTable.createTableIfNotExists('usersValidationUsername', function() {
        storageTable.insertEntity(
            'usersValidationUsername',
            {
                PartitionKey: response.locals.user.username.toLowerCase(),
                RowKey: 'Unique usernames in lowercase'
            }.toAzureEntity(),
            function(error) {
                if (error) {
                    if (!response.locals.errors)
                        response.locals.errors = {};

                    response.locals.errors.username = 'The username you have picked is already in use. Please use a different one.';
                    response.render('user/register');
                }

                storageTable.createTableIfNotExists('users', function() {
                    storageTable.executeBatch(
                        'users',
                        userCreateBatch,
                        function(error) {
                            if (error)
                                throw error;

                            response.redirect('/');
                        });
                });
            });

    });
}

function getHash(data) {
    var hash = crypto.createHash('sha256');
    hash.update(data);
    return hash.digest('hex');
}