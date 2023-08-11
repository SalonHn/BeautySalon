

var productos = [];

function mostrarProductos() {
    $('#buscarProductosTabla tbody').empty();

    var tabla = $('#buscarProductosTabla');

    for (var i = 0; i < productos.length; i++) {
        var nuevaFila = '<tr><th>' + productos[i].sku + '</th><td>' + productos[i].name
            + '</td><td>' +
            '<button class="btn btn-sm btn-primary" onclick="seleccionar(' + i +');" data-bs-dismiss="modal"> Seleccionar </button> </td ></tr>';

        tabla.find('tbody').append(nuevaFila);
    }
}


function seleccionar(index) {
    $('#idProduct').val(productos[index].id);
    $('#sku').val(productos[index].sku);
    $('#nombre').val(productos[index].name);
    $('#addRecurso').show();
    $('#btnAddRecursoModal').hide();
}

function cancelarAddRecurso() {
    $('#addRecurso').hide();
    $('#btnAddRecursoModal').show();
}