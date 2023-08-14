
var horas = [];

function cargarHorario() {
    $("#horaReserva").empty();
    var opcionSeleccione = new Option('Seleccione', 0);
    $("#horaReserva").append(opcionSeleccione);

    for (var i = 0; i < horas.length; i++) {
        var nuevaOpcion = new Option(horas[i].hora, horas[i].id);
        $("#horaReserva").append(nuevaOpcion);
    }
}

function comprobarDatos() {
    var fecha = $("#fechaReserva").val();
    var hora = parseInt($("#horaReserva").val());

    if (fecha != '' && hora > 0) {
        $("#btnReservar").prop("disabled", false);
    } else {
        $("#btnReservar").prop("disabled", true);
    }
}