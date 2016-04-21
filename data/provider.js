const path = require('path');
const fs = require('fs');

const acceptedExtensions = ['.js', '.json', '.node'];

function getDirectoryNameFor(providerName) {
    return path.join(path.dirname(__filename), providerName);
}

function initializeFrom(providerName) {
    if (!providerName)
        throw new Error('A data provider must be specified!');

    var providerDirectoryPath = getDirectoryNameFor(providerName);
    var provider = {};
    var toVisit = [{ directoryPath: providerDirectoryPath, object: provider }];
    do {
        current = toVisit.pop();

        fs.readdirSync(current.directoryPath).forEach(function (fileName) {
            var filePath = path.join(current.directoryPath, fileName);
            var fileStats = fs.statSync(filePath);
            var fileExtension = path.extname(filePath);

            if (fileStats.isFile() && acceptedExtensions.indexOf(fileExtension) != -1)
                Object.defineProperty(
                    current.object,
                    fileName.substring(0, fileName.length - fileExtension.length),
                    {
                        writable: true,
                        configurable: true,
                        enumerable: true,
                        value: withContext(require(filePath), provider)
                    });
            else {
                var object = {};
                Object.defineProperty(
                    current.object,
                    fileName,
                    {
                        writable: true,
                        configurable: true,
                        enumerable: true,
                        value: object
                    });
                toVisit.push({ directoryPath: filePath, object: object });
            }
        });
    } while (toVisit.length > 0);

    return provider;
}

module.exports = initializeFrom(process.env.APPSETTING_dataProvider);

function withContext(object, context) {
    var root = { value: object };
    var toVisit = [root];
    var visited = [];

    do {
        var current = toVisit.pop();
        Object.getOwnPropertyNames(current).forEach(function (propertyName) {
            var propertyValue = current[propertyName];
            if (typeof propertyValue === 'function')
                current[propertyName] = current[propertyName].bind(context);
            else if (typeof propertyValue === 'object' && visited.indexOf(propertyValue) === -1) {
                visited.push(propertyValue);
                toVisit.push(propertyValue);
            }
        });
    } while (toVisit.length > 0);

    return root.value;
}