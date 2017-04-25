<%@ Page Language="C#" MasterPageFile="../../masterpages/umbracoPage.Master" AutoEventWireup="true" CodeBehind="SubscriptionImportWizard.aspx.cs" Inherits="NewsletterStudio.Pages.SubscriptionImportWizard" %>

<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <link rel="Stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("/umbraco/newsletterstudio/css/style.css") %>" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">

<umb:Pane ID="panSubscription" runat="server" Text="">
    
    <div id="loading" style="display:none; position: absolute; top: -2px; left: -2px; width:100%; height:100%; z-index: 100; text-align: center;">
        <img src="/umbraco/newsletterstudio/images/trans_white_95.png" width="100%" height="100%" />
        <div id="loading-ajax" style="display:none; position: absolute; top: 50px; width:100%; text-align:center;">
            <img src="/umbraco/newsletterstudio/images/ajax.gif" />
            <p>Adding new subscribers... (may take several minutes)</p>
        </div>
    </div>
    <div class="import-file-container">
         <asp:Button id="importFromFile" Runat="server" Text="Import from file" OnClientClick="return RedirectToFileImport();"></asp:Button> 
    </div>
    <div class="import-text">
        <p>Just paste the new subscribers in the textarea below. Each subscriber should be on a new line.  Make sure to start with the e-mail followed by comma and the name.</p>
    </div>
    <div class="placeholder">
        john@johnson.com, John Johnsson<br/>
        henry.brown@supercompany.com, Henry Brown
    </div>
    <asp:TextBox TextMode="MultiLine" ID="txtSubscribers" runat="server" Width="600px" Height="230px" CssClass="placeholder" />
    
    <div style="padding-top: 25px;">
        <asp:Literal id="litError" runat="server" />
	    <asp:Button id="btnSubmit" Runat="server" style="Width:150px" onclick="btnSubmit_Click" OnClientClick="return StartImport();" Enabled="false" CssClass="submit" Text="Import these subscribers"></asp:Button>
	    &nbsp; <em><%= umbraco.ui.Text("or") %></em> &nbsp;
       <a href="#" style="color: blue"  onclick="UmbClientMgr.closeModalWindow()"><%=umbraco.ui.Text("cancel")%></a>
    </div>

    <script type="text/javascript">

        function RedirectToFileImport() {
            window.location = 'SubscriptionsImport.aspx?id=<%=Request.QueryString["id"]%>';
            return false;
        }

        function StartImport() {
            $('#loading').show();
            $('#loading-ajax').show();
        }
        
        $(document).ready(function () {

            $('div.placeholder').click(function () {
                $('textarea').focus();
            });

            $('textarea.placeholder').keydown(function () {
                $('div.placeholder').hide();
            });

            $('textarea.placeholder').keyup(function () {
                
                if ($('textarea.placeholder').val() == '') {
                    $('div.placeholder').show();
                    $("input[type=submit].submit").attr("disabled", "disabled");
                } else {
                    $('div.placeholder').hide();
                    $("input[type=submit].submit").removeAttr("disabled");
                }
            });
        });

    </script>
  

</umb:Pane>

</asp:Content>