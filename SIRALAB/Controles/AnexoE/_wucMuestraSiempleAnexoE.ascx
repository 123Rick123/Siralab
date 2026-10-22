<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="_wucMuestraSiempleAnexoE.ascx.cs" Inherits="SIRALAB.Controles.AnexoE._wucMuestraSiempleAnexoE" %>
<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<link href="style/botones.css" rel="stylesheet" type="text/css" />
<link href="style/iconos.css" rel="stylesheet" type="text/css" />
<%--<script src='<%= Page.ResolveUrl("~/js/_tablasAnexo.js") %>'></script>--%>
<style type="text/css">
    .table-list > tbody > tr:nth-child(even) > td {
        background-color: #F2F2F2;
    }

    .auto-style1 {
        height: 34px;
    }

    .auto-style2 {
        text-align: center;
    }

    .auto-style3 {
        font-weight: normal;
        text-align: center;
        height: 26px;
    }
    .auto-style4 {
        height: 26px;
    }
</style>
<asp:Panel ID="Panel1" runat="server">

    <div id="div_wucAnexoE" runat="server"></div>



    <table style="width: 1024px;" class="table-list" id="anexE">
        <caption>Muestras</caption>
        <tr class="TEXTO" style="color: #4A966A">
            <td style="width: 90px; color: #4A966A;">Muestra simple</td>
            <td style="width: 90px; color: #4A966A;">Fecha de muestreo</td>
            <td style="width: 90px; color: #4A966A;">Hora de muestreo</td>
            <td style="width: 90px; color: #4A966A;">Caudal (l/s)</td>
            <td style="width: 90px; color: #4A966A;">pH(UpH)</td>
            <td style="width: 90px; color: #4A966A;">Temperatura (°C)</td>
            <td style="width: 90px; color: #4A966A;">Materia flotante</td>
            <td style="width: 90px; color: #4A966A;">Participación del caudal (Qi/Qt)</td>
            <td style="width: 90px; color: #4A966A;">Volumen de muestra simple</td>
            <td style="width: 90px; color: #4A966A;">Coliformes fecales</td>
            <td style="width: 35px; color: #4A966A;">Lc/Mc</td>
            <td style="width: 90px; color: #4A966A;">Grasas y aceites</td>
            <td style="width: 35px; color: #4A966A;">Lc/Mc</td>
            <td style="width: 90px; color: #4A966A;">Valor ponderado de grasas y aceites</td>
            <td style="width: 90px; color: #4A966A;">Observaciones</td>
        </tr>
        <tr id="M1" runat="server">
            <td style="width: 100px;" class="TEXTGRIS">1</td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox1" runat="server" class="control-campo" Width="60px" onkeyup="javascript:rep();" onchange="javascript:rep();"></asp:TextBox>
                <asp:CalendarExtender ID="ce_txtMuestraFechaMuestreo" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox1">
                </asp:CalendarExtender>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox2" runat="server" class="control-campo" Width="50px" onkeypress="return valH(event, this);"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CA1" runat="server" class="control-campo" onkeypress="return valN(event, this);" onblur="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="PH1" runat="server" class="control-campo" onkeypress="return valN(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="T1" runat="server" class="control-campo" onkeypress="return valNC(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:DropDownList ID="ddlana1" runat="server" class="control-campo" Width="70px">
                    <asp:ListItem Value="true">Ausente</asp:ListItem>
                    <asp:ListItem Value="false">Presente</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label1" runat="server" Text="" CssClass="label1"></asp:Label>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label2" runat="server" Text="" CssClass="label2"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CF1" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc1" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:repCol1();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="GA1" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc7" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:reptex1();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label3" runat="server" Text="" CssClass="label3"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox8" runat="server" class="control-campo" TextMode="MultiLine" Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr id="M2" runat="server">
            <td style="width: 100px;" class="TEXTGRIS">2</td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox12" runat="server" class="control-campo" Width="60px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox12">
                </asp:CalendarExtender>

            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox13" runat="server" class="control-campo" Width="50px" onkeypress="return valH(event, this);"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CA2" runat="server" class="control-campo" onkeypress="return valN(event, this);" onblur="javascript:sum();" Width="50px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="PH2" runat="server" class="control-campo" onkeypress="return valN(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="T2" runat="server" class="control-campo" onkeypress="return valNC(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:DropDownList ID="ddlana2" runat="server" class="control-campo" Width="70px">
                    <asp:ListItem Value="true">Ausente</asp:ListItem>
                    <asp:ListItem Value="false">Presente</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label10" runat="server" Text="" CssClass="label10"></asp:Label>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label11" runat="server" Text="" CssClass="label11"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CF2" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc2" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:repCol2();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="GA2" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc8" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:reptex2();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label12" runat="server" Text="" CssClass="label12"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox20" runat="server" class="control-campo" TextMode="MultiLine"  Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr id="M3" runat="server">
            <td style="width: 100px;" class="TEXTGRIS">3</td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox17" runat="server" class="control-campo" Width="60px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox17">
                </asp:CalendarExtender>

            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox21" runat="server" class="control-campo" Width="50px" onkeypress="return valH(event, this);"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CA3" runat="server" class="control-campo" onkeypress="return valN(event, this);" onblur="javascript:sum();" Width="50px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="PH3" runat="server" class="control-campo" onkeypress="return valN(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="T3" runat="server" class="control-campo" onkeypress="return valNC(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:DropDownList ID="ddlana3" runat="server" class="control-campo" Width="70px">
                    <asp:ListItem Value="true">Ausente</asp:ListItem>
                    <asp:ListItem Value="false">Presente</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label13" runat="server" Text="" CssClass="label13"></asp:Label>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label14" runat="server" Text="" CssClass="label14"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CF3" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc3" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:repCol3();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="GA3" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc9" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:reptex3();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label15" runat="server" Text="" CssClass="label15"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox42" runat="server" class="control-campo" TextMode="MultiLine"  Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr id="M4" runat="server">
            <td style="width: 100px;" class="TEXTGRIS">4</td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox25" runat="server" class="control-campo" Width="60px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox25">
                </asp:CalendarExtender>

            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox26" runat="server" class="control-campo" Width="50px" onkeypress="return valH(event, this);"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CA4" runat="server" class="control-campo" onkeypress="return valN(event, this);" onblur="javascript:sum();" Width="50px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="PH4" runat="server" class="control-campo" onkeypress="return valN(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="T4" runat="server" class="control-campo" onkeypress="return valNC(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:DropDownList ID="ddlana4" runat="server" class="control-campo" Width="70px">
                    <asp:ListItem Value="true">Ausente</asp:ListItem>
                    <asp:ListItem Value="false">Presente</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label16" runat="server" Text="" CssClass="label16"></asp:Label>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label17" runat="server" Text="" CssClass="label17"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CF4" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc4" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:repCol4();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="GA4" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="l10lcmc" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:reptex4();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label18" runat="server" Text="" CssClass="label18"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox45" runat="server" class="control-campo" TextMode="MultiLine" Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr id="M5" runat="server">
            <td class="TEXTGRIS" style="width: 100px;">5</td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox30" runat="server" class="control-campo" Width="60px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox30">
                </asp:CalendarExtender>

            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox31" runat="server" class="control-campo" Width="50px" onkeypress="return valH(event, this);"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CA5" runat="server" class="control-campo" onkeypress="return valN(event, this);" onblur="javascript:sum();" Width="50px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="PH5" runat="server" class="control-campo" onkeypress="return valN(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="T5" runat="server" class="control-campo" onkeypress="return valNC(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:DropDownList ID="ddlana5" runat="server" class="control-campo" Width="70px">
                    <asp:ListItem Value="true">Ausente</asp:ListItem>
                    <asp:ListItem Value="false">Presente</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label19" runat="server" Text="" CssClass="label19"></asp:Label>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label20" runat="server" Text="" CssClass="label20"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CF5" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc5" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:repCol5();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="GA5" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="l11lcmc" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:reptex5();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label21" runat="server" Text="" CssClass="label21"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox48" runat="server" class="control-campo" TextMode="MultiLine" Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr id="M6" runat="server">
            <td class="TEXTGRIS" style="width: 100px;">6</td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox35" runat="server" class="control-campo" Width="60px"></asp:TextBox>
                <asp:CalendarExtender ID="CalendarExtender5" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox35">
                </asp:CalendarExtender>

            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox36" runat="server" class="control-campo" Width="50px" onkeypress="return valH(event, this);"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CA6" runat="server" class="control-campo" onkeypress="return valN(event, this);" onblur="javascript:sum();" Width="50px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="PH6" runat="server" class="control-campo" onkeypress="return valN(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="T6" runat="server" class="control-campo" onkeypress="return valNC(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:DropDownList ID="ddlana6" runat="server" class="control-campo" Width="70px">
                    <asp:ListItem Value="true">Ausente</asp:ListItem>
                    <asp:ListItem Value="false">Presente</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label22" runat="server" Text="" CssClass="label22"></asp:Label>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label23" runat="server" Text="" CssClass="label23"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="CF6" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="lcmc6" runat="server" class="control-campo" onkeypress="return valNCF(this.value, this);" onkeyup="javascript:repCol6();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="GA6" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="l12lcmc" runat="server" class="control-campo" onkeypress="return valNGA(this.value, this);" onkeyup="javascript:reptex6();" Width="25px"></asp:TextBox>
            </td>
            <td style="width: 100px;" class="TEXTGRIS">
                <asp:Label ID="Label24" runat="server" Text="" CssClass="label24"></asp:Label>
            </td>
            <td style="width: 100px;">
                <asp:TextBox ID="TextBox51" runat="server" class="control-campo" TextMode="MultiLine" Width="95px"></asp:TextBox>
            </td>
        </tr>
        <tr class="TEXTO" style="color: #4A966A">
            <td colspan="4" style="color: #4A966A">Volumen de muestra compuesta (l)</td>

            <td class="auto-style4">
                <asp:TextBox ID="TextBox9" runat="server" class="control-campo" onkeypress="return valN(event, this);" onkeyup="javascript:sum();" Width="50px">0</asp:TextBox>
            </td>
            <td class="auto-style4"></td>
            <td class="auto-style4"></td>
            <td class="auto-style4"></td>
            <td class="auto-style4"></td>

            <td class="auto-style4"></td>
            <td class="auto-style4"></td>
            <td class="auto-style4"></td>
            <td class="auto-style4"></td>
            <td class="auto-style4" colspan="2" style="color: #4A966A;">Fecha de ingreso al laboratorio</td>
        </tr>
        <tr class="TEXTO" style="color: #4A966A">
            <td colspan="4" style="color: #4A966A;">Laboratorio</td>
            <td>
                <asp:DropDownList ID="ddlLab1" runat="server" AutoPostBack="True" class="control-campo" OnSelectedIndexChanged="ddlLab1_SelectedIndexChanged" Width="70px">
                </asp:DropDownList>
            </td>

            <td>
                <asp:DropDownList ID="ddlLab2" runat="server" AutoPostBack="True" class="control-campo" OnSelectedIndexChanged="ddlLab2_SelectedIndexChanged" Width="70px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlLab3" runat="server" AutoPostBack="True" class="control-campo" OnSelectedIndexChanged="ddlLab3_SelectedIndexChanged" Width="70px">
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlLab4" runat="server" AutoPostBack="True" class="control-campo" OnSelectedIndexChanged="ddlLab4_SelectedIndexChanged" Width="70px">
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlLab5" runat="server" AutoPostBack="True" class="control-campo" OnSelectedIndexChanged="ddlLab5_SelectedIndexChanged" Width="70px">
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
              <td colspan="2">
                  <asp:TextBox ID="TextBox10" runat="server" class="control-campo" Width="60px"></asp:TextBox>
                  <asp:CalendarExtender ID="CalendarExtender6" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox10">
                  </asp:CalendarExtender>

            </td>
        </tr>
        <tr class="TEXTO" style="color: #4A966A">
            <td colspan="4" style="color: #4A966A;">Método</td>
            <td>
                <asp:DropDownList ID="ddlMet1" runat="server" class="control-campo" Width="70px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlMet2" runat="server" class="control-campo" Width="70px">
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlMet3" runat="server" class="control-campo" Width="70px">
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlMet4" runat="server" class="control-campo" Width="70px">
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlMet5" runat="server" class="control-campo" Width="70px">
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td colspan="2" style="color: #4A966A;">Hora de ingreso al laboratorio</td>
        </tr>
        <tr class="TEXTO" style="color: #4A966A">
            <td colspan="4" class="auto-style2">Acción</td>
            <td>
                <asp:DropDownList ID="ddlAction1" runat="server" class="control-campo" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAction1_SelectedIndexChanged">
                  
                    <asp:ListItem Value="1">Exento</asp:ListItem>
                    <asp:ListItem Value="2">Parámetro no analizado </asp:ListItem>
                       <asp:ListItem Value="3">Ninguna </asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlAction2" runat="server" class="control-campo" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAction2_SelectedIndexChanged">
                   
                    <asp:ListItem Value="1">Exento</asp:ListItem>
                    <asp:ListItem Value="2">Parámetro no analizado </asp:ListItem>
                    <asp:ListItem Value="3">Ninguna </asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:DropDownList ID="ddlAction3" runat="server" class="control-campo" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAction3_SelectedIndexChanged">

                    <asp:ListItem Value="1">Exento</asp:ListItem>
                    <asp:ListItem Value="2">Parámetro no analizado </asp:ListItem>
                     <asp:ListItem Value="3">Ninguna </asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlAction4" runat="server" class="control-campo" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAction4_SelectedIndexChanged">
       
                    <asp:ListItem Value="1">Exento</asp:ListItem>
                    <asp:ListItem Value="2">Parámetro no analizado </asp:ListItem>
                     <asp:ListItem Value="3">Ninguna </asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:DropDownList ID="ddlAction5" runat="server" class="control-campo" Width="70px" AutoPostBack="True" OnSelectedIndexChanged="ddlAction5_SelectedIndexChanged">
                
                    <asp:ListItem Value="1">Exento</asp:ListItem>
                    <asp:ListItem Value="2">Parámetro no analizado </asp:ListItem>
                     <asp:ListItem Value="3">Ninguna </asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>&nbsp;</td>
              <td colspan="2">
                  <asp:TextBox ID="TextBox11" runat="server" class="control-campo" onkeypress="return valH(event, this);" Width="60px"></asp:TextBox>
            </td>
        </tr>
        <tr class="TEXTO" style="color: #4A966A">
            <td colspan="3" rowspan="3" style="color: #4A966A;">Resultados</td>
            <td style="color: #4A966A;">Promedio aritmético<br />
                (l/s)</td>
            <td rowspan="3">&nbsp;</td>
            <td style="color: #4A966A;">Promedio aritmético (°C)</td>

            <td></td>
            <td style="color: #4A966A;">Total (Qi/Qt)</td>
            <td style="color: #4A966A;">Total (MS)</td>
            <td style="color: #4A966A;">Media geométrica</td>
            <td>&nbsp;</td>
            <td></td>
            <td>&nbsp;</td>
            <td style="color: #4A966A;">PPFC</td>
            <td>&nbsp;</td>
        </tr>
        <tr class="TEXTGRIS">
            <td>
                <asp:Label ID="Label4" runat="server" Text="" CssClass="totalLS"></asp:Label>
                <br />
                <div id="PAcaudal" runat="server">
                </div>
            </td>
            <td>
                <asp:Label ID="Label5" runat="server" Text="" CssClass="totalC"></asp:Label>
            </td>

            <td></td>
            <td>
                <asp:Label ID="Label6" runat="server" Text="" CssClass="totalqq"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label7" runat="server" Text="" CssClass="totalMS"></asp:Label>

            </td>
            <td>
                <asp:Label ID="Label8" runat="server" Text="" CssClass="totalmg"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td></td>
            <td>&nbsp;</td>
            <td>
                <asp:Label ID="Label9" runat="server" Text="" CssClass="totalppfc"></asp:Label>
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr class="TEXTOROJO">
            <td class="auto-style1">
                <asp:Label ID="Label25" runat="server" Text="Label" CssClass="total" Visible="False"></asp:Label>
            </td>
            <td colspan="7" style="color: red;">
                <asp:CheckBox ID="CheckBox1" runat="server" Text="Los valores de la hoja y bitácora de campo concuerda con los de esta hoja electrónica."  OnCheckedChanged="CheckBox1_CheckedChanged" />
            </td>
            <td class="auto-style1" style="color: red">&nbsp;</td>
            <td class="auto-style1" colspan="2">

                <%--<input id="calc" type="button" value="Calcular" class="boton" onclick="sum();" />--%>
              <%--  <asp:Button ID="Gtemp" runat="server" Text="Guardado temporal" OnClick="Gtemp_Click" CssClass="boton" />
                &nbsp;
                --%>
    <%--            <asp:Button ID="Button1" runat="server" CssClass="boton" OnClick="Button1_Click" Text="Calcular" />--%>
                &nbsp;<asp:Button ID="Button2" runat="server" CssClass="boton" OnClick="Button2_Click" Text="Guardar" />
                <asp:Button ID="ex" runat="server" Text="Button" OnClick="ex_Click" Visible="False" />
            </td>
        </tr>
    </table>
    <div style="text-align: right; width: 90%; margin: 15px auto 0 auto;">
        &nbsp;&nbsp;
        <asp:LinkButton ID="lnkbtnMuestraCompuesta" runat="server" ToolTip="Generar muestra compuesta" CssClass="icono-compuesta" OnClick="lnkbtnMuestraCompuesta_Click"><div></div></asp:LinkButton>
    </div>
</asp:Panel>


<script type="text/javascript">
    
    function valH(e, field) {//ok grados centigrados
        //debugger;
        var regexp;
        var key = window.event ? window.event.keyCode : e.which;
        // backspace

        if (key == 8) return true;

        // 0-9 a partir del .decimal  

        if (field.value != "") {
            if ((field.value.indexOf(":")) > 0) {
                //si tiene un punto valida dos digitos en la parte decimal

                if (key > 47 && key < 58) {
                    if (field.value == "") return true;
                    //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

                    regexp = /[0-9]{2}$/;
                    return !(regexp.test(field.value));
                }
            }
        }
        // 0-9 

        if (key > 47 && key < 58) {
            if (field.value == "") return true;
            regexp = /[0-9]{2}/;
            return !(regexp.test(field.value));
        }
        // .

        if (key == 58) {
            if (field.value == "")
            return false;
            regexp = /^[0-9]+$/;
            return regexp.test(field.value);
        }
        // other key

        return /\d/.test(String.fromCharCode(key));
    }
    // function keyPress(e) {
    function keyPress(e) {
 
        var keynum = window.event ? window.event.keyCode : e.which;
        if ((keynum == 8) || (keynum == 46))
            return true;

        return /\d/.test(String.fromCharCode(keynum));

    }
    function valNGA(e, field) {//ok grados centigrados
        var regexp;
        var key = window.event ? window.event.keyCode : e.which;
        // backspace

        if (key == 8) return true;

        // 0-9 a partir del .decimal  

        if (field.value != "") {
            if ((field.value.indexOf(".")) > 0) {
                //si tiene un punto valida dos digitos en la parte decimal

                if (key > 47 && key < 58) {
                    if (field.value == "") return true;
                    //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

                    regexp = /[0-9]{4}$/;
                    return !(regexp.test(field.value));
                }
            }
        }
        // 0-9 

        if (key > 47 && key < 58) {
            if (field.value == "") return true;
            regexp = /[0-9]{10}/;
            return !(regexp.test(field.value));
        }
        // .

        if (key == 46) {
            if (field.value == "")
                return false;
            regexp = /^[0-9]+$/;
            return regexp.test(field.value);
        }
        // other key

        return /\d/.test(String.fromCharCode(key));
    }
    function valNCF(e, field) {//ok grados centigrados
        var regexp;
        var key = window.event ? window.event.keyCode : e.which;
        // backspace

        if (key == 8) return true;

        // 0-9 a partir del .decimal  

        if (field.value != "") {
            if ((field.value.indexOf(".")) > 0) {
                //si tiene un punto valida dos digitos en la parte decimal

                if (key > 47 && key < 58) {
                    if (field.value == "") return true;
                    //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

                    regexp = /[0-9]{0}$/;
                    return !(regexp.test(field.value));
                }
            }
        }
        // 0-9 

        if (key > 47 && key < 58) {
            if (field.value == "") return true;
            regexp = /[0-9]{10}/;
            return !(regexp.test(field.value));
        }
        // .

        if (key == 46) {
            //if (field.value == "")
            return false;
            //regexp = /^[0-9]+$/;
            //return regexp.test(field.value);
        }
        // other key

        return /\d/.test(String.fromCharCode(key));
    }
    function valNC(e, field) {//ok grados centigrados
        var regexp;
        var key = window.event ? window.event.keyCode : e.which;
        // backspace

        if (key == 8) return true;

        // 0-9 a partir del .decimal  

        if (field.value != "") {
            if ((field.value.indexOf(".")) > 0) {
                //si tiene un punto valida dos digitos en la parte decimal

                if (key > 47 && key < 58) {
                    if (field.value == "") return true;
                    //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

                    regexp = /[0-9]{0}$/;
                    return !(regexp.test(field.value));
                }
            }
        }
        // 0-9 

        if (key > 47 && key < 58) {
            if (field.value == "") return true;
            regexp = /[0-9]{3}/;
            return !(regexp.test(field.value));
        }
        // .

        if (key == 46) {
            //if (field.value == "") return false;
            //regexp = /^[0-9]+$/;
            //return regexp.test(field.value);
            return false;
        }
        // other key

        return /\d/.test(String.fromCharCode(key));
    }
    function valN(e, field) {
        //debugger;
        var regexp;
        var key = window.event ? window.event.keyCode : e.which;
        // backspace

        if (key == 8) return true;

        // 0-9 a partir del .decimal  

        if (field.value != "") {
            if ((field.value.indexOf(".")) > 0) {
                //si tiene un punto valida dos digitos en la parte decimal

                if (key > 47 && key < 58) {
                    if (field.value == "") return true;
                    //regexp = /[0-9]{1,10}[\.][0-9]{1,3}$/

                    regexp = /[0-9]{2}$/;
                    return !(regexp.test(field.value));
                }
            }
        }
        // 0-9 

        if (key > 47 && key < 58) {
            if (field.value == "") return true;
            regexp = /[0-9]{10}/;
            return !(regexp.test(field.value));
        }
        // .

        if (key == 46) {
            if (field.value == "") return false;
            regexp = /^[0-9]+$/;
            return regexp.test(field.value);
        }
        // other key

        return /\d/.test(String.fromCharCode(key));

    }

    function rep() {
        var boton;

        if ($('#anexE >tbody >tr').length == 10) {
            boton = document.getElementById('<%= TextBox1.ClientID %>').value;
            document.getElementById('<%= TextBox12.ClientID %>').value = boton;


        }
        if ($('#anexE >tbody >tr').length == 12) {
            boton = document.getElementById('<%= TextBox1.ClientID %>').value;
            document.getElementById('<%= TextBox12.ClientID %>').value = boton;
            document.getElementById('<%= TextBox17.ClientID %>').value = boton;
            document.getElementById('<%= TextBox25.ClientID %>').value = boton;

        }
        if ($('#anexE >tbody >tr').length == 14) {
            boton = document.getElementById('<%= TextBox1.ClientID %>').value;
            document.getElementById('<%= TextBox12.ClientID %>').value = boton;
            document.getElementById('<%= TextBox17.ClientID %>').value = boton;
            document.getElementById('<%= TextBox25.ClientID %>').value = boton;
            document.getElementById('<%= TextBox30.ClientID %>').value = boton;
            document.getElementById('<%= TextBox35.ClientID %>').value = boton;
        }



    }
   
</script>

