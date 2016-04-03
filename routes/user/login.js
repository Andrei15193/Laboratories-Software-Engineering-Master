require(modules.objectExtensions);
const bodyParser = require('body-parser');
const common = require(modules.common);
const data = require(modules.data.provider);

module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        response.render('user/login');
    })
    .post('/', bodyParser(), function(request, response, next) {
        response.locals.errors = { _: 'Invalid username or password.' };
        response.render('user/login');
    });