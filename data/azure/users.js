const azureStorage = require('azure-storage');
const common = require(modules.common);
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);
const crypto = require('crypto');

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

        tryGetUser: function(username, password, callback) {
            storageTable.createTableIfNotExists(
                'users',
                function() {
                    storageTable.retrieveEntity(
                        'users',
                        username,
                        getHashFor(password),
                        function(error, result) {
                            if (error)
                                callback(null);
                            else
                                callback(result.fromAzureEntity(
                                    {
                                        partitionKey: 'username',
                                        rowKey: 'password'
                                    }));
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
                        rowKeyMap: getHashFor
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

function getHashFor(data) {
    var hash = crypto.createHash('sha256');
    hash.update(data);
    return hash.digest('hex');
}