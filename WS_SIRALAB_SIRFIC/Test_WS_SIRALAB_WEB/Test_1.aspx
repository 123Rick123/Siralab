<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test_1.aspx.cs" Inherits="Test_WS_SIRALAB_WEB.Test_1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btn" runat="server" Text="Test" OnClick="btn_Click" />
            <br />
            <asp:Label ID="lbl" runat="server"></asp:Label>
        </div>
        <div>
            <asp:TextBox ID="txt" runat="server"></asp:TextBox>
            <asp:Button ID="btnPDF" runat="server" Text="Test PDF" OnClick="btnPDF_Click" />
        </div>
        <div>
            <asp:GridView ID="gvAnalisis" runat="server" ItemType="Test_WS_SIRALAB_WEB.ws_SIRALAB.Analisis">
                <Columns>
                    <asp:TemplateField>
                        
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gvMuestra" runat="server" ItemType="Test_WS_SIRALAB_WEB.ws_SIRALAB.Muestra">
                <Columns>
                    <asp:TemplateField>
                                    
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
