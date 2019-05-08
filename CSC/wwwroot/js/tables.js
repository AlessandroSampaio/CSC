$(document).ready(function () {
    var tableAtendimentos = $('#TbAtendimentos').DataTable({
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
                window.location.href = '/Atendimentos/Novo/';
            }
        }
        ],
        ajax: {
            url: '/Atendimentos/Listagem',
            dataSrc: ''
        },
        columns: [
            { "data": "Id" },
            { "data": "Funcionario.Nome" },
            { "data": "Cliente.nome" },
            { "data": "Abertura" },
            { "data": "Status" }
        ],
        autoWidth: true

    });

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
                        return '<a href="Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
                            '<a href="Inventario\\' + data + '"><i class="fas fa-clipboard-list"></i></a>';
                    },
                    searchable: false,
                    orderable: false
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

    $('#TbUsuarios').on('click', 'button', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        $("input[name = id]").val(data["Id"]);
    });

    $('#UserForm').on('hidden.bs.modal', function () {
        tableUser.ajax.reload();
    })

    $('#NovoCliente').on('hidden.bs.modal', function () {
        tableClientes.ajax.reload();
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