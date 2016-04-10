const siteAuth = require(modules.auth.site);

module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        siteAuth.removeToken(response);
        response.redirect('/');
    });