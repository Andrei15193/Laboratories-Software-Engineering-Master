const jwt = require('jsonwebtoken');

module.exports = {
    '!mosaic-token': function (request, response, next, signedCookie) {
        if (response.locals.user == null)
            response.redirect('/');
        else
            next();
    }
}