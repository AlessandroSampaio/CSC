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
            { "data": "Conclusao" },
            { "data": "Conclusao" }
        ],
        columnDefs:
            [
                {
                    targets: 5,
                    data: "Conclusao",
                    render: function (data) {
                        if (data == "" || data == null) {
                            return '<div class="badge badge-success">Aberto</div>'
                        }
                        return '<div class="badge badge-danger">Fechado</div>'
                    }
                },
                {
                    targets: 6,
                    data: "Id",
                    render: function (data) {
                        return '<div class="btn-group btn-group-justified">' +
                            '<a class="btn btn-primary " title="Editar" href="Tarefas\\Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
                            '<button data-id="' + data +'" title="Concluir Tarefa" class="btn btn-primary concluirTarefa"><i class="fas fa-times"></i></button>' +
                            '</div>';
                    }
                }
            ]
    });

    $('#TbTarefas tbody').on('dblclick', 'tr', function () {
        var data = tableTarefas.row(this).data();
        if (data['Conclusao'] == 'null') {
            SWALBloqueio('Não é possível editar uma tarefa já encerrada');
        } else {
            var url = "/Tarefas/Editar/" + data['Id'];
            window.location.href = url;
        }
    });

    $('#TbTarefas').on('click', '.concluirTarefa', function () {
        var id = tableTarefas.row($(this).parents('tr')).data();
        SWALConcluirTarefa(id['Id']);
    });

});

function SWALConcluirTarefa(Id) {
    Swal.fire({
        type: 'question',
        title: 'Concluir Tarefa',
        text: 'Todos os atendimentos vinculados serão encerrados em conjunto. Tem certeza que deseja concluir esta tarefa?',
        showCancelButton: true,
        focusConfirm: false,
        focusCancel: true
    }).then(willFinish => {
        if (willFinish.value == true) {
            $.ajax({
                url: '/Tarefas/Concluir',
                type: 'post',
                data: { 'id': Id },
                dataType: 'Json',
                success: function (result) {
                    if (result == true) {
                        SWALSuccess("Tarefa encerrada com sucesso!");
                        $('#TbTarefas').DataTable().ajax.reload();
                    } else {
                        SWALBloqueio(result);
                    }
                }
            });
        }
    });
}