const fs = require('fs');
const path = require('path');
const express = require('express');

module.exports = {
    from: function (routesDirectory) {
        var root = {
            directoryPath: path.resolve(routesDirectory),
            routerConfig: {},
            parent: null,
            routerOptions: null,
            rotuer: null
        };

        var toVisitStack = [root];

        do {
            var current = toVisitStack.pop();
            var directoryPath = current.directoryPath;
            var routerConfig = current.routerConfig;

            fs.readdirSync(directoryPath).forEach(function (fileName) {
                var filePath = path.join(directoryPath, fileName);
                var fileStat = fs.statSync(filePath);

                if (fileStat.isDirectory())
                    toVisitStack.push({
                        directoryPath: filePath,
                        routerConfig: {},
                        routerOptions: null,
                        parent: current,
                        router: null
                    });
                else if (fileStat.isFile()) {
                    if (fileName.toLowerCase() === '_options.json')
                        routerConfig.options = require(filePath);
                    else if (path.extname(filePath) === '.js')
                        append(routerConfig, require(filePath));
                }
            });

            current.router = createRouterFrom(current);
            if (current.parent != null)
                current.parent.router.use(current.router);
        } while (toVisitStack.length > 0);

        return root.router;
    }
};

function append(routerConfig, other) {
    Object
        .getOwnPropertyNames(other)
        .forEach(function (propertyName) {
            var propertyValue = other[propertyName];
            if (isRoute(propertyName)) {
                if (!routerConfig.hasOwnProperty(propertyName))
                    Object.defineProperty(
                        routerConfig,
                        propertyName,
                        {
                            writable: false,
                            configurable: false,
                            enumerable: true,
                            value: {}
                        });
                try {
                    appendToRouteConfig(routerConfig[propertyName], ensureIsRouteConfig(propertyValue));
                }
                catch (error) {
                    throw new Error(error.message + ' Route \'' + propertyName + '\'');
                }
            }
            else if (isParameter(propertyName)) {
                if (routerConfig.hasOwnProperty(propertyName))
                    throw new Error('Parameter/header/cookie handler \'' + propertyName + '\' is already defined.');

                Object.defineProperty(
                    routerConfig,
                    propertyName,
                    {
                        writable: false,
                        configurable: false,
                        enumerable: true,
                        value: ensureIsArray(propertyValue)
                    });

            }
            else
                throw new Error('Unknown mapping: \'' + propertyName + '\'');
        });
}

function appendToRouteConfig(routeConfig, other) {
    Object
        .getOwnPropertyNames(other)
        .forEach(function (httpMethod) {
            var propertyValue = other[httpMethod];

            if (Object.hasOwnProperty(routeConfig, httpMethod))
                throw new Error('Route handler ' + httpMethod + ' is already defined.');
            else
                Object.defineProperty(
                    routeConfig,
                    httpMethod,
                    {
                        writable: false,
                        configurable: false,
                        enumerable: true,
                        value: ensureIsArray(propertyValue).slice()
                    });
        });
}

function createRouterFrom(routerConfigNode) {
    var router = express.Router(getRouterOptionsFrom(routerConfigNode));

    var parameterHandlers = flattenParameterHandlers(
        getNodesFromParent(routerConfigNode)
            .map(function (node) { return node.routerConfig; }));
    Object
        .getOwnPropertyNames(routerConfigNode.routerConfig)
        .filter(isRoute)
        .forEach(function (route) {
            var routeConfig = routerConfigNode.routerConfig[route];

            setParameterHandlersFor(router, route, parameterHandlers);

            Object
                .getOwnPropertyNames(routeConfig)
                .forEach(function (httpMethod) {
                    router[httpMethod](route, routeConfig[httpMethod]);
                });
        });

    return router;
}

function getRouterOptionsFrom(routerConfigNode) {
    var node = routerConfigNode;
    var routerOptions = null;

    while (routerOptions == null && node != null) {
        routerOptions = node.routerConfig.options;
        node = node.parent;
    }

    return routerOptions;
}

function setParameterHandlersFor(router, route, handlers) {
    var parametersMiddleWare = []
        .concat(getMiddlewareFor(handlers.header, function (request, headerName) {
            return request.header(headerName);
        }))
        .concat(getMiddlewareFor(handlers.cookie, function (request, cookieName) {
            if (!request.cookies)
                throw new Error('Cookie parser has not been configured! In order to make use of the cookie parameter binding \'cookie-parser\' must be set at the application level.');
            return request.cookies[cookieName];
        }))
        .concat(getMiddlewareFor(handlers.signedCookie, function (request, signedCookieName) {
            if (!request.signedCookies)
                throw new Error('Cookie parser has not been configured! In order to make use of the cookie parameter binding \'cookie-parser\' must be set at the application level.');
            return request.signedCookies[signedCookieName];
        }))
        .concat(getMiddlewareForRoute(route, handlers.route))
        .concat(getMiddlewareFor(handlers.query, function (request, queryParameterName) {
            return request.query[queryParameterName];
        }));

    if (parametersMiddleWare.length > 0)
        router.use(route, parametersMiddleWare);
}

function getMiddlewareFor(handlers, valueProvider) {
    var callbacks = [];

    Object
        .getOwnPropertyNames(handlers)
        .forEach(function (parameterName) {
            return handlers[parameterName]
                .forEach(function (handler) {
                    callbacks.push(function (request, response, next) {
                        handler(request, response, next, valueProvider(request, parameterName));
                    });
                });
        });

    return callbacks;
}

function getMiddlewareForRoute(route, routeHandlers) {
    var callbacks = []

    Object
        .getOwnPropertyNames(routeHandlers)
        .map(function (routeParameter) {
            return {
                index: route.search(new RegExp('/:' + routeParameter + '(/|$)')),
                name: routeParameter,
                handlers: routeHandlers[routeParameter]
            }
        })
        .filter(function (parameterInfo) {
            return parameterInfo.index != -1;
        })
        .sort(function (left, right) {
            return left.index - right.index;
        })
        .forEach(function (parameterInfo) {
            parameterInfo
                .handlers
                .forEach(function (handler) {
                    callbacks.push(function (request, response, next) {
                        handler(request, response, next, request.params[parameterInfo.name]);
                    });
                });
        });

    return callbacks;
}

function flattenParameterHandlers(routeConfigs) {
    var parameterHandlers = {
        header: {},
        cookie: {},
        signedCookie: {},
        route: {},
        query: {}
    };

    routeConfigs
        .forEach(function (routeConfig) {
            Object
                .getOwnPropertyNames(routeConfig)
                .filter(isParameter)
                .forEach(function (propertyName) {
                    var parameterName = propertyName.substr(1);
                    var specificParameterHandlers = null;

                    switch (propertyName[0]) {
                        case '^':
                            specificParameterHandlers = parameterHandlers.header;
                            break;

                        case '&':
                            specificParameterHandlers = parameterHandlers.cookie;
                            break;

                        case '!':
                            specificParameterHandlers = parameterHandlers.signedCookie;
                            break;

                        case ':':
                            specificParameterHandlers = parameterHandlers.route;
                            break;

                        case '?':
                            specificParameterHandlers = parameterHandlers.query;
                            break;

                        default:
                            throw new Error('Unknown mapping: \'' + propertyName + '\'');
                    }

                    var handlers;
                    if (Object.hasOwnProperty(specificParameterHandlers, parameterName))
                        handlers = specificParameterHandlers[parameterName].concat(routeConfig[propertyName])
                    else
                        handlers = routeConfig[propertyName];
                    Object.defineProperty(
                        specificParameterHandlers,
                        parameterName,
                        {
                            writable: true,
                            configurable: false,
                            enumerable: true,
                            value: handlers
                        });
                });
        });

    return parameterHandlers;
}

function getNodesFromParent(routerConfigNode) {
    var node = routerConfigNode;
    var nodes = [];

    while (node != null) {
        nodes.unshift(node);
        node = node.parent;
    }

    return nodes;
}

function isRoute(propertyName) {
    return propertyName.startsWith('/');
}

function isParameter(propertyName) {
    switch (propertyName[0]) {
        case '^':
        case '&':
        case '!':
        case ':':
        case '?':
            return true;
        default:
            return false;
    }
}

function isRouteParameter(propertyName) {
    return propertyName[0] === ':';
}

function ensureIsArray(value) {
    if (value instanceof Array)
        return value;
    else
        return [value];
}

function ensureIsRouteConfig(value) {
    if (value instanceof Function)
        return { use: value };
    else
        return value;
}