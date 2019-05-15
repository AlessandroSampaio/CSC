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
                                return '<div class="badge badge-warning">Transferido</div>'
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
                            '<a class="btn btn-primary" title="Editar" href="Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
                            '<button class="btn btn-primary transferir" title="Transferir Atendimento"><i class="fas fa-angle-double-right"></i></button>' +
                            '<button class="btn btn-primary finalizar" title="Encerrar Atendimento"><i class="fas fa-check"></i></button></div>';
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
        var url = "/Atendimentos/Novo?ClienteId=" + data['Id'];
        window.location.href = url;
    });

    $('#TbAtendimentos').on('click', '.transferir', function () {
        var data = tableAtendimentos.row($(this).parents('tr')).data();
        console.log(data);
        if (data['Status'] != 'Aberto') {
            alert('Impossivel transferir esse atendimento!');
        } else {
            AtdTransfer = data;
            $('#FuncionarioOrigem').val(data["Funcionario"]["Nome"]);
            $("#funcDestino option[value='" + data["FuncionarioId"] + "']").remove();
            $('#TransferirAtendimento').modal('show');
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
                alert("Transferido com sucesso!");
                $('#TransferirAtendimento').modal('hide');
                tableAtendimentos.ajax.reload();
            }
        });
    });

    $('#TransferirAtendimento').on("hidden.bs.modal", function () {
        $("#funcDestino").append('<option value="' + AtdTransfer['FuncionarioId'] + '">' + AtdTransfer['Funcionario']['Nome'] + '</option>');
    });
});