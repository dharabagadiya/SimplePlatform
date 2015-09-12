var conventionDetail = {};
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
            "data": "Name",
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                //currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                //currentObj.off("click.updateStatus").on("click.updateStatus", function () {
                //    audiences.UpdateAudienceAttendStatus(rowData.ID);
                //});
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
        "columns": [{
            "data": "Name",
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                //currentObj.css({ "text-align": "center" }).data("task_detail", rowData);
                //currentObj.off("click.updateStatus").on("click.updateStatus", function () {
                //    audiences.UpdateAudienceAttendStatus(rowData.ID);
                //});
            },
            render: function (o) {
                return "<a href=\"#\">" + (o ? "<i class=\"icon ion-android-checkbox-outline\" style=\"font-size: 22px;\"></i>" : "<i class=\"icon ion-android-checkbox-outline-blank\" style=\"font-size: 22px;\"></i>") + "</a>";
            },
            "orderable": false,
            "width": '2%'
        },
            { "data": "Name" },
            { "data": "StartDate" },
            { "data": "EndDate" }]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
$(document).ready(function () {
    conventionDetail.LoadAudienceList();
    conventionDetail.LoadEventList();
});
