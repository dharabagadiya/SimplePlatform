var simplePlatform = {};
simplePlatform.ValidateModalUserForm = function (obj) {
    obj.find("form").submit(function (event) {
        event.preventDefault();
        return false;
    }).validate({
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
        submitHandler: function (form, event) {
            var formObj = $(form);
            var firstName = formObj.find("#txtUserFirstName").val();
            var lastName = formObj.find("#txtUserLastName").val();
            var emailID = formObj.find("#txtUserEmailAddress").val();
            var userRoleID = formObj.find("#dwnUserRoles").val();
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: formObj.attr('action'),
                async: true,
                data: JSON.stringify({ "firstName": firstName, "lastName": lastName, "emildID": emailID, "userRoleID": userRoleID }),
                success: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                    } else {
                        obj.find("#divCommonMessage").removeClass("hidden");
                    }
                }
            });
            event.preventDefault();
            return false;
        }
    });
};
simplePlatform.BindHeaderAddUserClickEvent = function () {
    var obj = $("#lnkAddUser");
    var dialogContentPlaceHolder = $("#divCommonModalPlaceHolder");
    dialogContentPlaceHolder.on('show.bs.modal', $.proxy(function (event) {
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        this.ValidateModalUserForm(dialogContentPlaceHolder);
    }, this));
    dialogContentPlaceHolder.on('shown.bs.modal', $.proxy(function (event) {
        dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        this.ValidateModalUserForm(dialogContentPlaceHolder);
    }, this));
    obj.off("click.lnkAddUser").on("click.lnkAddUser", $.proxy(function () { }, this));
};
simplePlatform.BindHeaderAddClickEvents = function () {
    this.BindHeaderAddUserClickEvent();
};
$(document).ready(function () {
    simplePlatform.BindHeaderAddClickEvents();
});