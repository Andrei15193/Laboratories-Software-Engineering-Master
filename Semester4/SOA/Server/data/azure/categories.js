require(modules.objectExtensions);
const azureStorage = require('azure-storage');
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

module.exports = {
    getFor: function getFor(site, callback) {
        const categoriesTableName = getTableNameFor(site);
        storageTable.createTableIfNotExists(categoriesTableName, function () {
            getEntries(
                categoriesTableName,
                new azureStorage.TableQuery().where('PartitionKey eq ?', site.id),
                function (entries) {
                    callback(entries
                        .map(function (entry) {
                            var category = entry.fromAzureEntity({
                                partitionKey: 'site',
                                rowKey: 'id'
                            });
                            category.site = site;

                            return category;
                        })
                        .sort(function (left, right) {
                            return parseInt(left.id) - parseInt(right.id);
                        }));
                });
        });
    },

    get: function (site, categoryId, callback) {
        const categoriesTableName = getTableNameFor(site);
        storageTable.createTableIfNotExists(categoriesTableName, function () {
            storageTable.retrieveEntity(
                categoriesTableName,
                site.id,
                categoryId,
                function (error, result, response) {
                    if (error)
                        callback(null);
                    else {
                        var category = result.fromAzureEntity({
                            partitionKey: 'site',
                            rowKey: 'id'
                        });
                        category.site = site;
                        callback(category);
                    }
                });
        });
    },

    add: function (category, callback) {
        const uniqueCategoryNameValidationTableName = getUniqueCategoryNameValidationTableNameFor(category.site);
        storageTable.createTableIfNotExists(uniqueCategoryNameValidationTableName, function () {
            storageTable.insertEntity(
                uniqueCategoryNameValidationTableName,
                {
                    PartitionKey: category.name.toLowerCase(),
                    RowKey: 'Unique names in lowercase'
                }.toAzureEntity(),
                function (error) {
                    if (error)
                        callback({ code: 1, name: 'A category with the given name already exists, please use a different one.' });
                    else {
                        const categoriesTableName = getTableNameFor(category.site);
                        storageTable.createTableIfNotExists(categoriesTableName, function () {
                            storageTable.insertEntity(
                                categoriesTableName,
                                {
                                    PartitionKey: category.site.id,
                                    RowKey: new Date().getTime().toString(),
                                    name: category.name,
                                    description: category.description
                                }.toAzureEntity(),
                                function (error) {
                                    if (error)
                                        callback({ code: 1, name: 'A category with the given name already exists, please use a different one.' });
                                    else
                                        callback();
                                }
                            )
                        });
                    }
                });
        });
    },

    remove: function remove(category, callback) {
        const data = require(modules.data.provider);
        const categoriesTableName = getTableNameFor(category.site);
        storageTable.createTableIfNotExists(categoriesTableName, function () {
            storageTable.deleteEntity(
                categoriesTableName,
                {
                    PartitionKey: category.site.id,
                    RowKey: category.id
                }.toAzureEntity(),
                function (error) {
                    if (error)
                        callback(error);
                    else {
                        const uniqueCategoryNameValidationTableName = getUniqueCategoryNameValidationTableNameFor(category.site);
                        storageTable.deleteEntity(
                            uniqueCategoryNameValidationTableName,
                            {
                                PartitionKey: category.name.toLowerCase(),
                                RowKey: 'Unique names in lowercase'
                            }.toAzureEntity(),
                            function (error) {
                                if (error)
                                    console.log(error);
                                data.posts.clear(category, callback);
                            });
                    }
                })
        });
    },

    clear: function (site, callback) {
        const data = require(modules.data.provider);
        const categoriesTableName = getTableNameFor(site);

        data.categories.getFor(site, function removeCategory(categories) {
            if (categories.length > 0)
                data.categories.remove(categories.pop(), function () {
                    removeCategory(categories);
                });
            else {
                storageTable.deleteTable(categoriesTableName, function (error) {
                    if (error)
                        console.log(error);

                    const uniqueCategoryNameValidationTableName = getUniqueCategoryNameValidationTableNameFor(site);
                    storageTable.deleteTable(uniqueCategoryNameValidationTableName, function (error) {
                        if (error)
                            console.error(error);

                        callback();
                    });
                });
            }
        });
    }
};

function getTableNameFor(site) {
    return 'Categories' + site.id.toString();
}

function getUniqueCategoryNameValidationTableNameFor(site) {
    return getTableNameFor(site) + 'UniqueNameValidation';
}

function getEntries(tableName, query, callback, context) {
    if (!context)
        context = {
            entries: [],
            continuationToken: null
        }
    storageTable.queryEntities(
        tableName,
        query,
        context.continuationToken,
        function (error, result, response) {
            result.entries.forEach(function (entry) { context.entries.push(entry); });
            if (result.continuationToken)
                getEntries(tableName, query, callback, { entries: result.entries, continuationToken: result.continuationToken });
            else
                callback(context.entries);
        });
}