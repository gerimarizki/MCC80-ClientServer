$(document).ready(function () {

    $(`#employeeTable`).DataTable({

        ajax: {
            url: "https://localhost:7124/api/employees",
            dataType: "JSON",
            dataSrc: "data"

        },


        columns: [

            {
                data: 'no',
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: "nik" },

            {
                data: "fullName",
                render: function (data, type, row) {
                    return row.firstName + " " + row.lastName;
                }
            },
            {
                data: 'birthDate',
                render: function (data, type, row) {
                    return moment(data).format("DD MMMM YYYY");
                }
            },
            { data: 'gender' },
            {
                data: 'hiringDate',
                render: function (data, type, row) {
                    return moment(data).format("DD MMMM YYYY");
                }
            },
            { data: "email" },
            { data: "phoneNumber" },

            {
                data: null,
                render: function (data, type, row) {
                    return `<button onclick="deleteEmployee('${row.guid}')" class="btn btn-secondary"> Delete </button>`;
                }
            }
        ]
    });
});
function addEmployee() {

    let data = {
        firstName: $("#firstName").val(),
        lastName: $("#lastName").val(),
        birthDate: $("#birthDate").val(),
        gender: parseInt($("#gender").val()),
        hiringDate: $("#hiringDate").val(),
        email: $("#email").val(),
        phoneNumber: $("#phoneNumber").val(),
    };

    $.ajax({
        url: "https://localhost:7124/api/employees",
        type: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(data),
    }).done((result) => {
        Swal.fire
            (
                'Data Has Been Successfuly Inserted',
                'Success'
            ).then(() => {
                location.reload();
            })
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops',
            text: 'Failed to insert data, Please Try Again',
        })
    })

}


function deleteEmployee(Guid) {
    Swal.fire({
        title: 'R u Sure?',
        text: 'Changes cant be reverted!',
        icon: 'Warn!',
        showCancelButton: true,
        confirmationButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, Delete Data'
    }).then((result) => {
        $.ajax({
            url: "https://localhost:7124/api/employees/?guid=" + Guid,
            type: "DELETE",
        }).done((result) => {
            Swal.fire(
                'Deleted',
                'Your Data Has Been Succesfully Deleted',
                'Success'
            ).then(() => {
                location.reload();
            }).fail((error) => {
                alert("Failed to delete data. Please Try Again!")
            });

        });

    });
}
