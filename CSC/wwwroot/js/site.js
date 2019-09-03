$('#btnUserLogonInfo').on('click', function () {
    /*$.ajax({
        url: '/Home/SessionTime',
        dataType: 'Json',
        async: true,
        cache: false
    }).done(function (result) {
        $('#UserLogonInfo').html('Logado a ' + result + ' minutos');
    })*/

    
});

function _datePicker() {
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
};

function verifica_cpf_cnpj(valor) {
    valor = valor.toString();
    valor = valor.replace(/[^0-9]/g, '');
    if (valor.length === 11) {
        return 'CPF';
    }
    else if (valor.length === 14) {
        return 'CNPJ';
    }
    else {
        return false;
    }
}

function calc_digitos_posicoes(digitos, posicoes = 10, soma_digitos = 0) {

    digitos = digitos.toString();
    for (var i = 0; i < digitos.length; i++) {
        soma_digitos = soma_digitos + (digitos[i] * posicoes);
        posicoes--;
        if (posicoes < 2) {
            posicoes = 9;
        }
    }
    soma_digitos = soma_digitos % 11;
    if (soma_digitos < 2) {
        soma_digitos = 0;
    } else {
        soma_digitos = 11 - soma_digitos;
    }
    var cpf = digitos + soma_digitos;
    return cpf;
}

function valida_cpf(valor) {
    valor = valor.toString();
    valor = valor.replace(/[^0-9]/g, '');
    var digitos = valor.substr(0, 9);
    var novo_cpf = calc_digitos_posicoes(digitos);
    var novo_cpf = calc_digitos_posicoes(novo_cpf, 11);
    if (novo_cpf === valor) {
        return true;
    } else {
        return false;
    }
}

function valida_cnpj(valor) {

    valor = valor.toString();
    valor = valor.replace(/[^0-9]/g, '');
    var cnpj_original = valor;
    var primeiros_numeros_cnpj = valor.substr(0, 12);
    var primeiro_calculo = calc_digitos_posicoes(primeiros_numeros_cnpj, 5);
    var segundo_calculo = calc_digitos_posicoes(primeiro_calculo, 6);
    var cnpj = segundo_calculo;
    if (cnpj === cnpj_original) {
        return true;
    }
    return false;
}

function SWALBloqueio(mensagem) {
    Swal.fire({
        text: mensagem,
        type: 'error'
    })
}

function SWALSuccess(mensagem) {
    Swal.fire({
        title: mensagem,
        toast: true,
        type: 'success',
        position: 'top-end',
        showConfirmButton: false,
        timer: 3000
    });
}

(function ($) {
    $.fn.inputFilter = function (inputFilter) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop", function () {
            if (inputFilter(this.value)) {
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            }
        });
    };
}(jQuery));
