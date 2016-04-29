module.exports = require('express').Router().use(
    require('./routes/webApp/globals.js'),
    require('./routes/webApp/public/index.js'),
    require('./routes/webApp/public/api.js'),
    require('./routes/webApp/public/register.js'),
    require('./routes/webApp/public/login.js'),
    require('./routes/webApp/private/globals.js'),
    require('./routes/webApp/private/logout.js'),
    require('./routes/webApp/private/sites.js'),
    require('./routes/api/globals'),
    require('./routes/api/users'),
    require('./routes/api/sites'),
    require('./routes/api/categories'),
    require('./routes/api/posts'));