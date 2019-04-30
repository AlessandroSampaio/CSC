$(document).ready(function () {
    $('#TbFuncionarios').DataTable({
        dom: '<"top"B>',
        buttons: [
            'excel', 'pdf', 'print', {
                text: 'Novo',
                action: function (e, dt, button, config) {
                    window.location.href = '/Funcionarios/Novo/';
                }
            }
        ]
    });
    $('#TbUsuarios').DataTable({
        dom: '<"top"B>',
        buttons: [
            'excel', 'pdf', 'print', {
                text: 'Novo',
                action: function (e, dt, button, config) {
                    window.location.href = '/Usuarios/Novo/';
                }
            }
        ]
    });
    $('#TbClientes').dataTable({
        dom: '<"top"B>',
        buttons: [{
            extend: 'collection',
            className: "btn-primary",
            text: 'Export',
            buttons:
                [
                    { extend: "excel", className: "btn-primary" },
                    { extend: "pdf", className: "btn-primary" },
                    { extend: "print", className: "btn-primary" }],
        },
        {
            text: 'Novo',
            action: function (e, dt, button, config) {
                window.location.href = '/Clientes/Novo/';
            }
        }
        ]
    });
});