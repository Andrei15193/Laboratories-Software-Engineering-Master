module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        if (response.locals.user)
            response.render('index/dashboard', { sites: [{ title: 'test site', key: 'site key', description: 'site description' }, { title: 'test site', key: 'site key', description: 'site description' }] });
        else
            response.render('index/index');
    });