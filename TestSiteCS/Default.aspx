<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestSiteCS.Default" %>
<%@ Register Assembly="Alarich.WebPaint" Namespace="Alarich" TagPrefix="AHE" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <AHE:WebPaint ID="webPaint" runat="server" Width="500" Height="500" Visible="false" Language="EN" DefaultColor="#73AD21" /><br />
            <asp:Button ID="btnLoadImage" runat="server" OnClick="btnLoadImage_Click" Text="Load" />
            <asp:Button ID="btnSaveImage" runat="server" OnClientClick="saveImageData();" OnClick="btnSaveImage_Click" Text="Save" />
        </div>
    </form>
</body>
</html>
