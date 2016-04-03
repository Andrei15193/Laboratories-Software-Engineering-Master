const azureStorage = require('azure-storage');
const common = require(modules.common);
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

module.exports =
    {
        isUsernameUnique: function(username, callback) {
            storageTable.createTableIfNotExists('usersValidationUsername', function() {
                storageTable.retrieveEntity('usersValidationUsername', username.toLowerCase(), 'Unique usernames in lowercase', function(error, result) {
                    if (error)
                        callback(true);
                    else
                        callback(false);
                });
            });
        },

        tryGetUserByCredentials: function(username, passwordHash, callback) {
            storageTable.createTableIfNotExists(
                'users',
                function() {
                    storageTable.retrieveEntity(
                        'users',
                        username,
                        passwordHash,
                        function(error, result) {
                            if (error)
                                callback(null);
                            else
                                callback(result);
                        });
                });
        },

        add: function(user, callback) {
            var userCreateBatch = new azureStorage.TableBatch();

            userCreateBatch.insertEntity(
                user.toAzureEntity(
                    {
                        partitionKey: 'username',
                        rowKey: 'password',
                        rowKeyMap: common.getHash
                    }));
            userCreateBatch.insertEntity(
                user.toAzureEntity(
                    {
                        partitionKey: 'username',
                        rowKey: 'password',
                        rowKeyMap: function() { return 'check'; }
                    }));

            storageTable.createTableIfNotExists('usersValidationUsername', function() {
                storageTable.insertEntity(
                    'usersValidationUsername',
                    {
                        PartitionKey: user.username.toLowerCase(),
                        RowKey: 'Unique usernames in lowercase'
                    }.toAzureEntity(),
                    function(error) {
                        if (error) 
                            callback({ username: 'The username you have picked is already in use. Please use a different one.' });

                        storageTable.createTableIfNotExists('users', function() {
                            storageTable.executeBatch(
                                'users',
                                userCreateBatch,
                                function(error) {
                                    if (error)
                                        throw error;

                                    callback();
                                });
                        });
                    });
            });
        }
    };