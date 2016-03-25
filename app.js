const express = require('express');

express()
    .use(function(request, response, next) {
        response
            .status(404)
            .end('The resource you are looking for does not exist');
    })
    .listen(process.env.PORT || 3000);

console.log("Server started");