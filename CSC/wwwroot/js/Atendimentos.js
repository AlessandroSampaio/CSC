$(document).ready(function(){
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
});