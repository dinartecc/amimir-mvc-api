var tableHdr = null;
var IdRecord = 0;

$(document).ready(function () {
    loadData();

    $('#btnnuevo').on('click', function (e) {
        e.preventDefault();
        IdRecord = 0;
        NewRecord();
    });

    $('#btnguardar').on('click', function (e) {
        e.preventDefault();
        Guardar();
    });

    $('#dt-records').on('click', 'button.btn-edit', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        loadDtl(data);
        IdRecord = data.ID;
    });

    $('#dt-records').on('click', 'button.btn-delete', function (e) {
        var _this = $(this).parents('tr');
        var data = tableHdr.row(_this).data();
        IdRecord = data.ID;
        if (confirm('¿Seguro de eliminar el registro?')) {
            Eliminar();
        }
    });

});

function loadData() {
    tableHdr = $('#dt-records').DataTable({
        responsive: true,
        destroy: true,
        ajax: "/Anime/Lista",
        order: [],
        columns: [
            { "data": "Nombre" },
            {
                "data": "FechaEstreno",
                "type": "datetime",
                "format": "D/M/YYYY",
                "render": function (data, type, row) {
                    var d = /\/Date\((\d*)\)\//.exec(data)
                    return (d) ? new Date(+d[1]) : '----'
                }
            },
            { "data": "Sinopsis" },
            { "data": "Puntuacion" },
            { "data": "Popularidad" },
            { "data": "EstadoID" },
        ],
        processing: true,
        language: {
            "decimal": "",
            "emptyTable": "No hay información",
            "info": "Mostrando _START_ a _END_ de _TOTAL_ Entradas",
            "infoEmpty": "Mostrando 0 to 0 of 0 Entradas",
            "infoFiltered": "(Filtrado de _MAX_ total entradas)",
            "infoPostFix": "",
            "thousands": ",",
            "lengthMenu": "Mostrar _MENU_ Entradas",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "search": "Buscar:",
            "zeroRecords": "Sin resultados encontrados",
            "paginate": {
                "first": "Primero",
                "last": "Ultimo",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        columnDefs: [
            {
                width: "40%",
                targets: 0,
                data: "Nombre"
            },
            {
                width: "40%",
                targets: 1,
                data: "FechaEstrena"
            },
            {
                width: "9%",
                targets: 2,
                data: "Sinopsis"
            },
            {
                width: "9%",
                targets: 3,
                data: "Puntuacion"
            },
            {
                width: "9%",
                targets: 4,
                data: "Popularidad"
            },
            {
                width: "9%",
                targets: 5,
                data: "EstadoID"
            },
            {
                width: "1%",
                targets: 6,
                data: null,
                defaultContent: '<button type="button" class="btn btn-info btn-sm btn-edit" data-target="#modal-record"><i class="fa fa-pencil"></i></button>'
            },
            {
                width: "1%",
                targets: 7,
                data: null,
                defaultContent: '<button type="button" class="btn btn-danger btn-sm btn-delete"><i class="fa fa-trash"></i></button>'

            }
        ]
    });
}

function NewRecord() {
    $(".modal-header h3").text("Crear Usuario");

    $('#txtNombre').val('');
    $('#txtFechaEstreno').val('');
    $('#txtSinopsis').val('');
    $('#txtPuntuacion').val('');
    $('#txtPopularidad').val('');
    $('#txtEstadoID').val('');

    $('#modal-record').modal('toggle');
}

function loadDtl(data) {
    var d = /\/Date\((\d*)\)\//.exec(data.FechaEstreno)
    var fecha = (d) ? new Date(+d[1]) : new Date()
    var mes = fecha.getMonth().toString().length == 1 ? '0' + fecha.getMonth() : '' + fecha.getMonth()
    var dia = fecha.getDate().toString().length == 1 ? '0' + fecha.getDate() : '' + fecha.getDate()
    var fechaParse = fecha.getFullYear() + '-' + mes + '-' + dia
    $(".modal-header h3").text("Editar Anime");
    $('#txtNombre').val(data.Nombre);
    $('#txtFechaEstreno').val(fechaParse);
    $('#txtSinopsis').val(data.Sinopsis);
    $('#txtPuntuacion').val(data.Puntuacion);
    $('#txtPopularidad').val(data.Popularidad);
    $('#txtEstadoID').val(data.EstadoID);

    $('#modal-record').modal('toggle');
}

function Guardar() {
    var record = "'ID':" + IdRecord;
    record += ",'Nombre':'" + $.trim($('#txtNombre').val()) + "'";
    record += ",'FechaEstreno':'" + $.trim($('#txtFechaEstreno').val()) + "'";
    record += ",'Sinopsis':'" + $.trim($('#txtSinopsis').val()) + "'";
    record += ",'Puntuacion':" + $.trim($('#txtPuntuacion').val());
    record += ",'Popularidad':" + $.trim($('#txtPopularidad').val());
    record += ",'EstadoID':" + $.trim($('#txtEstadoID').val());

    var send = '({' + record + '})'
    console.log(send)

    $.ajax({
        type: 'POST',
        url: '/Anime/Guardar',
        data: eval(send),
        success: function (response) {
            if (response.success) {
                $("#modal-record").modal('hide');
                $.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            }
            else {
                $("#modal-record").modal('hide');
                $.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}

function Eliminar() {
    $.ajax({
        type: 'POST',
        url: '/Anime/Eliminar/?ID=' + IdRecord,
        success: function (response) {
            if (response.success) {
                $.notify(response.message, { globalPosition: "top center", className: "success" });
                $('#dt-records').DataTable().ajax.reload(null, false);
            } else {
                $.notify(response.message, { globalPosition: "top center", className: "error" });
            }
        }
    });
}