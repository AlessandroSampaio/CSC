$(document).ready(function () {
    var tableUser = $('#TbUsuarios').DataTable({
        dom: '<"top"t>',
        ajax: {
            url: '/Usuarios/Listagem',
            dataSrc: ''
        },

        "columns": [
            { "data": "UserId" },
            { "data": "UserName" },
            { "data": "Nome" },
            { "data": "Admissao" }
        ],
        autoWidth: true,
        columnDefs:
            [
                {
                    "targets": 4,
                    "data": null,
                    "defaultContent": '<div class="btn-group btn-group-justified">' +
                        '<button class="btn btn-primary userLogon" title="Alterar Logon" type="button"><i class="fas fa-user-edit"></i></button>' +
                        '<button class="btn btn-primary userSenha" title="Alterar Senha" type="button" ><i class="fas fa-key"></i></button>' +
                        '<button class="btn btn-primary userConfirmEmail" title="Alterar Senha" type="button" ><i class="fas fa-envelope"></i></button>' +
                        '</div>',
                    order: false
                }
            ]
    });

    $('#TbUsuarios').on('click', '.userLogon', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        SWALAlterarUser(data["UserName"]);
    });

    $('#TbUsuarios').on('click', '.userSenha', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        SWALAlterarSenha(data["UserName"]);
    });

    $('#TbUsuarios').on('click', '.userConfirmEmail', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        SWALReenvioEmail(data["UserName"]);
    });

    $('#txtSearch').on('keyup', function () {
        tableUser.columns($('#slctOption').val()).search(this.value).draw();
    })
});

function SWALAlterarUser(username) {
    Swal.fire({
        title: "Alterar logon:",
        text: "Digite o novo logon :",
        input: 'text',
        confirmButtonText: 'Alterar'
    }).then(novoLogon => {
        if (novoLogon.value != null) {
            if (novoLogon.value != '') {
                var filtro = {
                    userName: username,
                    newUserName: novoLogon.value
                };
                $.ajax({
                    url: '/Usuarios/AlterarUserName',
                    type: 'POST',
                    cache: false,
                    async: true,
                    dataType: "Json",
                    data: filtro,
                    success: function (e) {
                        if (e == true) {
                            $('#TbUsuarios').DataTable().ajax.reload();
                            return SWALSuccess('Logon alterado com sucesso!');
                        } else {
                            return SWALBloqueio(e);
                        }
                    },
                });
            } else {
                SWALBloqueio("Novo login não pode ser vazio!");
            }
        }
    });
}

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
}

function SWALReenvioEmail(username) {
    Swal.fire({
        title: "Reenviar e-mail",
        text: "Utilizar o e-mail do cadastro?",
        showCloseButton: true,
        showCancelButton: true,
        focusConfirm: false,
        cancelButtonText: "Alterar e-mail?"
    }).then(result => {
        if (result.value == true) {
            let filtro = {
                userName : username,
                email : null
            };
            $.ajax({
                url: '/Usuarios/ReenviarEmail',
                type: 'POST',
                cache: false,
                async: true,
                dataType: "Json",
                data: filtro,
                success: function (e) {
                    if (e != false) {
                        return SWALSuccess(e);
                    } else {
                        return SWALBloqueio(e);
                    }
                }
            });
        } else {
            Swal.fire({
                title: "Reenviar Email",
                text: "Digite o novo email :",
                input: 'text',
                confirmButtonText: 'Enviar'
            }).then(novoEmail => {
                if (novoEmail.value != null && novoEmail.value != '') {
                    let filtro = {
                        userName : username,
                        email : novoEmail.value
                    };
                    $.ajax({
                        url: '/Usuarios/ReenviarEmail',
                        type: 'POST',
                        cache: false,
                        async: true,
                        dataType: "Json",
                        data: filtro,
                        success: function (e) {
                            if (e != false) {
                                return SWALSuccess(e);
                            } else {
                                return SWALBloqueio(e);
                            }
                        }
                    });
                }
            });
        }
    });
};
