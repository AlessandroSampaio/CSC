$(document).ready(function () {
    var tableTarefas = $('#TbTarefas').DataTable({
        dom: '<"top"t>',
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
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        },
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
                            '<a class="btn btn-primary " title="Editar" href="Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
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

    $('#txtSearch').on('keyup', function () {
        tableTarefas.columns($('#slctOption').val()).search(this.value).draw();
    })
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