<%@ Page language="c#" CodeBehind="Settings.aspx.cs" MasterPageFile="../../masterpages/umbracoPage.Master" ValidateRequest="false" AutoEventWireup="True" Inherits="NewsletterStudio.Pages.Settings" %>
<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>


<asp:Content ContentPlaceHolderID="head" runat="server">
    <link rel="Stylesheet" type="text/css" href="/umbraco/newsletterstudio/css/style.css" />
    	   
    <script type="text/javascript" language="javascript">
        function ConfirmDeleteSmtp() {
            // Count number of items in ddl.
            var items = $(".ddlSmtpServer option").length;
            if(items > 1)
            {
                return confirm('Are you sure that you want to delete this server?');
            }
            else
            {
                alert('You need at least one SMTP-server, can\'t delete the last one');
                return false;
            }

        }

    </script>
</asp:Content>

<asp:Content ContentPlaceHolderID="body" runat="server">

    
    <div class="settings">
    
    <umb:TabView ID="TabView1" runat="server" Width="552px" Height="392px">
    </umb:TabView>
    <umb:Pane ID="panSmtpServer" runat="server">
        <umb:PropertyPanel ID="PropertyPanel21" runat="server" Text="Choose SMTP Server">
            <asp:DropDownList runat="server" ID="ddlSmtpServer" CssClass="ddlSmtpServer" AutoPostBack="true" OnSelectedIndexChanged="ddlSmtpServer_SelectedIndexChanged" style="width:200px;" />
            <asp:Button ID="btnAddServer" runat="server" Text="Add server" OnClick="btnAddServer_OnClick" />
            <asp:Button ID="brnRemoveServer" runat="server" Text="Remove" OnClick="btnRemoveServer_OnClick" OnClientClick="return ConfirmDeleteSmtp();" />
        </umb:PropertyPanel>
    </umb:Pane>

    <umb:Pane ID="panMailSettings" runat="server" Text="">

        <umb:PropertyPanel ID="PropertyPanel1" runat="server" Text="Host (smtp)">
            <asp:HiddenField ID="hiddenCurrentServerName" runat="server" value="" />
	        <asp:Textbox id="txtHost" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel2" runat="server" Text="Port<br/><small>Default (25)</small>">
	        <asp:Textbox id="txtPort" runat="server" style="width:50px;" />
            <asp:RegularExpressionValidator ControlToValidate="txtPort" ErrorMessage="Please Enter Only Numbers" CssClass="error" ValidationExpression="^\d+$" runat="server" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel3" runat="server" Text="Username<br /><small>If server needs authorization, enter username here.</small>">
	        <asp:Textbox id="txtUsername" runat="server" style="width:300px" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel4" runat="server" Text="Password<br /><small>If server needs authorization, enter password here.</small>">
	        <asp:Textbox id="txtPassword" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel5" runat="server" Text="Enable Ssl<br /><small>If checked, a secure connection to the mail-server will be used.</small>">
	        <asp:CheckBox ID="cbEnebleSsl" runat="server" />
        </umb:PropertyPanel>
        <umb:PropertyPanel ID="pp35" runat="server" Text="">
        <div class="propertyItemheader">
            <asp:Button ID="btnSendTestEmail" OnClientClick="return SetSmtpResponseAsAjax();" Text="Test SMTP-connection" runat="server" OnClick="btnSendTestEmail_Click" />
        </div>
        <div class="propertyItemContent">
            <script type="text/javascript">
            function SetSmtpResponseAsAjax() {
                $('#SmtpTestResponse').html('<img src="/umbraco/newsletterstudio/images/ajax.gif" height="20"> Testing connection...');
                
                return true;
            }
            </script>
            <div style="width:450px;" id="SmtpTestResponse">
                <img src="/umbraco/newsletterstudio/images/ajax.gif" height="20" style="display:none;">
                <asp:Label ID="labelSmtpTest" runat="server"/>
            </div>
        </div>
           
        </umb:PropertyPanel>
    </umb:Pane>
    <umb:Pane ID="panThrottling" runat="server" Text="Other settings for outgoing e-mails">
        
        <umb:PropertyPanel id="PropertyPanel13" runat="server" Text="Default sender name<br /><small>The default display name of the sender of outgoing e-mails.</small>">
            <asp:Textbox id="txtDefaultSenderName" runat="server" style="width:300px" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel14" runat="server" Text="Default sender e-mail<br /><small>The default e-mail of the sender of outgoing e-mails.</small>">
            <asp:Textbox id="txtDefaultSenderEmail" runat="server" style="width:300px" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel15" runat="server" Text="Force sender defaults<br /><small>If checking this the editors will not have the option to change the defualts, they will always be used automatically.</small>">
            <asp:CheckBox ID="cbDefaultSenderSettingsForced" runat="server" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel22" runat="server" Text="Jump to next server after<br /><small>If multiple SMTP-servers are specified, specify the number of e-emails to send before jumping to the next server.</small>">
            <asp:Textbox id="txtMessagesBeforeJumpingToNextServer" runat="server" style="width:50px;" />
            <asp:RegularExpressionValidator ControlToValidate="txtMessagesBeforeJumpingToNextServer" ErrorMessage="Please Enter Only Numbers" CssClass="error" ValidationExpression="^\d+$" runat="server" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel24" runat="server" Text="Throttling enabled<br /><small>With this enabled, the server will send X e-mails and wait for Y seconds and loop that behavior.</small>">
            <asp:CheckBox ID="cbThrottlingEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="cbThrottlingEnabled_OnCheckedChanged" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppThrottling1" runat="server" Text="Throttling - Messages per batch<br /><small>The number of messages to send before waiting for next batch to fire.</small>">
            <asp:Textbox id="txtThrottlingMessagesPerBatch" runat="server" style="width:50px;" />
            <asp:RegularExpressionValidator ControlToValidate="txtThrottlingMessagesPerBatch" ErrorMessage="Please Enter Only Numbers" CssClass="error" ValidationExpression="^\d+$" runat="server" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppThrottling2" runat="server" Text="Throttling - Seconds between batches<br /><small>Specifies the number of seconds to wait beetwen each batch.</small>">
            <asp:Textbox id="txtThrottlingSecondsBetweenBatches" runat="server" style="width:50px;" />
            <asp:RegularExpressionValidator ControlToValidate="txtThrottlingSecondsBetweenBatches" ErrorMessage="Please Enter Only Numbers" CssClass="error" ValidationExpression="^\d+$" runat="server" />
        </umb:PropertyPanel>
    </umb:Pane>

    <umb:Pane ID="panAppSettings" runat="server" Text="Application settings">
        <umb:PropertyPanel id="PropertyPanel9" runat="server" Text="Force unsubscribe<br /><small>Will insert a generic link for unsubscription if not inserted in skin/message body.</small>">
	        <asp:CheckBox ID="cbForceUnsubscribe" runat="server" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel11" runat="server" Text="Settings only for admins<br /><small>Checking this button will hide the settings-node from non-admins.</small>">
            <asp:CheckBox ID="cbSettingsOnlyForAdmins" runat="server" />
        </umb:PropertyPanel>
        
        <umb:PropertyPanel id="PropertyPanel12" runat="server" Text="TinyMCE DataType Id<br /><small>Which data type id to render as wysiwyg-editor</small>">
            <asp:Textbox ID="txtTinyMCEDataTypeId" runat="server" style="width:50px;" />
            <asp:RegularExpressionValidator ControlToValidate="txtTinyMCEDataTypeId" ErrorMessage="Please Enter Only Numbers" CssClass="error" ValidationExpression="^\d+$" runat="server" />
        </umb:PropertyPanel>

        <umb:PropertyPanel id="PropertyPanel19" runat="server" Text="Unsubscribe confirmation url<br /><small>When unsubscribing, a user will be redirected to this url.</small>" >
            <asp:Textbox id="txtUnsubscribeConfirmUrl" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel27" runat="server" Text="Send failures before unsubscribing<br /><small>Specify the number of allowed send failures before settings the subscriber as unsubscribed. (0 = unlimited)</small>" >
            <asp:Textbox id="txtSendFailuresBeforeUnsubscribing" runat="server" style="width:50px;" />
            <asp:RequiredFieldValidator ControlToValidate="txtSendFailuresBeforeUnsubscribing" CssClass="error" Text="Please enter a number" runat="server" />
            <asp:RegularExpressionValidator ControlToValidate="txtSendFailuresBeforeUnsubscribing" ErrorMessage="Please Enter Only Numbers" CssClass="error" ValidationExpression="^\d+$" runat="server" />
        </umb:PropertyPanel>
    </umb:Pane>

    <umb:Pane ID="panGoogleAnalytics" runat="server" Text="Google Analytics Tracking">
        <umb:PropertyPanel id="PropertyPanel23" runat="server" Text="Enable Google Analytics Tracking<br /><small>Will add queryparameters to links in the messagebody to track traffic from emails in Google Analytics.</small>">
            <asp:CheckBox ID="cbEnableGoogleAnalyticsTracking" runat="server" OnCheckedChanged="cbEnableGoogleAnalyticsTracking_CheckedChanged" AutoPostBack="true" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppGoogle1" runat="server" Text="Medium<br /><small>To identify a medium such as email or cost-per-click. Example: cpc, email. Default: email</small>" >
            <asp:Textbox id="txtGoogleAnalyticsMedium" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppGoogle2" runat="server" Text="Source<br /><small>To identify a search engine, newsletter name, or other source. Example: google. Default: NewsletterStudio</small>" >
            <asp:Textbox id="txtGoogleAnalyticsSource" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppGoogle3" runat="server" Text="Name<br /><small>To identify a specific product promotion or strategic campaign. Example: spring_sale. Leaving this blank will insert the name of the specific newsletter (recommended)</small>" >
            <asp:Textbox id="txtGoogleAnalyticsName" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppGoogle4" runat="server" Text="Content<br /><small>To differentiate ads or links that point to the same URL. Examples: logolink or textlink. Leaveing this blank will insert the subject of the specific newsletter (recommended)</small>" >
            <asp:Textbox id="txtGoogleAnalyticsContent" runat="server" style="width:300px;" />
        </umb:PropertyPanel>

    </umb:Pane>

    <umb:Pane ID="panBounceSettings" runat="server" Text="E-mail bounce management">
        
        <umb:PropertyPanel id="PropertyPanel10" runat="server" Text="Active bounce management<br /><small>Checking this actives e-mail bounce management</small>">
            <asp:CheckBox ID="cbBounceActive" runat="server" AutoPostBack="true" OnCheckedChanged="cbBounceActive_Changed" />
        </umb:PropertyPanel>
         <umb:PropertyPanel id="ppBounce1" runat="server" Text="Bounce email address<br /><small>The e-mail where bounces should end up (will also be used as send out email)</small>">
            <asp:Textbox ID="txtBounceEmail" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
         <umb:PropertyPanel id="ppBounce2" runat="server" Text="Bounce server<br /><small>Requires a POP3 mail server</small>">
            <asp:Textbox ID="txtBounceHost" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppBounce3" runat="server" Text="Server-port<br /><small>Default (110)</small>">
            <asp:Textbox ID="txtBouncePort" runat="server" style="width:50px;" />
            <asp:RegularExpressionValidator ControlToValidate="txtBouncePort" ErrorMessage="Please Enter Only Numbers" CssClass="error" ValidationExpression="^\d+$" runat="server" />
        </umb:PropertyPanel>
         <umb:PropertyPanel id="ppBounce4" runat="server" Text="Bounce mailbox username<br /><small>Used to logon to the mail server</small>">
            <asp:Textbox ID="txtBounceUser" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
         <umb:PropertyPanel id="ppBounce5" runat="server" Text="Bounce mailbox password<br /><small>Password to the mailbox</small>">
            <asp:Textbox ID="txtBouncePassword" runat="server" style="width:300px;" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppBounce6" runat="server">
            <div class="propertyItemheader">
                <asp:Button ID="btnBounceTestConnecction" Text="Test bounce connection" runat="server" OnClick="btnBounceTestConnecction_Click" /><br />
            </div>
           <div class="propertyItemContent">
                <asp:Label ID="labelBounceConnection" runat="server" />
            </div>
        </umb:PropertyPanel>
         
    </umb:Pane>

    <umb:Pane ID="panAllowedTemplates" runat="server" Text="Allowed templates for Render URL">
        <p>Check the templates that you would like the editors to be able to pick in the "Insert URL content"-popup.</p>
        <asp:CheckBoxList ID="cbListTemplates" runat="server"></asp:CheckBoxList>
    
    </umb:Pane>


    
    <umb:Pane ID="panTemplateChoose" runat="server" Text="">
        <asp:HiddenField ID="hiddenCurrentTemplate" runat="server" />
        <umb:PropertyPanel id="PropertyPanel6" runat="server" Text="Choose skin">
	        <asp:DropDownList ID="ddlTemplateChoose" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplateChoose_SelectedIndexChange"></asp:DropDownList> <asp:ImageButton Visible="false" ID="btnAddTemplate" ImageUrl="../../images/umbraco/add.png" runat="server" OnClick="btnAddTemplate_Click" />
        </umb:PropertyPanel>
    </umb:Pane>

    <umb:Pane ID="panTemplates" runat="server" Text="">
        <umb:PropertyPanel id="PropertyPanel7" runat="server" Text="Name">
            <asp:Textbox ID="txtTemplateName" runat="server" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel8" runat="server" >
            
                <umb:CodeArea ID="codeArea" runat="server" AutoResize="true" OffSetX="75" OffSetY="50" />
                <asp:TextBox ID="txtTemplateContent" runat="server" TextMode="MultiLine" Visible="false" />
            
        </umb:PropertyPanel>
    </umb:Pane>
 </div>


 <script type="text/javascript">

     $(document).ready(function () {
         // Refreshes the editor if version is > v4.9 and the refresh-method is found on the object.
         if (UmbEditor._editor.refresh != undefined) {

             $("li.tabOff").click(function () { UmbEditor._editor.refresh(); });
             UmbEditor._editor.refresh();
         }
     });

 </script>

</asp:Content>