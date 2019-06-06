$(document).ready(function () {
    var date = new Date();

    $('#datai').datepicker({
        format: "dd/mm/yyyy",
        clearBtn: true,
        language: "pt-BR",
        keyboardNavigation: false,
        autoclose: true,
        todayHighlight: true,
        endDate: date
    }).on('changeDate', function (e) {
        $('#dataf').datepicker('setStartDate', e.date);
    });

    $('#dataf').datepicker({
        format: "dd/mm/yyyy",
        clearBtn: true,
        language: "pt-BR",
        keyboardNavigation: false,
        autoclose: true,
        todayHighlight: true,
        endDate: date
    }).on('changeDate', function (e) {
        $('#datai').datepicker('setEndDate', e.date);
    });

    $('.input-group.date').datepicker('setDate', new Date(date.getFullYear(), date.getMonth(), date.getDate()));

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