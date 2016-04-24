var express = require('express');
var path = require('path');
var app = express();

app.locals.siteName = 'Andrei15193 Starcraft Fansite';

app
    .set('views', path.join(__dirname, 'views'))
    .set('view engine', 'jade')
    .use(express.static(path.join(__dirname, 'public')))
    .use(require('./routes/common'))
    .use('/', require('./routes/index'))
    .listen(process.env.PORT || 7000);

console.log('Server started');