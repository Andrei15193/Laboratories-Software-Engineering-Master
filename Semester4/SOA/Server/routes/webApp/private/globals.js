const jwt = require('jsonwebtoken');

module.exports = require('express')
    .Router()
    .use(function (request, response, next) {
        if (response.locals.user == null)
            response.redirect('/');
        else
            next();
    });