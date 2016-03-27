function getReCaptchaAnswerFrom(responseBody) {
    if (responseBody) {
        const gRecaptchaResponse = 'g-recaptcha-response';
        if (responseBody instanceof String)
            return responseBody;
        if (responseBody instanceof Object && responseBody.hasOwnProperty(gRecaptchaResponse))
            return responseBody[gRecaptchaResponse];
    }

    throw new Error('Invalid reCaptcha response body, expected a string or an object with a \'' + gRecaptchaResponse + '\' field');
}

function safeCall(func, parameters) {
    if (func)
        func.call(parameters);
}

function handleErrors(errorHandlers, errorCodes) {
    if (errorHandlers && errorCodes)
        errorCodes.forEach(function(error) {
            var errorHandler;
            switch (error) {
                case 'missing-input-secret':
                    safeCall(errorHandlers.missingInputSecret, errorHandlers);
                    break;

                case 'invalid-input-secret':
                    safeCall(errorHandlers.invalidInputSecret, errorHandlers);
                    break;

                case 'missing-input-response':
                    safeCall(errorHandlers.missingInputResponse, errorHandlers);
                    break;

                case 'invalid-input-response':
                    safeCall(errorHandlers.invalidInputResponse, errorHandlers);
                    break;
            }
        });
}

module.exports = {
    'verify': function(errorHandlers) {
        return function(request, response, next) {
            require('request')
                .post(
                {
                    'url': 'https://www.google.com/recaptcha/api/siteverify',
                    'form': {
                        'secret': process.env.APPSETTING_reCaptchaSecretKey,
                        'response': getReCaptchaAnswerFrom(request.body)
                    }
                },
                function(error, htttResponse, htttResponseBody) {
                    if (error)
                        throw error;

                    var reCaptchaResponse = JSON.parse(htttResponseBody);

                    handleErrors(errorHandlers, reCaptchaResponse['error-codes']);

                    response.locals.reCaptcha = reCaptchaResponse;
                    next();
                });
        }
    }
};