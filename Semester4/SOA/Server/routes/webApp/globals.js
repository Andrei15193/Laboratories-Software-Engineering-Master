const jwt = require('jsonwebtoken');

module.exports = require('express')
    .Router()
    .use(function (request, response, next) {
        const signedCookie = request.signedCookies['mosaic-token'];
        jwt.verify(
            signedCookie,
            process.env.APPSETTING_jwtSecret,
            function (error, user) {
                if (error)
                    response.locals.user = null;
                else
                    response.locals.user = user;
                next();
            });
    })
    .use(function (request, response, next) {
        if (response.locals.user)
            response.locals.navigation = {
                items: [
                    {
                        label: 'API',
                        path: '/api'
                    },
                    {
                        label: 'Log out',
                        path: '/user/logout'
                    }
                ]
            };
        else
            response.locals.navigation = {
                items: [
                    {
                        label: 'API',
                        path: '/api'
                    },
                    {
                        label: 'Register',
                        path: '/user/register'
                    },
                    {
                        label: 'Log in',
                        path: '/user/login'
                    }
                ]
            };
        next();
    });