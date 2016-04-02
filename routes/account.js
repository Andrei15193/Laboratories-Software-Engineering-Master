module.exports = require('express')
    .Router()
    .use('/register/', require('./account/register'));