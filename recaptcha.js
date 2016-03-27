const request = require('request');

function getReCaptchaAnswerFrom(response) {
    const gRecaptchaResponse = 'g-recaptcha-response';
    if (response instanceof String)
        return response;
    if (response instanceof Object && response.hasOwnProperty(gRecaptchaResponse))
        return response[gRecaptchaResponse];

    throw new Error('Invalid reCaptcha response, expected a string or an object with \'' + gRecaptchaResponse + '\' field');
}

function handleErrors(errors, errorHandlers) {
    errors.forEach(function(error) {
        switch (error) {
            case 'missing-input-secret':
                if (errorHandlers.missingInputSecret)
                    errorHandlers.missingInputSecret();
                break;

            case 'invalid-input-secret':
                if (errorHandlers.invalidInputSecret)
                    errorHandlers.invalidInputSecret();
                break;

            case 'missing-input-response':
                if (errorHandlers.missingInputResponse)
                    errorHandlers.missingInputResponse();
                break;

            case 'invalid-input-response':
                if (errorHandlers.invalidInputResponse)
                    errorHandlers.invalidInputResponse();
                break;
        }
    });
}

module.exports = {
    'verify': function(errorHandlers) {
        return function(request, response, next) {
            request.post(
                {
                    'url': 'https://www.google.com/recaptcha/api/siteverify',
                    'form': {
                        'secret': process.env.APPSETTING_reCaptchaSecretKey,
                        'response': getReCaptchaAnswerFrom(response)
                    }
                },
                function(error, htttResponse, htttResponseBody) {
                    if (error)
                        throw error;

                    var reCaptchaResponse = JSON.parse(htttResponseBody);

                    handleErrors(callback(body['error-codes']), errorHandlers);

                    response.locals.reCaptcha = reCaptchaResponse;
                    next();
                });
        }
    }
};