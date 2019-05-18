$(document).ready(function () {
    var atdTotal = $('#AtdAbertos');
    var atdPorFunc = $('#AtdPorFuncionarios');

    $.ajax({
        url: '/Atendimentos/SituacaoAtendimentosChart',
        type: 'post',
        async: true,
        cache: false,
        dataType: 'Json',
        success: function (e) {
            var myPieChart = new Chart(atdTotal, {
                type: 'pie',
                data: {
                    labels: ["Transferido", "Fechado", "Aberto"],
                    datasets: [{
                        labels: "Numero de Atendimentos",
                        backgroundColor: ["blue", "green", "red"],
                        data: e
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

    $.ajax({
        url: '/Atendimentos/AtendimentosPorFuncionario',
        type: 'post',
        async: true,
        cache: false,
        dataType: 'Json',
        success: function (e) {
            console.log(e);
            var funcionario = e.map(item => item.funcionario);
            var totais = e.map(item => item.atendimentos);
            console.log(funcionario);
            console.log(totais);
            var atdPorFuncionarios = new Chart(atdPorFunc, {
                type: 'pie',
                data: {
                    labels: funcionario,
                    datasets: [{
                        labels: "Numero de Atendimentos",
                        backgroundColor: function () {
                            var r = Math.floor(Math.random() * 255);
                            var g = Math.floor(Math.random() * 255);
                            var b = Math.floor(Math.random() * 255);
                            return "rgb(" + r + "," + g + "," + b + ")";
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

});


function getRandomColor() {
    var letters = '0123456789ABCDEF'.split('');
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}