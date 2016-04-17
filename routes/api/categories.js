const bodyParser = require('body-parser');
const data = require(modules.data.provider);

module.exports = {
    ':categoryName': function (request, response, next, categoryName) {
        data.categories.get(response.locals.site, categoryName, function (category) {
            if (!category)
                response
                    .status(404)
                    .end('The category does not exists.');
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
                                    .status(200)
                                    .end();
                        }
                    )
            }
        ]
    },
    '/api/categories/:categoryName': {
        delete: function (request, response, next) {
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
                            .status(200)
                            .end();
                });
        }
    }
};

function validateCategory(request, response, next) {
    if (!/\S/.test(request.body.name))
        response.locals.errors = {
            code: 2,
            name: 'The name of a category cannot be empty. Please provide a name.'
        };
    next();
}