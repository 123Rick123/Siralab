<%@ Page Title="" Language="C#" MasterPageFile="~/Application.Master" AutoEventWireup="true" CodeBehind="AnalisisParametro.aspx.cs" Inherits="SIRALAB.AnalisisParametro" ClientIDMode="AutoID"
    Culture="es-MX" UICulture="es-MX" %>

<%@ Register Src="Controles/wucMuestreador.ascx" TagName="wucMuestreador" TagPrefix="uc1" %>
<%@ Register Src="Controles/wucMuestraDatos.ascx" TagName="wucMuestraDatos" TagPrefix="uc2" %>
<%@ Register Src="~/FirmaElectronica/wucAppletFIEL.ascx" TagPrefix="uc1" TagName="wucAppletFIEL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .viewGrid tbody tr td {
            vertical-align: middle !important;
            text-align: center !important;
            font-size: 14px !important;
        }

        .viewGrid th {
            vertical-align: middle !important;
            text-align: center !important;
            font-size: 14px;
        }

        .container {
            width: auto;
            max-width: 88%;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="updOrdenTrabajo" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class=" container">
                <div class="row">
                    <div class="col-sm-1">
                        <br />
                        <asp:LinkButton ID="lnkbtnBack" runat="server" OnClick="lnkbtnBack_Click" CssClass="btn btn-default btn-ms" ToolTip="Regresar a la pagina anterior" CausesValidation="false">
                                      <span class="glyphicon glyphicon-chevron-left"></span>
                        </asp:LinkButton>
                    </div>
                    <div class="col-sm-6">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6">
                    <h2>Datos de la muestra</h2>
                    <hr class="red">
                    <div class="col-sm-6">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-md-offset-9">
                    <h6>
                        <asp:Label ID="lblFolio" runat="server"></asp:Label></h6>
                </div>
            </div>

            <asp:Panel ID="pnlTitulo" runat="server" Style="width: 800px; height: 10px;">
            </asp:Panel>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h5><span>Muestra compuesta</span></h5>
                        </div>
                        <div class="panel-body">
                            <uc2:wucMuestraDatos ID="wucMuestraDatos1" runat="server"
                                OnGuardar="wucMuestraDatos1_Guardar"
                                OnTipoMuestra="wucMuestraDatos1_TipoMuestra"
                                OnDatosIncompletos="wucMuestraDatos1_DatosIncompletos"
                                OnSimple_DatosIncompletos="wucMuestraDatos1_Simple_DatosIncompletos"
                                OnCompuesta_DatosIncompletos="wucMuestraDatos1_Compuesta_DatosIncompletos"
                                OnGuardaParametros="wucMuestraDatos1_GuardaParametros"
                                OnRecalculaCompuesta="wucMuestraDatos1_RecalculaCompuesta"
                                OnGuardadoSimple="wucMuestraDatos1_GuardadoSimple" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default" id="liSignatario" runat="server">
                        <div class="panel-heading">
                            <h5><span>Datos del Signatario</span></h5>
                        </div>
                        <div class="panel-body">
                            <div class="table-responsive">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <uc1:wucMuestreador ID="wucMuestreador1" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            </ul>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <uc1:wucAppletFIEL runat="server" ID="wucAppletFIEL" OnFirmarColeccion="wucAppletFIEL_OnFirmarColeccion" />

    <div id="mdlCheckbox" class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="divModalHeader">
                    <h5 class="modal-title">Verifique los datos.</h5>
                </div>
                <div class="modal-body" id="divMensajes">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

     <div id="modalConfirm" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <h4 class="modal-title"><label id="tituloConfirm"></label></h4>
                </div>
                <div class="modal-body text-center">
                    <p id="cuerpoConfirm"> </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancelar</button>
                    <button id="btnAceptarModalConfirm" type="button" class="btn btn-primary">Aceptar</button>
                </div>
            </div>
        </div>
    </div>

     <div id="modalMensajeNomSSTDQO"  class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document" style="width: 30%;">
            <div class="modal-content">
                <div class="modal-header" id="divModalHeaderSSTDQO">
                    <h5 class="modal-title">Mensaje del sistema</h5>
                </div>
                <div class="modal-body" id="divMensajesSSTDQO" style="text-align:justify">
                   <p id="lblMensajeModalNomSSTDQO" runat="server">El análisis no puede ser firmado debido a que se encuentra fuera de la vigencia de la norma.</p> 
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

      <div id="modalMensajeCloruros"  class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document" style="width: 30%;">
            <div class="modal-content">
                <div class="modal-header" id="divModalHeaderCloruros">
                    <h5 class="modal-title">Mensaje del sistema</h5>
                </div>
                <div class="modal-body" id="divMensajesCloruros" style="text-align:justify">
                   <p id="mensajeCloruros" runat="server">El análisis puede ser firmados debido a que es necesario validar lo correspondiente a cloruros.</p> 
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

        <div id="modalMensajeConductividad"  class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document" style="width: 30%;">
            <div class="modal-content">
                <div class="modal-header" id="divModalHeaderConductividad">
                    <h5 class="modal-title">Mensaje del sistema</h5>
                </div>
                <div class="modal-body" id="divMensajesConductividad" style="text-align:justify">
                   <p id="Conductividad" runat="server">El análisis puede ser firmados debido a que es necesario validar lo correspondiente a conductividad eléctrica.</p> 
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

  

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitializeHndMC);

        function pageLoad() {
            InitializeHndMC();
        }

        var accionEjecutar = '';
        var accionConfirm = '';
        var guardarSSTDQOCOT = 'guardarSSTDQOCOT';

        function InitializeHndMC() {
  
            if (!Math.sign) {
                Math.sign = function (x) {
                    // Si x es NaN, el resultado es NaN.
                    // Si x es -0, el resultado es -0.
                    // Si x es +0, el resultado es +0.
                    // Si x es negativo y no -0, el resultado es -1.
                    // Si x es positivo y no +0, el resultado es +1.
                    x = +x; // convertir a número
                    if (x === 0 || isNaN(x)) {
                        return Number(x);
                    }
                    return x > 0 ? 1 : -1;
                };
            }
            if (!Math.log10) {
                Math.log10 = function (x) {
                    return Math.log(x) * Math.LOG10E;
                };
            }


            //RFD 20/08/2020 -> TAB Index.
            var indice = 0;
            $("input[id*='txtLimite']").each(function () {
                $(this).attr("tabindex", indice++);
            });

            $("input[id*='txtResultado']").each(function () {
                $(this).attr("tabindex", indice++);
            });

            $("[id*='Resultado']").each(function () {
                if (this.id.includes('txtResultado'))
                    $(this).attr("tabindex", indice++);
                else if (this.id.includes('ddlResultado'))
                    $(this).attr("tabindex", indice++);
            });

            $("input[id*='txtLC']").each(function () {
                $(this).attr("tabindex", indice++);
            });

            $("select[id*='ddlLaboratorio']").each(function () {
                $(this).attr("tabindex", indice++);
            });
            $("select[id*='ddlMetodo']").each(function () {
                $(this).attr("tabindex", indice++);
            });

            indice = 0;



            //Establece valores al cargar.
            $("input[data-scale]").each(function () {
                var value = parseFloat($(this).val());
                var scale = parseFloat($(this).attr("data-scale"));
                value = isNaN(value) ? 0 : value;

                roundValue = $(this).data("truncamiento") === "True" ? truncate(value, scale) : roundNumber(value, scale);
                $(this).val(Number(roundValue).toFixed(scale));

            });

            $("input[data-scale]").unbind("keypress").keypress(function (event) {
                if (isNumber(event, this)) {
                    return true;
                }
                return false;
            });


            $("input[data-scale]").on("blur", function () {
                var value = parseFloat($(this).val());
                value = isNaN(value) ? 0 : value;
                var scale = parseFloat($(this).attr("data-scale"));
                var roundValue = roundNumber(value, scale);
                roundValue = Number(roundValue).toFixed(scale);
                var minAceptado = $(this).data("minaceptado");
                var maxAceptado = $(this).data("maxaceptado");
                $(this).val(roundValue);
                if (minAceptado != undefined && parseFloat(value) < parseFloat(minAceptado)) {
                    $(this).parent().addClass("has-error");
                    $(this).parent().find("[id*='spnMinMax']").show();
                    $(this).select();
                    $(this).focus();
                }
                else if (maxAceptado != undefined && parseFloat(value) > parseFloat(maxAceptado)) {
                    $(this).parent().addClass("has-error");
                    $(this).parent().find("[id*='spnMinMax']").show();
                    $(this).select();
                    $(this).focus();
                } else {
                    $(this).parent().removeClass("has-error");
                    $(this).parent().find("[id*='spnMinMax']").hide();
                }
            });


            $("input[id*='txtResultado'][data-grupoid]").each(
                function () {
                    var rompeLoop = false;
                    if ($("[id*='txtResultado'][data-grupoid=" + $(this).data('grupoid') + "][data-noanalizado='true']").size() > 0) {
                        $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val("---");
                        rompeLoop = true;
                    }

                    if (!rompeLoop) {
                        var suma = 0;
                        $("[id*='txtResultado'][data-grupoid=" + $(this).data('grupoid') + "]").each(function () {
                            valor = parseFloat($(this).val() == "" ? 0 : $(this).val());
                            if (valor == 0) {
                                $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val("---");
                                rompeLoop = true;
                                return false;
                            }
                            suma += valor;
                        });
                    } else
                        return false;

                    if (!rompeLoop) {
                        $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val(suma > 0 ? Number(suma).toFixed($(this).data('scale')) : Number($("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val()).toFixed($(this).data('scale')));
                    } else
                        return false;
                });

            $("input[id*='txtResultado'][data-grupoid]").on("keyup",
                function () {
                    var rompeLoop = false;
                    if ($("[id*='txtResultado'][data-grupoid=" + $(this).data('grupoid') + "][data-noanalizado='true']").size() > 0) {
                        $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val("---");
                        rompeLoop = true;
                    }

                    if (!rompeLoop) {
                        var suma = 0;
                        $("[id*='txtResultado'][data-grupoid=" + $(this).data('grupoid') + "]").each(function () {
                            valor = parseFloat($(this).val() == "" ? 0 : $(this).val());
                            if (valor == 0) {
                                $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val("---");
                                rompeLoop = true;
                                return false;
                            }
                            suma += valor;
                        });
                    } else
                        return false;

                    if (!rompeLoop) {
                        $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val(suma > 0 ? Number(suma).toFixed($(this).data('scale')) : Number($("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val()).toFixed($(this).data('scale')));
                    } else
                        return false;
                });

            $("input[id*='txtResultado'][data-grupoid]").on("blur",
                function () {
                    var rompeLoop = false;
                    if ($("[id*='txtResultado'][data-grupoid=" + $(this).data('grupoid') + "][data-noanalizado='true']").size() > 0) {
                        $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val("---");
                        rompeLoop = true;
                    }

                    if (!rompeLoop) {
                        var suma = 0;
                        $("[id*='txtResultado'][data-grupoid=" + $(this).data('grupoid') + "]").each(function () {
                            valor = parseFloat($(this).val() == "" ? 0 : $(this).val());
                            if (valor == 0) {
                                $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val("---");
                                rompeLoop = true;
                                return false;
                            }
                            suma += valor;
                        });
                    } else
                        return false;

                    if (!rompeLoop) {
                        $("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val(suma > 0 ? Number(suma).toFixed($(this).data('scale')) : Number($("[id*='txtResultado'][data-grupo-padre=" + $(this).data('grupoid') + "]").val()).toFixed($(this).data('scale')));
                    } else
                        return false;
                });

            $("[id*='lnkbtnGuardarPmtos']").off().on("click",function (e) {
                e.stopPropagation();

                var mensaje = '<ul class = "text-danger text-justify">';
                var enviar = true;

                $("input[id*='txtResultado'][data-minaceptado]").each(function () {
                    var noValidaLimites = $(this).data("novalidalimites");
                    if (!noValidaLimites) {
                        var valor = $(this).val();
                        var min = $(this).data("minaceptado");
                        valor = isNaN(valor) ? 0 : parseFloat(valor);

                        if (valor < parseFloat(min)) {
                            $(this).select();
                            $(this).focus();
                            $(this).parent().addClass("has-error");
                            $(this).parent().find("[id*='spnMinMax']").show();
                            enviar = false;
                            return false;
                        }
                    }
                });

                $("input[id*='txtResultado'][data-maxaceptado]").each(function () {
                    var noValidaLimites = $(this).data("novalidalimites");
                    if (!noValidaLimites) {
                        var valor = $(this).val();
                        var max = $(this).data("maxaceptado");
                        valor = isNaN(valor) ? 0 : parseFloat(valor);

                        if (valor > parseFloat(max)) {
                            $(this).select();
                            $(this).focus();
                            $(this).parent().addClass("has-error");
                            $(this).parent().find("[id*='spnMinMax']").show();
                            enviar = false;
                            return false;
                        }
                    }
                });

                if (!enviar)
                    e.preventDefault();

                if (enviar) {
                    $("input[id*='txtResultado'][data-parametroid]").each(function () {
                        var parametroId = $(this).data('parametroid');
                        $("#divModalHeader").empty();
                        $("#divModalHeader").append("<h5>Verifique los datos</h5>");
                        $("#divMensajes").empty();

                        //Validar límite inferior
                        if ($("input[id*='txtLimiteInferior'][data-parametroid=" + parametroId + "]").size() > 0) {

                            if ($("input[id*='txtLimiteInferior'][data-parametroid=" + parametroId + "]").val() == "") {
                                mensaje += "<li>Introduzca un límite inferior para el parámetro  " + $("[data-etiquetaid=" + parametroId + "]").html() + ".</li>";
                                enviar = false;
                            }

                            if (parseFloat(("input[id*='txtLimiteInferior'][data-parametroid=" + parametroId + "]").val()) < 0) {
                                mensaje += "<li>El límite inferior de " + $("[data-etiquetaid=" + parametroId + "]").html() + "  no puede ser negativo.</li>";
                                enviar = false;
                            }

                        }

                        //Validar límite superior.
                        if ($("input[id*='txtLimiteSuperior'][data-parametroid=" + parametroId + "]").size() > 0) {
                            if ($("input[id*='txtLimiteSuperior'][data-parametroid=" + parametroId + "]").val() == "") {
                                mensaje += "<li>Introduzca un límite superior para el parámetro  " + $("[data-etiquetaid=" + parametroId + "]").html() + ".</li>";
                                enviar = false;
                            }

                            if (parseFloat($("input[id*='txtLimiteSuperior'][data-parametroid=" + parametroId + "]").val()) < 0) {
                                mensaje += "<li>El límite superior de " + $("[data-etiquetaid=" + parametroId + "]").html() + "  no puede ser negativo.</li>";
                                enviar = false;
                            }
                        }

                        //Validar resultado
                        if ($(this).val() == "") {
                            mensaje += "<li>Introduzca un resultado para el parámetro  " + $("[data-etiquetaid=" + parametroId + "]").html() + ".</li>";
                            enviar = false;
                        }

                        if (parseFloat($(this).val()) < 0) {
                            mensaje += "<li>El resultado de " + $("[data-etiquetaid=" + parametroId + "]").html() + "  no puede ser negativo.</li>";
                            enviar = false;
                        }

                        //Validar LCM
                        if ($("input[id*='txtLC'][data-parametroid=" + parametroId + "]").size() > 0) {

                            if ($("input[id*='txLC'][data-parametroid=" + parametroId + "]").val() == "") {
                                mensaje += "<li>Introduzca un LC/MC para el parámetro  " + $("[data-etiquetaid=" + parametroId + "]").html() + ".</li>";
                                enviar = false;
                            }

                            var lcm = parseFloat($("input[id*='txtLC'][data-parametroid=" + parametroId + "]").val());
                            if (lcm < 0) {
                                mensaje += "<li>El LC/MC de " + $("[data-etiquetaid=" + parametroId + "]").html() + "  no puede ser negativo.</li>";
                                enviar = false;
                            }


                            if (parseFloat($(this).val()) < lcm) {
                                mensaje += "<li>El valor de " + $("[data-etiquetaid=" + parametroId + "]").html() + "  no puede ser menor que su LC/MC.</li>";
                                enviar = false;
                            }
                        }

                    });

                    //Validacion para mensaje NomSSTDQOCOT
                    
                    var COT = $("input[id*='chkCOT']");
                    var DQO = $("input[id*='chkDQO']")

                    if ((COT.length > 0 && DQO.length > 0)) {
                        var TipoDeDato = "";
                        if (DQO[0].checked == true) {
                            TipoDeDato = "DQO";
                        }
                        else if (COT[0].checked == true) {
                            TipoDeDato = "COT";
                        }
                        else {
                            mensaje += "<li>El valor de " + $("[data-etiquetaid=" + parametroId + "]").html() + "  no puede ser menor que su LC/MC.</li>";
                            enviar = false;
                        }
                        if (enviar) {
                            if (accionEjecutar == '') {
                                accionConfirm = guardarSSTDQOCOT;
                                ejecutarModalConfirm("Mensaje de Sistema", "¿Desea confirmar que la muestra se guardará con el parámetro " + TipoDeDato +"?");
                            }
                            if (accionEjecutar == guardarSSTDQOCOT) {
                                accionEjecutar = '';
                               
                            }
                            else {
                                
                                return false;
                            }

                        }

                    }



                    //-------------------------------------------
                    if (!enviar) {
                        mensaje += "</ul>";
                        $("#divMensajes").append(mensaje);
                        $("#mdlCheckbox").modal();
                        e.preventDefault();
                        return false;
                    }
                }
           
            }); 

        }

        function ejecutarModalConfirm(encabezado, cuerpo) {
            /*  $('#modalConfirm').modal('toggle');*/
            $('#modalConfirm').modal("show");

            $("#tituloConfirm").html(encabezado);
            $("#cuerpoConfirm").html(cuerpo);
        }

        $('#btnAceptarModalConfirm').click(function () {
            
            if (accionConfirm == guardarSSTDQOCOT) {
                accionEjecutar = guardarSSTDQOCOT;
                accionConfirm = '';
                $('#modalConfirm').modal('hide');
                $("[id*='lnkbtnGuardarPmtos']")[0].click();
            }
        });

        function roundNumber(num, scale) {
            if (Math.round(num) != num) {
                if (Math.pow(10, -scale) > num) {
                    return 0;
                }
                var sign = Math.sign(num);
                var arr = ("" + Math.abs(num)).split(".");
                if (arr.length > 1) {
                    if (arr[1].length > scale) {
                        var integ = +arr[0] * Math.pow(10, scale);
                        var dec = integ + (+arr[1].slice(0, scale) + Math.pow(10, scale));
                        var proc = +arr[1].slice(scale, scale + 1);
                        if (proc >= 5) {
                            dec = dec + 1;
                        }
                        dec = sign * (dec - Math.pow(10, scale)) / Math.pow(10, scale);
                        return dec;
                    }
                }
            }
            return num;
        }

        function isNumber(evt, element) {

            var charCode = (evt.which) ? evt.which : event.keyCode;
            var section = $(element).val().split('.');
            var scale = parseInt($(element).attr("data-scale"));

            if (isTextSelected(element) && charCode == 46 && $(element).data('scale') > 0)
                return true;

            if ((charCode !== 45 || $(element).val().indexOf('-') !== -1) &&
                (charCode !== 46 || $(element).val().indexOf('.') !== -1 || (scale === 0 && $(element).val().indexOf('.') !== 1)) &&
                (charCode < 48 || charCode > 57))
                return false;
            if (section[0].length <= 10) {
                if (scale)
                    if (section.length > 1) {
                        var count = section[0].length + 1;
                        if (evt.originalEvent.srcElement.selectionStart >= count)
                            return section[1].length < scale;
                    }
                return true;
            }
            else return false;
        }

        function truncate(valor, scale) {
            var arr = ("" + Math.abs(valor)).split(".");
            if (arr.length > 1) {
                var parteDecimal = arr[1].substring(0, scale);
                return parseFloat(arr[0] + "." + parteDecimal).toFixed(scale);
            } else {
                return parseFloat(valor).toFixed(scale);
            }
        }

        function isTextSelected(input) {
            if (typeof input.selectionStart == "number") {
                return input.selectionStart == 0 && input.selectionEnd == input.value.length;
            } else if (typeof document.selection != "undefined") {
                input.focus();
                return document.selection.createRange().text == input.value;
            }
        }
    </script>
</asp:Content>
