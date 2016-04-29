module.exports = require('express')
    .Router()
    .get('/user/logout', function (request, response, next) {
        response.clearCookie('mosaic-token');
        response.redirect('/');
    });