module.exports = require('express')
    .Router()
    .get('/api/sites/details', function (request, response, next) {
        response
            .status(200)
            .send(response.locals.site)
            .end();
    });