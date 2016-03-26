const request = require('request');

function getReCaptchaAnswerFrom(response) {
    const gRecaptchaResponse = 'g-recaptcha-response';
    if (response instanceof String)
        return response;
    if (response instanceof Object && response.hasOwnProperty(gRecaptchaResponse))
        return response[gRecaptchaResponse];

    throw new Error('Invalid reCaptcha response, expected a string or an object with \'' + gRecaptchaResponse + '\' field');
}

module.exports = {
    'verify': function(response, callback) {
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

                var body = JSON.parse(htttResponseBody);

                if (body.success)
                    callback(null);
                else
                    callback(body['error-codes']);
            });
    }
};