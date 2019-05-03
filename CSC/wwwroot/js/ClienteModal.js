$(document).ready(function(){
    $('#btnPessoaJuridica').on('click', function () {
        $('#PessoaOpcao').addClass('hide');
        $('#FormPessoaJuridica').removeClass('hide');
    });

    $('#NovoCliente').on('hidden.bs.modal', function () {
        $('#PessoaOpcao').removeClass('hide');
        $('#FormPessoaJuridica').addClass('hide');
        $('#NovoCnpj').val('');
    });

    $('#NovoCnpj').mask('00.000.000/0000-00');

});