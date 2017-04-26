<%@ Page language="c#" CodeBehind="Newsletter.aspx.cs" MasterPageFile="../../masterpages/umbracoPage.Master" AutoEventWireup="True" Inherits="NewsletterStudio.Pages.Newsletter" ValidateRequest="false" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register tagprefix="umbEdit" namespace="umbraco.editorControls.tinyMCE3.webcontrol" Assembly="umbraco.editorControls" %>

<asp:Content ContentPlaceHolderID="head" runat="server">
	
    <link rel="Stylesheet" type="text/css" href="/umbraco/newsletterstudio/css/style.css" />		   
</asp:Content>

<asp:Content ContentPlaceHolderID="body" runat="server">
    
<div class="newsletter">

    <umb:TabView ID="TabView1" runat="server" Width="552px" Height="392px">
    </umb:TabView>

    <umb:Pane ID="panInfoBasic" runat="server" Text="">
        
         <div class="error" style="width:75%; clear:both; margin-bottom:20px;" runat="server" id="divError" Visible="false">
            <h3>This e-mail has status error</h3>
            <p>
                This status is used when something goes wrong during the send out to prevent the send out engine from keep sending e-mails when there is a problem. <br/>
                <br/>
                This status is only used when an error is thrown in during send out and is most of the time related to custom render tasks, custom subscription providers, invalid data in the database and so on.<br/>
                <br/>
                To find out more about the error details, please check the Umbraco log and/or the trace log.
                <br/><br/>
            </p>
        </div>

        <umb:PropertyPanel ID="PropertyPanel1" runat="server" Text="Name<br/><small>For internal use only</small>" >
	        <asp:TextBox ID="txtName" runat="server" style="width:350px" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="PropertyPanel2" runat="server" Text="Subject<br/><small>Subject of the e-mail</small>">
	        <asp:TextBox ID="txtEmailSubject" runat="server" style="width:350px" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppSenderEmail" runat="server" Text="Sender e-email<br/><small>Must be a valid e-mail address</small>">
	        <asp:TextBox ID="txtEmailFrom" runat="server" style="width:350px" />
        </umb:PropertyPanel>
        <umb:PropertyPanel id="ppSenderName" runat="server" Text="Sender name<br/><small>The name of the sender that will appear in the receiver client</small>">
	        <asp:TextBox ID="txtSenderName" runat="server" style="width:350px" />
        </umb:PropertyPanel>
       
        
    </umb:Pane>

    <umb:Pane ID="panInfoReceivers" runat="server" Text="" Visible="false">
        <umb:PropertyPanel ID="PropertyPanel5" runat="server" Text="Select receviers">
            <asp:DropDownList ID="ddlReceivers" runat="server" />
        </umb:PropertyPanel>
    </umb:Pane>

    <umb:Pane ID="panInfoTemplate" runat="server" Text="">
        <umb:PropertyPanel ID="PropertyPanel7" runat="server" Text="Select skin<br/><small>Choose a skin from the settings</small>">
            <asp:DropDownList ID="ddlTemplate" runat="server" />
        </umb:PropertyPanel>
    </umb:Pane>
    <umb:Pane ID="panRTE" runat="server" Text="">
    </umb:Pane>
</div>

<script type="text/javascript">
/*
    $(document).ready(function () {
        
        setTimeout("DoIt()", 2500);

    });

    function DoIt() {
        
        // loop over them, grab the class, replace 104 and palce it back.

        $(".editorArrow").each(function (index) {

            console.log('hej' + index + 'this: ' + this);

            var classValue = new String($(this).attr('onmouseover'));
            alert(classValue);
            classValue = classValue.replace('130', '1004');
            alert(classValue);
            $(this).attr('onmouseover', classValue);


        });
    }
    */
</script>

</asp:Content>