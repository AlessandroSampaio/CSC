$(document).ready(function () {
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
});