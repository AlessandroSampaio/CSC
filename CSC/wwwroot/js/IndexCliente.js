$(function () {
    listFuncionarios();

    $('#btnPessoaJuridica').on('click',function () {
        $('#PessoaOpcao').addClass('hide');
        $('#FormPessoaJuridica').removeClass('hide');
    });

    $('#NovoCliente').on('hidden.bs.modal', function () {
        $('#PessoaOpcao').removeClass('hide');
        $('#FormPessoaJuridica').addClass('hide');
    });

    $('#NovoCnpj').mask('00.000.000/0000-00');

    $('#btnPesquisar').on('click', function () {
        var filtro = $('#nameSearch').val();
        listFuncionarios(filtro);
    })
})


function listFuncionarios(filtro) {

    $.ajax({
        url: '/Clientes/Listagem',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html",
        data: {_name : filtro}
    })
        .done(function (result) {
            $('#content').html(result);
        }).fail(function (xhr) {
            console.log('error :' + xhr.statusText);
        })
}

