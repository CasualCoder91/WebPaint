<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="TestSite._Default" %>
<%@ Register Assembly="Alarich.WebPaint" Namespace="Alarich" TagPrefix="AHE" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="ScriptManager"></asp:ScriptManager>
        <div>
            <AHE:WebPaint ID="webPaint" runat="server" Width="500" Height="500"
                Visible="false"
                DefaultColor="#FFFFFF"
                CSSLink="/CustomWebPaint.css"/><br />
            <asp:Button ID="btnLoadImage" runat="server" OnClick="btnLoadImage_Click" Text="Load" />
            <asp:Button ID="btnSaveImage" runat="server" OnClientClick="saveImageData();" OnClick="btnSaveImage_Click" Text="Save" />
        </div>
    </form>
</body>
</html>
