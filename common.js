const crypto = require('crypto');
const azureStorage = require('azure-storage');
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

function getHash(data) {
    var hash = crypto.createHash('sha256');
    hash.update(data);
    return hash.digest('hex');
}

function tryGetUser(username, password, callback) {
    storageTable.createTableIfNotExists('users', function() {
        storageTable.retrieveEntity('users', username, getHash(password), function(error, result) {
            if (error)
                callback(null);
            else
                callback(result);
        });
    });
}

function tryGetUserByToken(token, callback) {
    storageTable.createTableIfNotExists('usersAuthenticationTokens', function() {
        storageTable.retrieveEntity('usersAuthenticationTokens', token, 'token', function(error, result) {
            if (error)
                callback(null);
            else
                tryGetUser(result.username, result.passwordHash, callback);
        });
    });
}

module.exprots = {
    getHash: getHash,
    loginCookieName: 'MOSAIC_AUTH',
    tryGetUser: tryGetUser,
    tryGetUserByToken: tryGetUserByToken
};