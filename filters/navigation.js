module.exports = require('express')
    .Router()
    .use(function(request, response, next) {
        response.locals.navigation = {
            items: [
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