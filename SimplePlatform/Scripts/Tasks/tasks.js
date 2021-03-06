﻿var tasks = {};
tasks.jqXHRData = null;
tasks.filesList = [];
tasks.filesName = [];
tasks.tableObj = null;
tasks.options = {
    EditViewURL: "/Tasks/Edit/",
    UpdateURL: "/Tasks/Update",
    UpdateTaskStatusURL: "/Tasks/Status",
    DeleteURL: "/Tasks/Delete",
    AddCommentURL: "/Comments/Add",
    UploadAttachment: "/Tasks/UploadAttachment/",
    DetailURL: function (id) { return ("/Tasks/GetDetail/" + id); },
    startDate: null,
    endDate: null,
    addWeek: ((new Date().getDay()) <= 4 ? 0 : 1),
    subtractWeek: ((new Date().getDay()) <= 4 ? 1 : 0)
};
tasks.ValidateModalTaskForm = function (obj) {
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
                            message: 'The name can contain a-z, A-Z, 0-9, ( ), or (_) only'
                        }
                    }
                }
            }
        }).off("success.form.bv").on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);
            var taskID = formObj.find("#hdnTaskID").val();
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
                url: tasks.options.UpdateURL,
                async: false,
                data: JSON.stringify({ "taskID": taskID, "name": name, "startDate": startDate, "endDate": endDate, "description": description, "officeID": officeID, "userID": userID }),
                success: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        ShowUpdateSuccessSaveAlert();
                    } else {

                    }
                }
            });
        });
};
tasks.EditTaskDetail = function (obj) {
    var currentObj = obj;
    var taskDetail = obj.data("task_detail");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), (tasks.options.EditViewURL + taskDetail.ID), null, $.proxy(function (event, dialogContentPlaceHolder) {
        dialogContentPlaceHolder.find('#datetimerange').daterangepicker();
        dialogContentPlaceHolder.find("#dwnOffices").chosen({ width: "100%" });
        dialogContentPlaceHolder.find("#dwnOffices").unbind("change").bind("change", function () {
            var groupObj = $(this.options[this.selectedIndex]).closest('optgroup');
            var officeID = 0, userID = 0;
            if (groupObj.length > 0) { officeID = groupObj.attr("id"); userID = $(this).val(); } else { officeID = $(this).val(); }
            dialogContentPlaceHolder.find("#hdnOfficeID").val(officeID);
            dialogContentPlaceHolder.find("#hdnUserID").val(userID);
        });
        if (taskDetail.UserID == 0) { dialogContentPlaceHolder.find("#dwnOffices").val(taskDetail.OfficeID); }
        else { dialogContentPlaceHolder.find("#dwnOffices").find("optgroup[id='" + taskDetail.OfficeID + "']").find("option[value='" + taskDetail.UserID + "']"); }
        dialogContentPlaceHolder.find("#dwnOffices").trigger("chosen:updated");
        this.ValidateModalTaskForm(dialogContentPlaceHolder);
    }, this));
};
tasks.DeletTaskDetail = function (obj) {
    var currentObj = obj;
    var taskDetail = obj.data("task_detail");
    ShowOkCancelDialogBox($("#divCommonModalPlaceHolder"), "Delete", "Are you sure you want to delete record?", function (event, dataModalPlaceHolder) {
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            url: tasks.options.DeleteURL,
            async: false,
            data: JSON.stringify({ "id": taskDetail.ID }),
            success: function (data) {
                var status = data;
                if (status) {
                    window.location.reload();
                } else { }
            }
        });
    }, function (event, dataModalPlaceHolder) { });
};
tasks.UpdateUserCommentList = function (message, obj) {
    var sb = new StringBuilder();
    sb.append("<div class=\"chat-box chat-out\">");
    sb.append("<div class=\"message col-lg-10 col-lg-offset-2\">");
    sb.append("<h5 class=\"pull-left\">Aug 16,2015</h5>");
    sb.append("<div class=\"clearfix\"></div>");
    sb.append("<p>this is second Test</p>");
    sb.append("</div>");
    sb.append("<div class=\"clearfix\"></div>");
    sb.append("</div>");
    obj.find(".chat-content").append(sb.toString());
    var scrollBottom = obj.find(".chat-content").scrollTop() + obj.find(".chat-content").height();
    obj.find(".chat-content").eq(0).scrollTop(scrollBottom);
    obj.find(".chat-content").niceScroll({
        cursorcolor: "#999",
        cursoropacitymin: 0,
        cursoropacitymax: 0.3,
        cursorwidth: 5,
        cursorborder: "0px",
        cursorborderradius: "0px",
        cursorminheight: 50,
        zindex: 1,
        mousescrollstep: 20
    });
};
tasks.ValidateModalTaskCommentForm = function (obj) {
    obj.find("form")
        .bootstrapValidator({
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                txtComment: {
                    message: 'The comment is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The comment is required and cannot be empty'
                        }
                    }
                },
                fileImage: {
                    message: 'The selected file is not valid',
                    validators: {
                        notEmpty: {
                            message: 'The image file is required and cannot be empty'
                        },
                        file: {
                            extension: 'jpeg,png,jpg,gif,mp4',
                            type: 'image/jpeg,image/png,image/jpg,image/gif',
                            maxSize: 2097152,   // 2048 * 1024
                            message: 'you can upload only image files only'
                        }
                    }
                }
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);
            var taskID = formObj.find("#hdnTaskID").val();
            var comment = formObj.find("#txtComment").val();
            $('#frmTaskUpload').fileupload("option", {
                formData: { "taskID": taskID, comment: comment },
                done: function (data) {
                    var status = data;
                    if (status) {
                        obj.modal('hide');
                        tasks.GetTaskDetail(taskID);
                    } else { }
                }
            });
            $('#frmTaskUpload').fileupload('send', { files: tasks.filesList });
        });
};
tasks.UploadTaskAttachment = function (obj) {
    var currentObj = obj;
    var taskID = currentObj.data("task-id");
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), (tasks.options.UploadAttachment + taskID), null, $.proxy(function (event, dialogContentPlaceHolder) {
        this.ValidateModalTaskCommentForm(dialogContentPlaceHolder);
        tasks.jqXHRData = null;
        tasks.filesList = [];
        tasks.filesName = [];
        $('#frmTaskUpload').fileupload({
            url: tasks.options.UploadAttachment,
            dataType: 'json',
            add: function (e, data) {
                tasks.filesList.push(data.files[0]);
                tasks.filesName.push(data.files[0].name);
            }
        });
        $("#fuImage").on('change', function () {
            $("#fuImageName").val(tasks.filesName.join(','));
        });
    }, this));
};
tasks.BindCommentControlClickEvent = function (obj) {
    obj.find("#btnAddComment").off("click.btnAddComment").on("click.btnAddComment", function () {
        var commentMessage = $("#txtComment").val();
        var taskID = obj.data("task-id");
        $.ajax({
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            async: false,
            data: JSON.stringify({ "id": taskID, message: commentMessage }),
            url: tasks.options.AddCommentURL,
            success: function (data) {
                tasks.GetTaskDetail(taskID);
            }
        });
    });
    obj.find(".panel-upload").off("click.panel-upload").on("click.panel-upload", function (event) {
        event.stopPropagation();
        tasks.UploadTaskAttachment(obj);
    });
};
tasks.UpdateTaskStatus = function (taskID) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        async: false,
        data: JSON.stringify({ "id": taskID }),
        url: tasks.options.UpdateTaskStatusURL,
        success: function (data) {
            tasks.ReloadTaskList();
            tasks.GetTaskDetail(taskID);
        }
    });
};
tasks.BindMarkTaskControlEvent = function (obj) {
    obj.find("#divMarkTaskButton").off("click.divMarkTaskButton").on("click.divMarkTaskButton", function () {
        var taskID = obj.data("task-id");
        obj.find("#divMarkTaskButton").off("click.divMarkTaskButton");
        tasks.UpdateTaskStatus(taskID);
    });
};
tasks.GetTaskDetail = function (taskID) {
    $("#divTaskList").removeClass("col-lg-12 col-sm-12").addClass("col-lg-8 col-sm-9")
    $.ajax({
        cache: false,
        type: "GET",
        url: tasks.options.DetailURL(taskID),
        success: function (data) {
            var commentControll = $("#divTaskDetailContainer");
            commentControll.empty().html(data).data("task-id", taskID);
            tasks.BindCommentControlClickEvent(commentControll);
            tasks.BindMarkTaskControlEvent(commentControll);
            var scrollBottom = commentControll.find(".chat-content").scrollTop() + commentControll.find(".chat-content").height();
            commentControll.find(".chat-content").eq(0).scrollTop(scrollBottom);
            commentControll.find(".chat-content").niceScroll({
                cursorcolor: "#999",
                cursoropacitymin: 0,
                cursoropacitymax: 0.3,
                cursorwidth: 5,
                cursorborder: "0px",
                cursorborderradius: "0px",
                cursorminheight: 50,
                zindex: 1,
                mousescrollstep: 20
            });
        }
    });
};
tasks.HideTaskDetail = function (obj) {
    $("#divTaskList").removeClass("col-lg-8 col-sm-9").addClass("col-lg-12 col-sm-12");
    $("#divTaskDetailContainer").empty();
};
tasks.BindTaskRowClickEvent = function (obj) {
    obj.DataTable().off("select.dt").on("select.dt", function (e, dt, type, indexes) {
        if (type === 'row') {
            var dataObj = obj.DataTable().rows(indexes).data();
            var taskID = dataObj.pluck("ID")[0];
            tasks.GetTaskDetail(taskID);
        }
    });
    obj.DataTable().off("deselect.dt").on("deselect.dt", function (e, dt, type, indexes) {
        if (type === 'row') {
            var dataObj = obj.DataTable().rows(indexes).data();
            tasks.HideTaskDetail(dataObj);
        }
    });
};
tasks.ReloadTaskList = function () {
    $('#userTaskList').dataTable().fnDestroy();
    var tableObj = $('#userTaskList').dataTable({
        "select": "single",
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Tasks/GetTasks",
            "type": "POST",
            "data": { startDate: tasks.options.startDate.toDateString(), endDate: tasks.options.endDate.toDateString() }
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "autoWidth": false,
        "columns": [
            { "data": "Title", "width": "45em" },
            { "data": "AssignTo" },
            { "data": "DueDate" },
            {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                    if (rowData.IsUpdateEnable) {
                        currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { tasks.EditTaskDetail($(this)); });
                    }
                },
                render: function (o) {
                    if (!o.IsUpdateEnable) { return "-"; }
                    return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>';
                },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                    if (rowData.IsUpdateEnable) {
                        currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { tasks.DeletTaskDetail($(this)); });
                    }
                },
                render: function (o) {
                    if (!o.IsUpdateEnable) { return "-"; }
                    return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>';
                },
                "orderable": false,
                "width": '2%'
            },
            {
                "data": "Status",
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                    currentObj.off("click.updateStatus").on("click.updateStatus", function () {
                        tasks.UpdateTaskStatus(rowData.ID);
                    });
                },
                render: function (o) { return '<a href="#">' + (o ? "<i class=\"icon ion-android-checkbox-outline\" style=\"font-size: 22px;\"></i>" : "<i class=\"icon ion-android-checkbox-outline-blank\" style=\"font-size: 22px;\"></i>") + '</a>'; },
                "width": '10%',
                "orderable": false,
            }
        ]
    });
    $('#userTaskList').removeClass('display').addClass('table table-striped table-bordered');
    tasks.BindTaskRowClickEvent($('#userTaskList'));
    tasks.tableObj = tableObj;
};
tasks.LoadTasksGrid = function () { tasks.ReloadTaskList(); };
tasks.UpdateGlobalTimePeriodSelection = function (start, end) {
    tasks.options.startDate = start.toDate();
    tasks.options.endDate = end.toDate();
    $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
};
tasks.LoadGlobalTimeFilter = function () {
    $('#reportrange').daterangepicker({
        "startDate": moment().startOf('week').isoWeekday(4).add(tasks.options.addWeek, "week"),
        "endDate": moment().endOf('week').isoWeekday(4).add(tasks.options.addWeek, "week"),
        ranges: {
            'Current Week': [moment().startOf('week').isoWeekday(4).add(tasks.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(tasks.options.addWeek, "week")],
            'Last 7 Days': [moment().startOf('week').isoWeekday(4).subtract(tasks.options.subtractWeek, "week"), moment().endOf('week').isoWeekday(4).subtract(tasks.options.subtractWeek, "week")],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    }, tasks.UpdateGlobalTimePeriodSelection).off("apply.daterangepicker").on('apply.daterangepicker', function (ev, picker) { tasks.LoadTasksGrid(); });
    tasks.UpdateGlobalTimePeriodSelection(moment().startOf('week').isoWeekday(4).add(tasks.options.addWeek, "week"), moment().endOf('week').isoWeekday(4).add(tasks.options.addWeek, "week"));
    tasks.LoadTasksGrid();
};
tasks.DoPageSetting = function () { tasks.LoadGlobalTimeFilter(); };

