const data = require(modules.data.provider);

module.exports = {
    '/': {
        get: function (request, response, next) {
            if (response.locals.user)
                data.sites.getSitesFor(response.locals.user, function (sites) {
                    response.render('index/dashboard', { sites: sites });
                });
            else
                response.render('index/index');
        }
    }
};