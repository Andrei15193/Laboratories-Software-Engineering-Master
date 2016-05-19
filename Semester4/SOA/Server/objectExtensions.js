const entityGenerator = require('azure-storage').TableUtilities.entityGenerator;

Object.defineProperty(
    Object.prototype,
    'toAzureEntity',
    {
        writable: true,
        configurable: true,
        enumerable: false,
        value: function(options) {
            var entity = new Object();
            if (!options)
                options = {};

            for (var propertyName in this)
                if (this.hasOwnProperty(propertyName) && canGetAzureEntityValueFrom(this[propertyName]))
                    switch (propertyName) {
                        case options.partitionKey:
                        case 'PartitionKey':
                            entity.PartitionKey = getAzureEntityValueFrom(mapIfPossible(this[propertyName], options.partitionKeyMap));
                            break;

                        case options.rowKey:
                        case 'RowKey':
                            entity.RowKey = getAzureEntityValueFrom(mapIfPossible(this[propertyName], options.rowKeyMap));
                            break;

                        default:
                            if (!options.properties || options.properties.indexOf(propertyName) != -1)
                                entity[propertyName] = getAzureEntityValueFrom(this[propertyName]);
                            break;
                    }

            return entity;
        }
    });

Object.defineProperty(
    Object.prototype,
    'fromAzureEntity',
    {
        writable: true,
        configurable: true,
        enumerable: false,
        value: function(options) {
            var object = new Object();
            if (!options)
                options = {};

            for (var propertyName in this)
                if (this.hasOwnProperty(propertyName))
                    switch (propertyName) {
                        case '.metadata':
                        case 'Timestamp':
                            break;

                        case 'PartitionKey':
                            object[options.partitionKey || 'PartitionKey'] = mapIfPossible(getValueFromAzureEntity(this[propertyName]), options.partitionKeyMap);
                            break;

                        case 'RowKey':
                            object[options.rowKey || 'RowKey'] = mapIfPossible(getValueFromAzureEntity(this[propertyName]), options.rowKeyMap);
                            break;

                        default:
                            if (!options.properties || options.properties.indexOf(propertyName) != -1)
                                object[propertyName] = getValueFromAzureEntity(this[propertyName]);
                            break;
                    }

            return object;
        }
    });

function mapIfPossible(value, mapCallback) {
    if (mapCallback && mapCallback instanceof Function)
        return mapCallback(value);
    else
        return value;
}

function canGetAzureEntityValueFrom(value) {
    switch (typeof value) {
        case 'number':
        case 'boolean':
        case 'string':
            return true;

        default:
            return (value instanceof Date);
    }
}

function getAzureEntityValueFrom(value) {
    switch (typeof value) {
        case 'number':
            return getAzureEntityNumericValueFrom(value);

        case 'boolean':
            return entityGenerator.Boolean(value);

        case 'string':
            return entityGenerator.String(value);

        default:
            if (value instanceof Date)
                return entityGenerator.DateTime(value);
            break;
    }

    throw new Error('Unsuported property type!');
}

function getValueFromAzureEntity(value) {
    return value._;
}

function getAzureEntityNumericValueFrom(value) {
    if (Math.floor(value) != value)
        return entityGenerator.Double(value);

    if (isInRangeOfInt32(value))
        return entityGenerator.Int32(value);

    if (isInRangeOfInt64(value))
        return entityGenerator.Int64(value);

    return entityGenerator.Double(value);
}

function isInRangeOfInt64(value) {
    return (-9223372036854775808 <= value && value <= 9223372036854775807);
}

function isInRangeOfInt32(value) {
    return (-2147483648 <= value && value <= 2147483647);
}