var conventionDetail = {};
conventionDetail.options = {
    UpdateAudienceAttendStatusURL: "/Audiences/AttendStatus",
    UpdateAudienceStatusURL: "/Audiences/UpdateAudienceStatus"
};
conventionDetail.UpdateAudienceAttendStatus = function (audienceID, status, obj) {
    $("#divCommonModalPlaceHolder").empty();
    ShowDialogBox($("#divCommonModalPlaceHolder"), conventionDetail.options.UpdateAudienceStatusURL, null, $.proxy(function (event, dialogContentPlaceHolder) {
        dialogContentPlaceHolder.find(".txtVisitDate").datepicker({ autoclose: true, todayHighlight: true });
        if (status) {
            dialogContentPlaceHolder.find("#divArrivalDate").hide();
            dialogContentPlaceHolder.find(".modal-body").hide();
            dialogContentPlaceHolder.find("#btnUpdateStatus").html("Un-Mark As Arrived");
        } else {
            dialogContentPlaceHolder.find("#divArrivalDate").show();
            dialogContentPlaceHolder.find(".modal-body").show();
            dialogContentPlaceHolder.find("#btnUpdateStatus").html("Mark As Arrived");
        }
        dialogContentPlaceHolder.find("form").bootstrapValidator({
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            }
        }).off('success.form.bv').on('success.form.bv', function (e) {
            e.preventDefault();
            var formObj = $(e.target);
            var arrivalDate = formObj.find(".txtArrivalDate").val();
            $.ajax({
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                type: "POST",
                async: false,
                data: JSON.stringify({ "id": audienceID, arrivalDateTime: arrivalDate }),
                url: conventionDetail.options.UpdateAudienceAttendStatusURL,
                success: function (data) {
                    dialogContentPlaceHolder.modal("hide");
                    conventionDetail.LoadAudienceList();
                }
            });
        });
    }, this));
};
conventionDetail.LoadAudienceList = function () {
    $('#audienceList').dataTable().fnDestroy();
    $('#audienceList').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Conventions/GetAudiences/" + $("#txtConventionID").val(),
            "type": "POST",
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [{
            "data": "IsAttended",
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                currentObj.css({ "text-align": "center" });
                currentObj.off("click.updateStatus").on("click.updateStatus", function () {
                    conventionDetail.UpdateAudienceAttendStatus(rowData.ID, rowData.IsAttended, $(this));
                });
            },
            render: function (o) {
                return "<a href=\"#\">" + (o ? "<i class=\"icon ion-android-checkbox-outline\" style=\"font-size: 22px;\"></i>" : "<i class=\"icon ion-android-checkbox-outline-blank\" style=\"font-size: 22px;\"></i>") + "</a>";
            },
            "orderable": false,
            "width": '2%'
        },
            { "data": "Name" },
            { "data": "Contact" }]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
conventionDetail.LoadEventList = function () {
    $('#eventList').dataTable().fnDestroy();
    $('#eventList').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Conventions/GetEvents/" + $("#txtConventionID").val(),
            "type": "POST",
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [
            { "data": "Name" },
            { "data": "StartDate" },
            { "data": "EndDate" }]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
$(document).ready(function () {
    conventionDetail.LoadAudienceList();
    conventionDetail.LoadEventList();
});
