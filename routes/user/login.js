require(modules.objectExtensions);
const bodyParser = require('body-parser');
const common = require(modules.common);
const data = require(modules.data.provider);
const auth = require(modules.auth);

module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        response.render('user/login');
    })
    .post('/', bodyParser(), function(request, response, next) {
        data.users.tryGetUserByCredentials(
            request.body.username,
            request.body.password,
            function(user) {
                if (user) {
                    auth.setToken(response, user);
                    response.redirect('/');
                }
                else {
                    response.locals.errors = { _: 'Invalid username or password.' };
                    response.render('user/login');
                }
            });
    });