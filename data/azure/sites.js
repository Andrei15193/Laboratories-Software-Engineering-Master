require(modules.objectExtensions);
const azureStorage = require('azure-storage');
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

module.exports =
    {
        add: function(site, callback) {
            var siteAddBatch = new azureStorage.TableBatch();

            storageTable.createTableIfNotExists('sitesValidationName', function() {
                storageTable.insertEntity(
                    'sitesValidationName',
                    {
                        PartitionKey: site.name.toLowerCase(),
                        RowKey: 'Unique names in lowercase'
                    }.toAzureEntity(),
                    function(error) {
                        if (error)
                            callback({ name: 'The name you have picked for your site is already in use. Please use a different one.' });
                        else
                            storageTable.createTableIfNotExists('userSites', function() {
                                storageTable.insertEntity(
                                    'userSites',
                                    {
                                        PartitionKey: site.owner.username,
                                        RowKey: site.name,
                                        description: site.description
                                    }.toAzureEntity(),
                                    function(error) {
                                        callback();
                                    });
                            });
                    });
            });
        },

        getSitesFor: function(user, callback) {
            storageTable.createTableIfNotExists('userSites', function() {
                var query = new azureStorage.TableQuery().where('PartitionKey eq ?', user.username);
                storageTable.queryEntities(
                    'userSites',
                    query,
                    null,
                    function(error, result, response) {
                        callback(result.entries.map(function(entry) {
                            return entry.fromAzureEntity({
                                partitionKey: 'owner',
                                rowKey: 'name'
                            });
                        }));
                    })
            });
        },

        remove: function(site, callback) {
            storageTable.deleteEntity(
                'userSites',
                {
                    PartitionKey: site.owner.username,
                    RowKey: site.name
                }.toAzureEntity(),
                function(error, response) {
                    if (error)
                        console.error(error);

                    storageTable.deleteEntity(
                        'sitesValidationName',
                        {
                            PartitionKey: site.name.toLowerCase(),
                            RowKey: 'Unique names in lowercase'
                        }.toAzureEntity(),
                        function(error, response) {
                            if (error)
                                console.error(error);

                            callback();
                        }
                    );
                });
        }
    };