// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



$(document).ready(function () {

    $(`#myTable`).DataTable({

        ajax: {
            url: "https://localhost:7124/api/employees",
            dataType: "JSON",
            dataSrc: "data"

        },

        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],


        columns: [
            {
                data: 'url',
                render: function (data, type, full, meta) {
                    return meta.row + 1;
                }
            },
            { data: 'nik' },
            {
                data: 'url',
                render: function (data, type, row) {
                    return row.firstName + ' ' + row.lastName;
                }
            },
            { data: 'birthDate' },
            {
                data: 'url',
                render: function (data, type, row) {
                    if (row.gender == 0) {
                        return "Wanita";
                    } else {
                        return "Pria";
                    }
                }
            },
            { data: 'hiringDate' },
            { data: 'email' },
            { data: 'phoneNumber' },
            {
                data: '',
                render: function (data, type, row) {
                    return `<button onclick="ShowUpdate('${row.guid}')" data-bs-toggle="modal" data-bs-target="#modalUpdateEmployee" class="btn btn-primary"> Update </button>` +
                        `   <button onclick="deleteEmployee('${row.guid}')" class="btn btn-secondary"> Delete </button>`;
                }
            }
        ]
    });
});


$(document).ready(function () {
    $('#employeeTable').DataTable({
        dom: 'Bfrtip',
        buttons: ['colvis',
            { extend: 'copy', exportOptions: { columns: ':visible' } },
            { extend: 'csv', exportOptions: { columns: ':visible' } },
            { extend: 'excel', exportOptions: { columns: ':visible' } },
            { extend: 'pdf', exportOptions: { columns: ':visible' } },
            { extend: 'print', exportOptions: { columns: ':visible' } }
        ]
    });
});




function Insert() {
    var obj = new Object(); //sesuaikan sendiri nama objectnya dan beserta isinya
    //ini ngambil value dari tiap inputan di form nya
    obj.firstName = $("#firstName").val();
    obj.lastName = $("#lastName").val();
    obj.birthDate = $("#birthDate").val();
    obj.gender = parseInt($("#gender").val());
    obj.hiringDate = $("#hiringDate").val();
    obj.email = $("#email").val();
    obj.phoneNumber = $("#phoneNumber").val();
    //isi dari object kalian buat sesuai dengan bentuk object yang akan di post
    $.ajax({
        /*url : "https://localhost:7124/api/employees",
        type: "POST",
        headers: {
            'Content-Type':'application/json'
        },
        data: obj*/
        url: "https://localhost:7124/api/employees",
        type: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(obj)
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


document.addEventListener('DOMContentLoaded', function () {
    // Mendapatkan data dari API
    $.ajax({
        url: "https://localhost:7124/api/employees", // Sesuaikan URL sesuai dengan endpoint API Anda
        type: "GET",
        dataType: "json"
    }).done(res => {
        //Mendapatkan jumlah jenis kelamin
        let femaleCount = 0;
        let maleCount = 0;
        for (let i = 0; i < res.data.length; i++) {
            if (res.data[i].gender === 0) {
                femaleCount++;
            } else if (res.data[i].gender === 1) {
                maleCount++;
            }
        }

        //Menghitung total data
        let totalCount = femaleCount + maleCount;

        //Menghitung persentase jenis kelamin
        let femalePercentage = (femaleCount / totalCount) * 100;
        let malePercentage = (maleCount / totalCount) * 100;

        let chart = Highcharts.chart('Gender-Chart', {

            chart: {
                type: 'pie'
            },

            title:
            {
                text: 'Employee Gender Distribitution 2023',
                align: 'left'
            },

            series: [{
                name: 'Gender',
                data: [{
                    name: 'Male',
                    y: malePercentage
                },
                {
                    name: 'Female',
                    y: femalePercentage

                }]
            }]
        });

    });

});
$(document).ready(function ageChart() {
    // Memuat data menggunakan Ajax
    $.ajax({
        url: "https://localhost:7124/api/employees/"
    }).done((result) => {
        // Process the fetched employee data here

        const currentDate = new Date(); // Current date
        const ageCounts = {};

        result.data.forEach(employee => {
            const birthDate = new Date(employee.birthDate);
            const age = currentDate.getFullYear() - birthDate.getFullYear();

            // Counting the occurrences of each age
            if (ageCounts[age]) {
                ageCounts[age]++;
            } else {
                ageCounts[age] = 1;
            }
        });

        var xValues = Object.keys(ageCounts); // Get unique ages
        var yValues = Object.values(ageCounts); // Get counts for each age
        var barColors = "#b91d47";

        new Chart("umur-chart", {
            type: "bar",
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: barColors,
                    data: yValues
                }]
            },
            options: {
                title: {
                    display: true,
                    text: "Employee Age Distribution"
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: "Age"
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: "Count"
                        }
                    }
                }
            }
        });
    }).fail((xhr, status, error) => {
        console.error("Error fetching employee data:", error);
    });
});
