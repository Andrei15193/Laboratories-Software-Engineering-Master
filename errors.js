const express = require('express');

const notFoundMessage = 'The resource you are looking for does not exist.';

module.exports =
    {
        notFound: express
            .Router()
            .use('/api/*', function (request, response, next) {
                response
                    .status(404)
                    .end(notFoundMessage);
            })
            .use('*', function (request, response, next) {
                response.render('errors/notFound', { title: ' You have ran into a 404!', message: notFoundMessage });
            })
    };