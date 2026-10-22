<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucNorma.ascx.cs" Inherits="SIRALAB.Controles.wucNorma" %>


<asp:UpdatePanel ID="upNorma" runat="server" class="norma-captura" ChildrenAsTriggers="false" UpdateMode="Conditional">
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lnkbtnCancelar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="lnkbtnGuardar" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="chkCaudal" EventName="CheckedChanged" />
        <asp:AsyncPostBackTrigger ControlID="chkMuestraCompuesta" EventName="CheckedChanged" />
    </Triggers>
    <ContentTemplate>
        <div class="row">
            <div class="col-md-9">
                <label for="name">Nombre<span class="control-label"></span>*:</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" Style="width: 97%;" ValidationGroup="NormaGroup" MaxLength="50" autocomplete="cc-csc" />
                <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ErrorMessage="Campo es requerido." ControlToValidate="txtNombre" ValidationGroup="NormaGroup" Display="None" />
                <asp:ValidatorCalloutExtender ID="vceNombre" runat="server" TargetControlID="rfvNombre" PopupPosition="BottomRight" />
            </div>
            <div class="col-md-3">
                <label for="name">Fecha Inicio<span class="control-label"></span>*:</label>
                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="form-control" ValidationGroup="NormaGroup" MaxLength="10" autocomplete="cc-csc" />
                <asp:CalendarExtender ID="ceFechaInicio" runat="server" TargetControlID="txtFechaInicio" Format="dd/MM/yyyy" PopupPosition="BottomRight"></asp:CalendarExtender>
                <asp:RequiredFieldValidator ID="rfvFechaInicio" runat="server" ErrorMessage="Este campo es requerido" ControlToValidate="txtFechaInicio" ValidationGroup="NormaGroup" Display="None" />
                <asp:ValidatorCalloutExtender ID="vceFechaInicio" runat="server" TargetControlID="rfvFechaInicio" PopupPosition="BottomLeft" Width="150px" />
                <asp:RegularExpressionValidator ID="revFechaInicio" runat="server" ErrorMessage="Debe tener el formato dd/mm/aaaa" ValidationGroup="NormaGroup" ControlToValidate="txtFechaInicio" ValidationExpression="(((0[1-9]|[12][0-9]|3[01])([-./])(0[13578]|10|12)([-./])(\d{4}))|(([0][1-9]|[12][0-9]|30)([-./])(0[469]|11)([-./])(\d{4}))|((0[1-9]|1[0-9]|2[0-8])([-./])(02)([-./])(\d{4}))|((29)(\.|-|\/)(02)([-./])([02468][048]00))|((29)([-./])(02)([-./])( [13579][26]00 ))|((29)([-./])(02)([-./])([0-9][0-9][0][48]))|((29)([-./])(02)([-./])([0-9][0-9][2468][048]))|((29)([-./])(02)([-./])([0-9][0-9][13579][26])))" Display="None" />
                <asp:ValidatorCalloutExtender ID="vcerevFechaInicio" runat="server" TargetControlID="revFechaInicio" PopupPosition="BottomLeft" Width="150px" />
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <label for="name">Descripción<span class="control-label"></span>:</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" ValidationGroup="NormaGroup" MaxLength="150" autocomplete="cc-csc"/>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <h3>Opciones</h3>
            </div>
            <div class="col-lg-12">
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chk2021" runat="server" /><b>2021</b>
                    </label>
                </div>
            </div>
            <div class="col-md-4">
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkCaudal" runat="server" AutoPostBack="true" OnCheckedChanged="chkCaudal_CheckedChanged" />Requiere caudal.
                    </label>
                </div>
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkHorario" runat="server" />Requiere captura de horario
                    </label>
                </div>
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkObservaciones" runat="server" />Requiere captura de observaciones
                    </label>
                </div>
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkMuestraCompuesta" runat="server" Visible="false" AutoPostBack="true" OnCheckedChanged="chkMuestraCompuesta_CheckedChanged" />Requiere muestra compuesta.
                    </label>
                </div>

                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkPromedioMensual" runat="server" Visible="false" />Requiere promedio mensual.
                    </label>
                </div>
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkAnalisisPrevio" runat="server" AutoPostBack="True" OnCheckedChanged="chkAnalisisPrevio_CheckedChanged" Enabled="False" />Requiere de análisis previo.
                    </label>
                </div>
                
                <div class="form-group">
                    <asp:Label runat="server" ID="lblNormaPredecesora" Text="Norma Predecesora" CssClass="control-label"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlNormaPredecesora" CssClass="form-control" Enabled="False" command-ddl />
                </div>
            </div>

            <div class="col-md-6">

                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkCPD" runat="server" />Es una <b>condición particular de descarga</b>.
                    </label>
                </div>
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkFuenteAbastecimiento" runat="server" />Es una <b>fuente de abastecimiento</b>.
                    </label>
                </div>
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkExentar" runat="server" />Puede exentar parámetros.
                    </label>
                </div>
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkMuestraSimples" runat="server"  />Captura muestras simples en <b>hoja de campo</b>.
                    </label>
                </div>

               <%-- <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkRequiereValorInstantaneo" runat="server"  />Requiere valor instantaneo.
                    </label>
                </div>--%>
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkRequiereUsoDeAgua" runat="server" />Requiere uso.
                    </label>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" ID="lblDesplazamiento" Text="Desplazamiento" CssClass="control-label"></asp:Label>
                    <asp:TextBox ID="txtDesplazamiento" runat="server" CssClass="form-control" placeholder="Desplazamiento" autocomplete="cc-csc"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revDesplazamiento" runat="server" ControlToValidate="txtDesplazamiento" ErrorMessage="Sólo se permiten números enteros" ValidationExpression="\d" ValidationGroup="NormaGroup"></asp:RegularExpressionValidator>
                </div>
                
            </div>
            <div class="col-md-2">
                
                ¿Valida la información?
                <asp:RadioButtonList ID="rblValida" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Text="Si" Value="1" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                </asp:RadioButtonList>
                
                <div class="checkbox">
                    <label>
                        <asp:CheckBox ID="chkActivo" runat="server" Checked="true" Text="Activo" />
                    </label>
                </div>

            </div>
        </div>
        <div class="button-row">
            <asp:LinkButton ID="lnkbtnCancelar" runat="server" CssClass="btn btn-danger" ToolTip="Cancelar" Text="Cancelar" OnClick="lnkbtnCancelar_Click"></asp:LinkButton>
            <asp:LinkButton ID="lnkbtnGuardar" runat="server" CssClass="btn btn-primary" ToolTip="Guardar" Text="Guardar" OnClick="lnkbtnGuardar_Click" ValidationGroup="NormaGroup"></asp:LinkButton>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
