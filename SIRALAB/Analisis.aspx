<%@ Page Title="" Language="C#" MasterPageFile="~/Application.Master" AutoEventWireup="true" CodeBehind="Analisis.aspx.cs" Inherits="SIRALAB.Analisis" ClientIDMode="AutoID"
    Culture="es-MX" UICulture="es-MX" EnableEventValidation="false" %>

<%@ Register Src="Controles/wucMuestraSimple.ascx" TagName="wucMuestraSimple" TagPrefix="uc7" %>
<%@ Register Src="Controles/wucDatosMuestra.ascx" TagName="wucDatosMuestra" TagPrefix="uc6" %>
<%@ Register Src="Controles/wucMuestraCompuesta.ascx" TagName="wucMuestraCompuesta" TagPrefix="uc1" %>
<%--<%@ Register Src="Controles/wucEvidencias.ascx" TagName="wucEvidencias" TagPrefix="uc2" %>--%>
<%@ Register Src="~/Controles/wucPopFiles.ascx" TagPrefix="uc1" TagName="wucPopFiles" %>
<%@ Register Src="Controles/AnexoE/_wucMuestraSiempleAnexoE.ascx" TagName="_wucMuestraSiempleAnexoE" TagPrefix="uc10" %>
<%@ Register Src="Controles/wucMuestreador.ascx" TagName="wucMuestreador" TagPrefix="uc11" %>
<%@ Register Src="~/Controles/wucMuestraXML.ascx" TagPrefix="uc1" TagName="wucMuestraXML" %>
<%@ Register Src="~/Controles/AnexoE/wucMuestrasSimples.ascx" TagPrefix="uc1" TagName="wucMuestrasSimples" %>
<%@ Register Src="~/Controles/AnexoE/wucAgregarParametroAnalisis.ascx" TagPrefix="uc1" TagName="wucAgregarParametroAnalisis" %>
<%@ Register Src="~/Controles/wucConductividadElectrica.ascx" TagPrefix="uc1" TagName="wucConductividadElectrica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

    <link href="style/inicio.css" rel="stylesheet" />
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />
    <link href="style/botones.css" rel="stylesheet" type="text/css" />
    <script src="js/ValidadorRequerido.js"></script>
   

    <script>
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {

                transformaFechaFromServer();

                $.datepicker.regional.es = {
                    closeText: 'Cerrar',
                    prevText: 'Ant',
                    nextText: 'Sig',
                    currentText: 'Hoy',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                    dayNamesMin: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'S&aacute;b'],
                    weekHeader: 'Sm',
                    dateFormat: 'dd/mm/y',
                    firstDay: 1,
                    isRTL: false,
                    showMonthAfterYear: false,
                    yearSuffix: ''
                };
                $.datepicker.setDefaults($.datepicker.regional.es);
                $('[id*=txtFecha]').datepicker();
                $('[data-toggle="tooltip"]').tooltip();
                $('[id*=txtHora]').datetimepicker({
                    format: 'HH:mm'
                });
            }

            if ($("select[id*='DDDatum']").val() == 0) {
                $("select[id*='DDDatum']").val(1);
            }
        });
    </script>
    <style type="text/css">
        .data-table {
            border-collapse: collapse;
            border: 1px solid #E2E2E2;
        }

            .data-table > tbody > tr > td, .data-table > tbody > tr > th {
                border: 1px solid #E2E2E2;
                padding: 3px;
            }

            .data-table input[type='text'], .data-table select, .data-table textarea {
                width: 100%;
                box-sizing: content-box;
            }

            .data-table > tbody > tr:nth-child(even) {
                background: #F2F2F2;
            }

            .data-table > tbody > tr > th {
                background: #CCC;
            }

        .container {
            width: auto;
            max-width: 88%;
        }

        input {
            font-size: 12px;
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel ID="upGrid" runat="server" ChildrenAsTriggers="False" UpdateMode="Conditional">
        <ContentTemplate>
            <%--Título de la pantalla--%>
            <div class="row">
                <div class="col-sm-1">
                    <asp:LinkButton ID="lnkbtnBack" runat="server" OnClick="lnkbtnBack_Click" CssClass="btn btn-default centrado" ToolTip="Regresar a la pagina anterior">   
                        <span class="glyphicon glyphicon-chevron-left"></span>                    
                    </asp:LinkButton>
                </div>
                <div class="col-sm-11">
                    <h2>Registro de análisis</h2>
                    <hr class="red">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-md-offset-9">
                    <h6>
                        <asp:Label ID="lblFolio" runat="server"></asp:Label>
                    </h6>
                </div>
            </div>

            <%--Información del punto de muestreo: wucDatosMuestra --> uc6--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h5><span>Punto de muestreo</span></h5>
                        </div>
                        <div class="panel-body">
                            <uc6:wucDatosMuestra ID="wucDatosMuestra1" runat="server" OnGuardar="wucDatosMuestra_Guardar" />
                        </div>
                    </div>
                </div>
            </div>

            <%--Información del signatario: wucMuestrador--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default" id="pnlSignatario" runat="server">
                        <div class="panel-heading">
                            <h5><span>Datos del signatario</span></h5>
                        </div>
                        <div class="panel-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <uc11:wucMuestreador ID="wucMuestreador1" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>

            <%--Datos conductividad electrica--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel panel-default" id="pnlConductividadElectrica" runat="server">
                        <div class="panel-heading">
                            <h5><span>Conductividad Eléctrica</span></h5>
                        </div><div class="panel-body">
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <uc1:wucConductividadElectrica ID="wucConductividadElectrica1" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        
                    </div>
                </div>
            </div>
            <%--wucMuestraXML--%>
            <div class="row" style="display: none">
                <div class="col-md-12">
                    <uc1:wucMuestraXML runat="server" ID="wucMuestraXML" />
                </div>
            </div>

            <%--Captura de muestra simple: wucMuestraSimple --> uc7--%>
            <div class="row">
                <div class="col-md-12">
                    <uc7:wucMuestraSimple ID="wucMuestraSimple1" runat="server" OnGuardar="wucMuestraSimple1_Guardar" OnEvidencia="wucMuestra_Evidencia" OnMSIncompleta="wucMuestra_MSIncompleta" />
                </div>
            </div>
            <div class="row" id="li1" runat="server" style="list-style: none;">
                <div class="col-md-12">
                    <div class="panel panel-default" id="pnlMuestraSimple" runat="server">
                        <div class="panel-heading">
                            <h5>
                                <asp:Label runat="server" ID="lblTtituloPanelMuestraSimple" CssClass="form-text"></asp:Label></h5>
                        </div>
                        <div class="panel-body">
                            <%--Aquí carga la tabla--%>
                            <uc1:wucMuestrasSimples runat="server" ID="wucMuestrasMultiple" />
                        </div>
                        <div class="panel-footer">
                            <div id="divMensajeAvisoGuardar" class="row">
                                <div class="col-lg-12 text-right">
                                    <asp:Label runat="server" ID="lbGuardarSimples" Text="Previamente a pulsar el botón Generar compuesta debe presionar el botón Guardar." CssClass="form-text"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12 text-right">
                                    <asp:LinkButton ID="lnkGuardarSimples" runat="server" OnClientClick="return false;" data-guardar Text="Guardar" CssClass="btn btn-primary"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkGenerarCompuesta" runat="server" OnClick="lnkGenerarCompuesta_OnClick" CssClass="btn btn-default classLnkGenerarCompuesta">Generar compuesta</asp:LinkButton>
                                    <asp:LinkButton ID="lnkSendSaveMessage" runat="server" OnClick="lnkSendSaveMessage_Click" CssClass="hidden"></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <uc1:wucMuestraCompuesta ID="wucMuestraCompuesta1" runat="server" OnEvidencia="wucMuestra_Evidencia" />
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>

    <%--<uc2:wucEvidencias ID="wucEvidencias1" runat="server" />--%>
    <div hidden>
        <asp:UpdatePanel ID="pnlEvidenciaPop" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="wucPopFiles" EventName="Cerrar" />
            </Triggers>
            <ContentTemplate>
                <uc1:wucPopFiles runat="server" ID="wucPopFiles" OnCerrar="wucEvidenciaPop1_Cerrar" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

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

    <!-- Modal de confirmación de generación de muestra compuesta -->
    <div class="modal fade" id="myModal" role="dialog" data-backdrop="static" data-keyboard="false" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h5 class="modal-title">
                                <asp:Label ID="lblModalTitle" runat="server" Text=""></asp:Label></h5>
                        </div>
                        <div class="modal-body">
                            <p>
                                <asp:Label ID="lblModalBody" runat="server" CssClass="text-success" Text=""></asp:Label>
                            </p>
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-secondary" data-dismiss="modal" aria-hidden="true">Cerrar</button>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
<%--    Modal Conductividad--%>
    <div id="modalMensajeConductividad"  class="modal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-lg" role="document" style="width: 30%;">
            <div class="modal-content">
                <div class="modal-header" id="divModalHeaderConductividad">
                    <h5 class="modal-title">Verifique los datos</h5>
                </div>
                <div class="modal-body" id="divMensajesConductividad" runat="server" style="text-align:justify">
                   <p id="lblMensajeModalConductividad" runat="server">Debe guardar las muestras simples antes de generar la muestra compuesta</p> 
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>
    <script src="js/moment.js"></script>
    <link href="style/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <script src="js/jquery-ui-datepicker.js"></script>
    <script src="js/bootstrap-datetimepicker.min.js"></script>
    <script src="js/decimal.js"></script>
    <script src="js/jquery.flexslider.js"></script>
    <script src="js/wuc_Evidencias.js"></script>
    <script type="text/javascript">
        $(function () {
            Decimal.precision = 50;
            if ($("#tablaMuestraSimple").size() > 0) {
                var x = document.getElementById("tablaMuestraSimple").rows[0].cells;
                $("#mnsValidaHojaCampo").attr("colSpan", x.length - 4);
                $("#tdConformacionMuestraCompuesta").attr("colSpan", x.length - 4);
                
                if (($("input[id*='hfNormaId']").val() == "" ? $("input[id*='hfNormaId']")[1].value : $("input[id*='hfNormaId']").val()) == 4) {
                    $(".anchoSelects").attr('colSpan', 1);
                    $("input[id*='txtHora']").val("11:35");
                    $("#tablaMuestraSimple").css("width", "850px");
                    $("#tablaMuestraSimple").css("margin", "auto");
                }
            }
        });
    </script>

    <script type="text/javascript">
        var OperacionesResumen = {
            Sumatoria: function (selector) {
                var resultado = new Decimal(0);
                $(selector).each(function (i, e) {
                    var value = new Decimal($(this).val());
                    if (isNaN(value)) value = 0;
                    resultado = resultado.plus(value);
                });

                return resultado.valueOf();
            },
            Promedio: function (selector) {
                var resultado = new Decimal(0);
                var numerador = new Decimal(OperacionesResumen.Sumatoria(selector));
                var divisor = 0;
                $(selector).each(function () {
                    var value = parseFloat($(this).val());
                    if (!isNaN(value) && value > 0) {
                        divisor++;
                    }
                });

                if (divisor !== 0 && !isNaN(divisor))
                    resultado = numerador.div(divisor);

                return resultado.valueOf();
            }
        };

        var tablaEditada = false;

        function InitializeMS() {

            //Inicio asignación tabindex.
            var posicion = 1;
            $("input[id*='txtFecha'][data-posicion-fecha]").each(function () {
                $(this).attr('tabindex', posicion++);
            });


            $("input[id*='txtHora'][data-posicion-hora]").each(function () {
                $(this).attr('tabindex', posicion++);
            });

            $("input[id*='txtCaudal'][data-posicion-caudal]").each(function () {
                $(this).attr('tabindex', posicion++);
            });

            var parametrosIdArray = [];

            $(".capturaParametro").each(function () {
                if ($("#" + this.id).data('parameter-id') != '' && $("#" + this.id).data('parameter-id') != undefined) {
                    parametrosIdArray.push($("#" + this.id).data('parameter-id'));
                }
            });

            var arrayParametrosFiltrados = new Set();
            for (let parametro of parametrosIdArray) {
                arrayParametrosFiltrados.add(parametro);
            }

            for (let parametro of arrayParametrosFiltrados) {
                $('input[id*=txtValue][data-parameter-id="' + parametro + '"]').each(function () {
                    if ($(this).data("data-lcmc") == undefined) {
                        $(this).attr("tabindex", posicion++);
                    }
                });
                $('input[id*=txtLcmc][data-parameter-id="' + parametro + '"][data-lcmc]').each(function () {
                    $(this).attr("tabindex", posicion++);
                });
                $('select[id*=ddlValue][data-parameter-id="' + parametro + '"]').each(function () {
                    $(this).attr("tabindex", posicion++);
                });
            }

            $("input[id*='txtObservaciones'][data-observacion]").each(function () {
                $(this).attr('tabindex', posicion++);
            });


            $("[data-volumen-compuesta]").each(function () {
                $(this).attr('tabindex', posicion++);
            });

            $("[data-lmp-parametroid]").each(function () {
                $(this).attr("tabindex", posicion++);
            });

            for (let parametro of arrayParametrosFiltrados) {
                $('select[id*=ddlLaboratorio][data-parameter-id="' + parametro + '"]').each(function () {
                    $(this).attr("tabindex", posicion++);
                });
                $('select[id*=ddlMetodo][data-parameter-id="' + parametro + '"]').each(function () {
                    $(this).attr("tabindex", posicion++);
                });
                $('select[id*=ddlAccion][data-parameter-id="' + parametro + '"]').each(function () {
                    $(this).attr("tabindex", posicion++);
                });
            }

            $("[data-fecha-conformacion]").each(function () {
                $(this).attr("tabindex", posicion++);
            });
            $("[data-hora-conformacion]").each(function () {
                $(this).attr("tabindex", posicion++);
            });
            $("[data-fecha-compuesta]").each(function () {
                $(this).attr("tabindex", posicion++);
            });
            $("[data-hora-compuesta]").each(function () {
                $(this).attr("tabindex", posicion++);
            });

            //Fin asignación tabindex

            $("input[type='text'][data-scale]").each(function () {
                var value = parseFloat($(this).val());
                var scale = parseFloat($(this).attr("data-scale"));
                value = isNaN(value) ? 0 : value;
                var roundValue = Number(roundNumber(value, scale)).toFixed(scale);
                $(this).val(roundValue);

            });



            $("input[id*='txtCaudal'][data-calc=caudal]").each(function () {
                var id = this.id;
                var elemento = $("#" + id).closest("tr");

                if (isNaN($("#" + id).val()) || $("#" + id).val() == "" || parseFloat($("#" + id).val()) == 0) {
                    $("input[data-parameter-id], select[data-parameter-id]", elemento).attr("disabled", "disabled");
                    $("input[data-parameter-id]", elemento).each(function () {
                        $("#" + this.id).val(Number(0).toFixed($("#" + this.id).data('scale')));
                    });

                    $("#" + id).removeAttr("disabled");

                    $("select[data-parameter-id]", elemento).each(function () {
                        if ($("#" + this.id).data('parameter-id') == 85 && $("#" + this.id).data('accion') == undefined) {
                            $("#" + this.id).val('true');
                        }
                    });
                } else {
                    $("input[data-parameter-id], select[data-parameter-id]", elemento).removeAttr("disabled");
                    $("input[data-parameter-id], select[data-parameter-id]", elemento).each(function () {
                        if (parseInt($("select[id*='ddlAccion'][data-parameter-id='" + $("#" + this.id).data('parameter-id') + "']").val()) != 0) {
                            $("#" + this.id).attr("disabled", "disabled");
                        }
                    });
                }
            });



            if ($("input[id*='txtVolumen']").val() == 0) {
                $("input[id*='txtVolumen']").val(0.00);
                $("span[data-volumen-result-total]").text(Number(0).toFixed(2));
            }

            $("select[id*='ddlAccion']").each(function () {
                if ($(this).val() != 0) {
                    $("input[data-parameter-id='" + $(this).data('parameter-id') + "']").each(function () {
                        var escalaDecimal = $("#" + this.id).data('scale');
                        $(this).attr("disabled", "disabled");
                        $(this).val(Number(0).toFixed(escalaDecimal));
                        $(this).parent().children("span").hide();
                        $(this).parent().removeClass("has-error");

                    });
                    $("[data-lmp-parametroid='" + $(this).data("lmp-parametroid") + "']").each(function () {
                        var escalaDecimal = $("#" + this.id).data('scale');
                        $(this).attr("disabled", "disabled");
                        $(this).val(Number(0).toFixed(escalaDecimal));
                    });

                    $("select[data-parameter-id='" + $(this).data('parameter-id') + "']").each(function () {
                        $(this).attr("disabled", "disabled");

                        if ($("#" + this.id).data('laboratorio-id') != undefined && $("#" + this.id).data('accion') == undefined) {
                            $("#" + this.id + " option[value=0]").attr("selected", "selected");
                        } else if ($("#" + this.id).data('parameter-id') == 85 && $("#" + this.id).data('accion') == undefined) {
                            $("#" + this.id).val('true');
                        }
                    });
                    $(this).removeAttr("disabled");
                }
            });


            $("[data-posicion-fecha]").on("change", function () {
                $("[data-fecha-compuesta]").val(this.value);
                $("[data-fecha-conformacion]").val(this.value);
            });

            $("[data-fecha-conformacion]").on("change",
                function () {
                    $("[data-fecha-compuesta]").val(this.value);
                });

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



            $("input[type='text'][data-volumen-compuesta]").unbind("keyup").keyup(function () {
                $("span[data-volumen-result-total]").text(Number($(this).val()).toFixed(2));
                Calculate();
            });
            $("input[type='text'][data-operation]").unbind("keyup").keyup(function () {
                Calculate();
            });

            $("input[type='text'][data-lcmc]").unbind("keyup").keyup(function () {
                var parametroId = $(this).attr("data-parameter-id");
                var value = $(this).val();
                $("input[type='text'][data-lcmc][data-parameter-id='" + parametroId + "']").each(function () {
                    $(this).val(value);
                    var index = $(this).parent().index() - 1;
                    var ValorValidar = 0;
                    if ($($(this).parent().parent().children('td')[index]).children().prop('type') == "text") {
                        ValorValidar = $($(this).parent().parent().children('td')[index]).children().val();
                    }
                    else {
                        index = index - 1;
                        ValorValidar = $($(this).parent().parent().children('td')[index]).children().val();
                    }
                    var valorCaudal = $(this).closest('tr').find($("input[id*='txtCaudal']")).val();
                    if (parseFloat(ValorValidar) < parseFloat(value) && parseFloat(valorCaudal) > 0) {
                        //$($(this).parent().parent().children('td')[index]).children("span").remove();
                        //$($(this).parent().parent().children('td')[index]).append('<span class="text-danger" id="SPANLCMC_' + String(index) + '">El resultado de laboratorio no puede ser menor que el LCM</span>');
                        //$('[id*=lnkGuardarSimples]').attr('disabled', 'disabled');
                    }
                    else {
                        //$($(this).parent().parent().children('td')[index]).children("span").remove();
                        //$('[id*=lnkGuardarSimples]').removeAttr('disabled');
                    }
                });

            });



            //$("input[type='text'][data-lcmc]").unbind("onblur").keyup(function () {

            //    var parametroId = $(this).attr("data-parameter-id");
            //    var value = $(this).val();


            //});


            $("input[type='text'][data-scale]").unbind("keypress").keypress(function (event) {
                if (isNumber(event, this)) {

                    return true;
                }
                var cantidad = $(this).val().split('.');
                if (cantidad[0].length > 10) {
                    if (cantidad.length == 1) {
                        $(this).val(cantidad[0].substring(0, cantidad[0].length - 1));
                    }
                    else {
                        $(this).val(cantidad[0].substring(0, cantidad[0].length - 1) + '.' + cantidad[1]);
                    }
                }
                return false;
            });


            $("input[type='text'][data-scale]").unbind("blur").blur(function (event) {
                if (isNumber(event, this)) {
                    var index = $(this).parent().index() + 1;
                    var value = isNaN($(this).val()) ? $(this).val() : 0;
                    var ValorValidar = 0;

                    if ($($(this).parent().parent().children('td')[index]).children().prop('type') == "text") {
                        ValorValidar = $($(this).parent().parent().children('td')[index]).children().val();
                    }
                    else {
                        index = index + 1;
                        ValorValidar = $($(this).parent().parent().children('td')[index]).children().val();
                    }
                    if ($($(this).parent().parent().children('td')[index]).children().attr('data-lcmc') == '') {
                        if (parseFloat(ValorValidar) <= parseFloat(value)) {
                            //$(this).parent().children("span").remove();
                            //$('[id*=lnkGuardarSimples]').removeAttr('disabled');

                        }
                        else {
                            //$(this).parent().children("span").remove();
                            //$(this).parent().append('<span class="text-danger">El resultado de laboratorio no puede ser menor que el LCM</span>');
                            //$('[id*=lnkGuardarSimples]').attr('disabled', 'disabled');
                        }
                    }

                    return true;
                }
                var cantidad = $(this).val().split('.');
                if (cantidad[0].length > 10) {
                    if (cantidad.length == 1) {
                        $(this).val(cantidad[0].substring(0, cantidad[0].length - 1));
                    }
                    else {
                        $(this).val(cantidad[0].substring(0, cantidad[0].length - 1) + '.' + cantidad[1]);
                    }
                }
                return false;
            });
            $("input[type='text']").unbind("focusin").focusin(function () {
                $(this).select();
            });

            $("input[type='text']").unbind("click").click(function () {
                $(this).select();
            });

            $("table[data-matrix] input[type='text']").unbind("paste").on("paste", function (e) {
                e.preventDefault();
            });

            $("select[data-accion]").unbind("change").change(function () {
                var accion = $("option:selected", this).val();
                var parameterId = $(this).attr("data-parameter-id");

                $(this).attr('data-accion', accion);
                switch (accion) {
                    case "1":
                    case "2":
                        $("input[data-parameter-id='" + parameterId + "']").each(function () {
                            var escalaDecimal = $("#" + this.id).data('scale');
                            $(this).attr("disabled", "disabled");
                            $(this).val(Number(0).toFixed(escalaDecimal));
                            $(this).parent().children("span").hide();
                            $(this).parent().removeClass("has-error");
                        });
                        $("select[data-parameter-id='" + parameterId + "']").each(function () {
                            $(this).attr("disabled", "disabled");

                            $(this).attr("disabled", "disabled");

                            if ($("#" + this.id).data('laboratorio-id') != undefined && $("#" + this.id).data('accion') == undefined) {
                                $("#" + this.id + " option[value=0]").attr("selected", "selected");
                            } else if ($("#" + this.id).data('parameter-id') == 85 && $("#" + this.id).data('accion') == undefined) {
                                $("#" + this.id).val('true');
                            }
                        });
                        $("[data-lmp-parametroid='" + parameterId + "']").each(function () {
                            var escalaDecimal = $("#" + this.id).data('scale');
                            $(this).attr("disabled", "disabled");
                            $(this).val(Number(0).toFixed(escalaDecimal));
                        });
                        $(this).removeAttr("disabled");

                        Calculate();

                        break;
                    default:
                        $("input[data-parameter-id='" + parameterId + "']").each(function () {
                            var fila = $('#' + this.id).closest('tr');
                            var caudal = $('input[type="text"][data-calc]', fila).val();
                            if (caudal > 0)
                                $(this).removeAttr("disabled");
                        });
                        $("select[data-parameter-id='" + parameterId + "']").each(function () {
                            var fila = $('#' + this.id).closest('tr');
                            var caudal = $('input[type="text"][data-calc]', fila).val();
                            if (caudal > 0 || caudal === undefined)
                                $(this).removeAttr("disabled");
                        });
                        $("[data-lmp-parametroid='" + parameterId + "']").each(function () {
                            $(this).removeAttr("disabled");
                        });
                        break;
                }
            });

            Calculate();

            $("span[data-volumen-result-total]").text(Number($("input[type='text'][data-volumen-compuesta]").val()).toFixed(2));



            $("td[id*='trLaboratorio'] > select").unbind("change").change(function () {
                var self = this;
                var laboratorioId = parseInt($("option:selected", $(this)).val());
                var parametroId = parseInt($(this).attr("data-parameter-id"));

                var refresh = function () {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "Analisis.aspx/CargaMetodos",
                        data: JSON.stringify({ laboratorioId: laboratorioId, parametroId: parametroId }),
                        dataType: "json",
                        success: function (data, textStatus) {
                            if (textStatus === "success") {
                                $("select[data-parameter-id='" + parametroId + "'][data-metodo-id]").empty();
                                //.find('option').remove().end();
                                $("select[data-parameter-id='" + parametroId + "'][data-metodo-id]").append("<option value=\"0\">Seleccione</option>");
                                $.each(data.d, function (index, element) {
                                    var Selecte = "";
                                    if (data.d.length == 1) {
                                        Selecte = "selected='selected'";
                                    }
                                    else {
                                        Selecte = "";
                                    }
                                    $("select[data-parameter-id='" + parametroId + "'][data-metodo-id]")
                                        .append("<option " + Selecte + " value=\"" + element.MetodoID + "\">" + element.Nombre + "</option>");
                                });
                            }
                        },
                        error: function (data, textStatus) {
                            alert(data);
                            alert(textStatus);
                        }
                    });
                };

                refresh();
            });

            $("select[data-muestra-valor-id]").each(function () {
                var valor = $(this).attr("data-muestra-valor");
                if (!tablaEditada)
                    $(">option[value='" + valor + "']", $(this)).prop("selected", true);
            });
            $("select[data-muestra-valor-id]").change(function () {
                tablaEditada = true;
            });

            $("select[data-accion]").each(function () {
                var action = $(this).attr("data-accion");
                $(">option[value='" + action + "']", $(this)).prop("selected", true);
                $(this).trigger("change");
            });

            $("a[data-guardar]").unbind("click").click(function (e) {

                e.stopPropagation();
                transformaFechaFromClient();

                if (!validaMinMax())
                    return false;

                if ($("[id*='pnlMuestraCompuesta']").size() > 0) {
                    if (!validaMuestras()) {
                        return false;
                    }
                }

                //Validación específica para la SDT.
                if ($("[id*='lnkGenerarCompuesta']").size() == 0 && $("[id*='pnlMuestraCompuesta']").size() == 0) {
                    if (!validaSDT()) {
                        return false;
                    }
                }

                showPopup();
                //Transformar la fecha que se envía al servidor.


                var LaboratorioSignatario = {
                    LaboratorioId: $("input[type='radio'][name*='rblTipoLaboratorio'][checked='checked']").val() == '1' ? $("input[type='hidden'][name*='hfLaboratorioAnalisisID']").val() : $("select[data-laboratorio-signatario] option:selected").val(),
                    PersonaID: $("select[data-signatario] option:selected").val() ? $("select[data-signatario] option:selected").val() : 0
                };

                var Muestracompleta = false;
                var Muestras = []; //Reiniciamos el arreglo
                var arrayFechasMuestras = [];

                $("tr[data-muestra-id]").each(function (index, element) {
                    var self = $(this);

                    var muestra = {
                        MuestraID: $(this).attr("data-muestra-id"),
                        AnalisisID: $("input[type='hidden'][id*='hfAnalisisID']").val() == "" ? $("input[type='hidden'][id*='hfAnalisisID']")[1].value : $("input[type='hidden'][id*='hfAnalisisID']").val(),
                        NivelID: 1,
                        Caudal: $("input[data-calc='caudal']", self).val(),
                        Fecha: undefined,
                        FechaRecepcion: undefined,
                        FechaConformacion: undefined,
                        Descripcion: "N/A",
                        Identificacion: "N/A",
                        Observaciones: $("input[data-observacion]", self).val(),
                        ValHojaCampo: $("input[id*='chkValidaHoja']").is(":checked") ? 1 : 0,
                        ValMuestraC: $("input[data-volumen-compuesta]").val(),
                        MuestraValors: []
                    };

                    //Se añade para evitar conflictos con zonas horarias:
                    var fechaMuestraSimple = {
                        MuestraId: $(this).attr("data-muestra-id"),
                        Fecha: $("input[data-date-muestra]", self).val(),
                        Hora: $("input[data-time-muestra]", self).val(),
                        FechaConformacion: $("input[data-fecha-conformacion]").val(),
                        HoraConformacion: $("input[data-hora-conformacion]").val(),
                        FechaRecepcion: $("input[data-fecha-compuesta]").val(),
                        HoraRecepcion: $("input[data-hora-compuesta]").val()
                    };
                    arrayFechasMuestras.push(fechaMuestraSimple);


                    $("td[data-parameter]", self).each(function (i, e) {
                        var control = $("input", this);
                        if (control.length == 0) control = $("select", this);
                        var muestraValorId = $(control).attr("data-muestra-valor-id");
                        var parametroId = $(control).attr("data-parameter-id");

                        var lcmc = $("input[data-lcmc][data-parameter-id='" + parametroId + "'").length > 0
                            ?
                            $("input[data-lcmc][data-parameter-id='" + parametroId + "'").val() == '0' || $("input[data-lcmc][data-parameter-id='" + parametroId + "'").val() == '' ? undefined : $("input[data-lcmc][data-parameter-id='" + parametroId + "'").val() : undefined;
                        var laboratorioId = $("td[id*='trLaboratorio'] select[data-parameter-id='" + parametroId + "'][data-laboratorio-id] option:selected").val() == '0' ? undefined : $("td[id*='trLaboratorio'] select[data-parameter-id='" + parametroId + "'][data-laboratorio-id] option:selected").val();
                        var metodoId = $("td[id*='trMetodo'] select[data-parameter-id='" + parametroId + "'][data-metodo-id] option:selected").val() == '0' ? undefined : $("td[id*='trMetodo'] select[data-parameter-id='" + parametroId + "'][data-metodo-id] option:selected").val();
                        var accion = $("select[data-parameter-id='" + parametroId + "'][data-accion] option:selected").val();
                        var valor = $(control).val();
                        var limiteInferior = $('[data-lmp-parametroid="' + parametroId + '"][data-lmp-inferior]').val();;
                        var limiteSuperior = $('[data-lmp-parametroid="' + parametroId + '"][data-lmp-superior]').val();
                        if (metodoId > 0 && laboratorioId > 0 && valor > 0 && LaboratorioSignatario.LaboratorioId > 0 && LaboratorioSignatario.PersonaID > 0) {
                            Muestracompleta = true;
                        }
                        else {
                            Muestracompleta = false;
                        }
                        var valor = {
                            MuestraValorID: muestraValorId,
                            MuestraID: muestra.MuestraID,
                            Valor: $(control).is("input") ? $(control).val() : "0",
                            ValorValidacion: $(control).is("select") ? $("option:selected", control).val() == 'true' : false,
                            ParametroID: parametroId,
                            LaboratorioID: laboratorioId == '0' ? undefined : laboratorioId,
                            ParametroMetodoID: metodoId,
                            Exento: accion == '1',
                            NoAnalizado: accion == '2',
                            LC_MC: lcmc,
                            LimiteInferior: limiteInferior != '' || limiteInferior != undefined ? parseFloat(limiteInferior) : undefined,
                            LimiteSuperior: limiteSuperior != '' || limiteSuperior != undefined ? parseFloat(limiteSuperior) : undefined
                        };
                        muestra.MuestraValors.push(valor);
                    });

                    Muestras.push(muestra);

                });
                transformaFechaFromServer();
                
                var conductividad = "";
                var GuardaConductividad = "false";
                if ($("input[type='hidden'][id*='hfConductividad']").val() != undefined && $("input[type='hidden'][id*='hfConductividad']").val()!="") {
                    conductividad = $("input[type='hidden'][id*='hfConductividad']").val();
                }
                if ($("input[type='hidden'][id*='hfGuardaConductividad']").val() != undefined && $("input[type='hidden'][id*='hfGuardaConductividad']").val() != "") {
                    GuardaConductividad = $("input[type='hidden'][id*='hfGuardaConductividad']").val();
                }
                
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Analisis.aspx/GuardarSimples",
                    data: JSON.stringify({ muestras: Muestras, analisisId: $("input[type='hidden'][id*='hfAnalisisID']").val() == "" ? $("input[type='hidden'][id*='hfAnalisisID']")[1].value : $("input[type='hidden'][id*='hfAnalisisID']").val(), laboratorioSignatario: LaboratorioSignatario, muestracompleta: Muestracompleta, listaFechaMuestraSimples: arrayFechasMuestras, Conductividad: conductividad, GuardaConductividad: GuardaConductividad}),
                    dataType: "json",
                    beforeSend: function () {
                        //showPopup();
                    },
                    success: function (data, textStatus) {
                   
                        if (textStatus === "success") {
                            var msg = data;
                            //document.getElementById("<%=lnkSendSaveMessage.ClientID%>").click();
                            hidePopup();
                            if (msg.d.Message != null && msg.d.Message != undefined && msg.d.Message != "") {
                                $("#divModalHeader").empty();
                                $("#divModalHeader").append("<h5>Mensaje del sistema</h5>");
                                $("#divMensajes").empty();
                                $("#divMensajes").append("<p class='text-danger'>" + msg.d.Message + "</p>");
                                $("#mdlCheckbox").modal();
                               
                            }
                            else {
                                $("#divModalHeader").empty();
                                $("#divModalHeader").append("<h5>Datos almacenados correctamente</h5>");
                                $("#divMensajes").empty();
                                $("#divMensajes").append("<p class='text-success'>Las muestras se han guardado con éxito</p>");
                                $("#mdlCheckbox").modal();
                                //if (msg.d.Finalized == true) { //se puso finalized en true desde el code behind para validar que ya hay una muestra compuesta
                                //    window.location.reload();
                                //}
                            }
                           
                        }
                    },
                    error: function (data, textStatus) {
                        hidePopup();
                    }
                });
            });

            $("input[type='text'][data-editable='False'],select[data-editable='False']").attr("disabled", "disabled");
            //});




            $("a[id *= 'lnkGenerarCompuesta']").unbind("click").click(function (e) {
                if (!validaMinMax())
                    return false;

                transformaFechaFromClient();
                if (!validaMuestras()) {
                    e.stopPropagation();
                    return false;
                }
            });

        }

        //Cambia los valores de las fechas de las muestras onchange.
        function cambiaFechasOnChange(id) {
            var valor = $("#" + id).val();
            var posicion = $("#" + id).data("posicion");
            $("input[id*=txtFecha][data-date-muestra]").each(function () {
                if ($("#" + this.id).data("posicion") > posicion) {
                    this.value = valor;
                }
            });
        }

        function FechaValida(fechaFormatoHispano) {
            var regEx = /^\d{2}\/\d{2}\/\d{4}$/;
            return fechaFormatoHispano.match(regEx);
        }

        function FechaValidaAnhoCorto(fechaCorta) {
            var regEx = /^\d{2}\/\d{2}\/\d{2}$/;
            return fechaCorta.match(regEx);
        }

        function horaValidador(hora) {
            var regEx = /^([01]\d|2[0-3]):?([0-5]\d)$/;
            return hora.match(regEx);
        }

        //Funciones para realizar cálculos en pantalla:
        function ParticipacionCaudal() {
            $("input[type='text'][data-calc='caudal']").each(function () {
                var row = $(this).closest("tr")[0];
                var spanResult = $("span[data-caudal-result]", row);
                var total = Sum("input[type='text'][data-calc='caudal']");
                var value = parseFloat($(this).val());
                if (total === 0)
                    $(spanResult, row).text(Number(0).toFixed(2));
                else {
                    if (isNaN(value))
                        value = 0;
                    $(spanResult, row).text(Number(roundNumber(value / total, 4)).toFixed(2));
                }
            });
        }

        function VolumenMuestra() {
            $("input[type='text'][data-calc='caudal']").each(function () {
                var row = $(this).closest("tr")[0];
                var spanResult = $("span[data-volumen-result]", row);
                var volumen = parseFloat($("input[type='text'][data-volumen-compuesta]").val());
                var total = Sum("input[type='text'][data-calc='caudal']");
                if (total === 0)
                    $(spanResult, row).text(Number(0).toFixed(2));
                else {
                    if (isNaN(volumen)) volumen = 0;

                    $(spanResult, row).text(Number(roundNumber(volumen * (parseFloat($(this).val()) / total), 2)).toFixed(2));
                }
            });
        }

        function PromedioPonderadoParcial() {
            $("input[type='text'][data-operation='3']").each(function () {
                var parameterId = $(this).attr("data-parameter-id");
                var scale = parseInt($(this).attr("data-scale"));
                var tr = $(this).closest("tr")[0];
                var inputResult = $("span[data-operation='3'][data-parameter-id='" + parameterId + "'][data-result]", tr)[0];

                var caudal = new Decimal($($("input[type='text'][data-calc='caudal']", tr)[0]).val());
                var total = new Decimal(Sum("input[type='text'][data-calc='caudal']"));
                var valor = isNaN(parseFloat($(this).val())) ? new Decimal(0) : new Decimal($(this).val());
                if (!isNaN(caudal.div(total).times(new Decimal(valor)))) {
                    var resultado = new Decimal(caudal.div(total).times(valor));
                    $(inputResult).text(Number(roundNumber(Number(resultado).toFixed(10), scale)).toFixed(scale));
                }
                else
                    $(inputResult).text(Number(0).toFixed(scale));
            });
        }

        function PromedioPonderado(parameterId) {
            var result = new Decimal(0);
            $("input[type='text'][data-operation='3'][data-parameter-id='" + parameterId + "']").each(function () {
                var tr = $(this).closest("tr")[0];
                var caudal = new Decimal($($("input[type='text'][data-calc='caudal']", tr)[0]).val());
                var total = new Decimal(Sum("input[type='text'][data-calc='caudal']"));
                var valor = isNaN(parseFloat($(this).val())) ? new Decimal(0) : new Decimal($(this).val());
                if (!isNaN(caudal.div(total).times(new Decimal(valor))))
                    result = result.plus(caudal.div(total).times(valor));
            });

            return Number(result.valueOf()).toPrecision(15);
        }

        function MediaGeometrica(parameterId) {
            
            var result = 0;
            var totalItems = 0;
            var scale = 0;
            $("input[type='text'][data-calc='caudal']").each(function () {
                var value = parseFloat($(this).val());
                if (!isNaN(value) && value > 0) {
                    totalItems++;
                }
            });
            var bandera = "false";
            $("input[type='text'][data-operation='4'][data-parameter-id='" + parameterId + "']").each(function () {
                var value = $(this).val();
                scale = $(this).attr("data-scale");
                if (!isNaN(value) && value > 0) {
                    result += Math.log10(value);
                    bandera = "true";
                }
            });

            if (result > 0 || bandera == "true") {
                result = result / totalItems;
                result = Math.pow(10, result);
            }

            if (isNaN(scale)) scale = 0;
            return Number(result).toPrecision(15);
        }

        function Sum(selector) {
            var result = 0;
            $(selector).each(function (id, item) {
                var value = parseFloat($(this).val());
                if (isNaN(value)) value = 0;
                result += value;
            });
            return result;
        }

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
                        dec = sign * Number((dec - Math.pow(10, scale)) / Math.pow(10, scale).toFixed(scale));
                        return Number(dec).toFixed(scale);
                    }
                }
            }
            return Number(num).toFixed(scale);
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

        function isTextSelected(input) {
            if (typeof input.selectionStart == "number") {
                return input.selectionStart == 0 && input.selectionEnd == input.value.length;
            } else if (typeof document.selection != "undefined") {
                input.focus();
                return document.selection.createRange().text == input.value;
            }
        }

        function dateTime(date, time) {
            try {
                var year = date.split('/')[2];
                var month = date.split('/')[1];
                var day = date.split('/')[0];
                var hour = time.split(':')[0];
                var minute = time.split(':')[1].split(' ')[0];
                if (time.split(':')[1].split(' ')[1] == "PM") {
                    hour = parseInt(hour, 32) + 12;
                }
                if (date.trim() === '' || isNaN(year) || isNaN(month) || isNaN(day))
                    return undefined;
                if (time.trim() === '' || isNaN(hour) || isNaN(minute))
                    return new Date(year, month, day);

                var dateT = new Date(year, month - 1, day, hour, minute);
                return dateT;
            } catch (e) {
                return undefined;
            }
        }

        function Calculate() {
            ParticipacionCaudal();
            VolumenMuestra();
            PromedioPonderadoParcial();

            var scaleCaudal = parseInt($("span[data-caudal-total]").attr("data-scale"));
            $("span[data-caudal-total]").text(
                Number(roundNumber(OperacionesResumen.Promedio("input[type='text'][data-calc='caudal']"), scaleCaudal)).toFixed(scaleCaudal)
            );

            var scaleParticipacion = parseInt($("span[data-caudal-result-total]").attr("data-scale"));
            var total = new Decimal(OperacionesResumen.Sumatoria("input[type='text'][data-calc='caudal']"));
            var totalQiQt = function () {
                var result = new Decimal(0);
                $("input[type='text'][data-calc='caudal']").each(function () {
                    var caudal = new Decimal($(this).val());
                    if (!isNaN(caudal.div(total)))
                        result = result.plus(caudal.div(total));
                });

                return Number(result).toFixed(0);
            };


            var result = roundNumber(totalQiQt(), scaleParticipacion);
            $("span[data-caudal-result-total]").text(Number(result).toFixed(2));


            //Aquí realiza las operaciones que se muestran en la fila de resultados:
            $("span[data-result-total][data-operation='1']").each(function () {
                var parametroId = $(this).attr("data-parameter-id");
                var scale = parseInt($(this).attr("data-scale"));
                if (isNaN(scale)) scale = 0;
                var resultado = $(this).data('truncamiento') === "True"
                    ? truncate(OperacionesResumen.Sumatoria("input[type='text'][data-operation='1'][data-parameter-id='" + parametroId + "']"), scale)
                    : roundNumber(OperacionesResumen.Sumatoria("input[type='text'][data-operation='1'][data-parameter-id='" + parametroId + "']"), scale);
                $(this).text(resultado);
            });
            $("span[data-result-total][data-operation='2']").each(function () {
                var parametroId = $(this).attr("data-parameter-id");
                var scale = parseInt($(this).attr("data-scale"));
                if (isNaN(scale)) scale = 0;

                var resultado = $(this).data('truncamiento') === "True"
                    ? truncate(OperacionesResumen.Promedio("input[type='text'][data-operation='2'][data-parameter-id='" + parametroId + "']"), scale)
                    : roundNumber(OperacionesResumen.Promedio("input[type='text'][data-operation='2'][data-parameter-id='" + parametroId + "']"), scale);
                $(this).text(resultado);
            });
            $("span[data-result-total][data-operation='3']").each(function () {
                var parametroId = $(this).attr("data-parameter-id");
                var scale = parseInt($(this).attr("data-scale"));
                if (isNaN(scale)) scale = 0;

                var resultado = $(this).data('truncamiento') === "True"
                    ? truncate(PromedioPonderado(parametroId), scale)
                    : roundNumber(PromedioPonderado(parametroId), scale);
                $(this).text(resultado);
            });
            $("span[data-result-total][data-operation='4']").each(function () {
                var parametroId = $(this).attr("data-parameter-id");
                var scale = parseInt($(this).attr("data-scale"));
                if (isNaN(scale)) scale = 0;
                
                var resultado = $(this).data("truncamiento") === "True" ? truncate(MediaGeometrica(parametroId), scale) : roundNumber(MediaGeometrica(parametroId), scale);
                $(this).text(resultado);
            });
        }


        function deshabilitarCeldas(id) {

            var elemento = $("#" + id).closest("tr");

            if (isNaN($("#" + id).val()) || $("#" + id).val() == "" || parseFloat($("#" + id).val()) == 0) {
                $("input[data-parameter-id], select[data-parameter-id]", elemento).attr("disabled", "disabled");

                $("input[data-parameter-id]", elemento).each(function () {
                    $("#" + this.id).val(Number(0).toFixed($("#" + this.id).data('scale')));
                });
                $("#" + id).removeAttr("disabled");

                $("select[data-parameter-id]", elemento).each(function () {
                    if ($("#" + this.id).data('parameter-id') == 85) {
                        $("#" + this.id).val('true');
                    }
                });
            }
            else {
                $("input[data-parameter-id], select[data-parameter-id]", elemento).removeAttr("disabled");
                $("input[data-parameter-id], select[data-parameter-id]", elemento).each(function () {
                    if (parseInt($("select[id*='ddlAccion'][data-parameter-id='" + $("#" + this.id).data('parameter-id') + "']").val()) != 0) {
                        $("#" + this.id).attr("disabled", "disabled");
                    }
                });

            }
            Calculate();
        }


        function establecerDecimalesOnBlur(id) {
            var value = $("#" + id).val();
            value = isNaN(value) ? 0 : value;
            var minAceptado = $("#" + id).data("minaceptado");
            var maxAceptado = $("#" + id).data("maxaceptado");

            $("#" + id).val(Number(value).toFixed($("#" + id).data("scale")));
            var elemento = document.getElementById(id);

            if (minAceptado != undefined && parseFloat(value) < parseFloat(minAceptado)) {
                $("#" + id).parent().find("span").show();
                $("#" + id).parent().addClass("has-error");
                $(elemento).focus();
                return false;
            } else if (maxAceptado != undefined && parseFloat(value) > parseFloat(maxAceptado)) {
                $("#" + id).parent().find("span").show();
                $("#" + id).parent().addClass("has-error");
                $(elemento).focus();
                return false;
            } else {
                $("#" + id).parent().removeClass("has-error");
                $("#" + id).parent().find("span").hide();
            }
        }

        function establecerDecimalesLCMOnBlur(id) {
            var parametroId = $("#" + id).attr('data-parameter-id');
            var escala = $("#" + id).data('scale');
            var value = $("#" + id).val();
            if (isNaN(value))
                value = 0;

            $("input[type='text'][data-lcmc][data-parameter-id='" + parametroId + "']").each(function () {
                $("#" + this.id).val(Number(value).toFixed(escala));
            });
        }

        function transformaFechaFromClient() {
            $("input[id*='txtFecha']").each(function () {
                if ($(this).val() != '' && FechaValidaAnhoCorto($(this).val())) {
                    var fechaTransformada = this.value.split('/');

                    $(this).val(fechaTransformada[0] + '/' + fechaTransformada[1] + '/20' + fechaTransformada[2]);
                }
            });
        }

        function transformaFechaFromServer() {
            $("input[id*='txtFecha']").each(function () {
                if ($(this).val() != '' && FechaValida($(this).val())) {
                    var fechaTransformada = this.value.split('/');
                    fechaTransformada[2] = fechaTransformada[2].substring(2);
                    $(this).val(fechaTransformada[0] + '/' + fechaTransformada[1] + '/' + fechaTransformada[2]);
                }
            });
        }

        function validaMinMax() {
            var elementos = [];
            var elementosMin = $("input[data-minaceptado]");
            var elementosMax = $("input[data-maxaceptado]");
            var enviar = true;
            for (let elemento of elementosMin) {
                var caudalInput = $(elemento).parent().parent().find("input[id*='txtCaudal']").val();
                var opcionSeleccionada = $("select[id*='ddlAccion'][data-parameter-id='" + $(elemento).data("parameter-id") + "']").val();
                caudalInput = isNaN(caudalInput) ? 0 : parseFloat(caudalInput);
                if (caudalInput > 0 && parseInt(opcionSeleccionada) === 0) {
                    var valor = $(elemento).val();
                    var min = $(elemento).data("minaceptado");
                    valor = isNaN(valor) ? 0 : parseFloat(valor);
                    if (valor < parseFloat(min)) {
                        elementos.push(elemento);
                        enviar = false;
                        break;
                    }
                }
            }

            for (let elemento of elementosMax) {
                var caudalInput = $(elemento).parent().parent().find("input[id*='txtCaudal']").val();
                var opcionSeleccionada = $("select[id*='ddlAccion'][data-parameter-id='" + $(elemento).data("parameter-id") + "']").val();
                caudalInput = isNaN(caudalInput) ? 0 : parseFloat(caudalInput);
                if (caudalInput > 0 && parseInt(opcionSeleccionada) === 0) {
                    var valor = $(elemento).val();
                    var max = $(elemento).data("maxaceptado");
                    valor = isNaN(valor) ? 0 : parseFloat(valor);
                    if (valor > parseFloat(max)) {
                        elementos.push(elemento);
                        enviar = false;
                    }
                }
            }

            if (!enviar) {
                var primerElemento = elementos[0];
                $(primerElemento).parent().find("span").show();
                $(primerElemento).parent().addClass("has-error");
                $(primerElemento).focus();
                transformaFechaFromServer();
            }
            return enviar;
        };

        function validaMuestras() {

            $("#divModalHeader").empty();
            $("#divModalHeader").append("<h5>Verifique los datos</h5>");
            $("#divMensajes").empty();
            var mensaje = "";
            if ($("[id*='pnlMuestraCompuesta']").size() > 0) {
                mensaje += "<p class='text-danger text-justify'> El análisis ya cuenta con una muestra compuesta, por tanto, los cambios guardados se reflejarán en la misma.</p>";
            }
            mensaje += '<ul class = "text-danger text-justify">';
            var enviar = true;

            //Verificar checkbox
            if (!$("input[id*='chkValidaHoja']").is(":checked")) {
                mensaje += '<li>Debe validar que los valores de la hoja de campo y bitácora concuerdan con los de la hoja electrónica.</li>';
                enviar = false;
            }

            //Verificar signatario
            if ($("select[id*='ddlMuestreador']").val() === '0' || $("select[id*='ddlMuestreador']").val() === undefined) {
                mensaje += '<li>Debe seleccionar un signatario.</li>';
                enviar = false;
            }

        //Valida Conductividad para la norma 2021
            if (($("input[id*='hfNormaId']").val() == "" ? $("input[id*='hfNormaId']")[1].value : $("input[id*='hfNormaId']").val()) == 15)
            {
                if (!$("input[id*='chkEColi']").is(":checked") && !$("input[id*='chkEnterococos']").is(":checked")) {
                    mensaje += '<li>Debe seleccionar un valor de conductividad eléctrica.</li>';
                    enviar = false;
                }
                
            }
            //Validar fechas.
            var fechaMuestrasArray = [];
            $("input[id*=txtFecha][data-date-muestra]").each(function () {
                fechaMuestrasArray.push(this.value);
            });

            var sonFechasCorrectas = true;
            for (var i = 0; i < fechaMuestrasArray.length; i++) {
                var fechaMuestra = fechaMuestrasArray[i];
                if (fechaMuestra === '') {
                    mensaje += '<li>Debe seleccionar una fecha para la muestra ' + (i + 1) + '.</li>';
                    enviar = false;
                    sonFechasCorrectas = false;
                } else if (!FechaValida(fechaMuestra)) {
                    enviar = false;
                    mensaje += '<li>El formato de fecha de la muestra ' + (i + 1) + ' es incorrecto.</li>';
                    sonFechasCorrectas = false;
                } else {
                    var fechaMuestraSplit = fechaMuestra.split('/');
                    var fechaMuestraDate = new Date(fechaMuestraSplit[2], fechaMuestraSplit[1] - 1, fechaMuestraSplit[0]);
                    for (var j = i + 1; j < fechaMuestrasArray.length; j++) {
                        if (!FechaValida(fechaMuestrasArray[j])) {
                            enviar = false;
                            break;
                        } else {
                            //Aquí se compara con todas las fechas de las muestras:
                            var fechaComparacionSplit = fechaMuestrasArray[j].split('/');
                            var fechaComparacionDate = new Date(fechaComparacionSplit[2], fechaComparacionSplit[1] - 1, fechaComparacionSplit[0]);
                            if (fechaMuestraDate > fechaComparacionDate) {
                                enviar = false;
                                mensaje += '<li>La fecha de la muestra ' + (i + 1) + ' no puede ser posterior a la de la(s) siguiente(s) muestra(s).</li>';
                                break;
                            }
                        }
                    }
                }
            }

            //Validar horas
            var sonHorasCorrectas = true;
            var arrayHorasTexto = [];
            $("input[id*='txtHora'][data-time-muestra]").each(function () {
                arrayHorasTexto.push(this.value);
                if (!horaValidador(this.value)) {
                    sonHorasCorrectas = false;
                }
            });

            for (var i = 0; i < arrayHorasTexto.length; i++) {
                if (!horaValidador(arrayHorasTexto[i])) {
                    enviar = false;
                    mensaje += "<li>El formato de la hora en la muestra " + (i + 1) + " es incorrecto.</li>";
                }
            }



            if (sonFechasCorrectas && sonHorasCorrectas) {
                var arrayHoras = [];
                $("input[id*='txtHora'][data-time-muestra]").each(function () {
                    var fila = $('#' + this.id).closest('tr');
                    var fecha = $('input[type="text"][data-date-muestra]', fila).val();
                    var dateParts = fecha.split("/");
                    var fechaDate = new Date(+dateParts[2], dateParts[1] - 1, +dateParts[0]);
                    var horaSplit = this.value.split(':');
                    fechaDate.setHours(horaSplit[0], horaSplit[1]);
                    arrayHoras.push(fechaDate);
                });

                var mensajeHoras = '';
                for (var i = arrayHoras.length - 1; i > 0; i--) {
                    for (var j = i - 1; j >= 0; j--) {
                        if (arrayHoras[i] <= arrayHoras[j]) {
                            mensajeHoras += '<li>La hora de la muestra ' + (i + 1) + ' no puede ser anterior o igual a la de la(s) muestra(s) previa(s).</li>|';
                            break;
                        }
                    }
                }

                if (mensajeHoras != '') {
                    enviar = false;
                    var mensajeHorasSplit = mensajeHoras.split('|');

                    for (var i = mensajeHorasSplit.length - 1; i >= 0; i--) {
                        mensaje += mensajeHorasSplit[i];
                    }
                }
            }


            //Validar caudal
            var i = 0;
            var muestrasConObservaciones = 0;
            $("input[id*='txtCaudal'][data-calc=caudal]").each(function () {
                i++;
                var caudal = this.Value != '' ? parseFloat(this.value) : 0;

                var muestraId = $("#" + this.id).closest('tr').data("muestra-id");

                ;
                if (caudal < 0) {
                    mensaje += '<li>El caudal de la muestra ' + i + ' no puede ser negativo.</li>';
                    enviar = false;
                }
                if (caudal == 0 && $("input[id*='txtObservaciones'][data-posicion=" + muestraId + "]").val() === '') {
                    mensaje += '<li>El caudal de la muestra ' + i + ' es igual a cero, por lo tanto las observaciones son obligatorias.</li>';
                    enviar = false;
                }

                if (caudal == 0 && $("input[id*='txtObservaciones'][data-posicion=" + muestraId + "]").val() != '') {
                    muestrasConObservaciones++;
                }
            });


            if ($("input[id*='txtCaudal'][data-calc=caudal]").size() === muestrasConObservaciones) {
                mensaje = '<ul class="text-danger text-justify"><li>Todas las muestras tienen un Caudal (l/s) igual a cero, por tanto, no es posible generar la muestra compuesta</li>';
                mensaje += "</ul>";
                $("#divMensajes").append(mensaje);
                $("#mdlCheckbox").modal();
                return false;

            }



            //Validar volumen muestra compuesta:
            var volumenCompuesta = $("input[id*='txtVolumen'][data-volumen-compuesta]").val() != '' ? parseFloat($("input[id*='txtVolumen'][data-volumen-compuesta]").val()) : 0;
            if (volumenCompuesta <= 0) {
                mensaje += '<li>El volumen de la muestra compuesta debe ser mayor a cero.</li>';
                enviar = false;
            }


            //Validar que no hay campos vacíos en los parámetros:
            var encabezados = document.getElementById('tablaMuestraSimple').rows[0].cells;
            var parametrosIdArray = [];
            for (let encabezado of encabezados) {
                if (encabezado.dataset.parameterId != '' && encabezado.dataset.parameterId != undefined) {
                    parametrosIdArray.push(encabezado.dataset.parameterId);
                }
            }

            //Obtener LCM:
            var celdas = document.getElementById('tablaMuestraSimple').rows[1].cells;

            var lcmIdArray = [];
            for (let td of celdas) {
                var input = ($(td).find("input:text"));
                var b = input.data("lcmc");
                if (input.data("parameter-id") != null && b != null) {
                    lcmIdArray.push(input.data("parameter-id"));
                }
            }

            //Comprueba que los parámetros que no tienen LCM tengan valores
            for (var i = 0; i < parametrosIdArray.length; i++) {
                var parametro = parametrosIdArray[i];
                var incluyeParametro = lcmIdArray.includes(parseFloat(parametro));
                if (incluyeParametro === false) {
                    if (parseFloat($('select[id*="ddlAccion"][data-parameter-id="' + parametrosIdArray[i] + '"]').val()) == 0) {
                        var columna = $("th[data-parameter-id=" + parametrosIdArray[i] + "]").text();

                        var j = 1;
                        $('input[id*=txtValue][data-parameter-id="' + parametrosIdArray[i] + '"]').each(function () {
                            var fila = $('#' + this.id).closest('tr');
                            var caudal = $('input[type="text"][data-calc]', fila).val();
                            if (caudal != '' && parseFloat(caudal) > 0) { //Solo se evalúa si tiene un caudal
                                if (this.value == '') {
                                    enviar = false;
                                    mensaje += '<li>El parámetro de ' + columna + ' en la muestra ' + j + ' debe tener un valor.</li>';
                                } else if (this.value < 0) {
                                    enviar = false;
                                    mensaje += '<li>El parámetro de ' + columna + ' en la muestra ' + j + ' no puede ser negativo.</li>';
                                }
                            }

                            j++;
                        });
                    }

                }
            }

            //Validar lcm
            for (var i = 0; i < lcmIdArray.length; i++) {
                var j = 1;
                var valorLCM = $('input[type="text"][data-parameter-id="' + lcmIdArray[i] + '"][data-lcmc]').val();;
                var columna = $("th[data-parameter-id=" + lcmIdArray[i] + "]").text();

                if (parseFloat($('select[id*="ddlAccion"][data-parameter-id="' + lcmIdArray[i] + '"]').val()) === 0) { //Si exenta o no se analiza y el caudal es mayor que cero.
                    if (valorLCM == '') {
                        mensaje += '<li>El valor del LC/MC de ' + columna + ' debe ser mayor a cero.</li>';
                    } else {
                        $('input[id*=txtValue][data-parameter-id="' + lcmIdArray[i] + '"]').each(function () {
                            var fila = $('#' + this.id).closest('tr');
                            var caudal = $('input[type="text"][data-calc]', fila).val();
                            if (this.value == '') {
                                enviar = false;
                                mensaje += '<li>El parametro de ' + columna + ' en la muestra ' + j + ' debe tener un valor.</li>';
                            } else if ((caudal != '' && parseFloat(caudal) > 0) && parseFloat(this.value) < parseFloat(valorLCM)) {
                                enviar = false;
                                mensaje += '<li>El valor de ' + columna + ' en la muestra ' + j + ' no puede ser menor que el LC/MC.</li>';
                            }
                            j++;
                        });
                    }
                }
            }


            //Validar fecha de conformación de la muestra:
            var fechaConformacionMuestraCompuesta = $("input[data-fecha-conformacion]").val();
            var horaConformacionMuestraCompuesta = $("input[data-hora-conformacion]").val();
            var fechaConformacionGlobDate = null;
            var validaConformacion = true;


            if (fechaConformacionMuestraCompuesta === '') {
                enviar = false;
                mensaje += '<li>Debe seleccionar una fecha de conformación de la muestra compuesta.</li>';
            } else if (horaConformacionMuestraCompuesta === '') {
                enviar = false;
                mensaje += '<li>Debe seleccionar una hora de conformación de la muestra compuesta.</li>';
            } else if (FechaValida(fechaConformacionMuestraCompuesta) && horaValidador(horaConformacionMuestraCompuesta)) {
                fechaConformacionArray = fechaConformacionMuestraCompuesta.split('/');
                var fechaConformacionDate = new Date(fechaConformacionArray[2], fechaConformacionArray[1] - 1, fechaConformacionArray[0]);
                var horaSplit = horaConformacionMuestraCompuesta.split(':');
                fechaConformacionDate.setHours(horaSplit[0], horaSplit[1]);
                fechaConformacionGlobDate = fechaConformacionDate;
                //Comparar con las fechas de la muestras
                var arrayFechasMuestra = [];
                var arrayHorasMuestra = [];

                $("input[id*=txtFecha][data-date-muestra]").each(function () {

                    var fila = $('#' + this.id).closest('tr');
                    var hora = $('input[id*="txtHora"]', fila).val();

                    arrayFechasMuestra.push(this.value);
                    arrayHorasMuestra.push(hora);

                });

                for (var i = 0; i < arrayFechasMuestra.length; i++) {
                    if (FechaValida(arrayFechasMuestra[i]) && horaValidador(arrayHorasMuestra[i])) {
                        var fechaMuestraArray = arrayFechasMuestra[i].split('/');
                        var fechaMuestraDate = new Date(fechaMuestraArray[2], fechaMuestraArray[1] - 1, fechaMuestraArray[0]);
                        var horaMuestraSplit = arrayHorasMuestra[i].split(':');
                        fechaMuestraDate.setHours(horaMuestraSplit[0], horaMuestraSplit[1]);

                        if (fechaConformacionDate < fechaMuestraDate) {
                            enviar = false;
                            mensaje += '<li>La fecha de conformación de la muestra compuesta no puede ser anterior a ninguna de las fechas de las muestras.</li>';
                            break;
                        }
                    }
                }
            } else {
                mensaje += '<li>El formato de la fecha de conformación de la muestra compuesta es incorrecto.</li>';
                enviar = false;
                validaConformacion = false;
            }


            //Validar fecha de recepción al laboratorio.
            var fechaConformacion = $("input[data-fecha-compuesta]").val();
            var horaConformacion = $("input[data-hora-compuesta]").val();
            var fechaRecepcionLabDate = null;
            var validaRecepcion = true;

            if (fechaConformacion === '') {
                enviar = false;
                mensaje += '<li>Debe seleccionar una fecha de ingreso al laboratorio.</li>';
            } else if (horaConformacion === '') {
                enviar = false;
                mensaje += '<li>Debe seleccionar una hora de ingreso al laboratorio.</li>';
            } else if (FechaValida(fechaConformacion) && horaValidador(horaConformacion)) {
                fechaConformacionArray = fechaConformacion.split('/');
                var fechaConformacionDate = new Date(fechaConformacionArray[2], fechaConformacionArray[1] - 1, fechaConformacionArray[0]);
                var horaSplit = horaConformacion.split(':');
                fechaConformacionDate.setHours(horaSplit[0], horaSplit[1]);
                fechaRecepcionLabDate = fechaConformacionDate;

                //Comparar con las fechas de la muestras
                var arrayFechasMuestra = [];
                var arrayHorasMuestra = [];

                $("input[id*=txtFecha][data-date-muestra]").each(function () {

                    var fila = $('#' + this.id).closest('tr');
                    var hora = $('input[id*="txtHora"]', fila).val();

                    arrayFechasMuestra.push(this.value);
                    arrayHorasMuestra.push(hora);

                });

                for (var i = 0; i < arrayFechasMuestra.length; i++) {
                    if (FechaValida(arrayFechasMuestra[i]) && horaValidador(arrayHorasMuestra[i])) {
                        var fechaMuestraArray = arrayFechasMuestra[i].split('/');
                        var fechaMuestraDate = new Date(fechaMuestraArray[2], fechaMuestraArray[1] - 1, fechaMuestraArray[0]);
                        var horaMuestraSplit = arrayHorasMuestra[i].split(':');
                        fechaMuestraDate.setHours(horaMuestraSplit[0], horaMuestraSplit[1]);

                        if (fechaConformacionDate < fechaMuestraDate) {
                            enviar = false;
                            mensaje += '<li>La fecha de ingreso al laboratorio no puede ser anterior a ninguna de las fechas de las muestras.</li>';
                            break;
                        }
                    }
                }
            } else {
                mensaje += '<li>El formato de la fecha de ingreso al laboratorio es incorrecto.</li>';
                enviar = false;
                validaRecepcion = false;
            }



            //validar hora de ingreso al laboratorio.
            if (horaConformacion === '' || horaConformacionMuestraCompuesta === '') {
                enviar = false;
            }

            //Validar fecha conformación de la muesta y fecha de recepción de la muestra.
            if (validaConformacion && validaRecepcion && fechaRecepcionLabDate != null && fechaConformacionGlobDate != null && fechaRecepcionLabDate < fechaConformacionGlobDate) {
                mensaje += '<li>La fecha de ingreso al laboratorio no puede ser anterior a la fecha de conformación de la muestra compuesta</li>';
                enviar = false;
            }


            //Validar laboratorios.
            for (var i = 0; i < parametrosIdArray.length; i++) {
                var parametro = parametrosIdArray[i];
                if (parseFloat($('select[id*="ddlAccion"][data-parameter-id="' + parametrosIdArray[i] + '"]').val()) == 0) {
                    var columna = $("th[data-parameter-id=" + parametrosIdArray[i] + "]").text();

                    $('select[id*=ddlLaboratorio][data-parameter-id="' + parametrosIdArray[i] + '"]').each(function () {
                        if (this.value == 0) {
                            enviar = false;
                            mensaje += '<li>Debe seleccionar un laboratorio para el parámetro ' + columna + '.</li>';
                        }
                    });
                }
            }


            //Validar métodos.
            for (var i = 0; i < parametrosIdArray.length; i++) {
                var parametro = parametrosIdArray[i];
                if (parseFloat($('select[id*="ddlAccion"][data-parameter-id="' + parametrosIdArray[i] + '"]').val()) == 0) {
                    var columna = $("th[data-parameter-id=" + parametrosIdArray[i] + "]").text();

                    $('select[id*=ddlMetodo][data-parameter-id="' + parametrosIdArray[i] + '"]').each(function () {
                        if (this.value == 0) {
                            enviar = false;
                            mensaje += '<li>Debe seleccionar un método para el parámetro ' + columna + '.</li>';
                        }
                    });
                }
            }

            //Validar Límite Máximo Permisible:
            var parametroAnalizado = false;
            $('[data-lmp-parametroid]').each(function () {

                if (!parametroAnalizado) {
                    var inferior = $("[data-lmp-parametroid='" + $(this).data("lmp-parametroid") + "'][data-lmp-inferior]").val();
                    var superior = $("[data-lmp-parametroid='" + $(this).data("lmp-parametroid") + "'][data-lmp-superior]").val();

                    if (inferior != undefined) {
                        if (inferior == '') {
                            enviar = false;
                            mensaje += '<li>Debe introducir un valor para límite inferior de  ' + $("th[data-parameter-id=" + $(this).data("lmp-parametroid") + "]").html() + '.</li>';
                        }
                        if (parseFloat(inferior) < 0) {
                            enviar = false;
                            mensaje += '<li>El límite inferior de  ' + $("th[data-parameter-id=" + $(this).data("lmp-parametroid") + "]").html() + ' no puede ser menor que cero.</li>';
                        }
                    }

                    if (superior != undefined) {
                        if (superior == '') {
                            enviar = false;
                            mensaje += '<li>Debe introducir un valor para límite superior de  ' + $("th[data-parameter-id=" + $(this).data("lmp-parametroid") + "]").html() + '.</li>';
                        }
                        if (parseFloat(superior) < 0) {
                            enviar = false;
                            mensaje += '<li>El límite superior de  ' + $("th[data-parameter-id=" + $(this).data("lmp-parametroid") + "]").html() + ' no puede ser menor que cero.</li>';
                        }
                    }

                    if (inferior != undefined && superior != undefined && parseFloat(inferior) > parseFloat(superior)) {
                        enviar = false;
                        mensaje += '<li>El límite inferior de  ' + $("th[data-parameter-id=" + $(this).data("lmp-parametroid") + "]").html() + ' no puede ser mayor que su límite superior.</li>';
                    }

                    parametroAnalizado = inferior != undefined && superior != undefined;
                } else {
                    parametroAnalizado = false;
                }
            });


            if (!enviar) {
                mensaje += '</ul>';
                $("#divMensajes").append(mensaje);
                $("#mdlCheckbox").modal();
                transformaFechaFromServer();
                return false;
            } else
                return true;
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


        function validaSDT() {

            $("#divModalHeader").empty();
            $("#divModalHeader").append("<h5>Verifique los datos</h5>");
            $("#divMensajes").empty();


            var mensaje = '<ul class="text-danger text-justify">';
            var enviar = true;
            var fechaMuestrasArray = [];

            //Verificar signatario
            if ($("select[id*='ddlMuestreador']").val() === '0' || $("select[id*='ddlMuestreador']").val() === undefined) {
                mensaje += '<li>Debe seleccionar un signatario.</li>';
                enviar = false;
            }

            //Validar fecha.
            $("input[id*=txtFecha][data-date-muestra]").each(function () {
                fechaMuestrasArray.push(this.value);
            });

            for (var i = 0; i < fechaMuestrasArray.length; i++) {
                var fechaMuestra = fechaMuestrasArray[i];
                if (fechaMuestra === '') {
                    mensaje += '<li>Debe seleccionar una fecha para la muestra.</li>';
                    enviar = false;
                } else if (!FechaValida(fechaMuestra)) {
                    enviar = false;
                    mensaje += '<li>El formato de fecha de la muestra es incorrecto.</li>';
                }
            }

            //Comprueba que los parámetros que no tienen LCM tengan valores

            $('input[id*=txtValue]').each(function () {

                if (this.value == '') {
                    enviar = false;
                    mensaje += '<li>El parámetro debe tener un valor.</li>';
                } else if (this.value < 0) {
                    enviar = false;
                    mensaje += '<li>El parámetro no puede ser negativo.</li>';
                }


            });

            //Validar laboratorios.
            $('select[id*=ddlLaboratorio]').each(function () {
                if (this.value == 0) {
                    enviar = false;
                    mensaje += '<li>Debe seleccionar un laboratorio para el parámetro.</li>';
                }
            });

            //Validar métodos.
            $('select[id*=ddlMetodo]').each(function () {
                if (this.value == 0) {
                    enviar = false;
                    mensaje += '<li>Debe seleccionar un método.</li>';
                }
            });

            if (!enviar) {
                mensaje += "</ul>";
                $("#divMensajes").append(mensaje);
                $("#mdlCheckbox").modal();
                return false;
            }


            return enviar;
        }


        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(InitializeMS);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitializeMS);
    </script>
</asp:Content>
