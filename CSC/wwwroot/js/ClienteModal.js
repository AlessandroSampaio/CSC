$(document).ready(function(){
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