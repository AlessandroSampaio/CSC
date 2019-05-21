$(document).ready(function () {

    var tableClientes = $('#TbClientes').DataTable({
        dom: '<"top"B>fp',
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
            { "data": "telefone" },
            { "data": "SituacaoCadastro" },
            { "data": "Id" }
        ],
        autoWidth: true,
        columnDefs:
            [
                {
                    targets: 4,
                    data: "SitucaoCadastro",
                    render: function (data) {
                        if (data == "Ativo") {
                            return '<div class="badge badge-success">Ativo</div>'
                        }
                        return '<div class="badge badge-danger">Inativo</div>'
                    }
                },
                {
                    targets: 5,
                    data: "Id",
                    "render": function (data) {
                        return '<div class="btn-group btn-group-justified">' +
                            '<a class="btn btn-primary" title="Editar" href="Clientes\\Editar\\' + data + '"><i class="fas fa-pen"></i></a>' +
                            '<a class="btn btn-primary inventario" title="Inventário de Licenças" href="Clientes\\Inventario\\' + data + '"><i class="fas fa-clipboard-list"></i></a>' +
                            '<button class="btn btn-primary inativar" title="Inativar"><i class="fas fa-times"></i></button>' +
                            '</div > ';
                    },
                    searchable: false,
                    orderable: false
                }
            ]
    });

    $('#TbClientes').on('click', '.inativar', function () {
        var data = tableClientes.row($(this).parents('tr')).data();
        if (data['SituacaoCadastro'] != "Ativo") {

            SWALBloqueio("Cliente já inativo!");
        }
        else {
            SWALInativarFunc(data['Id']);
        }
    });

    $('#TbClientes').on('click', '.inventario', function (evt) {
        let data = tableClientes.row($(this).parents('tr')).data();
        if (data['Mono'] == true) {
            evt.preventDefault();
            SWALBloqueio("Ciente Mono!");
        }
    });

    $('#NovoCliente').on('hidden.bs.modal', function () {
        $('#PessoaOpcao').removeClass('hide');
        $('#FormPessoaJuridica').addClass('hide');
        $('#FormPessoaFisica').addClass('hide');
        $('#NovoCnpj').val('');
        $('#NovoCpf').val('');
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

    setInterval(function () {
        tableClientes.ajax.reload(null, false); 
    }, 30000);

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


function SWALInativarFunc(id) {
    Swal.fire({
        text: 'Deseja realmente inativar o Cliente?',
        type: 'question',
        showCancelButton: true,
        focusConfirm: false
    })
        .then((willDelete) => {
            if (willDelete.value) {
                $.ajax({
                    url: '/Clientes/Inativar',
                    type: 'post',
                    data: { 'id': id },
                    dataType: 'Json',
                    async: true,
                    cache: false,
                    success: function (e) {
                        if (e == true) {
                            return SWALSuccess('Cliente Inativado!')
                        } else {
                            return SWALBloqueio(e);
                        }
                    },
                });
            } else { null }
        });
};