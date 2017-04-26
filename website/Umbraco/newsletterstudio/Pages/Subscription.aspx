<%@ Page language="c#" CodeBehind="Subscription.aspx.cs" EnableViewState="true" MasterPageFile="../../masterpages/umbracoPage.Master" AutoEventWireup="True" Inherits="NewsletterStudio.Pages.Subscription" ValidateRequest="false" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register tagprefix="umbEdit" namespace="umbraco.editorControls.tinyMCE3.webcontrol" Assembly="umbraco.editorControls" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="server">
	<link rel="Stylesheet" type="text/css" href="/umbraco_client/tabview/style.css" />
    <link rel="Stylesheet" type="text/css" href="/umbraco_client/scrollingmenu/style.css" />
    <link rel="Stylesheet" type="text/css" href="/umbraco_client/propertypane/style.css" />
    <link rel="Stylesheet" type="text/css" href="/umbraco_client/menuicon/style.css" />

    <link rel="Stylesheet" type="text/css" href="/umbraco/newsletterstudio/css/style.css" />

    <script src="<%=Page.ResolveClientUrl("~/umbraco_client/tabview/javascript.js") %>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/umbraco/newsletterstudio/js/newsletterstudio.js") %>" type="text/javascript"></script>

    <script>
        function showImport() {
             UmbClientMgr.openModalWindow('/umbraco/NewsletterStudio/Pages/SubscriptionImportWizard.aspx?id=<%=Request.QueryString.Get("id") %>', 'Import subscribers', true, 700, 430, 100, 0, '', '');
        }
    </script>
    
</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="body" runat="server">
    <div id="body_TabView1" class="subscription">
    
    <div class="header">
            <ul>
                <li class="tabOn">
                    <a href="#"><span>Subscribers</span></a>
                </li>
            </ul>
        </div>

    <div class="tabpagecontainer">
        
        <div id='body_TabView1_tab01layer' class="tabpage" style="display:block;">
            <div class="menubar">
                <span>
                    <table>
                    <tbody>
                        <tr>
                            <td></td>
                            <td>
                                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" AlternateText="Save changes" ImageUrl="/umbraco/images/editor/save.gif" CssClass="editorIcon" ToolTip="Save" Style="height: 23px; width: 22px; border: 0px none;" onmousedown="this.className='editorIconDown'" onmouseup="this.className='editorIconOver'" onmouseout="this.className='editorIcon'" onmouseover="this.className='editorIconOver'" />
                                <input type="image" id="btnImport" title="Import subscribers" src="/umbraco/newsletterstudio/images/import-data.gif" onclick="showImport(); return false;" class="editorIcon" Style="height: 23px; width: 22px; border: 0px none;" onmousedown="this.className='editorIconDown'" onmouseup="this.className='editorIconOver'" onmouseout="this.className='editorIcon'" onmouseover="this.className='editorIconOver'" />
                                <asp:ImageButton ID="btnExportToCsv" runat="server" ImageUrl="/umbraco/newsletterstudio/images/page_white_excel-22x23.gif" OnClick="btnExportToCsv_Click" CssClass="editorIcon" ToolTip="Export to CSV" Style="height: 23px; width: 22px; border: 0px none;" onmousedown="this.className='editorIconDown'" onmouseup="this.className='editorIconOver'" onmouseout="this.className='editorIcon'" onmouseover="this.className='editorIconOver'" />
                            </td>
                            <td></td>
                        </tr>
                        </tbody>
                    </table>
                </span>
            </div>
            <div class="tabpagescrollinglayer" id="body_TabView1_tab01layer_contentlayer"  style='height:342px;width:552px'>
                <div class="tabpageContent">

                <!-- **********   Content starts here ******************* -->

                    <umb:Pane ID="panInfo" runat="server" Text="">
                        <umb:PropertyPanel ID="PropertyPanel4" runat="server" Text="Mailing list name">
	                        <asp:TextBox ID="txtSubscriptionName" runat="server" style="width:350px;" />
                        </umb:PropertyPanel>
                        <umb:PropertyPanel ID="PropertyPanel35" runat="server" Text="Mailing list ID">
                            <asp:Literal id="litSubscriptionId" runat="server" />
                        </umb:PropertyPanel>
                        <umb:PropertyPanel ID="PropertyPanel5" runat="server" Text="Total subscribers">
	                        <asp:Literal id="litSubscribersCount" runat="server" />
                        </umb:PropertyPanel>
                    </umb:Pane>

                    <umb:Pane ID="panSubscription" runat="server" Text="Add new subscriber">
                        <umb:PropertyPanel ID="PropertyPanel1" runat="server" Text="Name">
	                        <asp:TextBox ID="txtName" runat="server" style="width:350px;"  />
                        </umb:PropertyPanel>
                        <umb:PropertyPanel id="PropertyPanel2" runat="server" Text="E-mail">
	                        <asp:TextBox ID="txtEmail" runat="server" style="width:350px;" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnAddSubscriber" runat="server" OnClick="btnAddSubscriber_Click" Text="Add" />
                        </umb:PropertyPanel>
                        <umb:PropertyPanel id="PropertyPanel3" runat="server" Text=" " Visible="false">
	                        
                        </umb:PropertyPanel>
                    </umb:Pane>

                    <umb:Pane ID="panSubscribersFilter" runat="server" Text="Subscribers">
                        <asp:Panel ID="panel" DefaultButton="btnApplyFiler" runat="server">
                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                                    <tr>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td style="width: 100px;">
                                            <asp:DropDownList runat="server" DataValueField="value" DataTextField="key" ID="ddlStatus" AppendDataBoundItems="true">
                                                <asp:ListItem Selected="True" Text="- All --" Value="" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 190px;"><asp:TextBox ID="txtFilterName" runat="server" style="width:150px" Placeholder="Filter on name" /></td>
                                        <td style="width: 190px;"><asp:TextBox ID="txtFilterEmail" runat="server" style="width:150px" Placeholder="Filter on e-mail" /></td>
                                        
                                        <td>
                                            <asp:Button ID="btnApplyFiler" runat="server" Width="90px" OnClick="btnApplyFiler_Click" Text="Apply filter" />
                                            <asp:Button ID="btnClearFilter" runat="server" Width="40px" OnClick="btnClearFilter_Click" Text="Clear" />
                                        </td>
                                        
                                    </tr>
                            </table>
                        </asp:Panel>
                    </umb:Pane>

    

                    <div class="propertypane">
                        
                        <asp:ListView ID="lwSubscribers" runat="server" OnItemCommand="lwSubscribers_ItemCommand" OnItemEditing="lwSubscribers_ItemEditing" DataKeyNames="Id">
                                
                                <LayoutTemplate>

                                    <table id="subscribersTable" class="data" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 20px;"><input id="selectedAll" type="checkbox" onchange="javascript:selectAll(this);" /></t>
                                            <td style="width: 100px;"></td>
                                            <td style="width: 190px;"><strong>Name</strong></td>
                                            <td style="width: 190px;"><strong>E-mail</strong></td>
                                            <td style="width: 110px;"><strong>Subscribed</strong></td>
                                            <td style="width: 50px;"></td>
                                        </tr>
                                        <asp:PlaceHolder id="itemPlaceHolder" runat="server" />
                                        <tr>
                                            <td colspan="5"><br /><asp:Button runat="server" id="btnDelete" OnClick="btnDelete_OnClick" OnClientClick="return deleteEmails();" Text="Delete selected" /></td>
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                     <tr class="item">
                                        <td>
                                            <asp:CheckBox ID="chkSelected" runat="server" CssClass=".checkbox" />
                                            <asp:HiddenField ID="hdID" runat="server" Value='<%# Eval("Id") %>' />
                                        </td>
                                        <td>&nbsp;&nbsp;<%#this.GetStatusImage(Eval("Status")) %></td>
                                        <td><%#Eval("Name") %></td>
                                        <td><%#Eval("Email")%></td>
                                        <td><%#String.Format("{0:g}",Eval("SubscribeDate"))%></td>
                                        <td>
                                            <div class="editcommands" style="display:none;">
                                                <asp:ImageButton CssClass="editcommand-btn" ImageUrl="/umbraco/newsletterstudio/images/vcard_edit.png" ToolTip="Edit subscriber" ID="EditButton" CommandName="Edit" runat="server" Text="Edit"></asp:ImageButton>
                                                <asp:ImageButton CssClass="editcommand-btn" ImageUrl="/umbraco/newsletterstudio/images/vcard_delete.png" ToolTip="Delete subscriber" ID="DeleteButton" CommandName="DeleteSubscriber" CommandArgument='<%#Eval("Id") %>' runat="server" Text="Delete Subscriber" OnClientClick="return confirm('Are you sure that you want to delete this subscriber?')"></asp:ImageButton>
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <tr class="editmode">
                                        <td><asp:HiddenField ID="hdID" runat="server" Value='<%# Eval("Id") %>' /></td>
                                        <td>
                                            <asp:DropDownList runat="server" DataValueField="value" DataTextField="key" ID="ddlStatus" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="editInput" Text='<%#Bind("Name") %>'/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmail" runat="server" style="" CssClass="editInput" Text='<%#Bind("Email") %>'/> 
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="* - Can't be empty" CssClass="valErr" Display="Dynamic" />
                                            <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="* - Invalid e-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="valErr" Display="Dynamic" />
                                        </td>
                                        <td><%#String.Format("{0:g}",Eval("SubscribeDate"))%></td>
                                        <td>
                                            <asp:ImageButton ImageUrl="/umbraco/newsletterstudio/images/vcard_save.png" ToolTip="Save changes" ID="UpdateButton" CommandName="UpdateSubscriber" CommandArgument='<%#Eval("Id") %>' runat="server" Text="Update"></asp:ImageButton>
                                        </td>
                                    </tr>
                                </EditItemTemplate>
                            </asp:ListView>
                            <div id="test" runat="server"></div>
                            <div style="clear:both;"></div>
                    </div>
                    
                    <!-- **********   Content ends here ******************* -->

                </div>
            </div>
        </div>
    </div>
    <div class="footer">
        <div class="status"><h2></h2></div>
    </div>

</div>

<script type="text/javascript">

    // Set active tab
    var body_TabView1_tabs = new Array("body_TabView1_tab01");
    setActiveTab('body_TabView1', 'body_TabView1_tab01', body_TabView1_tabs);

    // Activating window resize
    jQuery(document).ready(function(){resizeTabView(body_TabView1_tabs, 'body_TabView1'); });
    jQuery(window).resize(function () { resizeTabView(body_TabView1_tabs, 'body_TabView1'); });


    function selectAll(obj) {
        var selected = $(obj).parents("table").find("input:checkbox").not(obj).attr("checked", $(obj).attr("checked"));
    }

    function deleteEmails() {
        var objs = $("#subscribersTable").find("input:checkbox:checked").not($("#selectedAll"));
        if (objs.length > 0) {
            return confirm("Are you sure to delete selected email?");
        }
        else {
            alert("Please select email");
            return false;
        }
    }
</script>


</asp:Content>