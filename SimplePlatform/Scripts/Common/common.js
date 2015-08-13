var simplePlatform = {};
simplePlatform.ValidateModalEventForm = function (obj) {
    obj.find("form")
        .bootstrapValidator({
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                name: {
                    message: 'The name is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The name is required and cannot be empty'
                        },
                        stringLength: {
                            min: 5,
                            max: 30,
                            message: 'The name must be more than 5 and less than 30 characters long'
                        },
                        regexp: {
                            regexp: /^[a-zA-Z0-9_]+$/,
                            message: 'The name can contain a-z, A-Z, 0-9, or (_) only'
                        }
                    }
                }
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);;
            var name = formObj.find("#txtName").val();
            var startDates = formObj.find("#txtDueDateStart").val();
            var endDates = formObj.find("#txtDueDateEnd").val();
            var description = formObj.find("#txtDescription").val();
            var officeID = formObj.find("#dwnOffices").val();
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: "/Events/Add",
                async: false,
                data: JSON.stringify({ "name": name, "startDate": startDates, "endDate": endDates, "description": description, "officeID": officeID }),
                success: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        if (!IsNullOrEmpty(office.ReloadOfficeCurrentPageData)) { office.ReloadOfficeCurrentPageData(); }
                    } else {
                        obj.find("#divCommonMessage").removeClass("hidden");
                    }
                }
            });
        });
};
simplePlatform.BindHeaderAddEventClickEvent = function () {
    var obj = $("#lnkAddEvents");
    obj.off("click.lnkAddEvents").on("click.lnkAddEvents", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            dialogContentPlaceHolder.find("#txtDueDateStart").val(new Date().mmddyyyy());
            dialogContentPlaceHolder.find("#txtDueDateEnd").val(new Date().mmddyyyy());
            dialogContentPlaceHolder.find('#datepicker').datepicker({ autoclose: true, todayHighlight: true });
            this.ValidateModalTaskForm(dialogContentPlaceHolder);
        }, this));
        return false;
    }, this));
};
simplePlatform.ValidateModalTaskForm = function (obj) {
    obj.find("form")
        .bootstrapValidator({
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                name: {
                    message: 'The name is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The name is required and cannot be empty'
                        },
                        stringLength: {
                            min: 5,
                            max: 30,
                            message: 'The name must be more than 5 and less than 30 characters long'
                        },
                        regexp: {
                            regexp: /^[a-zA-Z0-9_]+$/,
                            message: 'The name can contain a-z, A-Z, 0-9, or (_) only'
                        }
                    }
                }
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);;
            var name = formObj.find("#txtName").val();
            var startDates = formObj.find("#txtDueDateStart").val();
            var endDates = formObj.find("#txtDueDateEnd").val();
            var description = formObj.find("#txtDescription").val();
            var officeID = formObj.find("#dwnOffices").val();
            //$.ajax({
            //    dataType: "json",
            //    contentType: "application/json; charset=utf-8",
            //    type: "POST",
            //    url: "/Offices/Add",
            //    async: false,
            //    data: JSON.stringify({ "name": name, "contactNo": contactNo, "city": city, "userID": userID }),
            //    success: function (data) {
            //        var status = data;
            //        if (status) {
            //            obj.modal('hide');
            //            if (!IsNullOrEmpty(office.ReloadOfficeCurrentPageData)) { office.ReloadOfficeCurrentPageData(); }
            //        } else {
            //            obj.find("#divCommonMessage").removeClass("hidden");
            //        }
            //    }
            //});
        });
};
simplePlatform.BindHeaderAddTaskClickEvent = function () {
    var obj = $("#lnkAddTasks");
    obj.off("click.lnkAddTasks").on("click.lnkAddTasks", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            dialogContentPlaceHolder.find("#txtDueDateStart").val(new Date().mmddyyyy());
            dialogContentPlaceHolder.find("#txtDueDateEnd").val(new Date().mmddyyyy());
            dialogContentPlaceHolder.find('#datepicker').datepicker({ autoclose: true, todayHighlight: true });
            this.ValidateModalTaskForm(dialogContentPlaceHolder);
        }, this));
        return false;
    }, this));
};
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
                message: 'The name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The name is required and cannot be empty'
                    },
                    stringLength: {
                        min: 5,
                        max: 30,
                        message: 'The name must be more than 5 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The name can contain a-z, A-Z, 0-9, or (_) only'
                    }
                }
            },
            contactNo: {
                message: 'The Contact No is not valid',
                validators: {
                    notEmpty: {
                        message: 'The Contact No is required and cannot be empty'
                    },
                    stringLength: {
                        min: 10,
                        max: 10,
                        message: 'The Contact No must be 10 characters long'
                    },
                    regexp: {
                        regexp: /^[1-9][0-9]{0,15}$/,
                        message: 'The city can contain 0-9 only'
                    }
                }
            },
            city: {
                message: 'The city is not valid',
                validators: {
                    notEmpty: {
                        message: 'The city is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 30,
                        message: 'The city must be more than 3 and less than 30 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_]+$/,
                        message: 'The city can contain a-z, A-Z, 0-9, or (_) only'
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
        var userID = formObj.find("#dwnUserID").val();
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: "/Offices/Add",
            async: false,
            data: JSON.stringify({ "name": name, "contactNo": contactNo, "city": city, "userID": userID }),
            success: function (data) {
                var status = data;
                if (status) {
                    obj.modal('hide');
                    if (!IsNullOrEmpty(office.ReloadOfficeCurrentPageData)) { office.ReloadOfficeCurrentPageData(); }
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
            url: "/Users/Add",
            async: false,
            data: JSON.stringify({ "firstName": firstName, "lastName": lastName, "emildID": emailID, "userRoleID": userRoleID }),
            success: function (data) {
                var status = data;
                if (status) {
                    obj.modal('hide');
                    $('#myDataTable').dataTable().api().ajax.reload(null, false);
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
    this.BindHeaderAddTaskClickEvent();
    this.BindHeaderAddEventClickEvent();
};
$(document).ready(function () {
    simplePlatform.BindHeaderAddClickEvents();
});