var stripe;
var elements;
var card;
var style = {
    base: {
        color: '#32325d',
        fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
        fontSmoothing: 'antialiased',
        fontSize: '16px',
        '::placeholder': {
            color: '#aab7c4'
        }
    },
    invalid: {
        color: '#fa755a',
        iconColor: '#fa755a'
    }
};


window.initializeStripe = (publishableKey) => {
    stripe = Stripe(publishableKey);
    elements = stripe.elements();
    card = elements.create('card', { style: style });
};

window.mountCard = (elementId) => {
    card.mount(`#${elementId}`);
};


window.confirmPayment = async (clientSecret) => {
    let result = await stripe.confirmCardPayment(clientSecret, {
        payment_method: {
            card: cardElement, // This assumes you've already created and mounted a card element.
            billing_details: {
                // Optionally, add billing details here.
            }
        }
    });

    if (result.error) {
        // Handle error on the frontend.
        console.log(result.error);
    } else {
        if (result.paymentIntent.status === 'succeeded') {
            // Payment has been processed! You can inform your backend to continue with order fulfillment.
            return true;
        }
    }

    return false;
};

window.createToken = function () {
    return new Promise((resolve, reject) => {
        stripe.createToken(card).then(function (result) {
            if (result.error) {
                // Inform the user if there was an error
                reject(result.error.message);
            } else {
                // Send the token to your server
                resolve(result.token.id);
            }
        });
    });
};

function saveAsFile(filename, bytesBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + bytesBase64;
    document.body.appendChild(link); // Needed for Firefox
    link.click();
    document.body.removeChild(link);
}