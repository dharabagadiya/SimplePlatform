var conventionDetail = {};
conventionDetail.options = {
    UpdateAttendStatusURL: "/Audiences/AttendStatus"
};
conventionDetail.UpdateAudienceAttendStatus = function (audienceID, obj) {
    $.ajax({
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        async: false,
        data: JSON.stringify({ "id": audienceID }),
        url: conventionDetail.options.UpdateAttendStatusURL,
        success: function (data) {
            if (obj.find("i").hasClass("ion-android-checkbox-outline-blank")) {
                obj.find("i").addClass("ion-android-checkbox-outline").removeClass("ion-android-checkbox-outline-blank");
            }
            else {
                obj.find("i").addClass("ion-android-checkbox-outline-blank").removeClass("ion-android-checkbox-outline");
            }
        }
    });
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
                    conventionDetail.UpdateAudienceAttendStatus(rowData.ID, $(this));
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
