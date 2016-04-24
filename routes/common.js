module.exports = require('express')
    .Router()
    .use(function(request, response, next) {
        response.locals.menuItems = [{ url: 'test 1', label: 'test 1' }, { url: 'test 2', label: 'test 2' }]
        next();
    })
    .use(function(request, response, next) {
        response.locals.subnavItems = [{ url: '#top', label: 'Top' }, { url: 'http://mosaicdemo.azurewebsites.net/', label: 'Mosaic', target: '_blank' }];
        next();
    });