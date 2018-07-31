var authKey;
$(document).ready(function () {
    $('#login_splash').on('show.bs.modal', function () {
        $('.mobileTB').val('');
        $('.invalidNumberMsg').addClass('hide');
        $('.invalidOTPMsg').addClass('hide');
        $('.loginSuccessMessage').addClass('hide')
        $('.enterOTPSection').addClass('hide');
        $('.phoneOTP').text('');
        $('.otbTB').val('');
        $('.loginSuccessMessage').addClass('hide')

    })
});


function authenticatePhoneNumber(phone, OTP) {
    $('.invalidNumberMsg').addClass('hide');
    $('.invalidOTPMsg').addClass('hide');
    $('.loginSuccessMessage').addClass('hide')

    if (OTP == null || OTP == "") {
        $('.enterOTPSection').addClass('hide');
        $('.phoneOTP').text('');
        $('.otbTB').val('');
    }

    if (phone == "") {
        $('.invalidNumberMsg').removeClass('hide');
    }
    else {
        //validate number using Twilio API
        //if success, show enterOTPSection
        if (phone.length > 10 || phone.length < 10) {
            $('.invalidNumberMsg').removeClass('hide');
        }
        else {
            if (OTP == null || OTP == "") {
                var PhoneNumber = phone;
                var url = "/Login/SendOTP/";
                $.ajax({
                    method: "POST",
                    url: url,
                    data: {
                        PhoneNumber: '+1' + PhoneNumber,
                    },
                    success: function (result) {
                        if (result == "fail") {
                            $('.invalidNumberMsg').removeClass('hide');
                        }
                        else {
                            $('.enterOTPSection').removeClass('hide');
                            $('.phoneOTP').text(PhoneNumber);
                            authKey = result;
                        }
                    }
                });
            }
            else {
                //OTP Present => Validate OTP
                var PhoneNumber = phone;
                var url = "/Login/ValidateOTP/";
                $.ajax({
                    method: "POST",
                    url: url,
                    data: {
                        otp: OTP,
                        authKey: authKey,
                    },
                    success: function (result) {
                        if (result == "Success") {
                            $('.enterOTPSection').removeClass('hide');
                            $('.phoneOTP').text(PhoneNumber);
                            $('#login_splash').modal('toggle');
                            if (!!$.cookie('isAuthenticated')) {
                                // have cookie
                                setGreetingCookie(phone);
                            } else {
                                // no cookie => create isAuthenticated cookie
                                $.cookie("isAuthenticated", true);
                                setGreetingCookie(phone);
                            }


                            window.location.href = "/Account/Cart";
                        }
                        else {
                            $('.invalidOTPMsg').removeClass('hide');
                        }
                    }
                });
            }
        }
    }
}

function setGreetingCookie(phone) {
    if (!!$.cookie('Greeting')) {
        var loggedInName = getNameFromPhone(phone);
        $.cookie("Greeting", loggedInName);
    } else {
        // no cookie => create isAuthenticated cookie
        var loggedInName = getNameFromPhone(phone);
        $.cookie("Greeting", loggedInName);
    }
}

function getNameFromPhone(phone) {
    if (phone == "4444441001") {
        return "Hello Michael";
    }
    else if (phone == "3333331001") {
        return "Hello Michael";

    }
    else if (phone == "3333332001") {
        return "Hello Aaron";

    }
    else if (phone == "3333333001") {
        return "Hello Michael ";

    }
    else if (phone == "3333334001") {
        return "Hello David";

    }
}