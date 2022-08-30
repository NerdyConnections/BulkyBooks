var dataTable;


$(document).ready(function () {
    var url = window.location.search;
    alert("url: " + url);
    if (url.includes("inprocess")) {
        alert("inprocess");
        loadDataTable("inprocess");
    }
    else if (url.includes("completed")) {
        loadDataTable("completed");
    }
    else if (url.includes("pending")) {
        loadDataTable("pending");
    }
    else if (url.includes("approved")) {
        alert("approved");
        loadDataTable("approved");
    }
    else if (url.includes("all")) {
        loadDataTable("all");
    }

    
   
});

function loadDataTable(status) {
    debugger;
    console.log("Status:" + status);
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": "/Admin/Order/GetAll?status=" + status

        },
        "columns": [
            { "data": "id", "width": "15%" },
            { "data": "name", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-35 btn-group" role="group">
							<a href="/Admin/Order/Details?orderid=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Details</a>
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