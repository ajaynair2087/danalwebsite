var postalCodeCheckModal;

$(document).ready(function () {

    $('#postal_check_splash').on('show.bs.modal', function (e) {
        var $modal = $(this);
        var yourParameter1 = e.relatedTarget.dataset.yourparameter1;// postalCode

        $modal.find('.postalCodeCheckTB').val(yourParameter1);
        postalCodeCheckModal = $modal
    })
});