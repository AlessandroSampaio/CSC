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

})