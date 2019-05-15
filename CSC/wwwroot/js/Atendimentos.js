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
            action: function () {
                $('#ClientSelect').modal('show');
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
            { "data": "Status" },
            { "data": "Id" }
        ],
        autoWidth: true,
        columnDefs:
            [
                {
                    targets: 4,
                    data: "Status",
                    render: function (data) {
                        if (data == "Aberto") {
                            return '<div class="badge badge-success">Aberto</div>'
                        }
                        return '<div class="badge badge-danger">Fechado</div>'
                    }
                },
                {
                    targets: 5,
                    data: "Id",
                    "render": function (data) {
                        return '<a href="Editar\\' + data + '"><i class="fas fa-pen"></i></a>';
                    },
                    searchable: false,
                    orderable: false
                }

            ]

    });

    var tableClientes = $('#TbClientes').DataTable({
        dom: '<"top"f>',
        ajax: {
            url: '/Clientes/Listagem',
            dataSrc: ''
        },
        "columns": [
            { "data": "Id" },
            { "data": "cnpj" },
            { "data": "nome" },
            { "data": "SituacaoCadastro" }
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
                }
            ]
    });

    $('#TbClientes tbody').on('dblclick', 'tr', function () {
        var data = tableClientes.row(this).data();
        console.log(data);
        console.log(data['Id']);
        var url = "/Atendimentos/Novo?ClienteId=" + data['Id'];
        window.location.href = url;
    });
});