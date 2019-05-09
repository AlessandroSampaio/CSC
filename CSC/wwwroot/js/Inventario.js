$(document).ready(function () {

    var tableInventario = $('#TbInventario').DataTable({
        dom: '<"top"B>',
        buttons: [
            {
                text: '<i class="fas fa-plus"></i>',
                className: 'btn-success',
                action: function (e, dt, button, config) {
                    $('#InsertTable').modal('show');;
                }
            }
        ],
        ajax: {
            url: '/Clientes/InventarioListagem',
            dataSrc: '',
            type: 'post',
            data: { 'id': $('#Id').val() }
        },
        "columns": [
            { "data": "Software" },
            { "data": "Quantidade" }
        ],
        autoWidth: true,
        columnDefs: [
            {
                targets: 2,
                data: $('#Id').val(),
                render: function (data) {
                    return '<button class="btn btn-danger"><i class="fas fa-minus"></i></button>'

                }
            }
        ]
    });

    $('#TbInventario').on('click', 'button', function () {
        let inv = tableInventario.row($(this).parents('tr')).data();
        $.ajax({
            url: '/Clientes/RemoveInventario',
            type: 'post',
            data: inv,
            success: function () {
                tableInventario.ajax.reload();
            }
        });
    });

    $('#InserTableContent').on('submit', function (event) {
        event.preventDefault();
        let obj = {
            id: $('#Id').val(),
            software: $('#SoftwareList option:selected').text(),
            quantidade: $('#Quantidade').val()
        };

        $.ajax({
            url: '/Clientes/AddInventario',
            type: 'post',
            data: obj,
            success: function (e) {
                $('#Quantidade').val("")
                $('#InsertTable').modal('hide');
                tableInventario.ajax.reload();
            },
            fail: function (e) { console.log(e) }
        });


    });
});