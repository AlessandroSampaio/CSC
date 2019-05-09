$(document).ready(function () {

    var tableClientes = $('#TbClientes').DataTable({
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
                $('#NovoCliente').modal('show');
            }
        }
        ],
        ajax: {
            url: '/Clientes/Listagem',
            dataSrc: ''
        },
        "columns": [
            { "data": "Id" },
            { "data": "cnpj" },
            { "data": "nome" },
            { "data": "SituacaoCadastro" },
            { "data": "Id" }
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
                },
                {
                    targets: 4,
                    data: "Id",
                    "render": function (data) {
                        return '<a href="Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
                            '<a href="Inventario\\' + data + '"><i class="fas fa-clipboard-list"></i></a>';
                    },
                    searchable: false,
                    orderable: false
                }
            ]
    });

    $('#NovoCliente').on('hidden.bs.modal', function () {
        tableClientes.ajax.reload();
    });

    $('#btnPessoaJuridica').on('click', function () {
        $('#PessoaOpcao').addClass('hide');
        $('#FormPessoaJuridica').removeClass('hide');
    });

    $('#btnPessoaFisica').on('click', function () {
        $('#PessoaOpcao').addClass('hide');
        $('#FormPessoaFisica').removeClass('hide');
    });

    $('#NovoCliente').on('hidden.bs.modal', function () {
        $('#PessoaOpcao').removeClass('hide');
        $('#FormPessoaJuridica').addClass('hide');
        $('#FormPessoaFisica').addClass('hide');
        $('#NovoCnpj').val('');
        $('#NovoCpf').val('');
    });

    $('#NovoCpf').mask('000.000.000-00');

    $('#NovoCnpj').mask('00.000.000/0000-00');

    $('#FormPessoaFisica').submit(function (event) {
        var cpf = $('#NovoCpf').val();
        if (!valida_cpf(cpf)) {
            event.preventDefault();
            $("#ValidPF").text("Cpf Inválido!").show().fadeOut(3000);
        } else {
            var consulta = ConsultaCliente(cpf);
            if (consulta == true) {
                event.preventDefault();
                $("#ValidPF").text("Cpf já cadastrado!").show().fadeOut(5000);
            }
        }
    });

    $('#FormPessoaJuridica').submit(function (event) {
        var cnpj = $('#NovoCnpj').val();
        if (!valida_cnpj(cnpj)) {
            event.preventDefault();
            $("#ValidPJ").text("Cnpj Inválido!").show().fadeOut(5000);
        } else {
            var consulta = ConsultaCliente(cnpj);
            if (consulta == true) {
                event.preventDefault();
                $("#ValidPJ").text("Cnpj já cadastrado!").show().fadeOut(5000);
            }
        }
    });

    function ConsultaCliente(doc) {
        var validacao = null;
        $.ajax({
            url: '/Clientes/ConsultaCliente',
            async: false,
            cache: false,
            data: { _doc: doc },
            dataType: 'Json',
            type: 'POST',
            success: function (result) {
                validacao = result;
            }
        });
        return validacao;
    }
});