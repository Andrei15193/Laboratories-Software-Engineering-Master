require(modules.objectExtensions);
const azureStorage = require('azure-storage');
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

module.exports = {
    getFor: function(site, callback) {
        storageTable.createTableIfNotExists(site.name.trim() + 'Categories', function() {
            var query = new azureStorage.TableQuery().where('PartitionKey eq ?', site.name);
            storageTable.queryEntities(
                site.name + 'Categories',
                query,
                null,
                function(error, result, response) {
                    callback(result.entries.map(function(entry) {
                        var category = entry.fromAzureEntity({
                            partitionKey: 'site',
                            rowKey: 'name'
                        });
                        category.site = site;

                        return category;
                    }));
                });
        });
    },

    get: function(site, categoryName, callback) {
        storageTable.createTableIfNotExists(site.name.trim() + 'Categories', function() {
            var query = new azureStorage.TableQuery().where('PartitionKey eq ?', site.name);
            storageTable.retrieveEntity(
                site.name.trim() + 'Categories',
                site.name,
                categoryName,
                function(error, result, response) {
                    if (error)
                        callback(null);
                    else {
                        var category = result.fromAzureEntity({
                            partitionKey: 'site',
                            rowKey: 'name'
                        });
                        category.site = site;
                        callback(category);
                    }
                });
        });
    },

    add: function(category, callback) {
        storageTable.createTableIfNotExists(category.site.name + 'Categories', function() {
            storageTable.insertEntity(
                category.site.name + 'Categories',
                {
                    PartitionKey: category.site.name,
                    RowKey: category.name,
                    description: category.description
                }.toAzureEntity(),
                function(error) {
                    if (error)
                        callback({ code: 1, name: 'A category with the given name already exists, please use a different one.' });
                    else
                        callback();
                }
            )
        });
    },

    remove: function(category, callback) {
        storageTable.createTableIfNotExists(category.site.name + 'Categories', function() {
            storageTable.deleteEntity(
                category.site.name + 'Categories',
                {
                    PartitionKey: category.site.name,
                    RowKey: category.name
                }.toAzureEntity(),
                function(error) {
                    if (error)
                        callback(error);
                    else
                        storageTable.deleteTable(
                            category.site.name + 'Category' + category.name.replace(' ', '') + 'Posts',
                            function(error) {
                                callback(error);
                            });
                }
            )
        });
    }
};