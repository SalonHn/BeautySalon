
var detalle = [];
var total = 0;


function agregar(resp) {
    var esta = false;

    for (var i = 0; i < detalle.length; i++) {
        if (detalle[i].idProducto == resp.idProducto) {
            detalle[i].cantidad++;
            esta = true;
        }
    }

    if (esta == false) {
        detalle.push(resp);
    }
    
    cargarTabla();
}

function cargarTabla() {
    $("#tableDetalle tbody").empty();
    for (var i = 0; i < detalle.length; i++) {
        var select = detalle[i];
        var nuevaFila = '<tr>' +
            '<td>' + select.name + '</td>' +
            '<td>' + select.precio + '</td>' +
            '<td>' + select.cantidad + '</td>' +
            '<td>' + (select.precio * select.cantidad) + '</td>' +
            '<td><button type="button" onclick="eliminar(' + select.idProducto + ');" class="btn btn-sm btn-danger">X</button></td>' +
            '</tr>';

        $("#tableDetalle tbody").append(nuevaFila);
    }
}

function eliminar(id) {
    var aux = [];
    for (var i = 0; i < detalle.length; i++) {
        if (detalle[i].idProducto != id) {
            aux.push(detalle[i]);
        }
    }

    detalle = aux;
    cargarTabla();
}