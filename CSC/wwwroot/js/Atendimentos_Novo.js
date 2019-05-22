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
        dom: 'ftp',
        ajax: {
            url: '/Atendimentos/AtendimentoHistorico',
            dataSrc: '',
            type: 'post',
            data: { 'id': $('#ClienteId').val() }
        },
        "scrollX": "true",
        "columns": [
            { "data": "Funcionario.Nome" },
            { "data": "Cliente.nome" },
            { "data": "Solicitante"},
            { "data": "AtendimentoTipo" },
            { "data": "Status" },
            { "data": "Detalhes" }
        ]
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
    Swal.fire({
        text: 'Deseja encerrar o atendimento?',
        type: 'question',
        showCancelButton: true,
        cancelButtonText: 'Não',
        confirmButtonText: 'Sim',
        cancelButtonColor: '#fc544b',
        allowEnterKey: false,
        focusConfirm: false
    })
        .then((willFinish) => {
            if (willFinish.dismiss != "backdrop") {
                if (willFinish.value == true) {
                    $('#Status').val('Fechado');
                }
                form.submit();
            }
        });
}