const bodyParser = require('body-parser');
const data = require(modules.data.provider);
const siteAuth = require(modules.auth.site);

module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        response.render('user/login');
    })
    .post('/', bodyParser.urlencoded({ extended: false }), function(request, response, next) {
        data.users.tryGetUser(
            request.body.username,
            request.body.password,
            function(user) {
                if (user) {
                    siteAuth.setToken(response, user);
                    response.redirect('/');
                }
                else {
                    response.locals.errors = { _: 'Invalid username or password.' };
                    response.render('user/login');
                }
            });
    });