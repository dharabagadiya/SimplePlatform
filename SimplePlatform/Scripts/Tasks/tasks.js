var tasks = {};
tasks.options = {
    EditViewURL: "/Users/Edit/",
    UpdateURL: "/Users/Update",
    DeleteURL: "/Users/Delete"
};
$(document).ready(function () {
    $('#myDataTable').dataTable({
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
            { "data": "Title", "width": '84%' },
            { "data": "AssignTo", "width": '10%' },
            { "data": "DueDate", "width": '6%' }
        ]
    }).removeClass('display').addClass('table table-striped table-bordered');
});
