const bodyParser = require('body-parser');
const data = require(modules.data.provider);

module.exports = require('express')
    .Router()
    .all('/api/categories/:categoryId/posts/:postId/', function (request, response, next) {
        const postId = request.params.postId;
        data.posts.tryGet(response.locals.category, postId, function (post) {
            if (!post)
                response
                    .status(404)
                    .end('Post with ID: ' + postId + ' does not exists.');
            else {
                response.locals.post = post;
                next();
            }
        });
    })
    .get('/api/categories/:categoryId/posts', function (request, response, next) {
        data.posts.getFor(
            response.locals.category,
            function (posts) {
                response
                    .status(200)
                    .json(posts)
                    .end();
            });
    })
    .post('/api/categories/:categoryId/posts', authorize, bodyParser.json(), validatePost, function (request, response, next) {
        if (response.locals.errors)
            response
                .status(400)
                .json(response.locals.errors)
                .end;
        else {
            var post = getPostFrom(request);
            post.category = response.locals.category;

            data.posts.add(
                post,
                function () {
                    response
                        .status(201)
                        .end();
                });
        }
    })
    .delete('/api/categories/:categoryId/posts/:postId', authorize, function (request, response, next) {
        data.posts.remove(response.locals.post, function () {
            response
                .status(204)
                .end();
        });
    });

function authorize(request, response, next) {
    if (response.locals.user && response.locals.site.owner == response.locals.user.username)
        next();
    else
        response
            .status(403)
            .end('You do not have access to this part of the application.');
}

function validatePost(request, response, next) {
    if (!/\S/.test(request.body.title))
        response.locals.errors = {
            code: 4,
            title: 'The title of a post cannot be empty. Please provide a title.'
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