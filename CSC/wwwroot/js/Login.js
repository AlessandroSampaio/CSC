$(document).ready(function () {
    $('#forgotPsw').on('click', function () {
        SWALForgot();
    })
});

function SWALForgot() {
    Swal.fire({
        title: "Recuperação de Senha",
        text: "Digite o e-mail :",
        input: "text"
    }).then(email => {
        if (email.value != null && email.value != "") {
            $.ajax({
                url: 'Usuarios/SendToken',
                type: 'POST',
                async: true,
                cache: false,
                data: { email: email.value },
                dataType: 'JSON',
                statusCode: {
                    500: function () { SWALBloqueio("Houve um erro, favor contatar o Supervisor/Administrador") }
                },
                success: function (result) {
                    SWALSuccess(result);
                }
            });
        }
    });
}