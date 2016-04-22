module.exports = {
    '/api/users/details': {
        get: [
            authorize,
            function (request, response, next) {
                response.locals.user.password = undefined;
                response
                    .status(200)
                    .send(response.locals.user)
                    .end();
            }
        ]
    }
};

function authorize(request, response, next) {
    if (response.locals.user)
        next();
    else
        response
            .status(403)
            .end('You do not have access to this part of the application.');
}