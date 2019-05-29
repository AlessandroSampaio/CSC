$(document).ready(function () {
    var tableUser = $('#TbUsuarios').DataTable({
        dom: '<"top"B>',
        buttons: [{
            extend: 'collection',
            className: "btn-primary",
            text: 'Exportar',
            buttons:
                [
                    { extend: "excel", className: "btn-block" },
                    { extend: "pdf", className: "btn-block" },
                    { extend: "print", className: "btn-block" }],
        },
        {
            text: 'Novo',
            className: 'btn-primary',
            action: function (e, dt, button, config) {
                window.location.href = '/Usuarios/Novo/';
            }
        }
        ],
        ajax: {
            url: '/Usuarios/Listagem',
            dataSrc: ''
        },

        "columns": [
            { "data": "Id" },
            { "data": "NomeLogon" },
            { "data": "Funcionario.Nome" },
            { "data": "Funcionario.Admissao" }
        ],
        autoWidth: true,
        columnDefs:
            [
                {
                    "targets": 4,
                    "data": null,
                    "defaultContent": '<div class="btn-group btn-group-justified"><button class="btn btn-primary userLogon" title="Alterar Logon" type="button"><i class="fas fa-user-edit"></i></button><button class="btn btn-primary userSenha" title="Alterar Senha" type="button" ><i class="fas fa-key"></i></button></div>',
                    order: false
                }
            ]
    });

    $('#TbUsuarios').on('click', '.userLogon', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        var userLogado = $('#userLogado').val();
        if (data["FuncionarioId"] == userLogado) {
            SWALAlterarUser(data["Id"]);
        } else {
            SWALBloqueio("Acesso Negado!");
        }
        tableUser.ajax.reload(null, false);
    });

    $('#TbUsuarios').on('click', '.userSenha', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        var userLogado = $('#userLogado').val();
        if (data["FuncionarioId"] == userLogado) {
            SWALAlterarSenha(data["Id"]);
        } else {
            SWALBloqueio("Acesso Negado!")
        }
    });

    setInterval(function () {
        tableUser.ajax.reload(null, false);
    }, 25000);

});

function SWALAlterarUser(id) {
    Swal.fire({
        title: "Alterar logon:",
        text: "Digite o novo logon :",
        input: 'text',
        confirmButtonText: 'Alterar'
    }).then(novoLogon => {
        if (novoLogon.value != null) {
            if (novoLogon.value != '') {
                var filtro = {
                    Id: id,
                    NomeLogon: novoLogon.value
                };
                $.ajax({
                    url: '/Usuarios/AlterarNomeLogon',
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

function SWALAlterarSenha(id) {
    Swal.fire({
        title: "Alterar Senha",
        text: "Digite a nova senha :",
        input: 'password',
       confirmButtonText: 'Alterar'
    }).then(novoLogon => {
        if (novoLogon.value != null) {
            if (novoLogon.value != '') {
                var filtro = {
                    Id: id,
                    Senha: novoLogon.value
                };
                $.ajax({
                    url: '/Usuarios/AlterarSenha',
                    type: 'POST',
                    cache: false,
                    async: true,
                    dataType: "Json",
                    data: filtro,
                    success: function (e) {
                        if (e == true) {
                            $('#TbUsuarios').DataTable().ajax.reload();
                            return SWALSuccess('Senha alterada com sucesso!');
                        } else {
                            return SWALBloqueio(e);
                        }
                    },
                });
            }
            else {
                return SWALBloqueio('Senha não pode ser vazia!');
            }
        }
    });
}