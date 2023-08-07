$(document).ready(function () {

    $(`#employeeTable`).DataTable({

        ajax: {
            url: "https://localhost:7124/api/employees",
            dataType: "JSON",
            dataSrc: "data"

        },


        columns: [
            {
                data: 'url',
                render: function (data, type, full, meta) {

                    return meta.row +1;
                }
            },
            {
                data: "nik"
            },
            {
                data: "firstName",
            },
            {
                data: 'birthDate',
            },
            {
                data: 'gender'
            },
            {
                data: 'hiringDate',
            },
            {
                data: "email"
            },
            {
                data: "phoneNumber"
            },

        ]
    });
});
