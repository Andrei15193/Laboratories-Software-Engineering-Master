module.exports = require('express')
    .Router()
    .use(function(request, response, next) {
        if (response.locals.user)
            response.locals.navigation = {
                items: [
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