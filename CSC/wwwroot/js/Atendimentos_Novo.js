$(document).ready(function () {
    var tableLicencas = $('#TbLicencas').DataTable({
        dom: '<"top"f>',
        ajax: {
            url: '/Clientes/InventarioListagem',
            dataSrc: '',
            type: 'post',
            data: { 'id': $('#ClienteId').val() }
        },
        "columns": [
            { "data": "Software" },
            { "data": "Quantidade" }
        ],
        autoWidth: true
    });

    var tableHistorico = $('#TbHistory').DataTable({
        dom: '<"top"f>',
        ajax: {
            url: '/Atendimentos/AtendimentoHistorico',
            dataSrc: '',
            type: 'post',
            data: { 'id': $('#ClienteId').val() }

        },
        "columns": [
            { "data": "Funcionario.Nome" },
            { "data": "Cliente.nome" },
            { "data": "AtendimentoTipo" },
            { "data": "Status" },
            { "data": "Detalhes" }
        ],
        autoWidth: true
    });

    (function AttBadges() {
        $.ajax({
            url: '/Atendimentos/Notificacoes',
            data: { 'id': $('#ClienteId').val() },
            dataType: 'Json',
            type: 'post',
            success: function (e) {
                if (e == 0) {
                    e = "";
                }
                $('#HistoricoBadge').text(e);
            }
        });
        setTimeout(AttBadges, 1000);
    }());

    $('#btnSubmit').on('click', function (evt) {
        evt.preventDefault();
        var form = $(this).parents('form');
        SWALEncerrar(form);
    });
});

function SWALEncerrar(form) {
    swal({
        text: 'Deseja encerrar o atendimento?',
        icon: 'warning',
        buttons: true,
        dangerMode: true,
    })
        .then((willFinish) => {
            if (willFinish) {
                $('#Status').val('Fechado');
            }
            form.submit();
        });
}