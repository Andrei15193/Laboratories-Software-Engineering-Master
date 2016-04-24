module.exports = require('express')
    .Router()
    .get('/login', function (request, response, next) {
        response.render('login');
    })
    .post('/login', function (request, response, next) {
        response.render('login', { error: 'Invalid credentials' });
    });