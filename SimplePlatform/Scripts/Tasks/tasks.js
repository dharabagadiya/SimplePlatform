var tasks = {};
tasks.options = {
    EditViewURL: "/Tasks/Edit/",
    UpdateURL: "/Tasks/Update",
    DeleteURL: "/Tasks/Delete",
    AddCommentURL: "/Comments/Add",
    DetailURL: function (id) { return ("/Tasks/GetDetail/" + id); }
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
                            regexp: /^[a-zA-Z0-9_]+$/,
                            message: 'The name can contain a-z, A-Z, 0-9, or (_) only'
                        }
                    }
                }
            }
        }).off("success.form.bv").on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);
            var taskID = formObj.find("#hdnTaskID").val();
            var name = formObj.find("#txtName").val();
            var startDate = formObj.find("#txtDueDateStart").val();
            var endDate = formObj.find("#txtDueDateEnd").val();
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
        dialogContentPlaceHolder.find('#datepicker').datepicker({ autoclose: true, todayHighlight: true });
        dialogContentPlaceHolder.find("#dwnOffices").chosen({ width: "100%" });
        dialogContentPlaceHolder.find("#dwnOffices").unbind("change").bind("change", function () {
            var groupObj = $(this.options[this.selectedIndex]).closest('optgroup');
            var officeID = 0, userID = 0;
            if (groupObj.length > 0) { officeID = groupObj.attr("id"); userID = $(this).val(); } else { officeID = $(this).val(); }
            dialogContentPlaceHolder.find("#hdnOfficeID").val(officeID);
            dialogContentPlaceHolder.find("#hdnUserID").val(userID);
        })
        if (taskDetail.UserID == 0) { dialogContentPlaceHolder.find("#dwnOffices").val(taskDetail.OfficeID); }
        else { dialogContentPlaceHolder.find("#dwnOffices").find("optgroup[id='" + taskDetail.OfficeID + "']").find("option[value='" + taskDetail.UserID + "']"); }
        dialogContentPlaceHolder.find("#dwnOffices").trigger("chosen:updated");
        this.ValidateModalTaskForm(dialogContentPlaceHolder);
    }, this));
};
tasks.DeletUserDetail = function (obj) {
    var currentObj = obj;
    var taskDetail = obj.data("task_detail");
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
                $('#userTaskList').dataTable().api().ajax.reload(null, false);
            } else { }
        }
    });
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
                if (data) {
                    tasks.UpdateUserCommentList(commentMessage, obj);
                } else {

                }
            }
        });
    });
};
tasks.GetTaskDetail = function (dataObj) {
    var taskID = dataObj.pluck("ID")[0];
    $("#divTaskList").removeClass("col-lg-12 col-sm-12").addClass("col-lg-8 col-sm-9")
    $.ajax({
        cache: false,
        type: "GET",
        url: tasks.options.DetailURL(taskID),
        success: function (data) {
            var commentControll = $("#divTaskDetailContainer");
            commentControll.empty().html(data).data("task-id", taskID);
            tasks.BindCommentControlClickEvent(commentControll);
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
            tasks.GetTaskDetail(dataObj);
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
    $('#userTaskList').dataTable({
        "select": "single",
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Tasks/GetTasks",
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "autoWidth": false,
        "columns": [
            { "data": "Title", "width": "45em" },
            { "data": "AssignTo" },
            { "data": "DueDate" }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                    currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { tasks.EditTaskDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                    currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { tasks.DeletUserDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }
        ]
    });
    $('#userTaskList').removeClass('display').addClass('table table-striped table-bordered');
    tasks.BindTaskRowClickEvent($('#userTaskList'));
};
$(document).ready(function () { tasks.ReloadTaskList(); });
