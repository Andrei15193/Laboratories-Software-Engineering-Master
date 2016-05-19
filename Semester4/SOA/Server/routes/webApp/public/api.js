module.exports = require('express')
    .Router()
    .get('/api', function (request, response, next) {
        response.render('api');
    });