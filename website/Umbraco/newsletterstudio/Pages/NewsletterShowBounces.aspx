<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../../masterpages/umbracoPage.Master" CodeBehind="NewsletterShowBounces.aspx.cs" Inherits="NewsletterStudio.Pages.NewsletterShowBounces" %>

<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="Stylesheet" type="text/css" href="/umbraco/newsletterstudio/css/style.css" />		   

    <script type="text/javascript">
        function CloseAndGoTo(url) 
        {
            UmbClientMgr.contentFrame(url);
            UmbClientMgr.closeModalWindow();
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <umb:Pane ID="panBounces" runat="server">

       <asp:ListView ID="lwBounces" runat="server">
                                
            <LayoutTemplate>
                <table id="bounceTable" border="0" cellpadding="0" cellspacing="0">  
                    <thead>
                        <th style="width: 190px;">Name</th>
                        <th style="width: 230px;">E-mail</th>
                        <th style="width: 390px;">Error message</th>
                        <th style="width: 80px;"></th>
                    </thead>
                    <asp:PlaceHolder id="itemPlaceHolder" runat="server" />
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr class="item">
                    <td><%#Eval("Name") %></td>
                    <td><%#Eval("Email") %></td>
                    <td class="err"><%#Eval("Error")%></td>
                    <td><a style="display:<%#Eval("Display")%>" href="javascript:<%#Eval("EditLink")%>">Go to edit</a></td>
                </tr>
            </ItemTemplate>
        </asp:ListView>
        
    </umb:Pane>
</asp:Content>