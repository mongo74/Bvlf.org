<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../../../masterpages/umbracoPage.Master" CodeBehind="Dialog.aspx.cs" Inherits="NewsletterStudio.Pages.Plugin.Dialog" %>

<%@ Register TagPrefix="umb" Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" %>
<%@ Register TagPrefix="ui" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register Src="../../../controls/Tree/TreeControl.ascx" TagName="TreeControl" TagPrefix="umbraco" %>
<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" %>



<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    
    
    <script type="text/javascript" src="/umbraco_client/tinymce3/tiny_mce_popup.js"></script>
    <script type="text/javascript" src="/umbraco_client/tinymce3/plugins/newsletterstudiourlcontent/js/dialog.js"></script>

    <%-- 
    
    <umb:JsInclude ID="JsInclude1" runat="server" FilePath="tinymce3/tiny_mce_popup.js" PathNameAlias="UmbracoClient" Priority="100" />
    <umb:JsInclude ID="JsInclude3" runat="server" FilePath="tinymce3/plugins/newsletterstudiourlcontent/js/dialog.js" PathNameAlias="UmbracoClient" Priority="101" />
    --%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
        <script type="text/javascript" language="javascript">
            var currentLink = "";
            var useDirectoryUrls = <%=this.UseDirectoryUrls.ToString().ToLower()%>;

            function dialogHandler(values) {
            // values could be: {localLink:1050}|/sida-2/mer-info-och-saant.aspx|Mer info och sånt

            var returnValues = values.split("|");

            if (returnValues.length > 1) {
                if(!returnValues[1].startsWith('http'))
                {
                    $("#txtUrl").val(tinyMCEPopup.getWindowArg('baseurl') + returnValues[1]);
                }
                else
                {
                    $("#txtUrl").val(returnValues[1]);
                }
            }

            ApplySelectedTemplate();
        
        }
        
        function ApplySelectedTemplate()
        {
            var template = $('.ddlTemplate').first().val();
            if(template != undefined && template != '')
            {
                ChangeTemplateTo(template);
            }
        }

        var lastTemplateApplyed = '';
        function ChangeTemplateTo(templateAlias) {

                var strUrl = $("#txtUrl").val().toLowerCase();
                var lookFor = '/' + templateAlias.toLowerCase();

                if(!useDirectoryUrls) 
                    lookFor = lookFor + ".aspx";

                // Is there any value in the textbox?
                if(strUrl == '')
                    return;
                
                // Is the template already there?
                if (strUrl.indexOf(lookFor) != -1)
                    alert('finns redan');

                // Remove last template
                if(lastTemplateApplyed != '')
                {
                    var stringToRemove = (useDirectoryUrls ?  '' : '/') + lastTemplateApplyed;
                    strUrl = strUrl.replace(stringToRemove,'');
                }

                // Apply template
                if(useDirectoryUrls)
                {   
                    strUrl = strUrl + templateAlias;
                }
                else
                {
                    var intTotalLenght = strUrl.length;
                    // If root is selected, no .aspx. Then remove the '/'-sign to makesure that the add is correct.
                    var intTotalSubstringLenght = (strUrl.indexOf('.aspx') == -1) ? intTotalLenght -1 : intTotalLenght-5;

                    // Insert template before .aspx
                    var contentBefore = strUrl.substring(0, intTotalSubstringLenght);

                    strUrl = contentBefore + '/' + templateAlias + '.aspx';
                }

                lastTemplateApplyed = templateAlias;

                // Replace.
                $("#txtUrl").val(strUrl);

            }

            /*
            alert(id);
            id = id.toString();
            if (id == "-1") return;
            var returnValues = id.split("|");
            if (returnValues.length > 1) {
                if (returnValues[1] != '')
                    setFormValue('href', returnValues[1]);
                else
                    setFormValue('href', returnValues[0]);
                
                setFormValue('localUrl', returnValues[0]);
                setFormValue('title', returnValues[2]);
            } else {
                if (id.substring(id.length - 1, id.length) == "|")
                  id = id.substring(0, id.length - 1);

                setFormValue('href', id);
                setFormValue('localUrl', id);

                //umbraco.presentation.webservices.legacyAjaxCalls.NiceUrl(id, updateInternalLink, updateInternalLinkError);
            }
            */

        </script>


    <%-- 
    <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="false" runat="server">
        <Services>
            <asp:ServiceReference Path="../../../webservices/legacyAjaxCalls.asmx" />
        </Services>
    </asp:ScriptManager>
    --%>

    <ui:Pane ID="panInfoBasic" runat="server" Text="">
        <ui:PropertyPanel ID="PropertyPanel1" runat="server" Text="Url" >
	        <input id="txtUrl" name="txtUrl" type="text" class="text" style="width:370px;" />
        </ui:PropertyPanel>
    </ui:Pane>

    <ui:Pane ID="Pane1" runat="server" Text="Choose from Umbraco">
        <ui:PropertyPanel id="PropertyPanel2" runat="server">
            <umbraco:TreeControl runat="server" ID="TreeControl2" App="content"
                    IsDialog="true" DialogMode="locallink" ShowContextMenu="false" FunctionToCall="dialogHandler"
                    Height="230"></umbraco:TreeControl>
        </ui:PropertyPanel>
    </ui:Pane>
    <ui:Pane ID="Pane2" runat="server" Text="">
        <ui:PropertyPanel id="PropertyPanel3" runat="server" Text="Template">
            <asp:DropDownList id="ddlTemplate" CssClass="ddlTemplate" AppendDataBoundItems="true" runat="server" onChange="ChangeTemplateTo(this.value)">
               <asp:ListItem Value="" Text="-- Default --"/>

            </asp:DropDownList>
        </ui:PropertyPanel>
    </ui:Pane>

    <div class="mceActionPanel">
		<input type="button" id="insert" name="insert" value="{#insert}" onclick="ExampleDialog.insert();" />
		<input type="button" id="cancel" name="cancel" value="{#cancel}" onclick="tinyMCEPopup.close();" />
	</div>
    
</asp:Content>