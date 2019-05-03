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
        alert('teste');
        var cpf = $('#NovoCpf').val();
        if (!valida_cpf(cpf)) {
            event.preventDefault();
            $("span").text("Cpf Inválido!").show().fadeOut(3000);
        } else {
            return;
        }
    });

});