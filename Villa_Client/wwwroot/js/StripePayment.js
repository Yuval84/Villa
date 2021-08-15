redirectToCheckout = function (sessionId) {
    var stripe = Stripe('pk_test_51JNwEfG8oK3rpYzOEEDH8S11FG0VQQ1BQwWJMi2EAfX8U1YTnSEPIVy8jmjRcTFAcT3d6CHft670qVz6gWmKiaMi00vISUjxH4');
    stripe.redirectToCheckout({
        sessionId: sessionId
    });
};