$(function () {
    listFuncionarios();

    $('#btnPesquisar').on('click', function () {
        var filtro = $('#nameSearch').val();
        listFuncionarios(filtro);
    })
})


function listFuncionarios(filtro) {

    $.ajax({
        url: '/Funcionarios/Listagem',
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

