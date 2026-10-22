<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucSucursalLogin.ascx.cs" Inherits="SIRALAB.Controles.wucSucursalLogin" ClientIDMode="AutoID" %>
<%@ Import Namespace="SIRALAB_DAL" %>

<asp:UpdatePanel ID="upSucursal" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbCerrar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="rptrSucursales" />
    </Triggers>
    <ContentTemplate>
        <div id="ModalSucursal" class="modal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title">
                            <asp:LinkButton ID="lnkbCerrar" runat="server" ToolTip="Cerrar" OnClick="lnkbCerrar_Click"><div></div></asp:LinkButton>
                            <asp:Label ID="lblTitulo" runat="server"><h4 class="modal-title">Seleccione su sucursal</h4></asp:Label>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <asp:Repeater ID="rptrSucursales" runat="server" ItemType="SIRALAB_DAL.Laboratorio">
                            <HeaderTemplate>
                                <table class="table table-condensed table-striped table-sm">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <%#:Item.LaboratorioNombre() + "-(" + Item.LaboratorioNombreEstado() + ")" %>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkbSeleccionar" runat="server" CssClass="btn btn-primary" ToolTip="Seleccionar" CommandArgument="<%#:Item.LaboratorioID %>" OnClick="lnkbSeleccionar_Click">
                                            <i class="glyphicon glyphicon-ok"></i>
                                        </asp:LinkButton>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal --> 
        <input runat="server" type="text" id="txtidLaboratorio" hidden />
        <input runat="server" type="text" id="txtURL" hidden />
    </ContentTemplate>
    
</asp:UpdatePanel>



<%--  AlertaSimple--%>
<div class="modal fade" id="alertaSimpleReincidencia" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="upModal" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header" style="align-self: center;">
                        <h4 class="modal-title">AVISO</h4>
                    </div>
                    <div class="modal-body">
                                   
                        <p runat="server" id="TextoAlertaSimpleReincidencia" style="text-align: center;">LABORATORIO REINCIDENTE: De conformidad con el artículo 278-B, 
                            fracción VIII, párrafo segundo de la Ley Federal de Derechos, al haber actualizado la reincidencia por incumplimiento a la obligación establecida en el primer párrafo de la citada fracción y artículo, 
                            la aprobación otorgada por la Comisión Nacional del Agua al laboratorio queda sin efectos, única y exclusivamente en el ámbito fiscal, es decir, 
                            todos los resultados de análisis o informes de laboratorio efectuados a las descargas de aguas residuales de los contribuyentes que lo hayan elegido como su laboratorio acreditado ante la Entidad Mexicana de Acreditación y aprobado por la aludida Comisión, 
                            para tal efecto, a partir de la materialización de la sanción y hasta la culminación de vigencia de la aprobación 
                            NO SERÁN CONSIDERADOS PARA EL OTORGAMIENTO DE BENEFICIOS FISCALES, ESTABLECIDOS EN LA CITADA LEY, EN CONSECUENCIA, 
                            TALES CONTRIBUYENTES NO PODRÁN GOZAR DEL ACREDITAMIENTO Y LA EXENCIÓN DESEADOS. </p>
                    </div>
                    <div class="modal-footer">
                        <%--<button type="button" class="btn btn-primary" id="btnAceptarModal" runat="server" data-dismiss="modal">Aceptar</button>--%>
                         <asp:LinkButton ID="btnAceptarModalReincidencia" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="lnkbAceptarReincidencia_Click"> 
                                  </asp:LinkButton>
                    </div>
                </div>
                <!-- /.modal-content -->
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- /.modal-dialog -->
</div>
 <%--  AlertaSimple--%>
<div class="modal fade" id="alertaSimpleApercibimiento" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header" style="align-self: center;">
                        <h4 class="modal-title">AVISO</h4>
                    </div>
                    <div class="modal-body">
                                   
                        <p runat="server" id="Texto" style="text-align: center;">LABORATORIO APERCIBIDO: En caso de reincidencia en el incumplimiento de la obligación de presentar los informes trimestrales en tiempo, 
                            a que se refiere el primer párrafo, de la fracción VIII, del artículo 278-B de la Ley Federal de Derechos, 
                            quedará sin efectos la aprobación que le fue otorgada por la Comisión Nacional del Agua, única y exclusivamente para efectos fiscales. 
                            Y, por lo tanto, los informes que sean generados y emitidos por este laboratorio no producirán el efecto deseado, en los contribuyentes que lo hayan elegido para el muestreo de sus aguas residuales, 
                            respecto a la obtención de los beneficios fiscales del acreditamiento y la exención establecidos en el artículo 278 y 282, fracción I del ordenamiento citado, 
                            al momento de presentar sus declaraciones correspondientes en el Sistema Declar@gua. </p>
                    </div>
                    <div class="modal-footer">
                        <%--<button type="button" class="btn btn-primary" id="btnAceptarModal" runat="server" data-dismiss="modal">Aceptar</button>--%>
                         <asp:LinkButton ID="btnAceptarModalApercibimiento" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="lnkbAceptarApercibimiento_Click"> 
                                  </asp:LinkButton>
                    </div>
                </div>
                <!-- /.modal-content -->
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- /.modal-dialog -->
</div>


<%--  AlertaSimple--%>
<div class="modal fade" id="alertaSimpleReincidenciaPreventiva" data-backdrop="static" data-keyboard="false">
    <div class="modal-dialog">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header" style="align-self: center;">
                        <h4 class="modal-title">AVISO</h4>
                    </div>
                    <div class="modal-body">
                                   
                        <p runat="server" id="TextoAlertaReincidenciaPreventiva" style="text-align: center;">LABORATORIO REINCIDENTE: De conformidad con el artículo 278-B, fracción VIII, 
                            párrafo segundo de la Ley Federal de Derechos, al haber actualizado la reincidencia por incumplimiento a la obligación establecida en el primer párrafo de la citada fracción y artículo, 
                            la aprobación otorgada por la Comisión Nacional del Agua al laboratorio queda sin efectos, única y exclusivamente en el ámbito fiscal, 
                            es decir, los contribuyentes que hayan elegido a este laboratorio para la emisión de sus resultados de análisis o informes de laboratorio, 
                            no podrán utilizarlos para gozar del acreditamiento o la exención al momento de presentar sus declaraciones. Sin embargo, 
                            al tener el derecho de impugnarla dentro de los plazos legales que la ley establece para tal efecto, la sanción antes descrita no se materializará hasta en tanto: 1) 
                            Hayan vencidos los mismos, o bien, 2) Hasta que se haya resuelto en definitiva, EN FAVOR DE LA CONAGUA, el medio promovido en contra de la resolución de reincidencia, 
                            momento hasta el cual todos los informes o resultados de los análisis efectuados a las descargas de los contribuyentes que lo hayan elegido como su laboratorio acreditado 
                            ante la Entidad Mexicana de Acreditación y aprobado por la aludida Comisión, NO SERÁN CONSIDERADOS PARA EL OTORGAMIENTO DE BENEFICIOS FISCALES, ESTABLECIDOS EN LA CITADA LEY, 
                            EN CONSECUENCIA, TALES CONTRIBUYENTES NO PODRÁN GOZAR DEL ACREDITAMIENTO Y LA EXENCIÓN.  </p>
                    </div>
                    <div class="modal-footer">
                        <%--<button type="button" class="btn btn-primary" id="btnAceptarModal" runat="server" data-dismiss="modal">Aceptar</button>--%>
                         <asp:LinkButton ID="btnAceptarModalReincidenciaPreventiva" runat="server" CssClass="btn btn-primary" Text="Aceptar" OnClick="lnkbAceptarReincidenciaPreventiva_Click"> 
                                  </asp:LinkButton>
                    </div>
                </div>
                <!-- /.modal-content -->
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <!-- /.modal-dialog -->
</div>