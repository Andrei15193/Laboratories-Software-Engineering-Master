var markdown = require('markdown').markdown;

module.exports = require('express')
    .Router()
    .get('/', function (request, response, next) {
        response.render(
            'index/index',
            {
                caption: 'test caption',
                posts: [
                    {
                        'title': 'test',
                        'content': markdown.toHTML('* test title'),
                        'author': 'User',
                        'time': '2016-1-15'
                    }]
            });
    });