const initStartTime = new Date();

require('./aliases').load({ propertyName: 'modules' });

const path = require('path');
const express = require('express');
const cookieParser = require('cookie-parser');
const app = express();

app
    .set('views', './views')
    .set('view engine', 'jade')
    .use(express.static(path.join(__dirname, 'public')))
    .use(cookieParser(process.env.APPSETTING_cookieSecret))
    .use(require('./routes'))
    .use(require('./errors').notFound)
    .listen(process.env.PORT || 3000);

const initEndTime = new Date();
const initTime = new Date(initEndTime.getTime() - initStartTime.getTime());
const initTimeString = initTime.getUTCHours() + ':' + initTime.getUTCMinutes() + ':' + initTime.getUTCSeconds() + ':' + initTime.getUTCMilliseconds();

console.log('Server started in ' + initTimeString + '.');