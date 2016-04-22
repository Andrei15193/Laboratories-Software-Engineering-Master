const data = require(modules.data.provider);

module.exports = {
    '^authorization': function (request, response, next, authorization) {
        var userInformation = tryGetUserInformationFrom(authorization);
        if (userInformation)
            data.users.tryGetUser(
                userInformation.username,
                userInformation.password,
                function (user) {
                    if (user)
                        response.locals.user = user;
                    next();
                });
        else
            next();
    },
    '^mosaic-site': function (request, response, next, siteId) {
        if (siteId)
            data.sites.tryGet(
                siteId,
                function (site) {
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
                .status(400)
                .end('You must provide a site name through \'mosaic-site\' header.');
    }
}

function tryGetUserInformationFrom(authorization) {
    if (authorization && /^basic\s+\S+/i.test(authorization)) {
        var base64Token = authorization.split(/\s+/).pop();
        var userInformation = new Buffer(base64Token, 'base64').toString();

        var index = indexOfNotEscaped(userInformation, ':');
        if (index < userInformation.length)
            return {
                username: unescape(userInformation.substring(0, index)),
                password: unescape(userInformation.substring(index + 1))
            };
    }

    return null;
}

function indexOfNotEscaped(value, separator) {
    var index = 0;
    var found = false;
    var isEscaped = false;

    while (index < value.length && !found) {
        if (!isEscaped && value[index] == separator)
            found = true;
        else {
            if (value[index] == '\\')
                isEscaped = !isEscaped;
            else
                isEscaped = false;
            index++;
        }
    }

    return index;
}

function unescape(value) {
    return value.replace(/(\\.)/g, function (matchedValue) { return matchedValue.substring(1); });
}