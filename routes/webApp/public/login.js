const bodyParser = require('body-parser');
const jwt = require('jsonwebtoken');
const data = require(modules.data.provider);

module.exports = {
    '/user/login': {
        get: function (request, response, next) {
            response.render('user/login');
        },
        post: [
            bodyParser.urlencoded({ extended: false }),
            function (request, response, next) {
                data.users.tryGetUser(
                    request.body.username,
                    request.body.password,
                    function (user) {
                        if (user) {
                            response.cookie(
                                'mosaic-token',
                                jwt.sign(user, process.env.APPSETTING_jwtSecret),
                                {
                                    maxAge: 86400000, // 24h
                                    httpOnly: true,
                                    signed: true
                                });
                            response.redirect('/');
                        }
                        else {
                            response.locals.errors = { _: 'Invalid username or password.' };
                            response.render('user/login');
                        }
                    });
            }
        ]
    }
};