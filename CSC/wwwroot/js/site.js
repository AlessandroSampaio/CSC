$('#btnUserLogonInfo').on('click', function () {
    $.ajax({
        url: '/Home/SessionTime',
        dataType: 'Json',
        async : true,
        cache: false
    }).done(function (result) {
        $('#UserLogonInfo').html('Logado a ' + result + ' minutos');
    })
});