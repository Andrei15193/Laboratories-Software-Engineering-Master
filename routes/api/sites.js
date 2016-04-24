module.exports = {
    '/api/sites/details': {
        get: function (request, response, next) {
            response
                .status(200)
                .send(response.locals.site)
                .end();
        }
    }
}