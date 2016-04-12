require(modules.objectExtensions);
const azureStorage = require('azure-storage');
const storageTable = azureStorage.createTableService(process.env.CUSTOMCONNSTR_azureTableStorage);

module.exports = {
    getFor: function(category, callback) {
        storageTable.createTableIfNotExists(category.site.name.trim() + 'Category' + category.name.replace(' ', '').trim() + 'Posts', function() {
            var query = new azureStorage.TableQuery().where('RowKey eq ?', 'post');
            storageTable.queryEntities(
                category.site.name.trim() + 'Category' + category.name.replace(' ', '') + 'Posts',
                query,
                null,
                function(error, result, response) {
                    callback(result.entries.map(function(entry) {
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

    add: function(post, callback) {
        storageTable.createTableIfNotExists(post.category.site.name + 'Category' + post.category.name.replace(' ', '') + 'Posts', function() {
            storageTable.insertEntity(
                post.category.site.name + 'Category' + post.category.name.replace(' ', '') + 'Posts',
                {
                    PartitionKey: new Date().getTime().toString(),
                    RowKey: 'post',
                    title: post.title,
                    content: post.content,
                    postTime: post.postTime
                }.toAzureEntity(),
                function(error, result, response) {
                    callback();
                });
        });
    },

    remove: function(post, callback) {
        storageTable.createTableIfNotExists(post.category.site.name + 'Category' + post.category.name.replace(' ', '') + 'Posts', function() {
            storageTable.deleteEntity(
                post.category.site.name + 'Category' + post.category.name.replace(' ', '') + 'Posts',
                {
                    PartitionKey: post.id,
                    RowKey: 'post'
                }.toAzureEntity(),
                function(error, result, response) {
                    callback();
                });
        });
    }
};