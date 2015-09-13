var eventDetail = {};
eventDetail.options = {
    UpdateAttendStatusURL: "/Audiences/AttendStatus"
};
eventDetail.LoadAudienceList = function () {
    $('#audienceList').dataTable().fnDestroy();
    $('#audienceList').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            //"url": "/Events/GetAudiences/" + $("#txtConventionID").val(),
            "url": "/Events/GetAudiences/2",
            "type": "POST",
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [
            { "data": "Name" },
            { "data": "Contact" },
            { "data": "VisitDate" },
            { "data": "ConventionName" },
            { "data": "FSMName" },
            { "data": "Status" },
            { "data": "GSBAmount" },
            { "data": "DonationAmount" }]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
$(document).ready(function () {
    eventDetail.LoadAudienceList();
});
