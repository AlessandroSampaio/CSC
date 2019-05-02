$(document).ready(function () {
    $(document).on("click", ".open-modal", function (e) {
       // $("input[name = id]").val($(this).data('id'));
        $("#NovoUser").val("");
        $("#NovaSenha").val("");
    });
    $("#btnSalvarSenha").on("click", function () {
        var filtro = {
            Id: $("#PswID").val(),
            Senha: $("#NovaSenha").val()
        };
        SalvarSenha(filtro);
    });
    function SalvarSenha(filtro) {
        $.ajax({
            url: '/Usuarios/AlterarSenha',
            type: 'POST',
            cache: false,
            async: true,
            dataType: "Json",
            data: filtro
        })
            .done(function () {
                $('#PasswordForm').modal('hide');
            })
    }
    $("#btnSalvarUser").on("click", function () {
        var filtro = {
            Id: $("#UserID").val(),
            NomeLogon: $("#NovoUser").val()
        };
        SalvarUser(filtro);

    });
    function SalvarUser(filtro) {
        $.ajax({
            url: '/Usuarios/AlterarNomeLogon',
            type: 'POST',
            cache: false,
            async: true,
            dataType: "Json",
            data: filtro
        })
            .done(function () {
                $('#UserForm').modal('hide');
            })
    }
});
