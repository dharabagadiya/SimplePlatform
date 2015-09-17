var simplePlatform = {};
simplePlatform.jqXHRData = null;
simplePlatform.filesList = [], simplePlatform.paramNames = [];
simplePlatform.ValidateModalUserDateDurationForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);;
        var dates = formObj.find("#datetimerange").val().split('-');
        var startDate = dates[0].trim();
        var endDate = dates[1].trim();
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: "/Users/AddDateDuration",
            async: false,
            data: JSON.stringify({ "startDate": startDate, "endDate": endDate }),
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
simplePlatform.BindHeaderDateDurationClickEvent = function () {
    var obj = $("#lnkDateDuration");
    obj.off("click.lnkDateDuration").on("click.lnkDateDuration", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            this.ValidateModalUserDateDurationForm(dialogContentPlaceHolder);
            dialogContentPlaceHolder.find('#datetimerange').daterangepicker();
            dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        }, this));
        return false;
    }, this));
};
simplePlatform.ValidateModalUserChangePwdForm = function (obj) {
    obj.find("form")
    .bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            oldPassword: {
                message: 'The name is not valid',
                validators: {
                    notEmpty: {
                        message: 'The old password is required and cannot be empty'
                    },
                }
            },
            newPassword: {
                validators: {
                    identical: {
                        field: 'confirmPassword',
                        message: 'The password and its confirm are not the same'
                    },
                    notEmpty: {
                        message: 'The new password is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 50,
                        message: 'The password name must be more than 3 and less than 50 characters long'
                    }
                }
            },
            confirmPassword: {
                validators: {
                    identical: {
                        field: 'newPassword',
                        message: 'The new password and its confirm are not the same'
                    },
                    notEmpty: {
                        message: 'The confirm password is required and cannot be empty'
                    },
                    stringLength: {
                        min: 3,
                        max: 50,
                        message: 'The Password name must be more than 3 and less than 50 characters long'
                    }
                }
            }
        }
    }).on('success.form.bv', function (e) {
        e.preventDefault();
        var formObj = $(e.target);;
        var oldPassword = formObj.find("#txtoldPassword").val();
        var newPassword = formObj.find("#txtnewPassword").val();
        var changePassword = formObj.find("#txtconfirmPassword").val();
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: "/Users/UpdatePassword",
            async: false,
            data: JSON.stringify({ "oldPassword": oldPassword, "newPassword": newPassword }),
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
simplePlatform.BindHeaderChangePwdClickEvent = function () {
    var obj = $("#lnkChangePassword");
    obj.off("click.lnkChangePassword").on("click.lnkChangePassword", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            this.ValidateModalUserChangePwdForm(dialogContentPlaceHolder);
            dialogContentPlaceHolder.find("#divCommonMessage").addClass("hidden");
        }, this));
        return false;
    }, this));
};
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
                message: 'The email address/contact no is not valid',
                validators: {
                    stringLength: {
                        min: 8,
                        max: 20,
                        message: 'The email address/contact no must be min 8 to 20 characters long'
                    },
                    notEmpty: {
                        message: 'The email address/contact no is required'
                    },
                    regexp: {
                        regexp: /^[0-9a-zA-Z@._]+$/,
                        message: 'The contact can contain 0-9, a-z, A-z, #, (.), or (_) only'
                    }
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
                },
                txtDescription: {
                    message: 'The description is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The description is required and cannot be empty'
                        }
                    }
                },
                imageConvention: {
                    message: 'The selected file is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The image file is required and cannot be empty'
                        },
                        file: {
                            extension: 'jpeg,png,jpg,gif',
                            type: 'image/jpeg,image/png,image/jpg,image/gif',
                            maxSize: 2097152,   // 2048 * 1024
                            message: 'The selected file is not valid'
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
            $('#frmConvention').fileupload("option", {
                formData: { "name": name, "startDate": startDates, "endDate": endDates, "description": description, "userId": userId, "city": city },
                done: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        ShowSuccessSaveAlert();
                    } else {
                        obj.find("#divCommonMessage").removeClass("hidden");
                    }
                }
            });
            simplePlatform.jqXHRData.submit();
        });
};
simplePlatform.BindHeaderAddConventionClickEvent = function () {
    var obj = $("#lnkAddConventions");
    obj.off("click.lnkAddConventions").on("click.lnkAddConventions", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            dialogContentPlaceHolder.find("#datetimerange").daterangepicker({ timePicker24Hour: true, timePicker: true, timePickerIncrement: 15, locale: { format: 'MM/DD/YYYY HH:mm' } });
            $('#frmConvention').fileupload({
                url: '/Conventions/UploadFile',
                dataType: 'json',
                add: function (e, data) {
                    //simplePlatform.filesList.push(data.files[0]);
                    //paramNames.push(data.fileInput[0].name);
                    simplePlatform.jqXHRData = data;
                }
            });
            $("#fuImage").on('change', function () {
                $("#fuImageName").val(this.files[0].name);
            });
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
                },
                txtDescription: {
                    message: 'The description is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The description is required and cannot be empty'
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
                },
                txtDescription: {
                    message: 'The description is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The description is required and cannot be empty'
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
            },
            image: {
                message: 'The selected file is not valid',
                validators: {
                    notEmpty: {
                        message: 'The image file is required and cannot be empty'
                    },
                    file: {
                        extension: 'jpeg,png,jpg,gif',
                        type: 'image/jpeg,image/png,image/jpg,image/gif',
                        maxSize: 2097152,   // 2048 * 1024
                        message: 'The selected file is not valid'
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
        var file = formObj.find('#myFile').val();
        $('#myFile').fileupload("option", {
            formData: { "name": name, "contactNo": contactNo, "city": city, "userID": userID },
            done: function (e, data) {
                var status = data.result;
                if (status === true) {
                    obj.modal('hide');
                    ShowSuccessSaveAlert();
                } else {
                    obj.find("#divCommonMessage").removeClass("hidden");
                }
            }
        });
        simplePlatform.jqXHRData.submit();
    });
};
simplePlatform.BindHeaderAddOfficeClickEvent = function () {
    var obj = $("#lnkAddOffice");
    obj.off("click.lnkAddOffice").on("click.lnkAddOffice", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, $.proxy(function (event, dialogContentPlaceHolder) {
            this.ValidateModalOfficeForm(dialogContentPlaceHolder);
            $('#myFile').fileupload({
                url: '/Offices/UploadFile',
                dataType: 'json',
                add: function (e, data) {
                    simplePlatform.jqXHRData = data;
                }
            });
            $("#myFile").on('change', function () {
                $("#txtFileName").val(this.files[0].name);
            });
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
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The first name can containe a-z, A-Z, 0-9, or (_) only'
                    }
                }
            },
            lastName: {
                message: 'The last name is not valid',
                validators: {
                    stringLength: {
                        min: 3,
                        max: 15,
                        message: 'The last name must be more than 3 and less than 15 characters long'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_ ]+$/,
                        message: 'The last name can containe a-z, A-Z, 0-9, ( ), or (_) only'
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
            },
            dwnUserRoles: {
                validators: {
                    notEmpty: {
                        message: 'Please select user role.'
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
simplePlatform.BindHeaderEditUserClickEvent = function () {
    var obj = $("#lnkUpdateUserDetail");
    obj.off("click.lnkUpdateUserDetail").on("click.lnkUpdateUserDetail", $.proxy(function (event) {
        var currentObj = $(event.currentTarget);
        $("#divCommonModalPlaceHolder").empty();
        ShowDialogBox($("#divCommonModalPlaceHolder"), currentObj.attr("url"), null, function (event, dialogContentPlaceHolder) {
            var userDetail = {};
            userDetail.id = parseInt(dialogContentPlaceHolder.find("#txtUserID").val());
            userDetail.userOfficesID = parseInt(dialogContentPlaceHolder.find("#userOfficesID").val());
            userDetail.userRolesID = parseInt(dialogContentPlaceHolder.find("#userRoleID").val());
            users.BindUserEditModelEvents(dialogContentPlaceHolder, userDetail);
        });
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
    this.BindHeaderChangePwdClickEvent();
    this.BindHeaderEditUserClickEvent();
    this.BindHeaderDateDurationClickEvent();
};
simplePlatform.UpdateParentMenu = function (obj) {
    if (obj.closest("ul").parent().find(">a[data-role=\"menu\"]").length > 0) {
        obj.closest("ul").parent().find(">a[data-role=\"menu\"]").addClass("active");
        simplePlatform.UpdateParentMenu(obj.closest("ul").parent().find(">a[data-role=\"menu\"]"));
    }
};
simplePlatform.UpdateMenuSelection = function () {
    $("a[data-role=\"menu\"]").removeClass("active");
    $("a[data-role=\"menu\"][data-page-name=\"" + pageName + "\"]").addClass("active");
    simplePlatform.UpdateParentMenu($("a[data-role=\"menu\"][data-page-name=\"" + pageName + "\"]"));
};
$(document).ready(function () {
    simplePlatform.BindHeaderAddClickEvents();
    simplePlatform.UpdateMenuSelection();
});