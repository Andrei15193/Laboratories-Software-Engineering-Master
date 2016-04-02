module.exports = require('express')
    .Router()
    .use(function(request, response, next) {
        response.locals.navigation = {
            items: [
                {
                    label: 'Register',
                    path: '/account/register'
                },
                {
                    label: 'Log in',
                    path: '/account/login'
                }
            ]
        };
        next();
    });