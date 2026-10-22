const newLocal = $("[data-query]").val(null);
$(document).ready(function () {

    $("#btnLimpiar").click(function (event) {
        event.preventDefault();
        $("[data-query]").val(null);
        $("#ddlEstatus").val('0');
        let tabla = $("#example");
        tabla.dataTable().fnClearTable();
        tabla.dataTable().fnDestroy();
        var table = tabla.DataTable({
            language: {

                "decimal": "",
                "emptyTable": "",
                "info": "Mostrando registros del START al END de un total de TOTAL registros ",
                "infoEmpty": "Mostrando 0 to 0 of 0 registros",
                "infoFiltered": "(Filtrado de MAX total registros)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar MENU registros",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "Buscar:",
                "zeroRecords": "",
                "paginate": {
                    "first": "Primero",
                    "last": "Ultimo",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },
                select: {
                    rows: ""
                }
            },
            searching: false,
        });
        return false;
    });
    $(".mostrar").click(function (event) {
        event.preventDefault();

        mostrarOculatar(this);

        return false;
    });


    function mostrarOculatar(btn) {
        var id = "#child-" + $(btn).attr("data-id");
        $(id).toggle();

    }

    $("#btnBuscar").click(function (event) {
        event.preventDefault();

        var fechaInicio=$('#txtFechaInicio').val();
        var fechaFin = $("#txtFechaFin").val();

        
        if (fechaInicio == "" && fechaFin != "") {
            $("#alertaSimple").modal("show");
            var texto = document.getElementById("TextoAlertaSimple");
            texto.innerHTML = "Falta capturar la fecha “Inicio”.";
        }
        if (fechaFin == "" && fechaInicio != "") {
            $("#alertaSimple").modal("show");
            var texto = document.getElementById("TextoAlertaSimple");
            texto.innerHTML = "Falta capturar la fecha “Fin”.";
        }
        if (fechaInicio != "" && fechaFin != "") {
            var arrayFInicio = new Array();
            var arrayFFin = new Array();
            arrayFInicio = fechaInicio.split("/");
            var diaInicio = arrayFInicio[0];
            var mesInicio = arrayFInicio[1];
            var añoInicio = arrayFInicio[2];
            var arrayFFin = new Array();
            arrayFFin = fechaFin.split("/");
            var diaFin = arrayFFin[0];
            var mesFin = arrayFFin[1];
            var añoFin = arrayFFin[2];

            var fInicio = añoInicio + "/" + mesInicio + "/" + diaInicio;
            var fFin = añoFin + "/" + mesFin + "/" + diaFin;
            fInicio = new Date(fInicio);
            fFin = new Date(fFin);
            var fechaActual = new Date();
            if (fFin > fechaActual && fInicio > fechaActual) {
                $("#alertaSimple").modal("show");
                var texto = document.getElementById("TextoAlertaSimple");
                texto.innerHTML = "La fecha “Fin” e “Inicio” no pueden ser mayor a la fecha actual.";
            }
            else if (fFin > fechaActual) {
                $("#alertaSimple").modal("show");
                var texto = document.getElementById("TextoAlertaSimple");
                texto.innerHTML = "La fecha “Fin” no puede ser mayor a la fecha actual.";
            }
            else if (fInicio > fechaActual) {
                $("#alertaSimple").modal("show");
                var texto = document.getElementById("TextoAlertaSimple");
                texto.innerHTML = "La fecha “Inicio” no puede ser mayor a la fecha actual.";
            }
            else if (fInicio > fFin) {
                $("#alertaSimple").modal("show");
                var texto = document.getElementById("TextoAlertaSimple");
                texto.innerHTML = "La fecha capturada en el campo “Fin” no debe ser menor a la fecha capturada en el campo “Inicio”.";
            }
            else {
                var paramsBusq = {
                    rfc: $("#txtRfc").val(),
                    nombreLaboratorio: $("#txtNombreLaboratorio").val(),
                    numeroAprobacion: $("#txtNoAprobacion").val(),
                    estatus: $("#ddlEstatus option:selected").val(),
                    periodoInicio: $("#txtFechaInicio").val(),
                    periodoFin: $("#txtFechaFin").val(),


                }


                $.ajax({
                    type: "POST",
                    url: "ApercibidosYReincidentes.aspx/BusqedaApercibidoReincidente",
                    cache: false,
                    aysnc: false,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(paramsBusq),
                    success: OnSuccess,
                    failure: function (data) {
                        console.log(data);
                    },
                    error: function (xhr, status, error) {
                        alert('No se encontaron resultados');
                    }
                });

                return false;
            }
        }
        else {
            var paramsBusq = {
                rfc: $("#txtRfc").val(),
                nombreLaboratorio: $("#txtNombreLaboratorio").val(),
                numeroAprobacion: $("#txtNoAprobacion").val(),
                estatus: $("#ddlEstatus option:selected").val(),
                periodoInicio: $("#txtFechaInicio").val(),
                periodoFin: $("#txtFechaFin").val(),


            }


            $.ajax({
                type: "POST",
                url: "ApercibidosYReincidentes.aspx/BusqedaApercibidoReincidente",
                cache: false,
                aysnc: false,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(paramsBusq),
                success: OnSuccess,
                failure: function (data) {
                    console.log(data);
                },
                error: function (xhr, status, error) {
                    alert('No se encontaron resultados');
                }
            });

            return false;
        }
    });
    BuscarInit();
    function BuscarInit() {
        var paramsBusq = {
            rfc: $("#txtRfc").val(),
            nombreLaboratorio: $("#txtNombreLaboratorio").val(),
            numeroAprobacion: $("#txtNoAprobacion").val(),
            estatus: $("#ddlEstatus option:selected").val(),
            periodoInicio: $("#txtFechaInicio").val(),
            periodoFin: $("#txtFechaFin").val(),


        }


        $.ajax({
            type: "POST",
            url: "ApercibidosYReincidentes.aspx/BusqedaApercibidoReincidente",
            cache: false,
            aysnc: false,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(paramsBusq),
            success: OnSuccess,
            failure: function (data) {
                console.log(data);
            },
            error: function (xhr, status, error) {
                alert('No se encontaron resultados');
            }
        });

        return false;
    }
    function OnSuccess(response) {
        //console.log(response);
        var laboratorios = response.d;
        let tablaResultado = $("#example");

        pintarTabla(tablaResultado, laboratorios);


    }


    function pintarTabla(tabla, laboratorios) {
       
        tabla.dataTable().fnClearTable();
        tabla.dataTable().fnDestroy();

        var table = tabla.DataTable({
            data: laboratorios,

            language: {

                "decimal": "",
                "emptyTable": "No se encontraron resultados",
                "info": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros ",
                "infoEmpty": "Mostrando 0 to 0 of 0 registros",
                "infoFiltered": "(Filtrado de _MAX_ total registros)",
                "infoPostFix": "",
                "thousands": ",",
                "lengthMenu": "Mostrar _MENU_ registros",
                "loadingRecords": "Cargando...",
                "processing": "Procesando...",
                "search": "Buscar:",
                "zeroRecords": "No hay información para mostrar",
                "paginate": {
                    "first": "Primero",
                    "last": "Ultimo",
                    "next": "Siguiente",
                    "previous": "Anterior"
                },
                select: {
                    rows: ""
                }
            },
            searching: false,
            columns: [
                {
                    className: 'dt-control',
                    orderable: false,
                    data: null,
                    defaultContent: '',
                },
                { data: 'RFC' },
                { data: 'Nombre' },
                { data: 'Fecha_Resolucion' },
                { data: 'Numero_Oficio' },
                { data: 'Fecha_Notificacion' },
                { data: 'Fecha_Vigencia' },
                { data: 'Estatus' },
                {
                    data: "EstatusId", render: function (estatusId, type, row, meta) {
                        //console.log("dataTable=>", estatusId);
                        if (estatusId === 1 || estatusId === 2) {
                            return `
                                    <div class="btn-toolbar" role="toolbar">
                                        <div class="btn-group">
                                            <button type="button" onclick="NuevaReincidencia(${row.Id},'${row.EstatusId}')" id="btnNuevaReincidencia" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="left" title="Nueva reincidencia"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span></button>
                                            <button type="button" onclick="Consulta(${row.Id},'${row.EstatusId}')" id="btnConsulta" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Consultar"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></button>
                                            <button type="button"  onclick="Edicion(${row.Id},'${row.EstatusId}')" id="btnEdicion"  class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="right" title="Editar"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>
                                            
</div>
                                    </div>
                        `;
                        } else {
                            return `
                                    <div class="btn-toolbar" role="toolbar">
                                        <div class="btn-group">                        
                                            <button type="button" onclick="ConsultaReincidencia(${row.Id},'${row.EstatusId}',${row.IdApercibimiento})" id="btnConsulta" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Consultar"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></button>
                                            <button type="button"  onclick="EdicionReincidencia(${row.Id},'${row.EstatusId}',${row.IdApercibimiento})" id="btnEdicion"  class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="right" title="Editar"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>
                                             
</div>
                                    </div>
                        `;
                        }
                    }
                },
                {
                    data: "EstatusId", render: function (estatusId, type, row, meta) {
                        if (estatusId === 1 ) {
                            return `
                        <div class="btn-toolbar" role="toolbar">
                            <div class="btn-group">
                                <button type="button" onclick="NuevaImpugnacionApercibimiento(${row.Id},'${row.EstatusId}')" id="btnNuevaImpugnacion" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="left" title="Nueva impugnación"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span></button>
                                <button type="button" onclick="ConsultaImpugnacionApercibimiento(${row.Id},'${row.EstatusId}')" id="btnConsultaImpugnacion" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Consultar impugnación"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></button>
                                <button type="button" onclick="EdicionImpugnacionApercibimiento(${row.Id},'${row.EstatusId}')" id="btnEdicionImpugnacion" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="right" title="Editar impugnación"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>

                            </div>
                        </div>
                        `;
                        } else if (estatusId === 2) {
                            return `
                        <div class="btn-toolbar" role="toolbar">
                            <div class="btn-group">                            
                                <button type="button" onclick="ConsultaImpugnacionApercibimiento(${row.Id},'${row.EstatusId}')" id="btnConsultaImpugnacion" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Consultar impugnación"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></button>
                                <button type="button" onclick="EdicionImpugnacionApercibimiento(${row.Id},'${row.EstatusId}')" id="btnEdicionImpugnacion" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="right" title="Editar impugnación"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>

                            </div>
                        </div>
                        `;
                        }
                        else if (estatusId === 3) {
                            return `
                        <div class="btn-toolbar" role="toolbar">
                            <div class="btn-group">
                                <button type="button" onclick="NuevaImpugnacionReincidencia(${row.Id},'${row.EstatusId}',${row.IdApercibimiento})" id="btnNuevaImpugnacionReincidencia" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="left" title="Nueva impugnación"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span></button>
                                <button type="button" onclick="ConsultaImpugnacionReincidencia(${row.Id},'${row.EstatusId}',${row.IdApercibimiento})" id="btnConsultaImpugnacionReinciencia" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Consultar impugnación"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></button>
                                <button type="button" onclick="EdicionImpugnacionReincidencia(${row.Id},'${row.EstatusId}',${row.IdApercibimiento})" id="btnEdicionImpugnacionReincidencia" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="right" title="Editar impugnación"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>

                            </div>
                        </div>
                        `;

                        }
                        else {
                            return `
                        <div class="btn-toolbar" role="toolbar">
                            <div class="btn-group">
                                <button type="button" onclick="ConsultaImpugnacionReincidencia(${row.Id},'${row.EstatusId}',${row.IdApercibimiento})" id="btnConsultaImpugnacionReinciencia" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="top" title="Consultar impugnación"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></button>
                                <button type="button" onclick="EdicionImpugnacionReincidencia(${row.Id},'${row.EstatusId}',${row.IdApercibimiento})" id="btnEdicionImpugnacionReincidencia" class="btn btn-default btn-xs" data-toggle="tooltip" data-placement="right" title="Editar impugnación"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></button>

                            </div>
                        </div>
                        `;
                        }
                    }
                }
            ],
            order: [[1, 'asc']],

        });



        // Add event listener for opening and closing details
        $('#example tbody').off('click').on('click', 'td.dt-control', function () {
            /* var datos = table.row($(this).parents("tr")).data();*/
            var tr = $(this).closest('tr');
            var row = table.row(tr);

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
                tr.removeClass('shown');
            } else {
                // Open this row
                row.child(format(row.data())).show();
                tr.addClass('shown');
            }
        });
    }





    function format(d) {
        var result = d.LaboratorioVigencias;
        let tablaChild = '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">';
        tablaChild += `
                <tr>
                    <td></td>
                    <td>Número Aprobación</td>
                    <td>Vigencia Aprobación</td>
                </tr>
            `;
        for (var k in result) {
            console.log(k, result[k]);
            const item = result[k];
            tablaChild += `
                    <tr>
                        <td>${k + 1}</td>
                        <td>${item.NumeroAprobacion}</td>
                        <td>${item.VigenciaAprobacion}</td>
                    </tr>
            `;
        }

        tablaChild += '  </table>';
        // `d` is the original data object for the row
        return (tablaChild);
    }



    $('.botonF1').hover(function () {
        $('.btn').addClass('animacionVer');
    });

    $('.contenedor').mouseleave(function () {
        $('.btn').removeClass('animacionVer');
    });



});

function NuevaReincidencia(idApercibimiento, EstatusId) {
    
  
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "ApercibidosYReincidentes.aspx/ExisteReincidencia",
        dataType: "json",
        data: JSON.stringify({ idApercibimiento: idApercibimiento }),
        success: function (data, textStatus) {
            if (data.d == true) {

                $("#alertaSimple").modal("show");
                var texto = document.getElementById("TextoAlertaSimple");
                texto.innerHTML = "Ya existe una reincidencia para este apercibimiento.";
            }
            else {

                if (EstatusId == 2) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ApercibidosYReincidentes.aspx/ApercibimientoEstaImpugnado",
                        dataType: "json",
                        data: JSON.stringify({ idApercibimiento: idApercibimiento }),
                        success: function (data, textStatus) {
                            if (data.d == false) {

                                $("#alertaSimple").modal("show");
                                var texto = document.getElementById("TextoAlertaSimple");
                                texto.innerHTML = "No puede agregar una reincidencia, porque el apercibimiento cuenta con una impugnación.";
                            }
                            else {
                                window.location.href = "NuevaReincidencia.aspx?idApercibimiento=" + idApercibimiento + "&idReincidencia=" + 0;
                            }


                        },
                        error: function (data, textStatus) {



                        }
                    });

                }
                else {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "ApercibidosYReincidentes.aspx/VigenciaAprobacionValida",
                        dataType: "json",
                        data: JSON.stringify({ idApercibimiento: idApercibimiento }),
                        success: function (data, textStatus) {
                            if (data.d == false) {

                                $("#alertaSimple").modal("show");
                                var texto = document.getElementById("TextoAlertaSimple");
                                texto.innerHTML = "No puede capturar la reincidenica porque la vigencia de aprobación no está vigente";
                            }
                            else {
                                window.location.href = "NuevaReincidencia.aspx?idApercibimiento=" + idApercibimiento + "&idReincidencia=" + 0;
                            }


                        },
                        error: function (data, textStatus) {



                        }
                    });
                    
                }
            }


        },
        error: function (data, textStatus) {



        }
    });
}


//Asi mostramos la consulta de el apercibimiento y si hay reincidenia muestra los dos paneles
function Consulta(id, EstatusId) {

    if (EstatusId == 1 || EstatusId == 2) {
        window.location.href = "ConsultaApercibimientoReincidencia.aspx?idApercibimiento=" + id;
    }
}
function ConsultaReincidencia(id, EstatusId, idApercibimiento) {
    //validar con el estatus
    if (EstatusId == 3 || EstatusId == 4) {
        window.location.href = "ConsultaApercibimientoReincidencia.aspx?idApercibimiento=" + idApercibimiento + "&idReincidencia=" + id;
    }
}
//asi mostramos la edicion del apercibimiento
function Edicion(idApercibimiento, EstatusId) {
    if (EstatusId == 1) {
        window.location.href = "NuevoApercibimiento.aspx?idApercibimiento=" + idApercibimiento;
    }
    else if (EstatusId == 2) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ApercibimientoEstaImpugnado",
            dataType: "json",
            data: JSON.stringify({ idApercibimiento: idApercibimiento }),
            success: function (data, textStatus) {
                if (data.d == false) {

                    $("#alertaSimple").modal("show");
                    var texto = document.getElementById("TextoAlertaSimple");
                    texto.innerHTML = "El apercibimiento no puede ser modificado, porque cuenta con una impugnación.";
                }
                else {
                    window.location.href = "NuevoApercibimiento.aspx?idApercibimiento=" + idApercibimiento;
                }


            },
            error: function (data, textStatus) {



            }
        });

    }



}
//Asi mostramos la edicion de la reincidencia
function EdicionReincidencia(idReincidencia, EstatusId, idApercibimiento) {

    if (EstatusId == 3) {
        window.location.href = "NuevaReincidencia.aspx?idReincidencia=" + idReincidencia + "&idApercibimiento=" + idApercibimiento;
    }
    else if (EstatusId == 4) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ReincidenciaEstaImpugnado",
            dataType: "json",
            data: JSON.stringify({ idReincidencia: idReincidencia }),
            success: function (data, textStatus) {
                if (data.d == false) {

                    $("#alertaSimple").modal("show");
                    var texto = document.getElementById("TextoAlertaSimple");
                    texto.innerHTML = "La reincidenciano puede ser modificada, porque cuenta con una impugnación.";
                }
                else {
                    window.location.href = "NuevaReincidencia.aspx?idReincidencia=" + idReincidencia + "&idApercibimiento=" + idApercibimiento;
                }


            },
            error: function (data, textStatus) {



            }
        });
    }
}

//Se muestra la ventana de impugnacion de un apercibimiento
function NuevaImpugnacionApercibimiento(idApercibimiento, EstatusId) {
    
    if (EstatusId == 1) {
        window.location.href = "ImpugnacionApercibimiento.aspx?idApercibimiento=" + idApercibimiento;
    }
    else if (EstatusId == 2) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ApercibimientoImpugnado",
            dataType: "json",
            data: JSON.stringify({ idApercibimiento: idApercibimiento }),
            success: function (data, textStatus) {
                if (data.d == true) {

                    $("#alertaSimple").modal("show");
                    var texto = document.getElementById("TextoAlertaSimple");
                    texto.innerHTML = "Ya existe impugnación para este apercibimiento";
                }
                else {
                    window.location.href = "ImpugnacionApercibimiento.aspx?idApercibimiento=" + idApercibimiento;
                }
            },
            error: function (data, textStatus) {
            }
        });
    }
}
//Se muestra la ventana de edicion de impugnacion de un apercibimiento
function EdicionImpugnacionApercibimiento(idApercibimiento, EstatusId) {
    
    if (EstatusId == 1) {
        $("#alertaSimple").modal("show");
        var texto = document.getElementById("TextoAlertaSimple");
        texto.innerHTML = "No hay impugnación en proceso para editar.";
        
    }
    else if (EstatusId == 2) {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ApercibimientoImpugnado",
            dataType: "json",
            data: JSON.stringify({ idApercibimiento: idApercibimiento }),
            success: function (data, textStatus) {
                if (data.d == true) {

                    window.location.href = "ImpugnacionApercibimiento.aspx?idApercibimiento=" + idApercibimiento;
                }
                else {

                }
            },
            error: function (data, textStatus) {
            }
        });
    }
}

//Se muestra la ventana de consulta de impugnacion de un apercibimiento (Falta funcionalidad para mostrar el historial)
function ConsultaImpugnacionApercibimiento(idApercibimiento, EstatusId) {
    
    if (EstatusId == 1) {
        window.location.href = "ImpugnacionApercibimientoHistorico.aspx?idApercibimiento=" + idApercibimiento + "&banderaImpugnacion=" + 1 + "&banderaRegresar=" + 1;

    }
    else if (EstatusId == 2) {

        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ApercibimientoImpugnado",
            dataType: "json",
            data: JSON.stringify({ idApercibimiento: idApercibimiento }),
            success: function (data, textStatus) {
                if (data.d == true) {
                    //$("#alertaSimple").modal("show");
                    //var texto = document.getElementById("TextoAlertaSimple");
                    //texto.innerHTML = "Aun no esta esta funcionalidad.";
                    window.location.href = "ImpugnacionApercibimientoHistorico.aspx?idApercibimiento=" + idApercibimiento + "&banderaImpugnacion=" + 1 + "&banderaRegresar=" + 1;
                }
                else {
                    $("#alertaSimple").modal("show");
                    var texto = document.getElementById("TextoAlertaSimple");
                    texto.innerHTML = "No hay impugnación por consultar.";
                }
            },
            error: function (data, textStatus) {
            }
        });
    }
}


//Se muestra la ventana de impugnacion de una reincidencia
function NuevaImpugnacionReincidencia(idReincidencia, EstatusId, idApercibimiento ) {
    
    if (EstatusId == 3) {
        window.location.href = "ImpugnacionReincidencia.aspx?idReincidencia=" + idReincidencia + "&idApercibimiento=" + idApercibimiento;
    }
    else if (EstatusId == 4) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ReincidenciaImpugnada",
            dataType: "json",
            data: JSON.stringify({ idReincidencia: idReincidencia }),
            success: function (data, textStatus) {
                if (data.d == true) {

                    $("#alertaSimple").modal("show");
                    var texto = document.getElementById("TextoAlertaSimple");
                    texto.innerHTML = "Ya existe impugnación para esta reincidencia";
                }
                else {
                    window.location.href = "ImpugnacionReincidencia.aspx?idReincidencia=" + idReincidencia + "&idApercibimiento=" + idApercibimiento;
                }
            },
            error: function (data, textStatus) {
            }
        });
    }
}
//Se muestra la ventana de edicion de impugnacion de una reincidencia
function EdicionImpugnacionReincidencia(idReincidencia, EstatusId, idApercibimiento ) {
    
    if (EstatusId == 3) {
        $("#alertaSimple").modal("show");
        var texto = document.getElementById("TextoAlertaSimple");
        texto.innerHTML = "No hay impugnación en proceso para editar.";
    }
    else if (EstatusId == 4) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ReincidenciaImpugnada",
            dataType: "json",
            data: JSON.stringify({ idReincidencia: idReincidencia }),
            success: function (data, textStatus) {
                if (data.d == true) {
                    window.location.href = "ImpugnacionReincidencia.aspx?idReincidencia=" + idReincidencia + "&idApercibimiento=" + idApercibimiento;
                }
                else {

                }
            },
            error: function (data, textStatus) {
            }
        });
    }
}
//Se muestra la ventana de consulta de impugnacion de una reincidencia (Falta funcionalidad para mostrar el historial)
function ConsultaImpugnacionReincidencia(idReincidencia, EstatusId, idApercibimiento ) {
    
    if (EstatusId == 3) {
        window.location.href = "ImpugnacionReincidenciaHistorico.aspx?idReincidencia=" + idReincidencia + "&idApercibimiento=" + idApercibimiento + "&banderaImpugnacion=" + 1 + "&banderaRegresar=" + 2;
    }
    else if (EstatusId == 4) {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ReincidenciaImpugnada",
            dataType: "json",
            data: JSON.stringify({ idReincidencia: idReincidencia }),
            success: function (data, textStatus) {
                if (data.d == true) {
                    //$("#alertaSimple").modal("show");
                    //var texto = document.getElementById("TextoAlertaSimple");
                    //texto.innerHTML = "Aun no esta esta funcionalidad.";
                    window.location.href = "ImpugnacionReincidenciaHistorico.aspx?idReincidencia=" + idReincidencia + "&idApercibimiento=" + idApercibimiento + "&banderaImpugnacion=" + 1 + "&banderaRegresar=" + 2;
                }
                else {
                    $("#alertaSimple").modal("show");
                    var texto = document.getElementById("TextoAlertaSimple");
                    texto.innerHTML = "No hay impugnación por consultar.";
                }
            },
            error: function (data, textStatus) {
            }
        });
    }
}

function ConsultaHistoricoImpugnacionReincidencia(idReincidencia, Estatus, idApercibimiento) {
    
    alert('1')
    if (Estatus == "Reincidente") {
        $("#alertaSimple").modal("show");
        var texto = document.getElementById("TextoAlertaSimple");
        texto.innerHTML = "No hay impugnación por consultar.";
    }
    else if (Estatus == "Reincidente con Inmpugnación") {
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "ApercibidosYReincidentes.aspx/ReincidenciaImpugnada",
            dataType: "json",
            data: JSON.stringify({ idReincidencia: idReincidencia }),
            success: function (data, textStatus) {
                if (data.d == true) {
                    //$("#alertaSimple").modal("show");
                    //var texto = document.getElementById("TextoAlertaSimple");
                    //texto.innerHTML = "Aun no esta esta funcionalidad.";
                    window.location.href = "ImpugnacionReincidenciaHistorico.aspx?idReincidencia=" + idReincidencia + "&idApercibimiento=" + idApercibimiento;
                }
                else {

                }
            },
            error: function (data, textStatus) {
            }
        });
    }
}


