const bodyParser = require('body-parser');
const data = require(modules.data.provider);

module.exports = {
    '/site/add': {
        get: function (request, response, next) {
            response.render('site/add');
        },
        post:
        [
            bodyParser.urlencoded({ extended: false }),
            validate,
            function (request, response, next) {
                if (response.locals.errors.name)
                    response.render('site/add');
                else
                    data.sites.add(
                        {
                            name: request.body.name,
                            description: request.body.description,
                            owner: response.locals.user
                        },
                        function (errors) {
                            if (errors && errors.name)
                                response.render('site/add', { errors: errors });
                            else
                                response.redirect('/');
                        });
            }
        ]
    },
    '/site/remove/:siteId': {
        get: function (request, response, next) {
            data.sites.tryGet(request.params.siteId, function (site) {
                if (site)
                    data.sites.remove(site, function () {
                        response.redirect('/');
                    });
                else {
                    console.error('Could not find site with ID: ' + request.params.siteId + '. Redirecting to \'/\'');
                    response.redirect('/');
                }
            });
        }
    }
};

function validate(request, response, next) {
    response.locals.site = {
        name: request.body.name,
        description: request.body.description
    };

    if (!response.locals.errors)
        response.locals.errors = {};

    if (!/^[^/\\#\?\t\n\r\u0000-\u001F\u007F-\u009F]{4,}$/.test(request.body.name))
        response.locals.errors.name = 'Please pick a site name that is at least 4 characters long and does not contain forward slashes (/), backslashes (\\), the number sign (#), the question mark (?), tab or new line characters or any control characters.';

    next();
}