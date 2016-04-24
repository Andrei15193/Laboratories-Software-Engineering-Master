var express = require('express');
var path = require('path');
var app = express();

app
    .set('views', path.join(__dirname, 'views'))
    .set('view engine', 'jade')
    .use(express.static(path.join(__dirname, 'public')))
    .use(require('./routes/common'))
    .use(require('./routes/user'))
    .use(require('./routes/index'))
    .all('*', function (request, response, next) {
        response.render(
            'error',
            {
                heading: 'You have encountered a 404',
                message: 'We are sorry but the resource you are looking for does not exist.'
            });
    })
    .listen(process.env.PORT || 7000);

console.log('Server started');