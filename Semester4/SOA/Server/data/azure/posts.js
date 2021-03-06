require(modules.objectExtensions);
const azureStorage = require('azure-storage');
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

module.exports = {
    getFor: function (category, callback) {
        const postsTableName = getTableNameFor(category);
        storageTable.createTableIfNotExists(postsTableName, function () {
            getEntries(
                postsTableName,
                new azureStorage.TableQuery().where('RowKey eq ?', 'post'),
                function (entries) {
                    callback(entries
                        .map(function (entry) {
                            var post = entry.fromAzureEntity({
                                partitionKey: 'id',
                                rowKey: 'type'
                            });
                            post.category = category;

                            return post;
                        })
                        .sort(function (left, right) {
                            return parseInt(right.id) - parseInt(left.id);
                        }));
                });
        });
    },

    tryGet: function (category, postId, callback) {
        const postsTableName = getTableNameFor(category);
        storageTable.createTableIfNotExists(postsTableName, function () {
            storageTable.retrieveEntity(
                postsTableName,
                postId,
                'post',
                function (error, result, response) {
                    if (error)
                        callback(null);
                    else {
                        var post = result.fromAzureEntity({
                            partitionKey: 'id',
                            rowKey: 'type'
                        });
                        post.category = category;
                        callback(post);
                    }
                });
        });
    },

    add: function (post, callback) {
        const postsTableName = getTableNameFor(post.category);
        storageTable.createTableIfNotExists(postsTableName, function () {
            storageTable.insertEntity(
                postsTableName,
                {
                    PartitionKey: new Date().getTime().toString(),
                    RowKey: 'post',
                    title: post.title,
                    content: post.content,
                    postTime: post.postTime
                }.toAzureEntity(),
                function (error, result, response) {
                    callback();
                });
        });
    },

    remove: function (post, callback) {
        const postsTableName = getTableNameFor(post.category);
        storageTable.createTableIfNotExists(postsTableName, function () {
            storageTable.deleteEntity(
                postsTableName,
                {
                    PartitionKey: post.id,
                    RowKey: 'post'
                }.toAzureEntity(),
                function (error, result, response) {
                    callback();
                });
        });
    },

    clear: function (category, callback) {
        const postsTableName = getTableNameFor(category);
        storageTable.deleteTable(postsTableName, function (error) {
            if (error)
                console.error(error);
            callback();
        });
    }
};

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

function getTableNameFor(category) {
    return 'Categories' + category.site.id.toString() + 'Posts' + category.id.toString();
}
