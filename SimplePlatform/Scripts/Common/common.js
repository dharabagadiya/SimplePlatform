var simplePlatform = {};
simplePlatform.ValidateModalUserForm = function (obj) {
    obj.find("form").validate({
        errorClass: 'help-block',
        rules: {
            FirstName: {
                required: true
            },
            LastName: {
                required: true
            },

        },
        highlight: function (label) {
            $(label).closest('.form-group').removeClass('has-success').addClass('has-error');
        },
        success: function (label) {
            $(label).closest('.form-group').removeClass('has-error');
        },
        submitHandler: function (form) {
            alert(1);
            return false;
            //// form validates so do the ajax
            //$.ajax({
            //    type: $(form).attr('method'),
            //    url: "../php/client/json.php",
            //    data: $(form).serialize(),
            //    success: function (data, status) {
            //        // ajax done
            //        // do whatever & close the modal
            //        $(this).modal('hide');
            //    }
            //});
            //return false; // ajax used, block the normal submit
        }
    });
};
simplePlatform.BindHeaderAddUserClickEvent = function () {
    var obj = $("#lnkAddUser");
    var dialogContentPlaceHolder = $("#divCommonModalPlaceHolder");
    obj.off("click.lnkAddUser").on("click.lnkAddUser", $.proxy(function () {
        dialogContentPlaceHolder.on('shown.bs.modal', $.proxy(function () {
            this.ValidateModalUserForm(dialogContentPlaceHolder);
        }, this));
    }, this));
};
simplePlatform.BindHeaderAddClickEvents = function () {
    this.BindHeaderAddUserClickEvent();
};
$(document).ready(function () {
    simplePlatform.BindHeaderAddClickEvents();
});