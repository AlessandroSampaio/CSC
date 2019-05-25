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
            { "data": "Descricao"},
            { "data": "Abertura" },
            { "data": "Conclusao" }
        ],
        columnDefs:
            [
                {
                    targets: 4,
                    data: "Conclusao",
                    render: function (data) {
                        if (data == "" || data == null) {
                            return '<div class="badge badge-success">Aberto</div>'
                        }
                        return '<div class="badge badge-danger">Fechado</div>'
                    }
                }
            ]
    });

    $('#TbTarefas tbody').on('dblclick', 'tr', function () {
        var data = tableTarefas.row(this).data();
        console.log(data);
        if (data['Conclusao'] == 'null') {
            SWALBloqueio('Não é possível editar uma tarefa já encerrada');
        } else {
            var url = "/Tarefas/Editar/" + data['Id'];
            window.location.href = url;
        }
    });
});