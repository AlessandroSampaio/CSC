$(document).ready(function () {
    var AtdTransfer;

    var tableAtendimentos = $('#TbAtendimentos').DataTable({
        dom: "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'li><'col-sm-12 col-md-7'p>>",
        ajax: {
            url: '/Atendimentos/Listagem',
            dataSrc: ''
        },
        columns: [
            { "data": "Id" },
            { "data": "User.Nome" },
            { "data": "Cliente.nome" },
            { "data": "Cliente.fantasia" },
            { "data": "Abertura" },
            { "data": "Status" },
            { "data": "Id" }
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
        autoWidth: true,
        columnDefs:
            [
                {
                    targets: 5,
                    data: "Status",
                    render: function (data) {
                        if (data == "Aberto") {
                            return '<div class="badge badge-success">Aberto</div>'
                        } else {
                            if (data == "Transferido") {
                                return '<div class="badge badge-info">Transferido</div>'
                            } else {
                                return '<div class="badge badge-danger">Fechado</div>'
                            }
                        }
                    }
                },
                {
                    targets: 6,
                    data: "Id",
                    "render": function (data) {
                        return '<div class="btn-group btn-group-justified">' +
                            '<a class="btn btn-primary editar" title="Editar" href="Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
                            '<button class="btn btn-primary transferir" title="Transferir Atendimento"><i class="fas fa-angle-double-right"></i></button>' +
                            '<button class="btn btn-primary finalizar" title="Encerrar Atendimento"><i class="fas fa-check"></i></button></div>';
                    },
                    searchable: false,
                    orderable: false
                }

            ],
        order: [[5, 'asc'], [4, 'asc']]

    });

    var tableClientes = $('#TbClientes').DataTable({
        dom: 'ftp',
        ajax: {
            url: '/Clientes/Listagem',
            dataSrc: ''
        },
        "columns": [
            { "data": "Id" },
            { "data": "cnpj" },
            { "data": "nome" },
            { "data": "fantasia" },
            { "data": "bairro" },
            { "data": "SituacaoCadastro" }
        ],
        columnDefs:
            [
                {
                    targets: 5,
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
        if (data["SituacaoCadastro"] == "Ativo") {
            var url = "/Atendimentos/Novo?ClienteId=" + data['Id'];
            window.location.href = url;
        }
        else {
            SWALBloqueio("Cliente Inativo, impossível abrir atendimento!");
        }
    });

    $('#TbAtendimentos').on('dblclick', 'tr', function () {
        var data = tableAtendimentos.row(this).data();
        if (data['Status'] != 'Aberto') {
            SWALInfoAtendimento(data);
        } else {
            var url = "/Atendimentos/Editar/" + data['Id'];
            window.location.href = url;
        }
    });

    $('#TbAtendimentos').on('click', '.transferir', function () {
        var data = tableAtendimentos.row($(this).parents('tr')).data();
        var userLogado = $('#UserLoggedIn').html();
        if (data["User"]["UserName"] == userLogado) {
            if (data['Status'] != 'Aberto') {
                SWALBloqueio('Impossivel transferir atendimento, atendimento já ' + data['Status'] + '!');
            } else {
                AtdTransfer = data;
                $('#FuncionarioOrigem').val(data["User"]["Nome"]);
                $("#funcDestino option[value='" + data["UserId"] + "']").remove();
                $('#TransferirAtendimento').modal('show');
            }
        } else {
            SWALBloqueio("Você não tem permissão para realizar esta operação!")
        }
    });

    $('#TbAtendimentos').on('click', '.finalizar', function () {
        var data = tableAtendimentos.row($(this).parents('tr')).data();
        if (data['TarefaId'] != null) {
            SWALBloqueio('Não é possível encerrar atendimento com tarefa em aberto!');
        } else {
            if (data['Status'] != 'Aberto') {
                SWALBloqueio('Impossivel encerrar atendimento, atendimento já ' + data['Status'] + '!');
            } else {
                SWALEncerrar(data['Id']);
            }
        }
    });

    $('#TbAtendimentos').on('click', '.editar', function (evt) {
        var data = tableAtendimentos.row($(this).parents('tr')).data();
        if (data['Status'] != 'Aberto') {
            evt.preventDefault();
            SWALBloqueio('Não é possível editar um atendimento ' + data['Status']);
        }
    });

    $('#btnTransferir').on('click', function () {
        $('body').css('cursor', 'progress');
        var FuncionarioDestino = $('#funcDestino').val();
        $.ajax({
            url: '/Atendimentos/TransferirAtendimento',
            type: 'post',
            async: true,
            cache: false,
            data: {
                'atdId': AtdTransfer['Id'],
                'funcionarioDestino': FuncionarioDestino
            },
            dataType: 'Json',
            success: function () {
                $("#funcDestino").append('<option value="' + AtdTransfer['UserId'] + '">' + AtdTransfer['User']['Nome'] + '</option>');
                $('body').css('cursor', 'default');
                AtdTransfer = null;
                SWALSuccess("Transferido com sucesso!");
                $('#TransferirAtendimento').modal('hide');
                tableAtendimentos.ajax.reload();
            }
        });
    });

    $('#TransferirAtendimento').on("hidden.bs.modal", function () {
        if (AtdTransfer != null) {
            $("#funcDestino").append('<option value="' + AtdTransfer['FuncionarioId'] + '">' + AtdTransfer['Funcionario']['Nome'] + '</option>');
        }
    });

    $('#ClientSelect').on("shown.bs.modal", function () { $('div.dataTables_filter input').focus(); });

    $('#ClientSelect').on("hidden.bs.modal", function () { $('#TbAtendimentos_filter input').focus(); });

    $('#TbAtendimentos_filter input').focus();

    $('#btnNovo').click(function () {
        tableClientes.ajax.reload();
        $('#ClientSelect').modal('show');
    })

    $('#slctOption').on('change', '', function () {
        tableAtendimentos.search('').columns().search('').draw();
        tableAtendimentos.columns($("option:selected", this).val()).search($('#txtSearch').val()).draw();
        $('#txtSearch').focus();
    });

    $('#txtSearch').on('keyup', function () {
        tableAtendimentos.columns($('#slctOption').val()).search(this.value).draw();
    })
});

function SWALEncerrar(Id) {
    Swal.fire({
        title: 'Encerrar',
        text: 'Detalhes adicionais',
        input: 'textarea',
        confirmButtonText: 'Encerrar',
        confirmButtonColor: '#fc544b',
        focusConfirm: false
    }).then(detalhes => {
        if (detalhes.value != null) {
            $.ajax({
                url: '/Atendimentos/EncerrarAtendimento',
                type: 'post',
                async: true,
                cache: false,
                data: {
                    'atdId': Id,
                    'detalhes': detalhes.value
                },
                dataType: 'Json',
                success: function (e) {
                    SWALSuccess("Encerrado com sucesso!");
                    $('#TbAtendimentos').DataTable().ajax.reload();
                }
            });
        }
    });
}

function SWALInfoAtendimento(data) {
    Swal.fire({
        title: data["AtendimentoTipo"],
        html: '<strong>Detalhes</strong>: ' + data["Detalhes"]

    });

}