
var detalle = [];
var total = 0;

var subtotal = 0;
var total = 0;
var isv = 0;

var idCliente = 7;

var tipoPago = 1;


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

    calcularTotal()
}

function calcularTotal() {
    subtotal = 0;
    isv = 0;

    for (var i = 0; i < detalle.length; i++) {
        subtotal += (detalle[i].precio * detalle[i].cantidad);
        isv += (detalle[i].tax * detalle[i].cantidad);
    }

    total = subtotal + isv;

    $("#viewSubTotal").html(subtotal);
    $("#viewISV").html(isv);
    $("#viewTotal").html(total);
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



//Cliente
function clienteEncontrado(resp) {
    idCliente = resp.id;
    $("#viewCliente").html(resp.nombre);
    $("#nombreCliente").val(resp.nombre);
    $("#btnQuitarCliente").show();
    $("#conCliente").show();
    $("#seccionBuscarCliente").hide();
    $("#sinCliente").hide();
}

function quitarCliente() {
    idCliente = 7;
    $("#viewCliente").html("");
    $("#btnQuitarCliente").hide();
    $("#conCliente").hide();
    $("#seccionBuscarCliente").show();
    $("#sinCliente").show();
    $("#nombreCliente").val("");
}


function facturar(url) {
    var recibido = 0;
    var listo = true;

    if (total == 0) {
        listo = false;
        $("#alertSinProducto").show();
    } else {
        $("#alertSinProducto").hide();
    }

    if (tipoPago == 1) {
        if ($("#inputRecivido").val() == null || $("#inputRecivido").val().trim() == "") {
            listo = false;
            $("#alertRecibido").show();
        } else {
            var recibido = parseInt($("#inputRecivido").val());
            if (recibido < total) {
                listo = false;
                $("#alertRecibido").show();
            } else {
                $("#alertRecibido").hide();
            }
        }
    }

    if (listo == true) {
        completarFactura(url);
    }
}


function completarFactura(url) {
    var rcb = 0;
    var nombre = "Sin Nombre";

    if ($("#inputRecivido").val() != null && $("#inputRecivido").val().trim() != "") {
        rcb = parseInt($("#inputRecivido").val());
    }

    if ($("#nombreCliente").val() != null && $("#nombreCliente").val().trim() != "") {
        nombre = $("#nombreCliente").val();
    }

    var params = {
        'IdCustomer': idCliente,
        'NameCustomer': nombre,
        'IdTipoPago': tipoPago,
        'Recibido': rcb,
        'DetalleFactura': detalle   
    }

    $.ajax({
        url: url,
        method: "POST",
        dataType: "json",
        contentType: "application/json",
        data: JSON.stringify(params),
        success: function (response) {
            if (response.status == true) {
                alert("Venta exitosa");
                window.location.href = 'Index';
            } else {
                alert("¡Ha ocurrido un error inesperado! Intentelo de nuevo.");
                window.location.href = 'Index';
            }
        },
        error: function (error) {
            console.error("Error en la petición AJAX:", error);
        }
    });
}