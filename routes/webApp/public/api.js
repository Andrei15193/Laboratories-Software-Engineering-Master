module.exports = {
    '/api': {
        get: function (request, response, next) {
            response.render('api');
        }
    }
};