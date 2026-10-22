<%@ Page Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="DesgloseHistoricoImpugnacionApercibimiento.aspx.cs" Inherits="SIRALAB.DesgloseHistoricoImpugnacionApercibimiento" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <div  class="row">
        <br />
        <br />
        <asp:GridView runat="server" ID="gvImpugnacionApercibimiento" DataKeyNames="IdImpugnacion"
            AutoGenerateColumns="False" EmptyDataText="No hay resultados para la búsqueda solicitada."
            AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True"
            Width="100%" CssClass="table table-striped" CellSpacing="2" BorderWidth="1px">
            <Columns>
                <asp:BoundField DataField="IdImpugnacion" HeaderText="IdImpugnacion" Visible="false" />
                <asp:BoundField DataField="MediosDeImpugnacion" HeaderText="Medio de impugnación" />
                 <asp:BoundField DataField="FechaImpugnacion" HeaderText="Fecha de impugnación" />
                <asp:BoundField DataField="AnteQuien" HeaderText="Ante quien" />
                <asp:BoundField DataField="Estatus" HeaderText="Estatus" />
                <asp:BoundField DataField="Expediente" HeaderText="Expediente" />
                <asp:BoundField DataField="FechaResolucion" HeaderText="Fecha de resolución" />
                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                <asp:TemplateField HeaderText="Detalle">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDEtalleImpugnacion" runat="server" Text="Ver" AutoPostBack="True" OnClick="lnkDEtalleImpugnacion_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>


    <asp:Panel ID="PanelFormulario" runat="server" Visible="false">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">

            <ContentTemplate>
                <div class="row">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h5>Métodos de impugnación </h5>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label for="name">Medio de impugnación*:</label>
                                <asp:DropDownList ID="ddlMedioImpugnacion" AutoPostBack="true" CssClass="form-control input-sm" autocompletetype="None" autocomplete="cc-csc" runat="server"></asp:DropDownList>
                                <div class="form-text MensajeError" id="MensajeErrorMedioImpugnacion" runat="server" style="color: #D0021B"></div>
                            </div>
                            <div class="col-md-6">
                                <label for="name">Fecha de impugnación*:</label>

                                <div class="form-group datepicker-group">
                                    <asp:TextBox ID="txtFechaImpugnacion" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" autocomplete="cc-csc" />

                                    <span class="glyphicon glyphicon-calendar" aria-hidden="true"></span>
                                    <div class="form-text MensajeError" id="MensajeErrorFechaImpugnacion" runat="server" style="color: #D0021B"></div>
                                    <asp:RegularExpressionValidator ID="RExprFechaImpugnacion"
                                        ControlToValidate="txtFechaImpugnacion"
                                        ValidationExpression="^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/([0-9]{4})$"
                                        Display="Static"
                                        ErrorMessage="La fecha de impugnación no tiene el formato correcto."
                                        Style="color: #D0021B"
                                        EnableClientScript="False"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label class="control-label">Ante quien*:</label>
                                <asp:TextBox ID="txtAnteQuien" placeholder="Ante quien" CssClass="form-control input-sm" MaxLength="50" AutoCompleteType="None" autocomplete="cc-csc" runat="server" />
                                <div class="form-text MensajeError" id="MensajeErrorAnteQuien" runat="server" style="color: #D0021B"></div>
                            </div>
                            <div class="col-md-6">
                                <label for="name">Estatus*:</label>
                                <asp:DropDownList ID="ddlEstatus" CssClass="form-control input-sm" AutoPostBack="true" autocompletetype="None" autocomplete="cc-csc" runat="server"></asp:DropDownList>
                                <div class="form-text MensajeError" id="MensajeErrorEstatus" runat="server" style="color: #D0021B"></div>
                            </div>

                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <br />
                                <label for="name">Número de expediente*:</label>
                                <div class="form-group datepicker-group">
                                    <asp:TextBox ID="txtNoExpediente" placeholder="Número de expediente" CssClass="form-control input-sm" AutoCompleteType="None" MaxLength="50" autocomplete="cc-csc" runat="server" />
                                    <div class="form-text MensajeError" id="MensajeErrorNoExpediente" runat="server" style="color: #D0021B"></div>
                                </div>
                            </div>
                            <br />
                            <div class="col-md-6">
                                <label id="lblFechaResolucion" runat="server" for="name" visible="false">Fecha de resolución*:</label>
                                <div class="form-group datepicker-group">
                                    <asp:TextBox ID="txtFechaResolucion" runat="server" CssClass="form-control input-sm" MaxLength="10" AutoCompleteType="None" autocomplete="cc-csc" Visible="false" />

                                    <span class="glyphicon glyphicon-calendar" id="spanCalendarioFechaRes" runat="server" aria-hidden="true" visible="false"></span>
                                    <div class="form-text MensajeError" id="MensajeErrorFechaResolucion" runat="server" style="color: #D0021B"></div>
                                    <asp:RegularExpressionValidator ID="RExprFechaResolucion"
                                        ControlToValidate="txtFechaResolucion"
                                        ValidationExpression="^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\/(0[1-9]|1[0-2])\/([0-9]{4})$"
                                        Display="Static"
                                        ErrorMessage="La fecha de resolución no tiene el formato correcto."
                                        Style="color: #D0021B"
                                        EnableClientScript="False"
                                        runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label>Observaciones*:</label>
                            <textarea class="form-control" rows="5" id="txtObservaciones" maxlength="500" runat="server"></textarea>
                            <div class="form-text MensajeError" id="MensajeErrorObservaciones" runat="server" style="color: #D0021B"></div>

                        </div>
                        <div class="row">
                            <div class="col-md-6" style="text-align: left;">
                                <%--<asp:Button ID="btnDescarga" Style="display: none" runat="server" CssClass="btn btn-primary" OnClick="Descarga_Click" Text="Descargar reincidencia" />--%>
                            </div>
                            <div class="col-md-6" style="text-align: right;">
                                <asp:Button ID="btnCerrarPagina" runat="server" CssClass="btn btn-default" Text="Cerrar" OnClick="btnCerrarPagina_Click" />
                                <%--<button type="button" class="btn btn-primary" onclick="ValidarCampos()">Guardar</button>--%>
                            </div>
                        </div>

                        <%-- controles hidden--%>
                        <input runat="server" type="text" id="txtidApercibimiento" hidden />
                        <input runat="server" type="text" id="txtidReincidencia" hidden />
                        <input runat="server" type="text" id="txtBanderaEsConsulta" hidden />
                    </div>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
     <div class="row">
        <div class="col-md-6" style="text-align: left;">
        </div>
        <div class="col-md-6" style="text-align: right;">
            <asp:Button ID="btnRegresar" runat="server" CssClass="btn btn-default" Text="Regresar" OnClick="btnRegresar_Click"  />
        </div>
    </div>
</asp:Content>
