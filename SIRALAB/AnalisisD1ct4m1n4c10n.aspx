<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AnalisisD1ct4m1n4c10n.aspx.cs" Inherits="SIRALAB.AnalisisD1ct4m1n4c10n" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="upd" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lnkbtn1" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtn2" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtn3" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkbtn4" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <div>
                <div>
                    <asp:LinkButton ID="lnkbtn1" runat="server" OnClick="lnkbtn1_Click">v</asp:LinkButton>
                </div>
                <div>
                    <asp:LinkButton ID="lnkbtn2" runat="server" OnClick="lnkbtn2_Click">v</asp:LinkButton>
                </div>
                <div>
                    <asp:LinkButton ID="lnkbtn3" runat="server" OnClick="lnkbtn3_Click">v</asp:LinkButton>
                </div>
                <div>
                    <asp:LinkButton ID="lnkbtn4" runat="server" OnClick="lnkbtn4_Click">v</asp:LinkButton>
                </div>

                <asp:Panel ID="pnl" runat="server" Visible="false">
                    <div>
                        <asp:TextBox ID="txtPas" runat="server" TextMode="Password"></asp:TextBox>
                    </div>
                    <div>
                        <asp:LinkButton ID="lnkbtnPas" runat="server" OnClick="lnkbtnPas_Click">V</asp:LinkButton>
                    </div>
                    <div>
                        <asp:Label ID="lblC1" runat="server"></asp:Label>
                    </div>
                    <div>
                        <asp:Label ID="lblC2" runat="server"></asp:Label>
                    </div>
                </asp:Panel>
            </div>

            <asp:HiddenField ID="h1" runat="server" />
    <asp:HiddenField ID="h2" runat="server" />
    <asp:HiddenField ID="h3" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
