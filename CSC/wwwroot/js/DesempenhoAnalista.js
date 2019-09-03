$(document).ready(function () {
    _datePicker();

    $('#btnPesquisar').on('click', function () {
        GerarDesempenho();
    });
});

function GerarDesempenho() {
    $.ajax({
        url: '/Relatorios/DesempenhoAnalistaPDF',
        dataType: 'html',
        type: 'post',
        data: {
            'Analista': $('#Analista').val(),
            'DataInicial': $('#DataInicial').val(),
            'DataFinal': $('#DataFinal').val(),
            'tipo': '1'
        },
        success: function (result) {
            $('#content').html(result);
        }
    });
}