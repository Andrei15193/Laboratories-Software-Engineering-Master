module.exports = {
    '/user/logout': {
        get: function (request, response, next) {
            response.clearCookie('mosaic-token');
            response.redirect('/');
        }
    }
};