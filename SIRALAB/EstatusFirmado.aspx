<%@ Page Title="Analisis en proceso de firma" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="EstatusFirmado.aspx.cs" Inherits="SIRALAB.EstatusFirmado" ClientIDMode="AutoID"
    Culture="es-MX" UICulture="es-MX" %>

<%@ Import Namespace="SIRALAB_BL.common" %>

<%@ Register Src="~/FirmaElectronica/wucAppletFIEL.ascx" TagPrefix="uc1" TagName="wucAppletFIEL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/inicio.css" rel="stylesheet" />
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />
    <link href="style/botones.css" rel="stylesheet" type="text/css" />
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
    <script src="js/ValidadorRequerido.js"></script>
    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitializeHndMC);

        function pageLoad() {
            InitializeHndMC();
        }
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <style type="text/css">
                .container {
                    width: auto;
                    max-width: 88%;
                }
            </style>
            <div class="row">
                <br />
                <br />
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <asp:LinkButton ID="lnkbtnBack" runat="server" OnClick="lnkbtnBack_Click" CssClass="btn btn-default centrado" ToolTip="Regresar a la pagina anterior"> <span class="glyphicon glyphicon-chevron-left"></span></asp:LinkButton>
                </div>
                <div class="col-sm-11">
                    <h2>Análisis en proceso de firma.</h2>
                    <hr class="red">
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="alert alert-danger">
                        <strong>¡Atención!</strong>
                        <p class="text-center">
                            El proceso de firma masiva puede demorar varios minutos en completarse dependiendo de la cantidad de análisis que se hubieren seleccionado.
                        </p>
                        <p class="text-center">
                            Es importante que los laboratorios tomen en cuenta dicha demora, pues para cumplir con la obligación establecida en el artículo 278-B, fracción VIII, primer párrafo de la Ley Federal de Derechos, únicamente se tendrán por firmados aquellos análisis cuya firma se complete a más tardar el séptimo día hábil posterior a que haya concluido el trimestre que corresponda, de conformidad con lo dispuesto en la regla 7.39 de la Resolución Miscelánea Fiscal.
                        </p>
                        <p class="text-center">
                            Los análisis firmados fuera de este plazo, serán considerados extemporáneos.  
                        </p>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div id="divBarraProgreso" class="panel panel-default">
                        <div class="panel-heading">
                            <h5>Progreso.</h5>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-12">
                                <div class="progress">
                                    <asp:HiddenField runat="server" ID="hfLaboratorioID" />
                                    <div id="myBar" class="progress-bar progress-bar-striped active" role="progressbar"
                                        aria-valuenow="40" aria-valuemin="0" aria-valuemax="100" style="width: 0%">
                                        <span id="spnPorcentaje"></span>
                                    </div>
                                </div>
                            </div>
                            <div id="divMensajeCompletos" class="pull-right">
                                <label><span id="spnAnalisisCompletados"></span> de <span id="spnTotales"></span></label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="table-responsive">
                        <div runat="server" id="divTablaAnalisis" class="panel panel-default">
                            <div class="panel-heading">
                                <h5>Análisis.</h5>
                            </div>
                            <div class="panel-body table-responsive">
                                <asp:HiddenField ID="hfOrdenTrabajo" runat="server" />
                                <asp:HiddenField runat="server" ID="hfAnalisisActivos" />
                                <asp:HiddenField runat="server" ID="hfMostrarModal" />
                                <div class="pull-right">
                                    <asp:LinkButton runat="server" ID="btnLimpiar" ToolTip="Nueva orden de trabajo" CssClass="btn btn-default btn-sm" OnClick="btnLimpiar_OnClick">Limpiar procesos finalizados</asp:LinkButton>
                                </div>
                                <br />
                                <br />
                                <table id="tablaMuestraSimple" class="table table-responsive table-condensed table-striped text-center">
                                    <thead>
                                        <tr>
                                            <th></th>
                                            <th class="text-center">Número de informe</th>
                                            <th class="text-center">Contribuyente</th>
                                            <th class="text-center">Análisis</th>
                                            <th class="text-center">Fecha de conformación</th>
                                            <th class="text-center">Hora de conformación</th>
                                            <th class="text-center">Fecha de firma</th>
                                            <th>Hora de firma</th>
                                            <th class="text-center">Estatus</th>
                                            <th class="text-center">Acción</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="rptEstatusFirma" ItemType="SIRALAB_DAL.AnalisisColaEncabezado" OnItemDataBound="rptEstatusFirma_OnItemDataBound">
                                            <ItemTemplate>
                                                <tr>
                                                    <td></td>
                                                    <td><%#:Item.Analisi.OrdenTrabajo.NumeroInforme%></td>
                                                    <td><%#Item.Analisi.OrdenTrabajo.Cliente.RFC %></td>
                                                    <td>
                                                        <%# Item.Analisi.OrdenTrabajo.Norma.RequiereCompuesta ? 
                                "Promedio Diario " + (Item.Analisi.OrdenTrabajo.Analisis.OrderBy(a => a.AnalisisID).ToList().FindIndex(a => a.AnalisisID == Item.AnalisisID) + 1) :
                                "Muestra " + (Item.Analisi.OrdenTrabajo.Analisis.OrderBy(a => a.AnalisisID).ToList().FindIndex(a => a.AnalisisID == Item.AnalisisID) + 1)
                                                        %>
                                                    </td>
                                                 
                                                    <td><%# Item.Analisi.OrdenTrabajo.Norma.RequiereCompuesta ? Item.Analisi.Muestras.Count == 0 ? "": Item.Analisi.Muestras.FirstOrDefault().FechaConformacion != null ? ((DateTime) Item.Analisi.Muestras.FirstOrDefault().FechaConformacion).ToString("dd/MM/yyyy") : "" : "No aplica" %></td>
                                                    <td runat="server"><%# Item.Analisi.OrdenTrabajo.Norma.RequiereCompuesta ? Item.Analisi.Muestras.Count == 0 ? "": Item.Analisi.Muestras.FirstOrDefault().FechaConformacion != null ? ((DateTime) Item.Analisi.Muestras.FirstOrDefault().FechaConformacion).ToString("HH:mm") : "" : "No aplica" %></td>
                                                    <td runat="server" data-analisis-fechaconformacionid="<%# Item.AnalisisID %>"><%# Item.FechaFirma != null ? ((DateTime) Item.FechaFirma).ToString("dd/MM/yyyy") : "" %></td>
                                                    <td runat="server" data-analisis-horaconformacionid="<%# Item.AnalisisID %>"><%# Item.FechaFirma != null ? ((DateTime) Item.FechaFirma).ToString("HH:mm") : "" %></td>
                                                    <td runat="server" data-analisisid="<%# Item.AnalisisID %>" data-cola-id="<%# Item.AnalisisColaDetalleID %>">
                                                        <span runat="server" visible="<%# Item.EstatusID == enumEstatusAnalisis.Finalizado.ParseToInt() %>" class=" text-success" data-toggle="tooltip" data-placement="top" title="Firmado y enviado"><i class="glyphicon glyphicon-ok"></i></span>
                                                        <span runat="server" visible="<%# Item.EstatusID == enumEstatusAnalisis.FirmaFallida.ParseToInt() %>" class="text-danger" data-toggle="tooltip" data-placement="top" title="No se pudo firmar porque el servicio de sellado de CONAGUA no estaba disponible. Vuelva a intentar la firma más tarde."><i class="glyphicon glyphicon-remove"></i></span>
                                                        <span runat="server" visible="<%# Item.EstatusID == enumEstatusAnalisis.EnvioFallido.ParseToInt() %>" class="text-danger" data-toggle="tooltip" data-placement="top" title="El análisis se firmó pero ocurrió un error al enviarlo. Intente nuevamente más tarde."><i class="glyphicon glyphicon-remove"></i></span>
                                                        <span runat="server" visible="<%# Item.EstatusID < enumEstatusAnalisis.Finalizado.ParseToInt()  %>" class="text-primary"><i class="glyphicon glyphicon-time"></i></span>

                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="btnFirmar" runat="server" Visible="<%# !Item.AnalisisColaDetalle.Activo && Item.EstatusID == enumEstatusAnalisis.FirmaFallida.ParseToInt() %>" OnClick="btnFirmar_OnClick" CommandArgument="<%# Item.AnalisisID%>" class="btn btn-default btn-xs firmar">
                                <span class="glyphicon glyphicon-certificate" data-toggle="tooltip" data-placement="top" title="Firmar análisis"></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="btnDescargaReporte" Visible="<%# !Item.AnalisisColaDetalle.Activo && (Item.EstatusID == enumEstatusAnalisis.Firmado.ParseToInt() || Item.EstatusID == enumEstatusAnalisis.EnvioFallido.ParseToInt())  %>" OnClick="btnDescargaReporte_OnClick" CommandArgument="<%# Item.AnalisisID %>" CssClass="btn btn-default btn-xs reporte" data-toggle="tooltip" data-placement="top" title="Descargar informe">
                                <span class="glyphicon glyphicon-file"></span>
                                                        </asp:LinkButton>

                                                        <asp:LinkButton runat="server" ID="btnInfoAnalisis" CssClass="btn btn-default btn-xs info" data-toggle="tooltip" data-placement="top" title="Ver información de la orden de trabajo" OnClick="btnInfoAnalisis_OnClick" CommandArgument="<%#Item.Analisi.OrdenTrabajo.OrdenTrabajoID %>">
                                <span class="glyphicon glyphicon-info-sign"></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" ID="btnEnvio" Visible="<%# !Item.AnalisisColaDetalle.Activo && (Item.EstatusID == enumEstatusAnalisis.Finalizado.ParseToInt() && Item.EstatusID == enumEstatusAnalisis.Firmado.ParseToInt() || Item.EstatusID == enumEstatusAnalisis.EnvioFallido.ParseToInt())  %>" OnClick="btnEnvio_OnClick" CommandArgument="<%# Item.AnalisisColaEncabezadoID %>" CssClass="btn btn-default btn-xs mail" data-toggle="tooltip" data-placement="top" title="Renviar informe y acuse al contribuyente">
                                <span class="glyphicon glyphicon-send"></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div style="margin: 10px">
                <uc1:wucAppletFIEL runat="server" ID="wucAppletFIEL" OnFirmarColeccion="wucAppletFIEL_FirmarColeccion" />
            </div>
            <script type="text/javascript">

                function InitializeHndMC() {
                    $(function () {

                        $("#divBarraProgreso").hide();
                        $("#divMensajeCompletos").hide();
                        actualizarBarraProgreso();
                    });

                    function llamaServidor() {
                        setTimeout(function () {
                            actualizarBarraProgreso();
                        },
                            10000);
                    }

                    function actualizarBarraProgreso() {

                        var laboratorioID = $("input[id*='hfLaboratorioID']").val();
                        var elem = document.getElementById("myBar");
                        var colas = [];
                        $("[data-cola-id]").each(function () {
                            var colaId = $(this).data('cola-id');
                            if (!colas.includes(colaId)) {
                                colas.push(colaId);
                            }
                        });


                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "EstatusFirmado.aspx/ActualizaBarraProgreso",
                            data: JSON.stringify({ laboratorioId: laboratorioID, listaColas: colas }),
                            dataType: "json"
                        })
                            .done(function (resultado) {
                                var objeto = JSON.parse(resultado.d.Message);
                                if (objeto != undefined) {
                                    var porcentaje = objeto.Porcentaje;
                                    if (parseFloat(porcentaje) >= 100) {
                                        $("#divBarraProgreso").hide();
                                        $("#divMensajeCompletos").hide();
                                        if (objeto.Analisis != undefined) {
                                            for (let analisis of objeto.Analisis) {
                                                pintaPantalla(analisis.AnalisisId, analisis.ColaId, analisis.FechaConformacion, analisis.HoraConformacion, analisis.EstatusId)
                                            }
                                        }
                                    } else {
                                        
                                        $("#divBarraProgreso").show();
                                        $("#divMensajeCompletos").show();
                                        elem.style.width = porcentaje.toString() + "%";
                                        $("#spnPorcentaje").html(Number(porcentaje).toFixed(0).toString() + "%");
                                        $("#spnAnalisisCompletados").html(parseInt(objeto.Total) - parseInt(objeto.EnProceso));
                                        $("#spnTotales").html(objeto.Total.toString());

                                        if (objeto.Analisis != undefined) {
                                            for (let analisis of objeto.Analisis) {
                                                pintaPantalla(analisis.AnalisisId, analisis.ColaId, analisis.FechaConformacion, analisis.HoraConformacion, analisis.EstatusId)
                                            }
                                        }
                                    }
                                }
                            })
                            .fail(function () {
                                llamaServidor();
                            })
                            .complete(function () {
                                if ($("#divBarraProgreso").is(":visible")) {
                                    llamaServidor();
                                }
                            });
                    }

                    function pintaPantalla(analisisId, colaId, fechaConformacion, horaConformacion, estatusId) {
                        if ($("td[data-analisisid='" + analisisId + "'][data-cola-id='" + colaId + "']").size() > 0) {

                            $("td[data-analisisid='" + analisisId + "'][data-cola-id='" + colaId + "']").empty();
                            if (parseInt(estatusId) < 11) {
                                $("td[data-analisisid='" + analisisId + "'][data-cola-id='" + colaId + "']").append('<span class="text-primary"><i class="glyphicon glyphicon-time"></i></span>');
                            } else if (parseInt(estatusId) == 11) {
                                $("td[data-analisisid='" + analisisId + "'][data-cola-id='" + colaId + "']").append('<span class=" text-success" data-toggle="tooltip" data-placement="top" title="Firmado y enviado"><i class="glyphicon glyphicon-ok"></i></span>');
                            } else {
                                $("td[data-analisisid='" + analisisId + "'][data-cola-id='" + colaId + "']").append('<span class="text-danger"><i class="glyphicon glyphicon-remove"></i></span>');
                            }
                        }

                        if ($("td[data-analisis-fechaconformacionid='" + analisisId + "']").size() > 0) {
                            $("td[data-analisis-fechaconformacionid='" + analisisId + "']").empty();
                            $("td[data-analisis-fechaconformacionid='" + analisisId + "']").append(fechaConformacion);
                        }

                        if ($("td[data-analisis-horaconformacionid='" + analisisId + "']").size() > 0) {
                            $("td[data-analisis-horaconformacionid='" + analisisId + "']").empty();
                            $("td[data-analisis-horaconformacionid='" + analisisId + "']").append(horaConformacion);
                        }
                    }

                }
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
