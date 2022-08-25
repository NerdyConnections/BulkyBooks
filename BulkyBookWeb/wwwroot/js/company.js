var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Admin/Product/GetAll"

        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "streetaddress", "width": "15%" },
            { "data": "city", "width": "15%" },
            { "data": "state", "width": "15%" },
            { "data": "postalcode.name", "width": "15%" },
            { "data": "phonenumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-35 btn-group" role="group">
							<a href="/Admin/Company/Upsert?id=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
						</div>
						<div class="w-35 btn-group" role="group">
							<a onclick="Delete('/Admin/Company/Delete/${data}')" class="btn btn-primary text-danger mx-2"><i class="bi bi-trash-fill"></i>Delete</a>
						</div>
                    `

                }
            },

        ]
    });
}

function Delete(url) {
    debugger;
    swal({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}