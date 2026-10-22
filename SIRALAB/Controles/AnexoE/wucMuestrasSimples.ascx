<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMuestrasSimples.ascx.cs" Inherits="SIRALAB.Controles.AnexoE.wucMuestrasSimples" %>
<%@ Import Namespace="SIRALAB_DAL" %>
<%@ Import Namespace="System.Windows.Forms.VisualStyles" %>
<%@ Import Namespace="System.Windows.Forms" %>


<div style="padding: 15px;">
    <div id="hiddenFields"></div>
    <div class="table-responsive">
        <asp:HiddenField ID="hfAnalisisID" runat="server" />
        <asp:HiddenField ID="hfNormaId" runat="server" />
        <asp:UpdatePanel ID="udpMuestrasSimples" runat="server"  ChildrenAsTriggers="False" UpdateMode="Conditional">
            <ContentTemplate>
                   
                <asp:Repeater ID="rptMuestrasSimples" runat="server" ItemType="SIRALAB_DAL.Muestra" OnItemDataBound="rptMuestrasSimples_OnItemDataBound">
                    <HeaderTemplate>
                        <table id="tablaMuestraSimple" class="table table-condensed table-striped text-center" data-matrix style="font-size: 12px;">
                            <thead>
                                <tr>
                                    <th runat="server" id="thIndex" class="ocultaCamposSDT" style="vertical-align: middle; text-align: center">Muestra simple</th>
                                    <th style="vertical-align: middle; text-align: center">Fecha</th>
                                    <th runat="server" id="thHoras" class="ocultaCamposSDT" style="vertical-align: middle; text-align: center">Hora</th>

                                    <%--Elimino caudal, participación del caudal y volumen de muestra simple porque están hardcodeados--%>
                                    <asp:Repeater ID="rptHeader" runat="server" ItemType="SIRALAB_DAL.Extensiones.Columnas">
                                        <%--Aquí inserta el encabezado de las columnas--%>
                                        <ItemTemplate>
                                            <th style="max-width: 85px; min-width: 110px; vertical-align: middle; text-align: center" data-parameter-id="<%#:Item.ParametroID %>"><%#:Item.Nombre%></th>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <th runat="server" id="thObservaciones" style="text-align: center; vertical-align: middle">Observaciones</th>
                                </tr>
                            </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr data-muestra-id="<%#:Item.MuestraID %>">
                            <td runat="server" id="tdIndex" class="text-center text-sm ocultaCamposSDT" style="text-align: center; vertical-align: middle"><%#:Item.Index%></td>
                            <td class="text-center text-sm" style="vertical-align: middle">
                                <asp:TextBox ID="txtFecha" runat="server" Text='<%#:Item.Fecha != null ? ((DateTime)Item.Fecha).ToString("dd/MM/yyyy") : "" %>' OnClientClick="$gmx('#'+this.id).datepicker();" CssClass="form-control input-sm" Style="max-width: 90px; min-width: 90px; text-align: center; font-size: 12px; margin: auto" data-date-muestra data-posicion="<%#Item.Index%>" data-posicion-fecha autocomplete="off" OnChange="cambiaFechasOnChange(this.id);"></asp:TextBox>
                            </td>
                            <td runat="server" id="tdHoras" style="vertical-align: middle">
                                <div class="text-center">
                                    <asp:TextBox ID="txtHora" runat="server" Text='<%#:Item.Fecha != null ? ((DateTime)Item.Fecha).ToString("HH:mm") : "" %>' MaxLength="5" CssClass="form-control input-sm" Style="max-width: 65px; min-width: 60px; margin: auto" data-time-muestra data-posicion="<%#Item.Index%>" data-posicion-hora autocomplete="off"></asp:TextBox>
                                </div>
                            </td>
                            <asp:Repeater ID="rptBody" runat="server" ItemType="SIRALAB_DAL.MuestraValor">
                                <ItemTemplate>
                                    <td cssclass="text-center text-sm" id="muestraCaudal" runat="server" visible="<%# Item.requiereCaudal ?? false  %>" style="vertical-align: middle; text-align: right;">
                                        <asp:TextBox ID="txtCaudal" runat="server" Text='<%#:Item.caudal%>' CssClass="form-control input-sm" data-calc="caudal" data-scale="2" Style="max-width: 65px; min-width: 55px; text-align: center; margin: auto" autocomplete="off" data-posicion="<%#Item.MuestraID %>" data-posicion-caudal OnBlur="deshabilitarCeldas(this.id); establecerDecimalesOnBlur(this.id);"></asp:TextBox>
                                    </td>
                                    <td id="muestraParticipacion" runat="server" class="text-center text-sm" style="text-align: center; vertical-align: middle"><span data-caudal-result></span></td>
                                    <td id="muestraVolumen" runat="server" class="text-center text-sm" style="text-align: center; vertical-align: middle"><span data-volumen-result></span></td>
                                    <td id="muestraParametro" runat="server" data-parameter style="vertical-align: middle; text-align: center" class="tdParametro">
                                        <asp:TextBox ID="txtValue" runat="server" data-muestra-valor-id="<%#:Item.MuestraValorID%>" data-parameter-id="<%#:Item.ParametroID%>" data-scale="<%#:Item.EscalaDecimal ?? 4%>" data-operation="<%#Item.OperacionID %>" Visible="<%#Item.AceptaValor%>" Text='<%#:Item.Valor%>' CssClass="form-control input-sm entradaParametro capturaParametro" Style="max-width: 90px; min-width: 90px; text-align: center; margin: auto" autocomplete="off" OnBlur="establecerDecimalesOnBlur(this.id);"></asp:TextBox>
                                        <asp:DropDownList ID="ddlValue" runat="server" data-muestra-valor-id="<%#:Item.MuestraValorID%>" data-muestra-valor="<%#:Item.ValorValidacion.ToString().ToLower() %>" data-parameter-id="<%#:Item.ParametroID%>" Visible="<%#!Item.AceptaValor%>" CssClass="form-control input-sm capturaParametro">
                                            <Items>
                                                <asp:ListItem Text="Seleccione" Value="true">Ausente</asp:ListItem>
                                                <asp:ListItem Text="Seleccione" Value="false">Presente</asp:ListItem>
                                            </Items>
                                        </asp:DropDownList>
                                        <span runat="server" id="spanMensaje" class="text-danger" visible="<%# Item.AceptaValor && (Item.LimitaCaptura ?? false) && (Item.MinAceptado != null || Item.MaxAceptado != null) %>" hidden="hidden" style="font-size: 8px">Valor improcedente.</span>
                                    </td>
                                    <td id="lcmc" runat="server" visible="<%#Item.LCMC ?? false %>" style="text-align: center; vertical-align: middle" class="text-center">
                                        <asp:TextBox ID="txtLcmc" runat="server" data-muestra-valor-id="<%#:Item.MuestraValorID%>" data-parameter-id="<%#:Item.ParametroID%>" data-scale="<%#:Item.EscalaDecimalLCM ?? 4%>" data-lcmc Visible="<%#Item.AceptaValor%>" Text='<%#:Item.LC_MC%>' CssClass="form-control input-sm" Style="text-align: center; align-content: center" autocomplete="off" OnBlur="establecerDecimalesLCMOnBlur(this.id)"></asp:TextBox>
                                    </td>
                                    <%--Cambio el orden en el que se muestra la operación Valor Ponderado. Se mostraba antes del LMC--%>
                                    <td id="operation" runat="server" visible="<%#Item.OperacionID==3 %>" class="text-center text-sm" style="text-align: center; vertical-align: middle">
                                        <asp:Label ID="lblResultado" runat="server" data-parameter-id="<%#:Item.ParametroID%>" data-scale="<%#:Item.EscalaDecimal ?? 4%>" data-operation="<%#Item.OperacionID %>" data-result Visible="<%#Item.AceptaValor%>"></asp:Label>
                                    </td>
                                </ItemTemplate>
                            </asp:Repeater>
                            <td runat="server" id="tdObservaciones" style="vertical-align: middle">
                                <asp:TextBox ID="txtObservaciones" runat="server" Text='<%#:Item.Observaciones%>' CssClass="form-control input-sm" data-observacion MaxLength="50" data-posicion="<%#Item.MuestraID %>"></asp:TextBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div id="trsCompuesta" runat="server">
                            <tr>
                                <td style="text-align: right; vertical-align: middle; text-align: right" colspan="2">Volumen de muestra compuesta</td>
                                <td style="vertical-align: middle">
                                    <asp:TextBox ID="txtVolumen" runat="server" CssClass="form-control input-sm" data-scale="2" data-volumen-compuesta Style="max-width: 85px; min-width: 75px; text-align: center; font-size: 12px; margin: auto" autocomplete="off" OnBlur="establecerDecimalesOnBlur(this.id);"></asp:TextBox>
                                </td>
                                <asp:Repeater ID="rptHeaderCompuesta" runat="server" ItemType="SIRALAB_DAL.Extensiones.Columnas">
                                    <ItemTemplate>
                                        <th style="max-width: 85px; min-width: 75px; text-align: center"><%#:Item.Nombre%></th>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <th></th>
                            </tr>
                            <tr>
                                <th colspan="3" style="max-width: 85px; min-width: 75px; text-align: center; vertical-align: middle"><b>Resultados</b></th>
                                <asp:Repeater ID="rptCamposCompuesta" runat="server" ItemType="SIRALAB_DAL.Extensiones.Columnas">
                                    <ItemTemplate>
                                        <td id="muestraCaudalTotal" runat="server" style="text-align: center;" visible="<%#Item.RequiereCaudal ?? false %>"><span data-caudal-total data-scale="2"></span></td>
                                        <td id="muestraParticipacionTotal" runat="server" style="text-align: center;" visible="<%#Item.RequiereParticipacion ?? false %>"><span data-caudal-result-total data-scale="2"></span></td>
                                        <td id="muestraVolumenTotal" runat="server" style="text-align: center;" visible="<%#Item.RequiereVolumen ?? false %>"><span data-volumen-result-total data-scale="2"></span></td>
                                        <td id="muestraParametroTotal" runat="server" style="text-align: center;" visible="<%#Item.RequiereParametro ?? false %>">
                                            <span data-parameter-id="<%#:Item.ParametroID%>" data-scale="6" data-operation="<%#Item.OperacionID %>" data-truncamiento="<%# Item.Truncamiento ?? false %>" data-result-total></span>
                                        </td>
                                        <td id="muestraTotalNoRequerido" runat="server" visible="<%#Item.muestraTotalNoRequerido ?? false  %>"></td>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <td></td>
                            </tr>
                        </div>


                        <div id="divLMP" runat="server">
                            <tr>
                                <th style="text-align: right; vertical-align: middle; text-align: right" colspan="2">Límite Máximo Permisible</th>
                                <td></td>
                                <asp:Repeater runat="server" ID="rptLimiteMaximoPermisible" ItemType="SIRALAB_DAL.Extensiones.Columnas">
                                    <ItemTemplate>
                                        <td style="text-align: center; vertical-align: middle">
                                            <span runat="server" visible="<%# Item.requiereLimiteInferior == true %>">Inferior</span>
                                            <input runat="server" id="limiteInferior" class="form-control input-sm" visible="<%# Item.ParametroID != null && Item.requiereLimiteInferior != null %>" value="<%# Item.limiteInferior ?? 0 %>" data-lmp-parametroid="<%# Item.ParametroID ?? 0 %>" data-lmp-inferior data-scale="<%#:Item.EscalaDecimal ?? 4%>" style="max-width: 90px; min-width: 90px; text-align: center; font-size: 12px; margin: auto" onblur="establecerDecimalesOnBlur(this.id);" />
                                            <span runat="server" visible="<%# Item.requiereLimiteInferior == true %>">Superior</span>
                                            <input runat="server" id="limiteSuperior" class="form-control input-sm" visible="<%# Item.ParametroID != null && Item.capturaLMP == true %>" value="<%# Item.limiteSuperior ?? 0 %>" data-lmp-parametroid="<%# Item.ParametroID ?? 0 %>" data-lmp-superior data-scale="<%#:Item.EscalaDecimal ?? 4%>" style="max-width: 90px; min-width: 90px; text-align: center; font-size: 12px; margin: auto" onblur="establecerDecimalesOnBlur(this.id);" />
                                            <span runat="server" visible="<%# !Item.capturaLMP ?? false %>">Ausente</span>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <td></td>
                            </tr>
                        </div>
                        <div id="trsSelects" runat="server">
                            <asp:Repeater ID="rptLaboratorio" runat="server" ItemType="SIRALAB_DAL.Extensiones.Columnas" OnItemDataBound="rptLaboratorio_OnItemDataBound">
                                <HeaderTemplate>
                                    <tr class="ocultaCamposSDT">
                                        <td colspan="3" class="anchoSelects" style="text-align: right; vertical-align: middle">Laboratorio</td>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td id="trLaboratorio" runat="server" visible="<%#Item.ParametroID != null %>" style="max-width: 90px">
                                        <asp:DropDownList ID="ddlLaboratorio" runat="server" data-parameter-id="<%#Item.ParametroID %>" data-laboratorio-id="<%#Item.LaboratorioID %>" CssClass="form-control input-sm" AppendDataBoundItems="true">
                                            <Items>
                                                <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                            </Items>
                                        </asp:DropDownList>
                                    </td>
                                    <td id="operation" runat="server" visible="<%#Item.OperacionID==3 %>"></td>
                                    <td id="lcmc" runat="server" visible="<%#Item.RequiereLC_MC %>"></td>
                                    <td runat="server" visible="<%#Item.RequiereSelect != null %>"></td>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <td runat="server" id="tdLaboratorios"></td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>

                            <asp:Repeater ID="rptMetodos" runat="server" ItemType="SIRALAB_DAL.Extensiones.Columnas" OnItemDataBound="rptMetodos_OnItemDataBound">
                                <HeaderTemplate>
                                    <tr class="ocultaCamposSDT">
                                        <td colspan="3" class="anchoSelects" style="text-align: right; vertical-align: middle;">Método</td>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td id="trMetodo" runat="server" visible="<%#Item.ParametroID != null %>" style="max-width: 90px">
                                        <asp:DropDownList ID="ddlMetodo" runat="server" data-parameter-id="<%#Item.ParametroID %>" data-laboratorio-id="<%#Item.LaboratorioID %>" data-metodo-id="<%#Item.MetodoID %>" CssClass="form-control input-sm" AppendDataBoundItems="true">
                                            <Items>
                                                <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                            </Items>
                                        </asp:DropDownList>
                                    </td>
                                    <td id="operation" runat="server" visible="<%#Item.OperacionID==3 %>"></td>
                                    <td id="lcmc" runat="server" visible="<%#Item.RequiereLC_MC %>"></td>
                                    <td runat="server" visible="<%#Item.RequiereSelect != null%>"></td>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <td runat="server" id="tdMetodos"></td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>

                            <asp:Repeater ID="rptAccion" runat="server" ItemType="SIRALAB_DAL.Extensiones.Columnas" OnItemDataBound="rptAccion_OnItemDataBound">
                                <HeaderTemplate>
                                    <tr>
                                        <td colspan="3" class="anchoSelects" style="text-align: right; vertical-align: middle">Acción</td>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <td id="trAccion" runat="server" visible="<%#Item.ParametroID != null %>" style="max-width: 90px">
                                        <asp:DropDownList ID="ddlAccion" runat="server" data-parameter-id="<%#Item.ParametroID %>" data-accion="<%#Item.Accion %>" CssClass="form-control input-sm" AppendDataBoundItems="true">
                                            <Items>
                                                <asp:ListItem Text="Seleccione" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Exento" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No analizado" Value="2"></asp:ListItem>
                                            </Items>
                                        </asp:DropDownList>
                                    </td>
                                    <td id="operation" runat="server" visible="<%#Item.OperacionID==3 %>"></td>
                                    <td id="lcmc" runat="server" visible="<%#Item.RequiereLC_MC %>"></td>
                                    <td runat="server" visible="<%#Item.RequiereSelect != null %>"></td>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <td runat="server" id="tdAcciones"></td>
                                    </tr>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                        <div id="divDatos" runat="server">
                            <tr>
                            </tr>
                            <tr>
                                <td id="tdConformacionMuestraCompuesta"  colspan="10"></td>
                                <td style="vertical-align: middle; text-align: right">Fecha de conformación de la Muestra
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaConformacionMuestraCompuesta" runat="server" CssClass="form-control input-sm" data-fecha-conformacion Style="max-width: 95px; min-width: 95px; text-align: center; font-size: 12px; margin: auto" autocomplete="off"></asp:TextBox>
                                </td>
                                <td style="vertical-align: middle; text-align: right">Hora de conformación de la Muestra
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHoraConformacionMuestraCompuesta" runat="server" CssClass="form-control input-sm" data-hora-conformacion Style="max-width: 95px; min-width: 95px; text-align: center; font-size: 12px; margin: auto" autocomplete="off"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td id="mnsValidaHojaCampo" style="vertical-align: middle; text-align: left; font-size: 15px" colspan="10">
                                    <asp:CheckBox runat="server" ID="chkValidaHoja" Text="Los valores de la hoja de campo y bitácora concuerdan con los de esta hoja de electrónica." CssClass="text-danger" Visible="False" /></td>
                                <td style="vertical-align: middle; text-align: right">Fecha de Ingreso al laboratorio
                                </td>
                                <td style="vertical-align: middle; text-align: right">
                                    <asp:TextBox ID="txtFechaConformacion" runat="server" CssClass="form-control input-sm" data-fecha-compuesta Style="max-width: 95px; min-width: 95px; text-align: center; font-size: 12px; margin: auto" autocomplete="off"></asp:TextBox>
                                </td>
                                <td style="vertical-align: middle; text-align: right">Hora de Ingreso al laboratorio
                                </td>
                                <td style="vertical-align: middle; text-align: right">
                                    <asp:TextBox ID="txtHoraConformacion" runat="server" CssClass="form-control input-sm" data-hora-compuesta Style="max-width: 90px; min-width: 90px; text-align: center; font-size: 12px; margin: auto" autocomplete="off"></asp:TextBox>
                                </td>
                            </tr>
                        </div>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="col-md-12 text-right">
        </div>
        <br />
    </div>
</div>
