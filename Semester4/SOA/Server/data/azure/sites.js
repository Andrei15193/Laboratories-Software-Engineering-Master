require(modules.objectExtensions);
const azureStorage = require('azure-storage');
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

module.exports =
    {
        add: function (site, callback) {
            storageTable.createTableIfNotExists('sitesValidationName', function () {
                storageTable.insertEntity(
                    'sitesValidationName',
                    {
                        PartitionKey: site.name.toLowerCase(),
                        RowKey: 'Unique names in lowercase'
                    }.toAzureEntity(),
                    function (error) {
                        if (error)
                            callback({ name: 'The name you have picked for your site is already in use. Please use a different one.' });
                        else
                            storageTable.createTableIfNotExists('userSites', function () {
                                storageTable.insertEntity(
                                    'userSites',
                                    {
                                        PartitionKey: site.owner.username,
                                        RowKey: new Date().getTime().toString(),
                                        name: site.name,
                                        description: site.description
                                    }.toAzureEntity(),
                                    function (error) {
                                        callback();
                                    });
                            });
                    });
            });
        },

        getSitesFor: function (user, callback) {
            storageTable.createTableIfNotExists('userSites', function () {
                var query = new azureStorage.TableQuery().where('PartitionKey eq ?', user.username);
                storageTable.queryEntities(
                    'userSites',
                    query,
                    null,
                    function (error, result, response) {
                        callback(result.entries.map(function (entry) {
                            return entry.fromAzureEntity({
                                partitionKey: 'owner',
                                rowKey: 'id'
                            });
                        }));
                    });
            });
        },

        remove: function (site, callback) {
            const data = require(modules.data.provider);
            storageTable.deleteEntity(
                'userSites',
                {
                    PartitionKey: site.owner,
                    RowKey: site.id
                }.toAzureEntity(),
                function (error, response) {
                    if (error)
                        console.error(error);

                    storageTable.deleteEntity(
                        'sitesValidationName',
                        {
                            PartitionKey: site.name.toLowerCase(),
                            RowKey: 'Unique names in lowercase'
                        }.toAzureEntity(),
                        function (error, response) {
                            if (error)
                                console.error(error);

                            data.categories.clear(site, callback);
                        }
                    );
                });
        },

        tryGet: function (id, callback) {
            var query = new azureStorage.TableQuery().top(1).where('RowKey eq ?', id);
            storageTable.queryEntities(
                'userSites',
                query,
                null,
                function (error, result, response) {
                    if (result.entries.length == 1)
                        callback(result.entries[0].fromAzureEntity({
                            partitionKey: 'owner',
                            rowKey: 'id'
                        }));
                    else
                        callback(null);
                });
        }
    };