const bodyParser = require('body-parser');
const data = require(modules.data.provider);

module.exports = {
    ':categoryId': function (request, response, next, categoryId) {
        data.categories.get(response.locals.site, categoryId, function (category) {
            if (!category)
                response
                    .status(404)
                    .end('Category with ID: ' + categoryId + ' does not exists.');
            else {
                response.locals.category = category;
                next();
            }
        });
    },

    '/api/categories': {
        get: function (request, response, next) {
            data.categories.getFor(
                response.locals.site,
                function (categories) {
                    response
                        .status(200)
                        .json(categories)
                        .end();
                });
        },
        post:
        [
            authorize,
            bodyParser.json(),
            validateCategory,
            function (request, response, next) {
                if (response.locals.errors)
                    response
                        .status(400)
                        .json(response.locals.errors)
                        .end();
                else
                    data.categories.add(
                        {
                            site: response.locals.site,
                            name: request.body.name.trim(),
                            description: request.body.description
                        },
                        function (errors) {
                            if (errors)
                                response
                                    .status(400)
                                    .json(errors)
                                    .end();
                            else
                                response
                                    .status(201)
                                    .end();
                        }
                    )
            }
        ]
    },
    '/api/categories/:categoryId': {
        delete: [
            authorize,
            function (request, response, next) {
                data.categories.remove(
                    response.locals.category,
                    function (error) {
                        if (error)
                            response
                                .status(404)
                                .json({ code: 3, name: 'The category you are trying to delete does not exist.' })
                                .end();
                        else
                            response
                                .status(204)
                                .end();
                    });
            }
        ]
    }
};

function authorize(request, response, next) {
    if (response.locals.user)
        next();
    else
        response
            .status(403)
            .end('You do not have access to this part of the application.');
}

function validateCategory(request, response, next) {
    if (!/\S/.test(request.body.name))
        response.locals.errors = {
            code: 2,
            name: 'The name of a category cannot be empty. Please provide a name.'
        };
    next();
}