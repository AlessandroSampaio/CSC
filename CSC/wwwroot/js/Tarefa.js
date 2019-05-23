$(document).ready(function () {
    var tableTarefas = $('#TbTarefas').DataTable({
        dom: '<"top"Bft>',
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
                window.location.href = '/Tarefas/Novo/';
            }
        }
        ],
        ajax: {
            url: '/Tarefas/Listagem',
            dataSrc: '',
            type: 'post'
        },
        "columns": [
            { "data": "Id" },
            { "data": "TarefaNumero" },
            { "data": "Atendimentos[0].Abertura" },
            { "data": "Conclusao" }
        ],
        columnDefs:
            [
                {
                    targets: 3,
                    data: "Conclusao",
                    render: function (data) {
                        if (data == "" || data == null) {
                            return '<div class="badge badge-success">Ativo</div>'
                        }
                        return '<div class="badge badge-danger">Inativo</div>'
                    }
                }
            ]
    });
});