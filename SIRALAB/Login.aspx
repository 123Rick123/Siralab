<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SIRALAB.Login" %>
<%@ Register Src="~/Controles/wucSucursalLogin.ascx" TagPrefix="uc1" TagName="wucSucursalLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://framework-gb.cdn.gob.mx/assets/styles/main.css"></script>
    <style>
        .table-sm {
            font-size: 10pt;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
        <ContentTemplate>
            
            <div class="container">
                <div class="row">
                    <br />
                    <div class="col-sm-3">
                        <img src="image/master/logo_big.png" class="img-responsive" />
                    </div>
                    <div class="col-sm-6"></div>
                    <div class="col-sm-3">
                        <img src="image/master/siralab-logo.png" class="img-responsive" />
                    </div>
                </div>
                
                <div class="row">
                    <div class="col-sm-6 col-md-offset-3">
                        <div class="panel panel-default">
                            <div class="panel-body">
                                <div id="PanelLogin">
                                    <div class="form-group">
                                        <label class="control-label">Usuario *</label>
                                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control" placeholder="Nombre de usuario"></asp:TextBox>
                                        <span class="form-text form-text-error"></span><br />

                                        <label class="control-label">Contraseña *</label>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Contraseña"></asp:TextBox>
                                        <span class="form-text form-text-error"></span>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-8 col-sm-12">
                                            <a href="javascript:Action()" class="login-forgot">
                                                <small>Olvide mi contraseña</small>
                                            </a><br/>
                                            <small>* Campos obligatorios</small>
                                        </div>
                                        
                                        <div class="col-md-4 col-sm-12 text-right">
                                            <asp:LinkButton ID="lnkbAceptar" runat="server" Text="Aceptar" CssClass="btn btn-primary" ValidationGroup="login_grp" OnClick="lnkbAceptar_Click"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            
                                <div id="PanelContrasenia" style="display: none">
                                    <div class="row">
                                        <div class="col-sm-12">
                                            <div class="form-group ">
                                                <label class="control-label">Correo *</label>
                                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Ingresa tu correo"></asp:TextBox>
                                                <span class="form-text form-text-error"></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-sm-5">
                                            <small>* Campos obligatorios</small>
                                        </div>
                                        <div class="col-sm-7 text-right">
                                            <asp:LinkButton ID="lnkbEnviar" runat="server" Text="Enviar" CssClass="btn btn-primary" ValidationGroup="mail_grp" OnClick="lnkbEnviar_Click"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkbCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" OnClientClick="Action(); return false;"></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <uc1:wucSucursalLogin runat="server" ID="wucSucursalLogin" OnSeleccionar="wucSucursalLogin_Seleccionar" />
  

    <script src="https://framework-gb.cdn.gob.mx/gobmx.js"></script>
    <script src="https://framework-gb.cdn.gob.mx/assets/scripts/jquery-ui-datepicker.js"></script>
    <script src="js/siralab-login.js" type="text/javascript"></script>
    <script src="js/ValidarLogin.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $.ajax({
                type: "POST",
                url: "closeSessionHandler.ashx",
                contentType: "application/json; charset=utf-8",
                data: "{'close': '1'}",
                dataType: "json",
                success: function () {

                },
                error: function () {

                }
            });
        });

        function Action() {
            passRecovery('#<%= txtCorreo.ClientID %>', '#<%= txtUsuario.ClientID %>');
        }

    </script>
</asp:Content>

