$(document).ready(function () {
    $('#Senha, #Confirm').on('keyup', function () {
        var senha = $('#Senha').val();
        var nsenha = $('#ConfirmSenha').val();
        if (senha && nsenha) {
            if (senha == nsenha) {
                $('#btnRedefinir').prop('disabled', false);
            }
        }
    });
});

