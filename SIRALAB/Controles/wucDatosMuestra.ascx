<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucDatosMuestra.ascx.cs" Inherits="SIRALAB.Controles.wucDatosMuestra" %>


<style>
    /*.div-colapsado
    {
        display: none;
    }*/

    a.tooltip {
        outline: none;
    }

        a.tooltip strong {
            line-height: 30px;
        }

        a.tooltip:hover {
            text-decoration: none;
        }

        a.tooltip span {
            z-index: 10;
            display: none;
            padding: 14px 20px;
            margin-top: -30px;
            margin-left: 28px;
            width: 300px;
            line-height: 16px;
        }

        a.tooltip:hover span {
            display: inline;
            position: absolute;
            color: #000;
            border: 1px solid #B38E5D;
            background-color: #D4C19C;
        }

    .callout {
        z-index: 20;
        position: absolute;
        top: 30px;
        border: 0;
        left: -12px;
    }


    a.tooltip span {
        border-radius: 4px;
        box-shadow: 5px 5px 8px #CCC;
    }
</style>
<style type="text/css">
    .formulario_muestra {
        width: 97%;
    }

        .formulario_muestra select,
        .formulario_muestra input[type='text'] {
            display: inline-block;
            margin: 0 auto;
        }
</style>
<script type="text/javascript">
    function t(d) {
        if (d == 1) {
            $('.div-colapsado').slideToggle(600);
        };
    }
</script>

<asp:Panel ID="pnlPtoDescarga" runat="server">
    <asp:UpdatePanel ID="updpnlPtoDescarga" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlPtoEstado" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="ddlPtoMunicipio" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="chPtoDescarga" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtnClienteConsulta" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div class="container">
                <div class="row">
                    <div class="col-md-2">
                        <asp:LinkButton ID="lnkbtnClienteConsulta" Visible="true" runat="server" OnClientClick="t(1); return false;" CssClass="btn btn-default btn-sm" ToolTip="Ver información completa">
                          <span class="glyphicon glyphicon-chevron-down"></span>
                        </asp:LinkButton>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-6">
                        <asp:CheckBox ID="chPtoDescarga" runat="server" AutoPostBack="true" Text="&quot;En el supuesto que el punto de muestreo esté cercano al punto de la descarga&quot;, ¿Desea precargar los datos del punto de la descarga para facilitarle el llenado del punto de muestreo?" CssClass="checkbox" OnCheckedChanged="chPtoDescarga_CheckedChanged" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-5">
                        <div class="form-group">
                            <label for="name">Calle* <span class="form-text"></span>:</label>
                            <asp:TextBox ID="txtPtoCalle" runat="server" CssClass="form-control" MaxLength="150" placeholder="Ingresa tu calle"></asp:TextBox>
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoCalle" ErrorMessage="Campo requerido" Display="Dynamic"
                                         CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <label for="name">No. exterior* <span class="form-text"></span>:</label>
                            <asp:TextBox ID="txtPtoNExterior" runat="server" CssClass="form-control" MaxLength="20" placeholder="Ingresa tu No. exterior"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoNExterior" ErrorMessage="Campo requerido" Display="Dynamic"
                                         CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <label for="name">No. interior<span class="form-text"></span>:</label>
                            <asp:TextBox ID="txtPtoNInterior" runat="server" CssClass="form-control" MaxLength="20" placeholder="Ingresa tu No. Interior"></asp:TextBox>
                          
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <label for="name">C.P.*<span class="form-text"></span>:</label>
                            <asp:TextBox ID="txtPtoCP" runat="server" CssClass="form-control" placeholder="Ingresa tu C.P"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoCP" ErrorMessage="Campo requerido" Display="Dynamic"
                                         CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div class="div-colapsado">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="row">
                        
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Estado<span class="form-text">*</span>:</label>
                                    <asp:DropDownList ID="ddlPtoEstado" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPtoEstado_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlPtoEstado" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlPtoEstado" ErrorMessage="Campo requerido" Display="Dynamic"
                                         CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>
                                  

                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Municipio<span class="form-text">*</span>:</label>
                                    <asp:DropDownList ID="ddlPtoMunicipio" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlPtoMunicipio_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlPtoMunicipio" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlPtoMunicipio" 
                                        ErrorMessage="Campo requerido" Display="Dynamic"
                                           CssClass="text-danger" Text="*Campo requerido"></asp:RequiredFieldValidator>
                                  
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Localidad<span class="form-text">*</span>:</label>
                                    <asp:DropDownList ID="ddlPtoLocalidad" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlPtoLocalidad" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlPtoLocalidad" 
                                     ErrorMessage="Campo requerido" Display="Dynamic"
                                        CssClass="text-danger"  Text="*Campo requerido"></asp:RequiredFieldValidator>
                                   
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="name">Colonia<span class="form-text">*</span>:</label>
                                    <asp:DropDownList ID="ddlPtoColonia" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfv_ddlPtoColonia" runat="server" ValidationGroup="punto" InitialValue="0" ControlToValidate="ddlPtoColonia" 
                                        ErrorMessage="Campo requerido" Display="Dynamic"
                                       CssClass="text-danger"   Text="*Campo requerido" ></asp:RequiredFieldValidator>
                                   
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="name">Datum*  <strong data-placement="left" data-toggle="tooltip" title="" data-original-title="Un Datum es un conjunto de puntos de referencia en la superficie terrestre con los cuales es posible georreferenciar un punto en la tierra y que de alguna forma es utilizado al momento de establecer las coordenadas de un punto de descarga o aprovechamiento. El Datum que aplican para América del Norte es el WGS84, el NAD27 y NAD83. Los GPS y los celulares por default utilizan el WGS84.
                              ">(?)</strong>
                                  <span class="form-text"></span>:</label>
                                    <asp:TextBox ID="txtPtoDatum" runat="server" CssClass="control-campo" Visible="False"></asp:TextBox>
                                    <asp:DropDownList ID="DDDatum" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="0">Selecciona</asp:ListItem>
                                        <asp:ListItem Value="1">WGS84</asp:ListItem>
                                        <asp:ListItem Value="2">NAD27</asp:ListItem>
                                        <asp:ListItem Value="3">NAD83</asp:ListItem>

                                    </asp:DropDownList>
                                    <asp:RadioButtonList ID="rblPtoEfluente" runat="server" RepeatDirection="Horizontal" Enabled="false" Visible="False">
                                        <asp:ListItem Value="true" Selected="True" Text="SI"></asp:ListItem>
                                        <asp:ListItem Value="false" Text="NO"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h6>Latitud</h6>
                                    </div>
                                    <div class="panel-body">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Grados<span class="form-text">*</span>:</label>
                                                <asp:TextBox ID="txtPtoLatitudGrados" runat="server" CssClass="form-control" placeholder="Ingrese grados"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfv_txtPtoLatitudGrados" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLatitudGrados" 
                                                   ErrorMessage="Campo requerido" Display="Dynamic"
                                      CssClass="text-danger"    Text="*Campo requerido"  ></asp:RequiredFieldValidator>
                                              

                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Minutos<span class="form-text">*</span>:</label>
                                                <asp:TextBox ID="txtPtoLatitudMinutos" runat="server" CssClass="form-control" placeholder="Ingrese minutos"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfv_txtPtoLatitudMinutos" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLatitudMinutos" 
                                                    ErrorMessage="Campo requerido" Display="Dynamic"
                                      CssClass="text-danger"    Text="*Campo requerido" ></asp:RequiredFieldValidator>
                                               
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Segundos<span class="form-text">*</span>:</label>
                                                <asp:TextBox ID="txtPtoLatitudSeg" runat="server" CssClass="form-control" placeholder="Ingrese segundos"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfv_txtPtoLatitudSeg" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLatitudSeg" 
                                                   ErrorMessage="Campo requerido" Display="Dynamic"
                                     CssClass="text-danger"     Text="*Campo requerido" ></asp:RequiredFieldValidator>
                                                
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <br />
                                                <asp:DropDownList ID="ddlLatitud" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="N" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="S" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h6>Longitud</h6>
                                    </div>
                                    <div class="panel-body">
                         
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Grados<span class="form-text">*</span>:</label>
                                                <asp:TextBox ID="txtPtoLongitudGrados" runat="server" CssClass="form-control" placeholder="Ingrese grados"></asp:TextBox>
                                                  <asp:RequiredFieldValidator ID="rfv_txtPtoLongitudGrados" runat="server" ControlToValidate="txtPtoLongitudGrados" ErrorMessage="Campo requerido" Display="Dynamic"
                                                     ValidationGroup="punto"  CssClass="text-danger" SetFocusOnError="true"   Text="*Campo Requerido" ></asp:RequiredFieldValidator>
                                               
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Minutos<span class="form-text">*</span>:</label>
                                                <asp:TextBox ID="txtPtoLongitudMinutos" runat="server" CssClass="form-control" placeholder="Ingrese minutos"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfv_txtPtoLongitudMinutos" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLongitudMinutos" 
                                                    ErrorMessage="Campo requerido" Display="Dynamic"
                                     CssClass="text-danger"     Text="*Campo requerido" ></asp:RequiredFieldValidator>
                                                
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label for="name">Segundos<span class="form-text">*</span>:</label>
                                                <asp:TextBox ID="txtPtoLongitudSeg" runat="server" CssClass="form-control" placeholder="Ingrese segundos"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfv_txtPtoLongitudSeg" runat="server" ValidationGroup="punto" ControlToValidate="txtPtoLongitudSeg" 
                                                   CssClass="text-danger"   ErrorMessage="Campo requerido" Display="Dynamic"  Text="*Campo requerido"  ></asp:RequiredFieldValidator>
                                               
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <br />
                                                <asp:DropDownList ID="ddlLongitud" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="E" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="O" Value="1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                 <div class="form-group">
                                    <label for="name">Observaciones<span class="form-text"></span>:</label>
                                      <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" MaxLength="150" ValidationGroup="punto"></asp:TextBox>
                                        
                                 </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4 col-md-offset-10">
                                 <asp:LinkButton ID="lnkbtnPtoGuardar" runat="server" OnClick="lnkbtnPtoGuardar_Click" CssClass="btn btn-default" ValidationGroup="punto" Text="Guardar"></asp:LinkButton>
                            </div>
                        </div>
                            
                    </div>
                </div>
                 
              
            </div>

            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfPtoId" runat="server" />
<asp:HiddenField ID="hfPtoCliente" runat="server" />
<asp:HiddenField ID="hfPtoDireccion" runat="server" />
<asp:HiddenField ID="hfPtoMuestraID" runat="server" />
<asp:HiddenField ID="hfPtoIdOpcionJS" runat="server" />
