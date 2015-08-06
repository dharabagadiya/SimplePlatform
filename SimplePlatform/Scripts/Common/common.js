var simplePlatform = {};
simplePlatform.ValidateModalOfficeForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            name: {
                message: 'The username is not valid',
                validators: {
                    notEmpty: {
                        message: 'The username is required and cannot be empty'
                    },
                    stringLength: {
                        min: 6,
                        max: 30,
                        message: 'The username must be more than 6 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The username can only consist of alphabetical, number and underscore'
                    }
                }
            },
            contactNo: {
                message: 'The username is not valid',
                validators: {
                    notEmpty: {
                        message: 'The username is required and cannot be empty'
                    },
                    stringLength: {
                        min: 6,
                        max: 30,
                        message: 'The username must be more than 6 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The username can only consist of alphabetical, number and underscore'
                    }
                }
            }
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);;
        var name = formObj.find("#txtName").val();
        var contactNo = formObj.find("#txtContactNo").val();
        var city = formObj.find("#txtCity").val();
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: "/Office/Add",
            async: false,
            data: JSON.stringify({ "name": name, "contactNo": contactNo, "city": city }),
            success: function (data) {
                var status = data;
                if (status) {
                    obj.modal('hide');
                } else {
                    obj.find("#divCommonMessage").removeClass("hidden");
                }
            }
        });
    });
};
simplePlatform.BindHeaderAddOfficeClickEvent = function () {
    var obj = $("#lnkAddOffice");
    obj.off("click.lnkAddOffice").on("click.lnkAddOffice", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            this.ValidateModalOfficeForm(dialogContentPlaceHolder);
            dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        }, this));
        return false;
    }, this));
};
simplePlatform.ValidateModalUserForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            firstName: {
                message: 'The first name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The first name cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The first name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The first name can containe a-z, A-Z, 0-9, or (_) only'
                    }
                }
            },
            lastName: {
                message: 'The last name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The last name is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The last name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The last name can containe a-z, A-Z, 0-9, or (_) only'
                    }
                }
            },
            emailAddress: {
                validators: {
                    notEmpty: {
                        message: 'The email address is required'
                    },
                    emailAddress: {
                        message: 'The input is not a valid email address'
                    }
                }
            }
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);;
        var firstName = formObj.find("#txtUserFirstName").val();
        var lastName = formObj.find("#txtUserLastName").val();
        var emailID = formObj.find("#txtUserEmailAddress").val();
        var userRoleID = formObj.find("#dwnUserRoles").val();
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: "/User/Add",
            async: false,
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
    });
};
simplePlatform.BindHeaderAddUserClickEvent = function () {
    var obj = $("#lnkAddUser");
    obj.off("click.lnkAddUser").on("click.lnkAddUser", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            this.ValidateModalUserForm(dialogContentPlaceHolder);
            dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        }, this));
        return false;
    }, this));
};
simplePlatform.BindHeaderAddClickEvents = function () {
    this.BindHeaderAddUserClickEvent();
    this.BindHeaderAddOfficeClickEvent();
};
$(document).ready(function () {
    simplePlatform.BindHeaderAddClickEvents();
});