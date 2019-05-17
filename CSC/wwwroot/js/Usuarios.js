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
        if (data["Id"] == userLogado) {
            SWALAlterarUser(data["Id"]);
        } else {
            SWALBloqueio("Acesso Negado!");
        }
        tableUser.ajax.reload(null, false);
    });

    $('#TbUsuarios').on('click', '.userSenha', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        var userLogado = $('#userLogado').val();
        if (data["Id"] == userLogado) {
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
    swal({
        title: "Alterar logon:",
        text: "Digite o novo logon :",
        content: "input",
        button: {
            text: "Alterar",
            closeModal: true
        },
    }).then(novoLogon => {
        if (novoLogon != null) {
            var filtro = {
                Id: id,
                NomeLogon: novoLogon
            };
            $.ajax({
                url: '/Usuarios/AlterarNomeLogon',
                type: 'POST',
                cache: false,
                async: true,
                dataType: "Json",
                data: filtro,
                success: function (e) {
                    return swal('Logon alterado com sucesso!', { icon: 'success' });
                },
            });
        }
    });
}

function SWALAlterarSenha(id) {
    swal({
        title: "Alterar Senha:",
        text: "Digite a nova senha :",
        content: {
            element: "input",
            attributes: {
                placeholder: "Nova Senha",
                type: "password",
            },
        },
        button: {
            text: "Alterar",
            closeModal: true
        },
    }).then(novoLogon => {
        if (novoLogon != null) {
            var filtro = {
                Id: id,
                Senha: novoLogon
            };
            $.ajax({
                url: '/Usuarios/AlterarSenha',
                type: 'POST',
                cache: false,
                async: true,
                dataType: "Json",
                data: filtro,
                success: function (e) {
                    return swal('Senha alterada com sucesso!', { icon: 'success' });
                },
            });
        }
    });
}

function SWALBloqueio(mensagem) {
    swal({
        text: mensagem,
        icon: 'error',
        button: true
    })

}