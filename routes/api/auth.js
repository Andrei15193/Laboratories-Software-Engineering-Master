const data = require(modules.data.provider);

module.exports = require('express')
    .Router()
    .use('/', function(request, response, next) {
        var userInformation = tryGetUserInformationFrom(request);

        data.users.tryGetUser(
            userInformation.username,
            userInformation.password,
            function(user) {
                if (user) {
                    response.locals.user = user;
                    next();
                }
                else
                    response
                        .status(403)
                        .end('You do not have access to this part of the application');
            });
    });

function tryGetUserInformationFrom(request) {
    var header = request.headers['authorization'];
    if (header && /^basic\s+\S+/i.test(header)) {
        var base64Token = header.split(/\s+/).pop();
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
    return value.replace(/(\\.)/g, function(matchedValue) { return matchedValue.substring(1); });
}