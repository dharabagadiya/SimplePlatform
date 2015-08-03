$.validator.setDefaults({
    ignore: null,
    ignore: 'input[type="hidden"]',
    errorPlacement: function (error, element) {
        var place = element.closest('.input-group');
        if (!place.get(0)) {
            place = element;
        }
        if (place.get(0).type === 'checkbox') {
            place = element.parent();
        }
        if (error.text() !== '') {
            place.after(error);
        }
    },
    errorClass: 'help-block',
    highlight: function (label) {
        $(label).closest('.form-group').removeClass('has-success').addClass('has-error');
    },
    unhighlight: function (label) {
        $(label).closest('.form-group').removeClass('has-error');
    }
});