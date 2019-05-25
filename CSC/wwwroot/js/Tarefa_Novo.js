$(document).ready(function () {
    $(document).on("keydown", ":input:not(textarea)", function (event) {
        if (event.key == "Enter") {
            event.preventDefault();
            if ($('#AtdID').is(':focus')) {
                AddOnEnter();
            }
        }
    });

    $('#btnGravar').on('click', function () {
        var list = 'Atendimentos';
        var index = 0;
        $('#AtdVinculados > tbody > tr').each(function () {
            var Id = $(this).find("td:first").html();
            $('#PostTarefa').prepend('<input type="hidden" name="' +
                list + '[' + index + '].Id" value="' + Id + '">');
            index += 1;
        });
    });

    $("#AtdID").inputFilter(function (value) {
        return /^\d*$/.test(value);
    });
});

function AddAtdRow(atd) {
    var newRow = $('<tr>');
    var cols = '';
    if (atd['Cliente']['telefone'] == null) {
        atd['Cliente']['telefone'] = '';
    }
    if (atd['Cliente']['Email'] == null) {
        atd['Cliente']['Email'] = '';
    }
    cols += '<td>' + atd['Id'] + '</td>';
    cols += '<td>' + atd['Cliente']['cnpj'] + '</td>';
    cols += '<td>' + atd['Cliente']['nome'] + '</td>';
    cols += '<td>' + atd['Funcionario']['Nome'] + '</td>';
    cols += '<td>' + atd['Cliente']['telefone'] + '</td>';
    cols += '<td>' + atd['Cliente']['Email'] + '</td>';

    newRow.append(cols);
    $("#AtdVinculados").append(newRow);
    $('#AtdID').val();
    $('#AtdID').focus();
}

function SearchAtd(id) {
    var response = 0;
    var TRows = $('tbody tr');
    TRows.each(function () {
        if ($(this).find("td:first").html() == id) {
            response += 1;
        }
    });
    return response;
}

function AddOnEnter() {
    var atdId = $('#AtdID').val();
    $.ajax({
        url: '/Tarefas/LocaLizaAtendimento',
        type: 'post',
        data: { 'id': atdId },
        dataType: 'Json',
        success: function (E) {
            if (E == null) {
                Swal.fire('Não existe atendimento com este código');
            } else {

                if (E['Status'] != 'Aberto') {
                    Swal.fire("Atendimento Fechado");
                } else {
                    if (E['TarefaId'] != null) {
                        SWALBloqueio("Já existe uma tarefa para este atendimento!")
                    } else {
                        Swal.fire({
                            type: 'question',
                            title: '<b>Adicionar Atendimento</b>',
                            html: '<div class="row"><b>CNPJ: </b>' + E['Cliente']['cnpj'] + '</div>' +
                                '<div class="row"><b>Cliente: </b>' + E['Cliente']['fantasia'] + '</div>' +
                                '<div class="row"><b>Analista: </b>' + E['Funcionario']['Nome'] + '</div>' +
                                '<div class="row"><b>Detalhes: </b> ' + E['Detalhes'] + '</div>',
                            showCancelButton: true,
                            focusCancel: true,
                            confirmButtonText: 'Adicionar Atendimento',
                            cancelButtonText: 'Cancelar'
                        }).then(add => {
                            if (add.dismiss == null && add.value == true) {

                                if (SearchAtd(E['Id']) > 0) {
                                    Swal.fire("Atendimento já inserido!");
                                } else {
                                    AddAtdRow(E);
                                }
                            }
                            
                        });

                    }
                }
            }
            $('#AtdID').select();
        }
    });
}