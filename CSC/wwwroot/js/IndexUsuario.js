$(function () {
    listUsuarios();

    $('#btnPesquisar').on('click', function () {
        var filtro = $('#nameSearch').val();
        listUsuarios(filtro);
    })
})


function listUsuarios(filtro) {

    $.ajax({
        url: '/Usuarios/Listagem',
        type: 'POST',
        cache: false,
        async: true,
        dataType: "html",
        data: { _name: filtro }
    })
        .done(function (result) {
            $('#content').html(result);
        }).fail(function (xhr) {
            console.log('error :' + xhr.statusText);
        })

}

