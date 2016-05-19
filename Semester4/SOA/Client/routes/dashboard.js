const bodyParser = require('body-parser');
const url = require('url');
const requestJs = require('request');
const jwt = require('jsonwebtoken');

module.exports = require('express')
    .Router()
    .param('categoryId', function (request, response, next, categoryId) {
        var categories = response.locals.categories.filter(function (category) { return category.id == categoryId; });

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
    })
    .use('/dashboard/*', function (request, response, next) {
        if (response.locals.user)
            next();
        else
            response.render(
                'error',
                {
                    heading: 'You have encountered a 403',
                    message: 'Nice try, but you are not authorized to access this page.'
                });
    })
    .get('/dashboard', function (request, response, next) {
        response.render('dashboard/dashboard');
    })

    .get('/dashboard/category/add', function (request, response, next) {
        response.render('dashboard/categoryForm');
    })
    .post('/dashboard/category/add', bodyParser.urlencoded({ extended: false }), function (request, response, next) {
        var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/categories');
        var options = {
            url: endpoint,
            headers: {
                'Authorization': response.locals.user.authorization,
                'mosaic-site': process.env.APPSETTING_mosaicSiteID
            },
            json: true,
            body: { name: request.body.name, description: request.body.description }
        };

        requestJs.post(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
            if (mosaicApiResponse.statusCode == 201)
                response.redirect('/dashboard');
            else if (mosaicApiResponse.statusCode == 400)
                response.render(
                    'dashboard/categoryForm',
                    {
                        error: mosaicApiResponseBody,
                        name: request.body.name,
                        description: request.body.description
                    });
            else
                response.render(
                    'error',
                    {
                        heading: 'You have encountered a 503',
                        message: 'Oh no! We have a problem with our own services (how embarrassing). Please check again in a bit, if the problem persist contact the site administrator.'
                    });
        });
    })

    .get('/dashboard/category/:categoryId/view', function (request, response, next) {
        var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/categories/' + request.params.categoryId + '/posts');
        var options = {
            url: endpoint,
            headers: {
                'mosaic-site': process.env.APPSETTING_mosaicSiteID
            }
        };

        requestJs.get(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
            if (mosaicApiResponse.statusCode == 200)
                response.render(
                    'dashboard/viewPosts',
                    {
                        posts: JSON.parse(mosaicApiResponseBody)
                    });
            else
                response.render(
                    'error',
                    {
                        heading: 'You have encountered a 503',
                        message: 'Oh no! We have a problem with our own services (how embarrassing). Please check again in a bit, if the problem persist contact the site administrator.'
                    });
        });
    })

    .get('/dashboard/category/:categoryId/post', function (request, response, next) {
        response.render('dashboard/postForm');
    })
    .post('/dashboard/category/:categoryId/post', bodyParser.urlencoded({ extended: false }), function (request, response, next) {
        var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/categories/' + request.params.categoryId + '/posts');
        var options = {
            url: endpoint,
            headers: {
                'Authorization': response.locals.user.authorization,
                'mosaic-site': process.env.APPSETTING_mosaicSiteID
            },
            json: true,
            body: { title: request.body.title, content: request.body.content }
        };

        requestJs.post(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
            if (mosaicApiResponse.statusCode == 201)
                response.redirect('/dashboard/category/' + response.locals.category.id + '/view');
            else if (mosaicApiResponse.statusCode == 400)
                response.render(
                    'dashboard/postForm',
                    {
                        error: mosaicApiResponseBody,
                        title: request.body.title,
                        content: request.body.content
                    });
            else
                response.render(
                    'error',
                    {
                        heading: 'You have encountered a 503',
                        message: 'Oh no! We have a problem with our own services (how embarrassing). Please check again in a bit, if the problem persist contact the site administrator.'
                    });
        });
    })
    .get('/dashboard/category/:categoryId/post/:postId/delete', function (request, response, next) {
        var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/categories/' + response.locals.category.id + '/posts/' + request.params.postId);
        var options = {
            url: endpoint,
            headers: {
                'Authorization': response.locals.user.authorization,
                'mosaic-site': process.env.APPSETTING_mosaicSiteID
            }
        };

        requestJs.delete(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
            if (mosaicApiResponse.statusCode == 204)
                response.redirect('/dashboard/category/' + response.locals.category.id + '/view');
            else
                response.render(
                    'error',
                    {
                        heading: 'You have encountered a 503',
                        message: 'Oh no! We have a problem with our own services (how embarrassing). Please check again in a bit, if the problem persist contact the site administrator.'
                    });
        });
    })

    .get('/dashboard/category/:categoryId/delete', function (request, response, next) {
        var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/categories/' + response.locals.category.id);
        var options = {
            url: endpoint,
            headers: {
                'Authorization': response.locals.user.authorization,
                'mosaic-site': process.env.APPSETTING_mosaicSiteID
            }
        };

        requestJs.delete(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
            if (mosaicApiResponse.statusCode == 204)
                response.redirect('/dashboard');
            else
                response.render(
                    'error',
                    {
                        heading: 'You have encountered a 503',
                        message: 'Oh no! We have a problem with our own services (how embarrassing). Please check again in a bit, if the problem persist contact the site administrator.'
                    });
        });
    });