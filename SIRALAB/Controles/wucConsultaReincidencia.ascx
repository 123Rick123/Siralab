<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConsultaReincidencia.ascx.cs" Inherits="SIRALAB.Controles.wucConsultaReincidencia" %>
<%--panel busqueda laboratorio--%>
<asp:Panel ID="pnlCapturaApercibido" runat="server" Visible="true">
    <asp:UpdatePanel ID="updBusquedaLab" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>
            <div class="panel-body">

                <div class="row">
                    <div class="table-responsive m-b" id="esElGrid">
                        <asp:GridView ID="grdHistorico" runat="server" DataKeyNames="idLaboratorio"
                            AutoGenerateColumns="False"
                            EmptyDataText="No hay resultados para la búsqueda solicitada."
                            AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True"
                            Width="97%" CssClass="table table-striped" CellSpacing="2" BorderWidth="1px">
                            <Columns>
                                <asp:BoundField DataField="idLaboratorio" ShowHeader="true" HeaderText="id" Visible="False" />
                                <asp:BoundField DataField="RFCLaboratorio" ShowHeader="true" HeaderText="RFC" />
                                <asp:BoundField DataField="NombreLaboratorio" ShowHeader="true" HeaderText="Nombre de laboratorio" />

                                <asp:TemplateField HeaderText="Números y vigencias de aprobación">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDEtalle" runat="server" Text="Ver" AutoPostBack="True" OnClick="lnkDEtalle_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="form-text MensajeError" id="MensajeErrorIdLab" runat="server" style="color: #D0021B"></div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<%--panel formulario alta--%>
<asp:Panel ID="PanelFormulario" runat="server" Visible="true">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">

        <ContentTemplate>
            <div class="row">
                <div class="col-md-6">
                    <label class="control-label">Número de oficio de la resolución*:</label>
                    <asp:TextBox ID="txtNoOficioResolucion" placeholder="Número de oficio de la resolución" CssClass="form-control input-sm" MaxLength="50" AutoCompleteType="None" autocomplete="cc-csc" runat="server" />
                    <div class="form-text MensajeError" id="MensajeErrorNoResolucion" runat="server" style="color: #D0021B"></div>

                </div>
                <div class="col-md-6">
                    <label for="name">Fecha de la resolución*:</label>
                    <br />

                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtFechaResolucion" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" autocomplete="cc-csc" />
                        <asp:RegularExpressionValidator ID="RExprFechaResolucion"
                            ControlToValidate="txtFechaResolucion"
                            ValidationExpression="^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/([0-9]{4})$"
                            Display="Static"
                            ErrorMessage="La fecha de la resolución no tiene el formato correcto."
                            Style="color: #D0021B"
                            EnableClientScript="False"
                            runat="server" />
                        <%-- <input id="txtFechaResolucion" type="text" runat="server" class="form-control" maxlength="10" autocompletetype="None" />--%>
                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                        <div class="form-text MensajeError" id="MensajeErrorFechaResolucion" runat="server" style="color: #D0021B"></div>

                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <br />
                    <br />
                    <label for="name">Fecha de notificación de la resolución*:</label>

                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtFechaNotificacioResolucion" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" autocomplete="cc-csc" />
                        <asp:RegularExpressionValidator ID="RExprFechaNotificacion"
                            ControlToValidate="txtFechaNotificacioResolucion"
                            ValidationExpression="^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/([0-9]{4})$"
                            Display="Static"
                            ErrorMessage="La fecha de notificación no tiene el formato correcto."
                            Style="color: #D0021B"
                            EnableClientScript="False"
                            runat="server" />

                        <%--<input id="txtFechaNotificacioResolucion" type="text" runat="server" class="form-control" maxlength="10" autocompletetype="None" />--%>
                        <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                        <div class="form-text MensajeError" id="MensajeErrorFechaNotificacion" runat="server" style="color: #D0021B"></div>
                    </div>
                </div>
                <div class="col-md-6">
                    <br />
                    <br />
                    <label class="control-label">Evidencia*:</label>
                    <br />
                    <div class="row">
                        <div class="col-md-3">
                            <asp:Button ID="btnDescargaDoc" runat="server" class="btn btn-success" OnClick="DescargarArchivo_Click" Text="Descargar" />

                        </div>
                        <div class="col-md-3">
                            <asp:FileUpload ID="flDocumento" Style="color: transparent" onchange="ChangeDocumento()" runat="server" class="inputfile" data-multiple-caption="{count} files selected" multiple />
                        </div>
                        <div class="col-md-9">
                            <p for="contenido_upf_req">
                                <span id="nameFile" runat="server">Ningún archivo selec&hellip;</span>
                            </p>
                        </div>
                    </div>
                    <%--  <input id="flDocumento" type="file" onchange="ChangeDocumento()" runat="server">--%>
                    <div class="form-text MensajeError" id="MensajeErrorDocumento" runat="server" style="color: #D0021B"></div>
                    <input runat="server" type="text" id="txtErrorDocumento" hidden />
                    <input runat="server" type="text" id="txtbasecode" hidden />
                    <input runat="server" type="text" id="txtbasecodeNombreArchivo" hidden />
                    <input type="hidden" id="HdIdIc2" runat="server" />

                    <input runat="server" type="text" id="txtidDoc" hidden />






                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <br />
                    <br />
                    <label for="name">Trimestre del incumplimiento*:</label>
                    <div class="form-group datepicker-group">
                        <asp:DropDownList ID="ddlTrimestre" CssClass="form-control input-sm" autocompletetype="None" autocomplete="cc-csc" runat="server"></asp:DropDownList>
                        <div class="form-text MensajeError" id="MensajeErrorTrimestre" runat="server" style="color: #D0021B"></div>
                    </div>
                </div>
                <div class="col-md-6">
                    <br />
                    <br />
                    <label class="control-label">Año del incumplimiento*:</label>
                    <asp:DropDownList ID="ddlAnio" runat="server" CssClass="form-control input-sm" autocompletetype="None" autocomplete="cc-csc"></asp:DropDownList>
                    <div class="form-text MensajeError" id="MensajeErrorAnio" runat="server" style="color: #D0021B"></div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6">
                    <br />
                    <br />
                    <label for="name">Número de expediente*:</label>
                    <div class="form-group datepicker-group">
                        <asp:TextBox ID="txtNoExpediente" placeholder="Número de expediente" CssClass="form-control input-sm" AutoCompleteType="None" MaxLength="50" autocomplete="cc-csc" runat="server" />
                        <div class="form-text MensajeError" id="MensajeErrorNoExpediente" runat="server" style="color: #D0021B"></div>
                    </div>
                </div>
                <div class="col-md-6">
                    <br />
                    <br />
                    <label class="control-label">Iniciales del responsable*:</label>
                    <asp:TextBox ID="txtIniciales" placeholder="Iniciales del responsable" CssClass="form-control input-sm" AutoCompleteType="None" MaxLength="50" autocomplete="cc-csc" runat="server" />
                    <div class="form-text MensajeError" id="MensajeErrorIniciales" runat="server" style="color: #D0021B"></div>
                </div>
            </div>


            <div class="form-group">
                <label>Observaciones*:</label>
                <textarea class="form-control" rows="5" id="txtObservaciones" maxlength="500" runat="server"></textarea>
                <div class="form-text MensajeError" id="MensajeErrorObservaciones" runat="server" style="color: #D0021B"></div>

            </div>


            <div class="row">
                <div class="col-md-3" style="text-align: left;">

                    <asp:Button ID="btnDescargaReincidencia" runat="server" CssClass="btn btn-primary" OnClick="DescargaReincidencia_Click" Text="Descargar reincidencia" />
                    <input runat="server" type="text" id="txtIdApercibimiento" hidden />
                    <input runat="server" type="text" id="txtIdReincidencia" hidden />
                </div>
                 <div class="col-md-3" style="text-align: left;">
                  
                    <asp:Button ID="btnVerImpugnacion" Visible="false" CssClass="btn btn-default" Text="Ver impugnaciones" runat="server" OnClick="btnVerImpugnacion_Click" />
                </div>
                <div class="col-md-6" style="text-align: right;">
                    <asp:Button ID="btnCerrarPagina" runat="server" CssClass="btn btn-default" OnClick="btnCerrarPagina_Click" Text="Cerrar" />


                    <%--<button type="button" class="btn btn-primary" onclick="ValidarCampos()">Guardar</button>--%>
                </div>
                
            </div>
        </ContentTemplate>
        <Triggers>

            <asp:PostBackTrigger ControlID="btnDescargaReincidencia" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Panel>
<%--Modal Detalle--%>
<div id="modalDetalleR" class="modal fade" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header text-center">
                        <h4 class="modal-title">
                            <label id="tituloDetalle">Números y vigencias de aprobación</label>
                        </h4>
                    </div>
                    <div class="modal-body text-center">
                        <asp:GridView runat="server" ID="gvdetalleR" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField DataField="Numero" HeaderText="Número de aprobación" />
                                <asp:BoundField DataField="FechaFin" HeaderText="Vigencia de aprobación" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<!-- /.modal -->

<%--Modal DetalleSinInfo--%>
<div id="modalDetalleRSinInfo" class="modal fade" role="dialog" aria-labelledby="myModalLabel" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header text-center">
                        <h4 class="modal-title">
                            <label id="tituloDetalleSinInfo">Números y vigencias de aprobación</label>
                        </h4>
                    </div>
                    <div class="modal-body text-center">
                        <p>Sin información</p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Cerrar</button>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<!-- /.modal -->

<script type="text/javascript">
    function MostrarModalDetalleR() {
        $("#modalDetalleR").modal("show");
    }
    function MostrarModalDetalleSinInfo() {
        $("#modalDetalleRSinInfo").modal("show");
    }
</script>
