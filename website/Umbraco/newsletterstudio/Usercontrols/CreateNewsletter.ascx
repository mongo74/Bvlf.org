<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateNewsletter.ascx.cs" Inherits="NewsletterStudio.Usercontrols.CreateNewsletter" %>

<div style="MARGIN-TOP: 20px">Newsletter name:<asp:RequiredFieldValidator id="RequiredFieldValidator1" ErrorMessage="*" ControlToValidate="txtName" runat="server">*</asp:RequiredFieldValidator><br />

<asp:TextBox id="txtName" CssClass="bigInput" Runat="server" width="350px"></asp:TextBox>
<!-- added to support missing postback on enter in IE -->
<asp:TextBox runat="server" style="visibility:hidden;display:none;" ID="Textbox1"/>

</div>

<div style="padding-top: 25px;">
	<asp:Button id="btnSubmit" Runat="server" style="Width:90px" onclick="btnSubmit_Click" Text="Create"></asp:Button>
	&nbsp; <em><%= umbraco.ui.Text("or") %></em> &nbsp;
   <a href="#" style="color: blue"  onclick="UmbClientMgr.closeModalWindow()"><%=umbraco.ui.Text("cancel")%></a>
</div>
