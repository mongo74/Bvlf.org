<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.ascx.cs" Inherits="NewsletterStudio.Usercontrols.Dashboard" %>


<link href="/umbraco/newsletterstudio/css/style.css" rel="stylesheet" type="text/css" />

<div class="dashboardWrapper">
    <h2 id="header" runat="server">Newsletter Studio</h2>
    <img src="/umbraco/newsletterstudio/images/newsletter_32x32.png" alt="Newsletter Studio" class="dashboardIcon" />
    
    <a href="#" title="Click here to create a newsletter" onclick="javascript:UmbClientMgr.openModalWindow('/umbraco/create.aspx?nodeId=rootNewsletters&nodeType=newsletterstudio_newsletters&nodeName=Newsletters&rnd=81.4&rndo=82.7', 'Create', true, 420, 270);" class="btn">Create new newsletter</a>
    <br /><br />


    <div class="dashboardColWrapper" runat="server" id="divShowProgress">
    <div class="dashboardCols">
        <div class="dashboardCol">
            <h3>In progress</h3>
            <asp:ListView ID="lwInProgress" runat="server" EnableViewState="false">
                <LayoutTemplate>
                    <table class="dashboardTable" cellpadding="0" cellspacing="0">
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </table>
                </LayoutTemplate>
                    
                <ItemTemplate>
                    <tr>
                        <td class="col1">
                            <a href="/umbraco/NewsletterStudio/Pages/Newsletter.aspx?id=<%#Eval("Id") %>">
                                    <%#Eval("Name") %>
                            </a>
                        </td>
                        <td class="col2" title="Created at"><%#Eval("CreatedDate", "{0:g}")%></td>
                        <td class="col3" title="Scheduled send out">
                            <img src="/umbraco/newsletterstudio/images/ajax.gif" width="10" title="This newsletter is being sent right now. Open it to show the progress."/>
                            
                        </td>
                        <td class="col4">
                            <a href="/umbraco/NewsletterStudio/Pages/Newsletter.aspx?id=<%#Eval("Id") %>&action=sendnow">
                                <img src="/umbraco/newsletterstudio/images/email-send.png" title="Send newsletter" />
                            </a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>

        </div>
    </div>
    <br /><br />
    </div>
    

    <div class="dashboardColWrapper" runat="server" id="divShowDraft">
    <div class="dashboardCols">
        <div class="dashboardCol">
            <h3>Drafts</h3>
                
            <asp:ListView ID="rptUnsent" runat="server" EnableViewState="false">
                <LayoutTemplate>
                    <table class="dashboardTable" cellpadding="0" cellspacing="0">
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </table>
                </LayoutTemplate>
                    
                <ItemTemplate>
                    <tr>
                        <td class="col1">
                            <a href="/umbraco/NewsletterStudio/Pages/Newsletter.aspx?id=<%#Eval("Id") %>">
                                    <%#Eval("Name") %>
                            </a>
                        </td>
                        <td class="col2" title="Created at"><%#Eval("CreatedDate", "{0:g}")%></td>
                        <td class="col3" title="Scheduled send out">
                            <%#(Eval("ScheduledSendDate") != null && !String.IsNullOrEmpty(Eval("ScheduledSendDate").ToString())) ? "<img src=\"/umbraco/newsletterstudio/images/scheduled-clock-10x10.png\" title=\"This newsletter is scheduled to be sent automatically\"/>" : ""%>
                            <%#Eval("ScheduledSendDate", "{0:g}") %>
                        </td>
                        <td class="col4">
                            <a href="/umbraco/NewsletterStudio/Pages/Newsletter.aspx?id=<%#Eval("Id") %>&action=sendnow">
                                <img src="/umbraco/newsletterstudio/images/email-send.png" title="Options for sending newsletter" />
                            </a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    <br /><br />
    </div>
    
    <div class="dashboardColWrapper" runat="server" id="divShowSent">
    <div class="dashboardCols">
        <div class="dashboardCol">
        
                <h3>Sent</h3>
                <asp:Repeater ID="rptLatestSent" runat="server">
                <HeaderTemplate>    
                    <table class="dashboardTable" cellpadding="0" cellspacing="0">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td class="col1">
                            <a href="/umbraco/NewsletterStudio/Pages/NewsletterAnalytics.aspx?id=<%#Eval("Id") %>">
                                    <%#Eval("Name") %>
                            </a>
                        </td>
                        <td class="col2" title="Sent date"><%#Eval("SentDate", "{0:g}")%></td>
                        <td class="col3" title="Percent of opened newsletters">
                            <div class="stasbar">
                                <div class="stasbar-fill" style='width:<%#this.GetOpenPercent(Convert.ToInt32(Eval("Id")))%>px'></div>
                                <div class="stasbar-text">
                                    <%#this.GetOpenPercent(Convert.ToInt32(Eval("Id")))%>%
                                </div>
                            </div>
                             
                        </td>
                        <td class="col4">
                            <a href="/umbraco/NewsletterStudio/Pages/NewsletterAnalytics.aspx?id=<%#Eval("Id") %>">
                                <img src="/umbraco/newsletterstudio/images/chart_pie.png" title="View newsletter analytics" />
                            </a>
                        </td>
                    </tr>

                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div>
    </div>

    <div class="dashboardColWrapper" runat="server" id="divFirstTime" visible="false">
        <iframe src="<%=this.GetIframeDashboardNewInstallUrl() %>" width="750" height="480" marginwidth="0" marginheight="0" align="top" scrolling="no" frameborder="0" hspace="0" vspace="0" border="0"></iframe>
    </div>
    <br /><br />
    <div class="dashboardColWrapper" runat="server" id="divTrial" runat="server" visible="false">
    <div class="dashboardCols">
        <div class="dashboardCol">
            
            <iframe src="<%=this.GetIframeUpgradeUrl() %>" width="650" height="480" ALLOWTRANSPARENCY="true" marginwidth="0" marginheight="0" align="top" scrolling="no" frameborder="0" hspace="0" vspace="0" border="0"></iframe>

        </div>
    
    </div>
        
    </div>
</div>
