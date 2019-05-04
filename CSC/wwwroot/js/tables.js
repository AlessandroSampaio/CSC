$(document).ready(function () {

    //Nova Tabela de Funcionarios
    var tableFuncionarios = $('#TbFuncionarios').DataTable({
        dom: '<"top"B>',
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
                    data: "Id",
                    "render": function (data) {
                        return '<a href="Editar\\' + data + '"><i class="fas fa-pen"></i></a>';
                    },
                    searchable: false,
                    orderable: false
                }
            ]
    });

    //Nova DataTableUser usando Ajax
    var tableUser = $('#TbUsuarios').DataTable({
        dom: '<"top"B>',
        buttons: [{
            extend: 'collection',
            className: "btn-primary",
            text: 'Exportar',
            buttons:
                [
                    { extend: "excel", className: "btn-block" },
                    { extend: "pdf", className: "btn-block" },
                    { extend: "print", className: "btn-block" }],
        },
        {
            text: 'Novo',
            className: 'btn-primary',
            action: function (e, dt, button, config) {
                window.location.href = '/Usuarios/Novo/';
            }
        }
        ],
        ajax: {
            url: '/Usuarios/Listagem',
            dataSrc: ''
        },

        "columns": [
            { "data": "Id" },
            { "data": "NomeLogon" },
            { "data": "Funcionario.Nome" },
            { "data": "Funcionario.Admissao" },
            { "data": "Funcionario.Demissao" }
        ],
        autoWidth: true,
        columnDefs:
            [
                {
                    "targets": 5,
                    "data": null,
                    "defaultContent": '<button class="btn my-2 my-sm-0 open-modal" type="button" data-toggle="modal" data-target="#UserForm"><i class= "fas fa-user-edit" ></i></button><button class="btn my-2 my-sm-0 open-modal" type="button" data-toggle="modal" data-target="#PasswordForm"><i class="fas fa-key"></i></button>',
                    order: false
                }
            ]
    });

    $('#TbUsuarios').on('click', 'button', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        $("input[name = id]").val(data["id"]);
    });

    $('#UserForm').on('hidden.bs.modal', function () {
        tableUser.ajax.reload();
    })

    var tableClientes = $('#TbClientes').DataTable({
        dom: '<"top"B>',
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
                $('#NovoCliente').modal('show');
            }
        }
        ],
        ajax: {
            url: '/Clientes/Listagem',
            dataSrc: ''
        },
        "columns": [
            { "data": "Id" },
            { "data": "cnpj" },
            { "data": "nome" },
            { "data": "SituacaoCadastro" },
            { "data": "Id" }
        ],
        autoWidth: true,
        columnDefs:
            [
                {
                    targets: 3,
                    data: "SitucaoCadastro",
                    render: function (data) {
                        if (data == "Ativo") {
                            return '<div class="badge badge-success">Ativo</div>'
                        }
                        return '<div class="badge badge-danger">Inativo</div>'
                    }
                },
                {
                    targets: 4,
                    data: "Id",
                    "render": function (data) {
                        return '<a href="Editar\\' + data + '"><i class="fas fa-pen"></i></a>';
                    },
                    searchable: false,
                    orderable: false
                }
            ]
    });

    $('#NovoCliente').on('hidden.bs.modal', function () {
        tableClientes.ajax.reload();
    });
});