const data = require(modules.data.provider);

module.exports = require('express')
    .Router()
    .use('/', function(request, response, next) {
        var siteName = tryGetSiteNameFrom(request);

        if (siteName)
            data.sites.tryGet(
                siteName,
                function(site) {
                    if (site) {
                        response.locals.site = site;
                        next();
                    }
                    else
                        response
                            .status(404)
                            .end('The site you are trying to access does not exist.');
                });
        else
            response
                .status(404)
                .end('The site you are trying to access does not exist.');
    });

function tryGetSiteNameFrom(request) {
    return request.headers['mosaic-site'];
}