$(document).ready(function () {
    $.fn.dataTable.moment('D/MM/YYYY');

    $('#TbFuncionarios').DataTable({
        dom: '<"top"B>',
        buttons: [{
            extend: 'collection',
            className: "btn-primary",
            text: 'Export',
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
                window.location.href = '/Funcionarios/Novo/';
            }
        }
        ]
    });

    //Nova DataTable usando Ajax
    $('#TbUsuarios').dataTable({
        dom: '<"top"B>',
        buttons: [{
            extend: 'collection',
            className: "btn-primary",
            text: 'Export',
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
            { "data": "id", visible: false },
            { "data": "nomeLogon"},
            { "data": "funcionario.nome" },
            { "data": "funcionario.admissao" },
            { "data": "funcionario.demissao" }
        ],
        autoWidth: true,
        columnDefs: [{
            "targets": 5,
            "data": null,
            "defaultContent": '<button class="btn my-2 my-sm-0 open-modal" type="button" data-id="item.Id" data-toggle="modal" data-target="#UserForm"><i class= "fas fa-user-edit" ></i></button><button class="btn my-2 my-sm-0 open-modal" type="button" data-id="item.Id" data-toggle="modal" data-target="#PasswordForm"><i class="fas fa-key"></i></button>'
        }]
    });





    //Tabela Usuarios, Funcional, mas dependente de inicialização estática
    /*  $('#TbUsuarios').DataTable({
          dom: '<"top"B>',
          buttons: [{
              extend: 'collection',
              className: "btn-primary",
              text: 'Export',
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
          ]
      });*/
    $('#TbClientes').dataTable({
        dom: '<"top"B>',
        buttons: [{
            extend: 'collection',
            className: "btn-primary",
            text: 'Export',
            buttons:
                [
                    { extend: "excel", className: "btn-block" },
                    { extend: "pdf", className: "btn-block" },
                    { extend: "print", className: "btn-block" }],
        },
        {
            text: 'Novo',
            className: "btn-primary",
            action: function (e, dt, button, config) {
                window.location.href = '/Clientes/Novo/';
            }
        }
        ]
    });
});