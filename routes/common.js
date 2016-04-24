const url = require('url');
const requestJs = require('request');

module.exports = require('express')
    .Router()
    .use(function (request, response, next) {
        response.locals.subnavItems =
            [
                {
                    url: '#top',
                    label: 'Top'
                },
                {
                    url: 'http://mosaicdemo.azurewebsites.net/',
                    label: 'Mosaic',
                    target: '_blank'
                }
            ];
        next();
    })
    .use(function (request, response, next) {
        var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/sites/details');
        var options = {
            url: endpoint,
            headers: {
                'mosaic-site': process.env.APPSETTING_mosaicSiteID
            }
        };

        requestJs.get(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
            if (mosaicApiResponse.statusCode == 200) {
                response.locals.site = JSON.parse(mosaicApiResponseBody);
                next();
            }
            else
                response.render(
                    'error',
                    {
                        site:
                        {
                            name: 'Site Not Found'
                        },
                        heading: 'You have encountered a 404',
                        message: 'We are sorry but the site you are trying to access does not exist. Please contact the administrator, it is most likely a configuration issue.'
                    });
        });
    })
    .use(function (request, response, next) {
        var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/categories');
        var options = {
            url: endpoint,
            headers: {
                'mosaic-site': process.env.APPSETTING_mosaicSiteID
            }
        };

        requestJs.get(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
            if (mosaicApiResponse.statusCode == 200) {
                response.locals.categories = JSON.parse(mosaicApiResponseBody);
                next();
            }
            else
                response.render(
                    'error',
                    {
                        heading: 'You have encountered a 404',
                        message: 'We are sorry but the resource you are looking for does not exist.'
                    });
        });
    });