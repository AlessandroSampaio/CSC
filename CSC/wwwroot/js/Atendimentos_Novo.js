$(document).ready(function () {
    var tableLicencas = $('#TbLicencas').DataTable({
        dom: '<"top"f>',
        ajax: {
            url: '/Clientes/InventarioListagem',
            dataSrc: '',
            type: 'post',
            data: { 'id': $('#ClienteId').val() }
        },
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
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
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        },
        "scrollX": "true",
        "columns": [
            { "data": "User.Nome" },
            { "data": "Abertura" },
            { "data": "Solicitante"},
            { "data": "AtendimentoTipo" },
            { "data": "Status" },
            { "data": "Detalhes" }
        ],
        order: [1, 'desc']
    });

    Mono();

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

function Mono() {
    if ($('#AtendimentoTipo option').length == 1) {
        Swal.fire("Cliente Mono","Possui direito apenas a chave", "info");
    }
}

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