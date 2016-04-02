const bodyParser = require('body-parser');
const crypto = require('crypto');
const azureStorage = require('azure-storage');

function getHash(data) {
    var hash = crypto.createHash('sha256');
    hash.update(data);
    return hash.digest('hex');
}

const entityGenerator = azureStorage.TableUtilities.entityGenerator;
const storageTable = azureStorage.createTableService(
    process.env.CUSTOMCONNSTR_azureTableStorage
);

function validateUser(request, response, next) {
    next();
}

function registerUser(request, response, next) {
    storageTable.createTableIfNotExists('users', function() {
        storageTable.insertEntity('users', {
            PartitionKey: entityGenerator.String(request.body.username),
            RowKey: entityGenerator.String(getHash(request.body.password)),
            email: entityGenerator.String(request.body.email)
        }, function(error) {
            if (error)
                throw error;

            next();
        })
    });
    response.render('user/register', request.body);
}

module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        response.render('user/register');
    })
    .post('/', bodyParser(), validateUser, registerUser);