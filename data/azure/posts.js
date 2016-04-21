require(modules.objectExtensions);
const azureStorage = require('azure-storage');
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

module.exports = {
    getFor: function (category, callback) {
        const postsTableName = getTableNameFor(category);
        storageTable.createTableIfNotExists(postsTableName, function () {
            var query = new azureStorage.TableQuery().where('RowKey eq ?', 'post');
            storageTable.queryEntities(
                postsTableName,
                query,
                null,
                function (error, result, response) {
                    callback(result.entries.map(function (entry) {
                        var post = entry.fromAzureEntity({
                            partitionKey: 'id',
                            rowKey: 'type'
                        });
                        post.category = category;

                        return post;
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
    }
};

function getTableNameFor(category) {
    return 'Categories' + category.site.id.toString() + 'Posts' + category.id.toString();
}
