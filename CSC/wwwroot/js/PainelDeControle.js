$(document).ready(function () {
    var atdTotal = $('#AtdAbertos');
    var atdPorFunc = $('#AtdPorFuncionarios');
    var AtdPorSit;
    var AtdPorFunc;
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

    $('#btnPesquisar').on('click', async function () {
        var dataini = $('#DataInicial').val();
        var datafim = $('#DataFinal').val();

        if (dataini == "" || datafim == "") {

        } else {

            if (AtdPorSit != null || AtdPorFunc != null) {
                AtdPorSit.destroy();
                AtdPorFunc.destroy();
            }

            var situacoes = await AtdPorSituacao(dataini, datafim);
            var funcs = await AtdPorFuncionarios(dataini, datafim);
            var funcionario = funcs.map(item => item.funcionario);
            var totais = funcs.map(item => item.atendimentos);
            AtdPorSit = new Chart(atdTotal, {
                type: 'pie',
                data: {
                    labels: ["Transferido", "Fechado", "Aberto"],
                    datasets: [{
                        labels: "Numero de Atendimentos",
                        backgroundColor: ["blue", "green", "red"],
                        data: situacoes
                    }]
                },
                options: {
                    legend: {
                        display: false
                    }
                }
            });
            AtdPorFunc = new Chart(atdPorFunc, {
                type: 'pie',
                data: {
                    labels: funcionario,
                    datasets: [{
                        labels: "Numero de Atendimentos",
                        backgroundColor: function () {
                            var g = Math.floor(Math.random() * 255);
                            var b = Math.floor(Math.random() * 255);
                            return "rgb( 0 ," + g + "," + b + ")";
                        },
                        data: totais
                    }]
                },
                options: {
                    legend: {
                        display: false
                    }
                }
            });
        }
    });

    $('#btnPesquisar').click();
});

async function AtdPorFuncionarios(dataIni, dataFim) {
    var result;
    await $.ajax({
        url: '/Atendimentos/AtendimentosPorFuncionario',
        type: 'post',
        async: true,
        cache: false,
        data: { dataIni, dataFim },
        dataType: 'Json'
    }).then(function (e) {
        result = e;
    });
    return result;
}

async function AtdPorSituacao(dataIni, dataFim) {
    var result;
    await $.ajax({
        url: '/Atendimentos/SituacaoAtendimentosChart',
        type: 'post',
        async: true,
        cache: false,
        data: { dataIni, dataFim },
        dataType: 'Json',
    }).then(function (e) {
        result = e;
    }
    );
    return result;
}