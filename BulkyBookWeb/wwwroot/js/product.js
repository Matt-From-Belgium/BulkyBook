var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-150 btn-group" role="group">
                        <a class="btn btn-primary" href="/Admin/Product/Upsert?id=${data}"><i class="bi bi-pencil-square"></i>&nbsp; Edit</a>&nbsp;
                        <a class="btn btn-danger" style="width:100px" asp-action="Delete" asp-route-id=@category.Id><i class="bi bi-trash"></i>&nbsp; Delete</a>
                    </div>
                    `
                }
            }
            
            
        ]
    });
    
}