$(document).ready(function () {
    var AtdTransfer;

    var tableAtendimentos = $('#TbAtendimentos').DataTable({
        dom: '<"top"Bf><"botton"p>',
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
                    targets: 5,
                    data: "Id",
                    "render": function (data) {
                        return '<div class="btn-group btn-group-justified">' +
                            '<a class="btn btn-primary editar" title="Editar" href="\Atendimentos\\Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
                            '<button class="btn btn-primary transferir" title="Transferir Atendimento"><i class="fas fa-angle-double-right"></i></button>' +
                            '<button class="btn btn-primary finalizar" title="Encerrar Atendimento"><i class="fas fa-check"></i></button></div>';
                    },
                    searchable: false,
                    orderable: false
                }

            ],
        order: [[4, 'asc'], [3, 'asc']]

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
            { "data": "SituacaoCadastro" }
        ],
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
        if (data["Mono"] == true) {
            SWALBloqueio("Cliente Mono, impossível abrir atendimento");
        } else {
            if (data["SituacaoCadastro"] == "Ativo") {
                var url = "/Atendimentos/Novo?ClienteId=" + data['Id'];
                window.location.href = url;
            }
            else {
                SWALBloqueio("Cliente Inativo, impossível abrir atendimento!");
            }
        }
    });

    $('#TbAtendimentos').on('dblclick', 'tr', function () {
        var data = tableAtendimentos.row(this).data();
        if (data['Status'] != 'Aberto') {
            SWALBloqueio('Não é possível editar um atendimento ' + data['Status']);
        } else {
            var url = "/Atendimentos/Editar/" + data['Id'];
            window.location.href = url;
        }
    });

    $('#TbAtendimentos').on('click', '.transferir', function () {
        var data = tableAtendimentos.row($(this).parents('tr')).data();
        var userLogado = $('#userLogado').val();
        if (data["FuncionarioId"] == userLogado) {
            if (data['Status'] != 'Aberto') {
                SWALBloqueio('Impossivel transferir atendimento, atendimento já ' + data['Status'] + '!');
            } else {
                AtdTransfer = data;
                $('#FuncionarioOrigem').val(data["Funcionario"]["Nome"]);
                $("#funcDestino option[value='" + data["FuncionarioId"] + "']").remove();
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
        console.log(FuncionarioDestino);
        console.log(AtdTransfer);
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
                $("#funcDestino").append('<option value="' + AtdTransfer['FuncionarioId'] + '">' + AtdTransfer['Funcionario']['Nome'] + '</option>');
                $('body').css('cursor', 'default');
                AtdTransfer = null;
                SWALSuccess("Transferido com sucesso!");
                $('#TransferirAtendimento').modal('hide');
                tableAtendimentos.ajax.reload();
            }
        });
    });

    $('#TransferirAtendimento').on("hidden.bs.modal", function () {
        $("#funcDestino").append('<option value="' + AtdTransfer['FuncionarioId'] + '">' + AtdTransfer['Funcionario']['Nome'] + '</option>');
    });

    $('#ClientSelect').on("shown.bs.modal", function () { $('div.dataTables_filter input').focus(); });

    $('#ClientSelect').on("hidden.bs.modal", function () { $('#TbAtendimentos_filter input').focus(); });

    $('#TbAtendimentos_filter input').focus();

    setInterval(function () {
        tableClientes.ajax.reload(null, false);
        tableAtendimentos.ajax.reload(null, false);
    }, 30000);
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
                }
            });
        }
    });
}