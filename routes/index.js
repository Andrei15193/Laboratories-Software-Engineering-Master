module.exports = require('express')
    .Router()
    .get('/', function(request, response, next) {
        response.render('index/index');
    });