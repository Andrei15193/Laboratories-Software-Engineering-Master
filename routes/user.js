const bodyParser = require('body-parser');
const url = require('url');
const requestJs = require('request');
const jwt = require('jsonwebtoken');

module.exports = require('express')
    .Router()
    .get('/login', function (request, response, next) {
        response.render('login');
    })
    .post('/login', bodyParser.urlencoded({ extended: false }), function (request, response, next) {
        var token = 'Basic ' + new Buffer((request.body.username || '').replace(':', '\\:') + ':' + request.body.password).toString('base64');

        var endpoint = url.resolve(process.env.APPSETTING_mosaicHost, '/api/users/details');
        var options = {
            url: endpoint,
            headers: {
                'Authorization': token,
                'mosaic-site': process.env.APPSETTING_mosaicSiteID
            }
        };

        requestJs.get(options, function (mosaicApiRequest, mosaicApiResponse, mosaicApiResponseBody) {
            if (mosaicApiResponse.statusCode == 200) {
                var userDetails = JSON.parse(mosaicApiResponseBody);

                response.cookie(
                    'mosaic-client-token',
                    jwt.sign({ userDetails: userDetails, authorization: token }, process.env.APPSETTING_jwtSecret),
                    {
                        maxAge: 86400000, // 24h
                        httpOnly: true,
                        signed: true
                    });
                response.redirect('/');
            }
            else
                response.render('login', { error: 'Invalid credentials' });
        });
    })
    .get('/logout', function (request, response, next) {
        response.clearCookie('mosaic-client-token');
        response.redirect('/');
    });