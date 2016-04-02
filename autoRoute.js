const fs = require('fs');
const path = require('path');

module.exports = {
    from: function(routesDirectoryPath, defaultRoute) {
        var absoluteRoutesDirectoryPath = getAbsoluteDirectoryPathFrom(routesDirectoryPath || __dirname);
        var normalizedDefaultRoute = getNormalizedRouteFrom(defaultRoute || 'index');

        var router = require('express').Router();

        getFilesOnAllLevelsFrom(absoluteRoutesDirectoryPath)
            .filter(function(filePath) {
                return path.extname(filePath) == '.js';
            })
            .map(function(filePath) {
                return {
                    'route': filePath.substring(absoluteRoutesDirectoryPath.length, filePath.length - '.js'.length).replace(/\\/g, '/'),
                    'router': require(filePath)
                }
            })
            .filter(function(routeInfo) {
                return routeInfo.router instanceof Function;
            })
            .forEach(function(routeInfo) {
                router.use(routeInfo.route == normalizedDefaultRoute ? '/' : routeInfo.route, routeInfo.router);
            });

        return router;
    }
};

function getAbsoluteDirectoryPathFrom(directoryPath) {
    var absoluteDirecotryPath;

    if (!path.isAbsolute(directoryPath))
        absoluteDirecotryPath = path.join(__dirname, directoryPath);
    else
        absoluteDirecotryPath = directoryPath;

    return absoluteDirecotryPath;
}

function getNormalizedRouteFrom(route) {
    var normalizedRoute;

    if (!route.startsWith('/'))
        normalizedRoute = '/' + route;
    else
        normalizedRoute = route;

    return normalizedRoute;
}

function getFilesOnAllLevelsFrom(direcotryPath) {
    var directoriesLeft = [direcotryPath];
    var files = [];

    while (directoriesLeft.length > 0) {
        var directory = directoriesLeft.pop();
        fs.readdirSync(directory).forEach(function(filePath) {
            var fileAbsolutePath = path.join(directory, filePath);
            var fileStat = fs.statSync(fileAbsolutePath);

            if (fileStat.isFile())
                files.push(fileAbsolutePath);
            else if (fileStat.isDirectory())
                directoriesLeft.push(fileAbsolutePath);
        });
    }

    return files;
}