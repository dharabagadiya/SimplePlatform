﻿var simplePlatform = {};
simplePlatform.ValidateModalAudienceForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            Name: {
                message: 'The name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The name is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The first name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The first name can containe a-z, A-Z, 0-9, ( ), or (_) only'
                    }
                }
            },
            Contact: {
                message: 'The Contact is not valid',
                validators: {
                    notEmpty: {
                        message: 'The Contact is required and cannot be empty'
                    },
                    stringLength: {
                        min: 5,
                        max: 50,
                        message: 'The Contact must be more than 5 and less than 50 characters long'
                    },
                    //regexp: {
                    //    regexp: /^[1-9][0-9]{0,15}$/,
                    //    message: 'The city can contain 0-9 only'
                    //}
                }
            },
            FSMName: {
                message: 'The FSM name is not valid',
                validators: {
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The FSM name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The FSM name can containe a-z, A-Z, 0-9, ( ), or (_) only'
                    }
                }
            }
        }
    }).off('success.form.bv').on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);;
        var name = formObj.find("#txtName").val();
        var contact = formObj.find("#txtContact").val();
        var visitDate = formObj.find(".txtVisitDate").val();
        var visitTypeID = formObj.find("#dwnPeopleVistiType").val();
        var officeID = formObj.find("#dwnOffices").val();
        var eventID = formObj.find("#dwnEvetns").val();
        var conventionID = formObj.find("#dwnConvensions").val();
        var fsmName = formObj.find(".txtFSMName").val();
        //var fsmID = formObj.find("#dwnFSMList").val();
        var bookingStatus = formObj.find("#dwnBookStatus").val();
        var gsbAmount = formObj.find("#txtGSBAmount").val();
        var donationAmount = formObj.find("#txtDonationAmount").val();
        if (IsNullOrEmpty(officeID) && officeID <= 0) { officeID = 0; }
        if (IsNullOrEmpty(eventID) && eventID <= 0) { eventID = 0; }
        //if (IsNullOrEmpty(fsmID) && fsmID <= 0) { fsmID = 0; 
        if (IsNullOrEmpty(conventionID) && conventionID <= 0) { conventionID = 0; }
        if (IsNullOrEmpty(gsbAmount)) { gsbAmount = 0; }
        if (IsNullOrEmpty(donationAmount) && donationAmount <= 0) { donationAmount = 0; }
        var dataObj = {
            name: name,
            visitDate: visitDate,
            contact: contact,
            visitType: visitTypeID,
            officeID: officeID,
            eventID: eventID,
            convensionID: conventionID,
            fsmName: fsmName,
            bookingStatus: bookingStatus,
            gsbAmount: gsbAmount,
            donationAmount: donationAmount
        };
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: "/Audiences/Add",
            async: false,
            data: JSON.stringify(dataObj),
            success: function (data) {
                var status = data;
                if (status) {
                    obj.modal('hide');
                    ShowSuccessSaveAlert();
                } else { }
            }
        });
    });
};
simplePlatform.BindHeaderAddAudienceDropDownChangeEvent = function (obj) {
    obj.find("#dwnPeopleVistiType").off("change.dwnPeopleVistiType").on("change.dwnPeopleVistiType", function () {
        obj.find(".divVisitTypeControl").hide();
        obj.find(".divVisitTypeControl[data-id='" + $(this).val() + "']").show();
    });
    obj.find("#dwnPeopleVistiType").val(1).change();
};
simplePlatform.BindHeaderAddAudienceClickEvent = function () {
    var obj = $("#lnkAddAudiences");
    obj.off("click.lnkAddAudiences").on("click.lnkAddAudiences", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            simplePlatform.BindHeaderAddAudienceDropDownChangeEvent(dialogContentPlaceHolder);
            dialogContentPlaceHolder.find(".txtVisitDate").datepicker({ autoclose: true, todayHighlight: true });
            this.ValidateModalAudienceForm(dialogContentPlaceHolder);
        }, this));
        return false;
    }, this));
};
simplePlatform.ValidateModalConventionForm = function (obj) {
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
                            regexp: /^[a-zA-Z0-9_ ]+$/,
                            message: 'The name can contain a-z, A-Z, 0-9'
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
                            regexp: /^[a-zA-Z0-9_ ]+$/,
                            message: 'The city can contain a-z, A-Z, 0-9'
                        }
                    }
                },
                ddlUser: {
                    validators: {
                        notEmpty: {
                            message: 'Please select user.'
                        }
                    }
                }
            }
        }).off('success.form.bv').on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);
            var name = formObj.find("#txtName").val();
            var dates = formObj.find("#datetimerange").val().split('-');
            var startDates = dates[0].trim();
            var endDates = dates[1].trim();
            var description = formObj.find("#txtDescription").val();
            var userId = 0;//formObj.find("#dwnUserId").val();
            var city = formObj.find("#txtCity").val();
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: "/Conventions/Add",
                async: false,
                data: JSON.stringify({ "name": name, "startDate": startDates, "endDate": endDates, "description": description, "userId": userId, "city": city }),
                success: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        ShowSuccessSaveAlert();
                    } else {
                        obj.find("#divCommonMessage").removeClass("hidden");
                    }
                }
            });
        });
};
simplePlatform.BindHeaderAddConventionClickEvent = function () {
    var obj = $("#lnkAddConventions");
    obj.off("click.lnkAddConventions").on("click.lnkAddConventions", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            dialogContentPlaceHolder.find("#datetimerange").daterangepicker({ timePicker24Hour: true, timePicker: true, timePickerIncrement: 15, locale: { format: 'MM/DD/YYYY HH:mm' } });
            this.ValidateModalConventionForm(dialogContentPlaceHolder);
        }, this));
        return false;
    }, this));
};
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
                            regexp: /^[a-zA-Z0-9_ ]+$/,
                            message: 'The name can contain a-z, A-Z, 0-9'
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
                            regexp: /^[a-zA-Z0-9_ ]+$/,
                            message: 'The city can contain a-z, A-Z, 0-9'
                        }
                    }
                },
                ddlOffice: {
                    validators: {
                        notEmpty: {
                            message: 'Please select office.'
                        }
                    }
                },
                ddlConvention: {
                    validators: {
                        notEmpty: {
                            message: 'Please select convention.'
                        }
                    }
                }
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);;
            var name = formObj.find("#txtName").val();
            var dates = formObj.find("#datetimerange").val().split('-');
            var startDates = dates[0].trim();
            var endDates = dates[1].trim();
            var description = formObj.find("#txtDescription").val();
            var officeID = formObj.find("#dwnOffices").val();
            var conventionID = formObj.find("#dwnConvention").val();
            var city = formObj.find("#txtCity").val();
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: "/Events/Add",
                async: false,
                data: JSON.stringify({ "name": name, "startDate": startDates, "endDate": endDates, "description": description, "officeID": officeID, "conventionID": conventionID, "city": city }),
                success: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        ShowSuccessSaveAlert();
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
            dialogContentPlaceHolder.find("#datetimerange").daterangepicker({ timePicker24Hour: true, timePicker: true, timePickerIncrement: 15, locale: { format: 'MM/DD/YYYY HH:mm' } });
            this.ValidateModalEventForm(dialogContentPlaceHolder);
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
                            regexp: /^[a-zA-Z0-9_ ]+$/,
                            message: 'The name can contain a-z, A-Z, 0-9'
                        }
                    }
                }
            }
        }).off("success.form.bv").on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);;
            var name = formObj.find("#txtName").val();
            var dates = formObj.find("#datetimerange").val().split('-');
            var startDate = dates[0].trim();
            var endDate = dates[1].trim();
            var description = formObj.find("#txtDescription").val();
            var officeID = formObj.find("#hdnOfficeID").val();
            var userID = formObj.find("#hdnUserID").val();
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                url: "/Tasks/Add",
                async: false,
                data: JSON.stringify({ "name": name, "startDate": startDate, "endDate": endDate, "description": description, "officeID": officeID, "userID": userID }),
                success: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        ShowSuccessSaveAlert();
                    } else { }
                }
            });
        });
};
simplePlatform.BindHeaderAddTaskClickEvent = function () {
    var obj = $("#lnkAddTasks");
    obj.off("click.lnkAddTasks").on("click.lnkAddTasks", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            dialogContentPlaceHolder.find('#datetimerange').daterangepicker();
            dialogContentPlaceHolder.find("#dwnOffices").chosen({ width: "100%" }).unbind("change").bind("change", function () {
                var groupObj = $(this.options[this.selectedIndex]).closest('optgroup');
                var officeID = 0, userID = 0;
                if (groupObj.length > 0) { officeID = groupObj.attr("id"); userID = $(this).val(); } else { officeID = $(this).val(); }
                dialogContentPlaceHolder.find("#hdnOfficeID").val(officeID);
                dialogContentPlaceHolder.find("#hdnUserID").val(userID);
            }).change();
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
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The name can contain a-z, A-Z, 0-9'
                    }
                }
            },
            contactNo: {
                message: 'The Contact is not valid',
                validators: {
                    notEmpty: {
                        message: 'The Contact is required and cannot be empty'
                    },
                    stringLength: {
                        min: 5,
                        max: 50,
                        message: 'The Contact must be more than 5 and less than 50 characters long'
                    },
                    //regexp: {
                    //    regexp: /^[1-9][0-9]{0,15}$/,
                    //    message: 'The city can contain 0-9 only'
                    //}
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
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The city can contain a-z, A-Z, 0-9'
                    }
                }
            },
            ddlUser: {
                validators: {
                    notEmpty: {
                        message: 'Please select user.'
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
                    ShowSuccessSaveAlert();
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
        var officeID = formObj.find("#dwnOfficeList").length > 0 ? formObj.find("#dwnOfficeList").val() : 0;
        if (IsNullOrEmpty(officeID) || userRoleID == 1) { officeID = 0; }
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: "/Users/Add",
            async: false,
            data: JSON.stringify({ "firstName": firstName, "lastName": lastName, "emildID": emailID, "userRoleID": userRoleID, "officeID": officeID }),
            success: function (data) {
                var status = data;
                if (status) {
                    obj.modal('hide');
                    ShowSuccessSaveAlert();
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
            dialogContentPlaceHolder.find("#dwnUserRoles").off("change.dwnUserRoles").on("change.dwnUserRoles", function () {
                dialogContentPlaceHolder.find(".divOfficeListContainer").show();
                var userType = $(this).val();
                //  Hide Office List if user type admin
                if (userType == 1) { dialogContentPlaceHolder.find(".divOfficeListContainer").hide(); }
            });
        }, this));
        return false;
    }, this));
};
simplePlatform.BindHeaderAddClickEvents = function () {
    this.BindHeaderAddUserClickEvent();
    this.BindHeaderAddOfficeClickEvent();
    this.BindHeaderAddTaskClickEvent();
    this.BindHeaderAddEventClickEvent();
    this.BindHeaderAddConventionClickEvent();
    this.BindHeaderAddAudienceClickEvent();
};
$(document).ready(function () {
    simplePlatform.BindHeaderAddClickEvents();
    // Example Of ShowOkCancelDialogBox
    //ShowOkCancelDialogBox($("#divCommonModalPlaceHolder"), "Test", "this is Good", function (event, dataModalPlaceHolder) { }, function (event, dataModalPlaceHolder) { });
});