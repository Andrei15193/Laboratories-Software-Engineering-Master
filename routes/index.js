module.exports = require('express')
    .Router()
    .use(require(modules.auth).authenticate)
    .get('/', function(request, response, next) {
        if (response.locals.user)
            response.render('index/dashboard', { user: request.user });
        else
            response.render('index/index');
    });