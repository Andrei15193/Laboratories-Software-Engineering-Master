const bodyParser = require('body-parser');
const data = require(modules.data.provider);

module.exports = require('express')
    .Router()
    .use('/', require(modules.site.lookup))
    .get('/', function(request, response, next) {
        data.categories.getFor(
            response.locals.site,
            function(categories) {
                response
                    .status(200)
                    .json(categories)
                    .end();
            });
    })
    .post('/', bodyParser.json(), validateCategory, function(request, response, next) {
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
                function(errors) {
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
    })
    .delete('/:name', function(request, response, next) {
        data.categories.remove(
            {
                site: response.locals.site,
                name: request.params.name.trim()
            },
            function(error) {
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
    })
    .get('/:name/posts', function(request, response, next) {
        var categoryName = request.params.name;

        data.categories.get(response.locals.site, categoryName, function(category) {
            if (!category)
                response
                    .status(404)
                    .end('The category does not exists.');
            else
                data.posts.getFor(
                    category,
                    function(posts) {
                        response
                            .status(200)
                            .json(posts)
                            .end();
                    });
        });
    })
    .post('/:name/posts', bodyParser.json(), validatePost, function(request, response, next) {
        var categoryName = request.params.name;

        data.categories.get(response.locals.site, categoryName, function(category) {
            if (!category)
                response
                    .status(404)
                    .end('The category does not exists.');
            else {
                var post = getPostFrom(request);
                post.category = category;

                data.posts.add(
                    post,
                    function() {
                        response
                            .status(200)
                            .end();
                    });
            }
        });
    })
    .delete('/:name/posts/:postId', function(request, response, next) {
        var categoryName = request.params.name;

        data.categories.get(response.locals.site, categoryName, function(category) {
            if (!category)
                response
                    .status(404)
                    .end('The category does not exists.');
            else
                data.posts.remove(
                    {
                        category: category,
                        id: request.params.postId
                    },
                    function() {
                        response
                            .status(200)
                            .end();
                    });
        });
    });

function validateCategory(request, response, next) {
    if (!/\S/.test(request.body.name))
        response.locals.errors = {
            code: 2,
            name: 'The name of a category cannot be empty. Please provide a name.'
        };
    next();
}

function validatePost(request, response, next) {
    if (!/\S/.test(request.body.title))
        response.locals.errors = {
            code: 4,
            name: 'The title of a post cannot be empty. Please provide a title.'
        };
    next();
}

function getPostFrom(request) {
    return {
        title: request.body.title,
        content: request.body.content,
        postTime: new Date()
    };
}