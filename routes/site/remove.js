const data = require(modules.data.provider);

module.exports = require('express')
    .Router()
    .use('/', function(request, response, next) {
        if (response.locals.user)
            next();
        else
            response.redirect('/');
    })
    .get('/:name', function(request, response, next) {
        data.sites.remove(
            {
                name: request.params.name,
                owner: response.locals.user
            },
            function() {
                response.redirect('/');
            });
    });