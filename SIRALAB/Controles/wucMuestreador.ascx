<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMuestreador.ascx.cs" Inherits="SIRALAB.Controles.wucMuestreador" %>


<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />
<%--<script src='<%= Page.ResolveUrl("~/js/_tablasAnexo.js") %>'></script>--%>
<style type="text/css">
    .fila {
        padding: 0;
        margin: 0 auto;
        width: 97%;
        list-style: none;
        font-family: 'Segoe UI';
    }

        .fila li {
            padding: 0;
            margin: 0;
        }

        .fila table {
            padding: 0;
            margin: 0;
            border-collapse: collapse;
            width: 100%;
        }

    .rbl input[type="radio"] {
        margin-left: 10px;
        margin-right: 1px;
    }
</style>
<div class="container" id="pnlConsultaLaboratorio" runat="server" visible="false">
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <label class="col-sm-4 control-label" for="email-03">El laboratorio realiza el muestreo*</label>
                <div class="col-sm-6">

                    <asp:RadioButtonList ID="rblTipoLaboratorio" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblTipoLaboratorio_SelectedIndexChanged" data-tipo-laboratorio-signatario AppendDataBoundItems="True" CssClass="rbl">
                        <asp:ListItem Value="1">Si</asp:ListItem>
                        <asp:ListItem Value="2">No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col-sm-4">
                    <asp:LinkButton ID="lnkbtnLimpiar" runat="server" ToolTip="Limpiar resultados" OnClick="lnkbtnLimpiar_Click" CssClass="btn btn-default" Style="display: none;"><span class="glyphicon glyphicon-fire"></span></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Si no es el mismo laboratorio el que muestrea, selecciona el laboratorio que sí lo hace -->
    <div id="pnlLab" runat="server" class="row">
        <div class="col-md-12">
            <div class="form-group">
                <label class="col-sm-4 control-label" for="email-03">Nombre del laboratorio*</label>
                <div class="col-sm-6">
                    <asp:DropDownList ID="ddlNombreLab" runat="server" CssClass="form-control" Width="97%" OnSelectedIndexChanged="ddlNombreLab_SelectedIndexChanged" AutoPostBack="true" data-laboratorio-signatario AppendDataBoundItems="True"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <!-- Selección del signatario que realizó la toma de la muestra -->
    <div class="row" id="pnlSignatario" runat="server" enabled="false" visible="false">
        <div class="col-md-12">
            <div class="form-group">
                <label class="col-sm-4 control-label" for="email-03">Nombre del signatario*</label>
                <div class="col-sm-6">
                    <asp:DropDownList ID="ddlMuestreador" runat="server" CssClass="form-control" Width="97%" OnSelectedIndexChanged="ddlMuestreador_SelectedIndexChanged" AutoPostBack="true" data-signatario AppendDataBoundItems="True"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
</div>

<div style="display: none;">
    <asp:HiddenField ID="hfLaboratorioAnalisisID" runat="server" />
    <asp:HiddenField ID="hfValor" runat="server" />
    <asp:HiddenField ID="hfValorSignatario" runat="server" />
</div>
