﻿$(document).ready(function () {

    $(`#employeeTable`).DataTable({

        ajax: {
            url: "https://localhost:7124/api/employees",
            dataType: "JSON",
            dataSrc: "data"

        },
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'colvis',
                title: 'Colvis',
                text: 'Column Visibility'
            },
            {
                extend: 'excelHtml5',
                title: 'Excel',
                text: 'Export to excel',
                /*Columns to export*/
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                title: 'PDF',
                text: 'Export to PDF',
                /*Columns to export*/
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'print',
                title: 'Print',
                text: 'Print Table',
                /*Columns to export*/
                exportOptions: {
                    columns: ':visible'
                }
            }
        ],

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
                    return `<button onclick="ShowUpdate('${row.guid}')" data-bs-toggle="modal" data-bs-target="#modalUpdateEmployee" class="btn btn-primary"> Update </button>` +
                        `   <button onclick="deleteEmployee('${row.guid}')" class="btn btn-secondary"> Delete </button>`;
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


function ShowUpdate(guid) {
    $.ajax({
        url: "https://localhost:7124/api/employees/" + guid,
        type: "GET",
        dataType: "json"
    }).done((result) => {

        $("#guidUpd").val(result.data.guid);
        $("#nikUpd").val(result.data.nik);
        $("#firstNameUpd").val(result.data.firstName);
        $("#lastNameUpd").val(result.data.lastName);
        let birthDateFormat = moment(result.data.birthDate).format("yyyy-MM-DD");
        $("#birthDateUpd").val(birthDateFormat);
        // Melakukan penyesuaian untuk nilai gender
        if (result.data.gender === 0) {
            $("input[name='gender'][value='Female']").prop("checked", true);
        } else {
            $("input[name='gender'][value='Male']").prop("checked", true);
        }
        let hiringDateFormat = moment(result.data.hiringDate).format("yyyy-MM-DD");
        $("#hiringDateUpd").val(hiringDateFormat);
        $("#emailUpd").val(result.data.email);
        $("#phoneNumberUpd").val(result.data.phoneNumber);

        $("#modalemp2").modal("show");
    }).fail((error) => {
        alert("Failed to fetch employee data. Please try again.");
    });
}

function UpdateEmployee() {


    let data = {
        guid: $("#guidUpd").val(),
        nik: $("#nikUpd").val(),
        firstName: $("#firstNameUpd").val(),
        lastName: $("#lastNameUpd").val(),
        birthDate: $("#birthDateUpd").val(),
        gender: $("input[name='gender']:checked").val() === "Female" ? 0 : 1,
        hiringDate: $("#hiringDateUpd").val(),
        email: $("#emailUpd").val(),
        phoneNumber: $("#phoneNumberUpd").val(),
    };
    $.ajax({
        url: "https://localhost:7124/api/employees/",
        type: "PUT",
        contentType: "application/json",
        data: JSON.stringify(data)
    }).done((result) => {
        Swal.fire(  
            'Data has been successfully updated!',
            'success'
        ).then(() => {
            location.reload();
        });
    }).fail((error) => {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Failed to insert data! Please try again.'
        })
        console.log(error)
    })
}