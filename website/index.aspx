<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="bvlf_v2.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div>
        <h1>Tryout mail</h1>
        <hr/>
        <asp:Button ID="cmdGoGetit" runat="server" Text="Go Get it" OnClick="cmdGoGetit_Click"/>
        <p>

            <asp:Literal ID="lblInfo" runat="server"></asp:Literal>
        </p>
    </div>
</form>
</body>
</html>