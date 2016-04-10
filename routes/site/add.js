const bodyParser = require('body-parser');
const data = require(modules.data.provider);

module.exports = require('express')
    .Router()
    .use('/', function(request, response, next) {
        if (response.locals.user)
            next();
        else
            response.redirect('/');
    })
    .get('/', function(request, response, next) {
        response.render('site/add');
    })
    .post('/', bodyParser.urlencoded({ extended: false }), validate, function(request, response, next) {
        if (response.locals.errors.name)
            response.render('site/add');
        else
            data.sites.add(
                {
                    name: request.body.name,
                    description: request.body.description,
                    owner: response.locals.user
                },
                function(errors) {
                    if (errors && errors.name)
                        response.render('site/add', { errors: errors });
                    else
                        response.redirect('/');
                });
    });

function validate(request, response, next) {
    response.locals.site = {
        name: request.body.name,
        description: request.body.description
    };

    if (!response.locals.errors)
        response.locals.errors = {};

    if (!/^[a-zA-Z0-8]{7,}$/.test(request.body.name))
        response.locals.errors.name = 'The name must be at least 7 characters long and may contain only alphanumerc characters.';

    next();
}