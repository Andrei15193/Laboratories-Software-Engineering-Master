const path = require('path');
const express = require('express');
const app = express();
const errors = require('./routes/errors');

app
    .set('views', './views')
    .set('view engine', 'jade')
    .use(express.static(path.join(__dirname, 'public')))
    .use(require('./filters/navigation'))
    .use('/', require('./routes/index'))
    .use('/user', require('./routes/user'))
    .use(errors.notFound)
    .listen(process.env.PORT || 3000);

console.log("Server started");