const path = require('path');

module.exports = {
    load: function(options) {
        if (!options)
            options = {};

        var aliases = require(path.join(__dirname, options.aliasesFileName ? options.aliasesFileName : 'aliases.json'));

        makePathsAbsolute(aliases);

        Object.defineProperty(
            global,
            (options.propertyName) ? options.propertyName : 'aliases',
            {
                writable: false,
                configurable: false,
                enumerable: false,
                value: aliases
            });
    }
};

function makePathsAbsolute(aliases) {
    var toVisit = [aliases];

    do {
        var current = toVisit.pop();
        for (var property in current) {
            switch (typeof current[property]) {
                case 'string':
                    current[property] = path.normalize(path.join(__dirname, current[property]));
                    break;
                case 'object':
                    toVisit.push(current[property]);
                    break;
            }
        }
    } while (toVisit.length > 0);
}