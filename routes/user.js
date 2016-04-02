module.exports = require('express')
    .Router()
    .use('/register', require('./user/register'));