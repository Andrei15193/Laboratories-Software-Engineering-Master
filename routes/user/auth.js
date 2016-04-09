const jwt = require('jsonwebtoken');

const authCookieName = 'mosaic-token';

module.exports = {
    setToken: function(response, user) {
        response.cookie(
            authCookieName,
            jwt.sign(user, process.env.APPSETTING_jwtSecret),
            {
                maxAge: 86400000, // 24h
                httpOnly: true,
                signed: true
            });
    },
    removeToken: function(response) {
        response.clearCookie(authCookieName);
    },
    authenticate: require('express')
        .Router()
        .use(function(request, response, next) {
            jwt.verify(
                request.signedCookies[authCookieName],
                process.env.APPSETTING_jwtSecret,
                function(error, user) {
                    if (error)
                        response.locals.user = null;
                    else
                        response.locals.user = user;
                    next();
                });
        })
};