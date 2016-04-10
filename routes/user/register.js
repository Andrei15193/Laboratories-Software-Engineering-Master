const reCaptcha = require('../../reCaptcha');
const bodyParser = require('body-parser');
const data = require(modules.data.provider);

module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        response.render('user/register');
    })
    .post('/', bodyParser(), reCaptcha.verify(), validateUser, verifyModelState, registerUser);

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
        data.users.isUsernameUnique(request.body.username, function(isUnique) {
            if (!isUnique)
                response.locals.errors.username = 'The username you have picked is already in use. Please use a different one.';

            next();
        });
}

function verifyModelState(request, response, next) {
    if (response.locals.errors && Object.keys(response.locals.errors).length > 0)
        response.render('user/register');
    else
        next();
}

function registerUser(request, response, next) {
    data.users.add(response.locals.user, function(errors) {
        if (errors) {
            if (response.locals.errors)
                resposne.locals.errors = errors;
            else
                response.locals.errors.username = errors.username;
            response.render('user/register');
        }
        else
            response.redirect('/');
    });
}