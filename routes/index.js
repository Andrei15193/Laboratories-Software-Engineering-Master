const url = require('url');
const requestJs = require('request');
var markdown = require('markdown').markdown;

module.exports = require('express')
    .Router()
    .get('/', function (request, response, next) {
        if (response.locals.categories.length > 0) {
            response.locals.category = response.locals.categories[0];
            next();
        }
        else
            response.render(
                'message',
                {
                    caption: 'You\'re early!',
                    heading: 'There are no categories! How can there be posts?',
                    message: 'Looks like there are no categories added on this site, there is no way there are any posts. Lets give the author a few more moments...'
                });
    },
    getPosts)
    .get('/:categoryId', function (request, response, next) {
        var categories = response
            .locals
            .categories
            .filter(function (category) { return category.id == request.params.categoryId; });
        if (categories.length === 1) {
            response.locals.category = categories[0];
            next();
        }
        else
            response.render(
                'error',
                {
                    heading: 'You have encountered a 404',
                    message: 'We are sorry but the category you are looking for does not exist.'
                });
    },
    getPosts);

function getPosts(request, response, next) {
    var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/categories/' + response.locals.category.id.toString() + '/posts');
    var options = {
        url: endpoint,
        headers: {
            'mosaic-site': process.env.APPSETTING_mosaicSiteID
        }
    };

    requestJs.get(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
        if (mosaicApiResponse.statusCode == 200) {
            var posts = JSON.parse(mosaicApiResponseBody);

            posts.forEach(function (post) {
                post.content = markdown.toHTML(post.content);
            });
            response.render(
                'posts',
                {
                    caption: response.locals.category.name,
                    posts: posts
                });
        }
        else
            response.render(
                'error',
                {
                    heading: 'You have encountered a 404',
                    message: 'We are sorry but the category you are looking for does not exist.'
                });
    });
}