var targets = {};
targets.options = {};
targets.LoadTagetsList = function () {
    $('#targetList').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/Targets/GetTasks",
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [
            { "data": "OfficeName" },
            { "data": "DueDate" },
            { "data": "Booking" },
            { "data": "FundRaising" },
            { "data": "GSB" },
            { "data": "Arrivals" },
            {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("audience_detail", rowData);
                    currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { audiences.EditAudienceDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }, {
                "data": null,
                "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                    var currentObj = $(cell);
                    currentObj.css({ "text-align": "center" }).data("audience_detail", rowData);
                    currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { audiences.DeletAudienceDetail($(this)); });
                },
                render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
                "orderable": false,
                "width": '2%'
            }]
    }).removeClass('display').addClass('table table-striped table-bordered');
};
$(document).ready(function () {
    targets.LoadTagetsList();
});