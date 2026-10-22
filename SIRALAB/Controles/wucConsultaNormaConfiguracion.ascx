<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucConsultaNormaConfiguracion.ascx.cs" Inherits="SIRALAB.Controles.wucConsultaNormaConfiguracion" %>


<link href="style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="style/titulo.css" rel="stylesheet" type="text/css" />
<div>
    <asp:UpdatePanel ID="udpNorma" runat="server">
        <ContentTemplate>
            <asp:GridView ID="GridViewNormaConfiguracion" runat="server" 
                SkinID="GridviewSmallSizePage"
                AutoGenerateColumns="False"
                AllowPaging="True" AllowSorting="True"
                ItemType="SIRALAB_DAL.NormaConfiguracion"
                DataKeyNames="ParametroID,CuerpoReceptorID,UsoAguaID,PromedioID" 
                PageSize="25" 
                OnRowCommand="GridViewNormaConfiguracion_RowCommand"
                OnRowDataBound="GridViewNormaConfiguracion_RowDataBound"
                OnRowEditing="GridViewNormaConfiguracion_RowEditing"
                OnPageIndexChanging="GridViewNormaConfiguracion_PageIndexChanging"
                OnSelectedIndexChanged="GridViewNormaConfiguracion_SelectedIndexChanged">
                <Columns>
                    <%--Campo: Parametro--%>
                    <asp:BoundField HeaderText="Parametro" DataField="ParametroNombre" />

                    <%--Campo: Cuerpo Receptor--%>
                    <asp:BoundField HeaderText="Cuerpo Receptor" DataField="CuerpoReceptorNombre" />

                    <%--Campo: Uso Agua--%>
                    <asp:BoundField HeaderText="Uso Agua " DataField="UsoAguaNombre" />

                    <%--Campo: Promedio--%>
                    <asp:BoundField HeaderText="Promedio" DataField="PromedioNombre" />

                    <%--Campo: LimiteInferior--%>
                    <asp:BoundField HeaderText="Límite Inferior" DataField="LimiteInferior" />

                    <%--Campo: LimiteSuperior--%>
                    <asp:BoundField HeaderText="Límite Superior" DataField="LimiteSuperior" />

                    <%--Campo: Acción--%>
                     <asp:CommandField ShowSelectButton="True" SelectText="" ControlStyle-CssClass="icono-editar-16"  />
                   
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridViewNormaConfiguracion" EventName="RowCommand" />
        </Triggers>
    </asp:UpdatePanel>
</div>
