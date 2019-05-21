﻿$(document).ready(function () {
    
    var tableFuncionarios = $('#TbFuncionarios').DataTable({
        dom: '<"top"B>fp',
        buttons: [{
            extend: 'collection',
            className: "btn-primary",
            text: 'Exportar',
            buttons:
                [
                    { extend: "excel", className: "btn-block" },
                    { extend: "pdf", className: "btn-block" },
                    { extend: "print", className: "btn-block" }]
        },
        {
            text: 'Novo',
            className: 'btn-primary',
            action: function (e, dt, button, config) {
                window.location.href = '/Funcionarios/Novo/';
            }
        }
        ],
        ajax: {
            url: '/Funcionarios/Listagem',
            dataSrc: ''
        },
        "columns": [
            { "data": "Id" },
            { "data": "Nome" },
            { "data": "Admissao" },
            { "data": "Veiculo" },
            { "data": "Demissao" },
            { "data": "Id" }
        ],
        autoWidth: true,
        columnDefs:
            [
                {
                    targets: 3,
                    data: "Veiculo",
                    render: function (data) {
                        if (data == false) {
                            return ' <i class="fas fa-times" style="color: red;"></i>';
                        } else {
                            return '<i class="fas fa-check" style="color: green;"></i>';
                        }
                    }
                },
                {
                    targets: 4,
                    data: "Demissao",
                    render: function (data) {
                        if (data == null) {
                            return '<div class="badge badge-success">Ativo</div>'
                        }
                        else {
                            return '<div class="badge badge-danger">Inativo</div>'
                        }
                    }
                },
                {
                    targets: 5,
                    data: "Id",
                    render: function (data) {



                        return '<div class="btn-group btn-group-justified">' +
                            '<a class="btn btn-primary" title="Editar" href="Funcionarios\\Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
                            '<button class="btn btn-primary inativar"><i class="fas fa-user-times"></i></button>  ' +
                            '</div>';
                    },
                    searchable: false,
                    orderable: false
                }
            ]
    });

    setInterval(function () {
        tableFuncionarios.ajax.reload(null, false);
    }, 30000);

    $('#TbFuncionarios').on('click', '.inativar', function () {
        var data = tableFuncionarios.row($(this).parents('tr')).data();
        console.log(data);
        if (data['Demissao'] != null) {
            
            SWALBloqueio("Usuario já inativo!");
        }
        else {
            SWALInativarFunc(data['Id']);
            setTimeout(2000);
            tableFuncionarios.ajax.reload();
        }
    });
});


function SWALInativarFunc(id) {
    Swal.fire({
        text: 'Deseja realmente inativar o funcionario?',
        type: 'question',
        showCancelButton: true,
        focusConfirm: false
    })
        .then((willDelete) => {
            console.log(willDelete);
            if (willDelete) {
                $.ajax({
                    url: '/Funcionarios/Inativar',
                    type: 'post',
                    data: { 'id': id },
                    dataType: 'Json',
                    async: true,
                    cache: false,
                    success: function (e) {
                        if (e == true) {
                            return SWALSuccess('Funcionario inativado com sucesso');
                        } else {
                            return SWALBloqueio(e);
                        }
                    },
                });
            } else { null }
        });
};