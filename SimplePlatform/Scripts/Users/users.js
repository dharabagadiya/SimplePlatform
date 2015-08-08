$(document).ready(function () {
    $('#myDataTable').dataTable({
        renderer: {
            "header": "bootstrap",
            "pageButton": "bootstrap"
        },
        "ajax": {
            "url": "/user/GetUsers",
            "type": "POST"
        },
        "displayLength": 25,
        responsive: true,
        "deferRender": true,
        "columns": [{ "data": "firstName" }, { "data": "lastName" }, { "data": "userRoles" }, {
            "data": "createDate"
        }, {
            "data": null,
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                currentObj.css({ "text-align": "center" }).data(rowData);
                currentObj.off("click.dataTableEditLink").on("click.dataTableEditLink", function () { alert(1); });
            },
            render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-pencil" style="font-size: 22px;" data-original-title="Edit"></i></a>'; },
            "orderable": false,
            "width": '2%'
        }, {
            "data": null,
            "createdCell": function (cell, cellData, rowData, rowIndex, colIndex) {
                var currentObj = $(cell);
                currentObj.css({ "text-align": "center" }).data(rowData);
                currentObj.off("click.dataTableDeleteLink").on("click.dataTableDeleteLink", function () { alert(1); });
            },
            render: function (o) { return '<a href="#"><i class="ui-tooltip fa fa-trash-o" style="font-size: 22px;" data-original-title="Delete"></i></a>'; },
            "orderable": false,
            "width": '2%'
        }]
    }).removeClass('display').addClass('table table-striped table-bordered');
});