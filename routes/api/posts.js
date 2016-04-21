const bodyParser = require('body-parser');
const data = require(modules.data.provider);

module.exports = {
    ':postId': function (request, response, next, postId) {
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
    },

    '/api/categories/:categoryId/posts': {
        get: function (request, response, next) {
            data.posts.getFor(
                response.locals.category,
                function (posts) {
                    response
                        .status(200)
                        .json(posts)
                        .end();
                });
        },

        post:
        [
            bodyParser.json(),
            validatePost,
            function (request, response, next) {
                var post = getPostFrom(request);
                post.category = response.locals.category;

                data.posts.add(
                    post,
                    function () {
                        response
                            .status(200)
                            .end();
                    });
            }
        ]
    },
    '/api/categories/:categoryId/posts/:postId': function (request, response, next) {
        data.posts.remove(response.locals.post, function () {
            response
                .status(200)
                .end();
        });
    }
};

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