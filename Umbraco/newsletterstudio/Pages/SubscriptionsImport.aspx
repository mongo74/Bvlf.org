<%@ Page Language="C#" MasterPageFile="../../masterpages/umbracoPage.Master" AutoEventWireup="true" CodeBehind="SubscriptionsImport.aspx.cs" Inherits="NewsletterStudio.Pages.SubscriptionsImport" %>

<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="body" runat="server">

<umb:Pane ID="panSubscription" runat="server" Text="">
    
    <div id="loading" style="display:none; position: absolute; top: -2px; left: -2px;">
        <img src="/umbraco/newsletterstudio/images/trans_white_95.png" width="487" height="152" />
        <div id="loading-ajax" style="display:none; position: absolute; top: 50px; left: 0px; width:485px; text-align:center;">
            <img src="/umbraco/newsletterstudio/images/ajax.gif" />
            <p>Adding new subscribers... (may take several minutes)</p>
        </div>
          
    </div>
    To import new subscribers please select a formatted  text file and input the file's format.<br />
    <br />
    <umb:PropertyPanel ID="PropertyPanel1" runat="server" Text="Choose a file">
	        <asp:FileUpload ID="FileUpload" runat="server" Style="width:340px;" />
    </umb:PropertyPanel>
    <umb:PropertyPanel ID="Pp2" runat="server" Text="Choose file format">
        <asp:DropDownList ID="ddlFormat" runat="server" Style="width:340px;">
            <asp:ListItem Value="0" Selected="True" Text="E-mail comma-separated" />
            <asp:ListItem Value="1" Text="E-mail separated with semicolon" />
            <asp:ListItem Value="2" Text="One e-mail per row" />
            <asp:ListItem Value="3" Text="Name and e-mail. One per row separated by semicolon" />
            <asp:ListItem Value="4" Text="Name and e-mail. One per row separated by comma" />
            <asp:ListItem Value="5" Text="(Outlook) First name, last name, e-mail. One per row separated comma" />
        </asp:DropDownList>
    </umb:PropertyPanel>
    
    <div style="padding-top: 25px;">
        <asp:Literal id="litError" runat="server" />
	    <asp:Button id="btnSubmit" Runat="server" style="Width:90px" onclick="btnSubmit_Click" OnClientClick="return StartImport();" Text="Import"></asp:Button>
	    &nbsp; <em><%= umbraco.ui.Text("or") %></em> &nbsp;
       <a href="#" style="color: blue"  onclick="UmbClientMgr.closeModalWindow()"><%=umbraco.ui.Text("cancel")%></a>
    </div>

    <script>
        function StartImport() {
            document.getElementById('loading').style.display = 'block';
            document.getElementById('loading-ajax').style.display = 'block';
        }
    </script>
  

</umb:Pane>

</asp:Content>