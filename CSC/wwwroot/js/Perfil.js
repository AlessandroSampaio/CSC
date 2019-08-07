$(document).ready(function () {
    $('#btnAlterarSenha').on('click', function () {
        SWALAlterarSenha($('#UserLoggedIn').html());
    });
});

function SWALAlterarSenha(username) {
    Swal.mixin({
        input: 'password',
        progressSteps: ['1', '2']
    }).queue([{
        title: "Alterar Senha",
        text: "Digite a nova senha :",
    }, {
        title: "Alterar Senha",
        text: "Confirme a nova senha :",
        }]).then((result) => {
            if (result.value[0] == result.value[1]) {
                var filtro = {
                    userName: username,
                    Password: result.value[1]
                };
                $.ajax({
                    url: '/Usuarios/AlterarSenha',
                    type: 'POST',
                    cache: false,
                    async: true,
                    dataType: "Json",
                    data: filtro,
                    statusCode: {
                        403: function () { SWALBloqueio("Você não tem as permissões necessárias!") },
                        404: function () { SWALBloqueio("Page not Found") }
                    },
                    success: function (e) {
                        if (e == true) {
                            $('#TbUsuarios').DataTable().ajax.reload();
                            return SWALSuccess('Senha alterada com sucesso!');
                        } else {
                            return SWALBloqueio(e);
                        }
                    },
                });
            } else {
                SWALBloqueio("As senhas não conferem!")
            }
        })
    };