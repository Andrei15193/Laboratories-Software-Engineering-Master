const auth = require(modules.auth);

module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        auth.removeToken(response);
        response.redirect('/');
    });