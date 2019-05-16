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
                    "defaultContent": '<div class="btn-group btn-group-justified"><button class="btn btn-primary open-modal" title="Alterar Logon" type="button" data-toggle="modal" data-target="#UserForm"><i class="fas fa-user-edit"></i></button><button class="btn btn-primary open-modal" title="Alterar Senha" type="button" data-toggle="modal" data-target="#PasswordForm"><i class="fas fa-key"></i></button></div>',
                    order: false
                }
            ]
    });

    $('#TbUsuarios').on('click', 'button', function () {
        var data = tableUser.row($(this).parents('tr')).data();
        $("input[name = id]").val(data["Id"]);
    });

    $('#UserForm').on('hidden.bs.modal', function () {
        tableUser.ajax.reload();
    });

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

    $("#btnSalvarUser").on("click", function () {
        var filtro = {
            Id: $("#UserID").val(),
            NomeLogon: $("#NovoUser").val()
        };
        SalvarUser(filtro);

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