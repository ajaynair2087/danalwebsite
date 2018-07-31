var authKey2;
var PhoneNumber2;

function autofill(element) {
    $('.invalidRegisterNumberMsg').addClass('hide');
    var PhoneNumber = $('#registerMobileTB').val();
    if ($('#autofillchkBox').is(':checked')) {
        $('.register_terms').addClass('hide');
    }
    else {
        $('.register_terms').removeClass('hide');
    }
    if (PhoneNumber == null || PhoneNumber == "" || PhoneNumber == " ") {
        $('.invalidRegisterNumberMsg').removeClass('hide');
        $('#autofillchkBox').prop('checked', false);
    }
    else {
        element.checked ? triggerAutoFill(PhoneNumber) : "";
    }
}

function triggerAutoFill(PhoneNumber) {
    $('#consumer_info_api_splash').modal('show');
    $('#explicitContentChkBox').prop('checked', false);
    $('postalCodeTB').val('');
   //runConsumerInfoAPI()
}

function runConsumerInfoAPI(postalCode) {
    $('.mismatchPostalCodeMsg').addClass('hide');
    $('.explicitContentChkBoxNotChecked').addClass('hide');
    $('.noPostalCodeMsg').addClass('hide');
    $('.explicitContentChkBoxNotChecked').addClass('hide');

    if (($('#explicitContentChkBox').is(':checked')) && ($('#postalCodeTB').val() != "")) {
        var url = "/Home/RunConsumerInfoApi/";
        $.ajax({
            method: "GET",
            url: url,
            data: {
                billingPostalCode: postalCode,
                authKey: authKey2
            },
            success: function (result) {
                if (result != "fail") {
                    $('#postalCodeCheckTB').val(result.consumerPostalCode);
                    $('#firstNameCheckTB').val(result.consumerFirstName);
                    $('#lastNameCheckTB').val(result.consumerLastName);
                    $('#streetCheckTB').val(result.consumerAddress1);
                    $('#street2CheckTB').val(result.consumerAddress2);
                    $('#emailCheckTB').val(result.consumerEmail);
                    $('#cityCheckTB').val(result.consumerCity);

                    if ($('#postalCodeCheckTB').val() == $('#postalCodeTB').val()) {
                        $('#consumer_info_api_splash').modal('hide');
                        $('#FNameTB').val(result.consumerFirstName);
                        $('#LNameTB').val(result.consumerLastName);
                        $('#emailTB').val(result.consumerEmail);
                        $('#addressTB').val(result.consumerAddress1);
                        $('#addressTB2').val(result.consumerAddress2);
                        $('#CityTB').val(result.consumerCity);
                    }
                    else {
                        $('.mismatchPostalCodeMsg').removeClass('hide');
                    }
                }
                else {
                    $('.mismatchPostalCodeMsg').removeClass('hide');
                }
            }
        })
    }
    else if (($('#explicitContentChkBox').is(':checked')) && ($('#postalCodeTB').val() ==  "")) {
        //postal code empty
        $('.noPostalCodeMsg').removeClass('hide');
    }
    else if (($('#explicitContentChkBox').not(':checked')) && ($('#postalCodeTB').val() != "" )) {
        //not checked
        $('.explicitContentChkBoxNotChecked').removeClass('hide');
    }
    else {
        $('.explicitContentChkBoxNotChecked').removeClass('hide');
        $('.noPostalCodeMsg').removeClass('hide');
    }
}

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
                            $('.invalidNumberMsg').addClass('hide');
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

function sendOTP(phone) {
    $('.enterRegisterOTPSection').addClass('hide');
    $('.phoneRegisterOTP').text('');
    $('.autoFillSection').addClass('hide');
    $('.invalidRegisterNumberMsg').addClass('hide');


    if (phone.length > 10 || phone.length < 10) {
        $('.invalidRegisterNumberMsg').removeClass('hide');
    }
    else {
        PhoneNumber2 = phone;
        var url = "/Login/SendOTP/";
        $.ajax({
            method: "POST",
            url: url,
            data: {
                PhoneNumber: '+1' + PhoneNumber2,
            },
            success: function (result) {
                if (result == "fail") {
                    $('.invalidRegisterNumberMsg').addClass('hide');
                }
                else {
                    $('.enterRegisterOTPSection').removeClass('hide');
                    $('.phoneRegisterOTP').text(PhoneNumber2);
                    authKey2 = result;
                }
            }
        });
    }
}

function ValidateOTP(OTP) {
    var url = "/Login/ValidateOTP/";
    $.ajax({
        method: "POST",
        url: url,
        data: {
            otp: OTP,
            authKey: authKey2,
        },
        success: function (result) {
            if (result == "Success") {
                $('.enterRegisterOTPSection').removeClass('hide');
                $('.phoneRegisterOTP').text(PhoneNumber2);
                $('.autoFillSection').removeClass('hide');
            }
            else {
                $('.invalidRegisterOTPMsg').removeClass('hide');
            }
        }
    });
}

function match_and_attributes() {
    $('.apiErrorMessage').addClass('hide');
    $('.customerCareErrorMessage').addClass('hide');
    var AuthKey = authKey2;
    var firstName = $('#FNameTB').val();
    var lastName = $('#LNameTB').val();
    var email = $('#emailTB').val();
    var streetAddress1 = $('#addressTB').val();
    var City = $('#CityTB').val();

    var url = "/Home/MatchAndAttributes/";
    $.ajax({
        method: "GET",
        url: url,
        data: {
            AuthKey: AuthKey,
            firstName: firstName,
            LastName: lastName,
            email: email,
            streetAddress1: streetAddress1,
            streetAddress2: "",
            City: City
        },
        success: function (result) {
            if (result != "fail") {
                if (result == "scorefail") {
                    $('.customerCareErrorMessage').removeClass('hide');
                }
                else {
                    window.location.href = "/Account/Cart";
                }
            }
            else {
                $('.apiErrorMessage').removeClass('hide');
            }
        }
    })
}