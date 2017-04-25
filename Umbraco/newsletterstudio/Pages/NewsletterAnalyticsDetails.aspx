<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../../masterpages/umbracoPage.Master" CodeBehind="NewsletterAnalyticsDetails.aspx.cs" Inherits="NewsletterStudio.Pages.NewsletterAnalyticsDetails" %>

<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register tagprefix="umbEdit" namespace="umbraco.editorControls.tinyMCE3.webcontrol" Assembly="umbraco.editorControls" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

  <!-- styles newsletter -->
    <link rel="Stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/umbraco/newsletterstudio/css/style.css") %>" />
    <script src="<%=Page.ResolveClientUrl("~/Umbraco/newsletterstudio/js/newsletterstudio.js") %>"></script>
  
</asp:Content>

<asp:Content ContentPlaceHolderID="body" runat="server">
    
    

    <umb:TabView ID="TabView1" runat="server" Width="552px" Height="392px">
    </umb:TabView>
    
    
    <umb:Pane ID="panHeadline" runat="server" Text="">
        <h1 id="headline" runat="server">[headline]</h1>
        <p runat="server" id="pIntroText">Use the drop down boxes and the text search field to filter as you need. Click the table row to see more information about each subscribers interaction.</p>
    </umb:Pane>
    
     <umb:Pane ID="panFilters" runat="server" Text="Filters">
     
         Status:
         <asp:DropDownList runat="server" ID="ddlStatus" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
             <asp:ListItem Value="">- Choose status -</asp:ListItem>
             <asp:ListItem Value="opened">Opened</asp:ListItem>
             <asp:ListItem Value="notopened">Not opened</asp:ListItem>
             <asp:ListItem Value="clicked">Clicked</asp:ListItem>
             <asp:ListItem Value="unsubscribed">Unsubscribed</asp:ListItem>
             <asp:ListItem Value="error">Error</asp:ListItem>
         </asp:DropDownList>

         Link:
         <asp:DropDownList id="ddlUrls" runat="server" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlUrls_SelectedIndexChanged">
             <asp:ListItem Value="">-- Choose url ---</asp:ListItem>
         </asp:DropDownList>

        <asp:TextBox ID="txtSearch" CssClass="analytics-search-text" runat="server" Placeholder="Search for name or e-mail..." />
        <asp:Button runat="server" CssClass="btnFilterText" ID="btnFilterText" Text="Search" OnClick="btnFilterText_Click" />
           
    </umb:Pane>

    <umb:Pane ID="panTable" runat="server" Text="Clicks">
        
        <asp:Literal ID="litCount" runat="server"/>

        <asp:Repeater ID="rptRecivers" runat="server">
            <HeaderTemplate>
                <table class="data" border="0" cellpadding="0" cellspacing="0" style="width:100%;">
                    <thead>
                        <th style="width:30%">Name</th>
                        <th style="width:30%">E-mail</th>
                        <th style="width:20%">Status</th>
                        <th style="width:20%">Clicks</th>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr class='<%# (Eval("StatusDisplayText") != "Did not open") ? "analyticsdetailsrow" : ""  %>'>
                        <td><%# Eval("Name")%> </td>
                        <td><%# Eval("Email")%></td>
                        <td><%# Eval("StatusDisplayText")%></td>
                        <td>
                            <span class="clicks"><%# Eval("Clicks")%></span>
                            <div class="clicksDetails" style="">
                                <asp:Repeater ID="Repeater1" runat="server" DataSource='<%#Eval("UrlsAsList") %>'>
                                <ItemTemplate>
                                   <%#Eval("Count") %>x <%#Eval("Url") %><br/>
                                </ItemTemplate>
                            </asp:Repeater>
                            </div>
                        </td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
                
            </FooterTemplate>
                    
        </asp:Repeater>
        
        <div id="pagerDiv" runat="server"></div>

    </umb:Pane>
    
    <script type="text/javascript">
        $(document).ready(function () {
            
            // Hook into the tr click
            $('tr.analyticsdetailsrow').click(function () {
                
                if($(this).hasClass('detailsOpen'))
                {
                    // remove details.
                    $(this).parent().find('tr.rowdetails').remove();
                    $(this).removeClass('detailsOpen');
                    return;
                }

                // fetch the email
                var email = $(this).children().eq(1).html();
                var me = this;
                
                $.get(location.href + '&email=' + email, null, function (returnedData) {
                    // This callback is executed if the post was successful
                    if (returnedData != null && returnedData != '') {

                        $(me).after(returnedData);
                        $(me).addClass('detailsOpen');
                    }

                });

                
            });
            
            // Hooks into span.clicks to show details of click when hooovering
            $('span.clicks').hover(function () {
                if ($(this).html() != "0")
                    $(this).parent().find('div.clicksDetails').show();
            }, function () {
                if ($(this).html() != "0")
                    $(this).parent().find('div.clicksDetails').hide();
            });
            
            $("input[type=text]").keypress(function (event) {
                if (event.which == 13) {
                    event.preventDefault();
                    $(".btnFilterText").click();
                }
            });


        });
    </script>

</asp:Content>