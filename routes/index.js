module.exports = require('express')
    .Router()
    .use(require(modules.auth).authenticate)
    .get('/', function(request, response, next) {
        if (request.user)
            response.render('index/dashboard', { user: request.user });
        else
            response.render('index/index');
    });