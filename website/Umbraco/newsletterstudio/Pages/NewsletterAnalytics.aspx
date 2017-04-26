<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../../masterpages/umbracoPage.Master" CodeBehind="NewsletterAnalytics.aspx.cs" Inherits="NewsletterStudio.Pages.NewsletterAnalytics" %>


<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register tagprefix="umbEdit" namespace="umbraco.editorControls.tinyMCE3.webcontrol" Assembly="umbraco.editorControls" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ContentPlaceHolderID="head" runat="server">

  <link rel="Stylesheet" type="text/css" href="/umbraco/newsletterstudio/css/style.css" />		   


            <!--Load the AJAX API-->
  <script type="text/javascript" src="https://www.google.com/jsapi"></script>
  <script type="text/javascript">

      // Load the Visualization API and the piechart package.
      google.load('visualization', '1.0', { 'packages': ['corechart'] });

      // Set a callback to run when the Google Visualization API is loaded.
      google.setOnLoadCallback(drawChart);

      // Callback that creates and populates a data table, 
      // instantiates the pie chart, passes in the data and
      // draws it.
      function drawChart() {

          // Create and populate the data table.
          var data = new google.visualization.DataTable();

          data.addColumn('string', 'Event');
          data.addColumn('number', 'Procent')
          data.addRows(<%=this.ChartsData %>);

           new google.visualization.PieChart(document.getElementById('chart_div')).
            
            draw(data, {width: 350, height: 350, chartArea:{left:0, top:0}, legend: 'none',colors:['#9ec920','#ffe242','#d02626']});   
        }


    </script>
   
    

</asp:Content>

<asp:Content ContentPlaceHolderID="body" runat="server">
    
    <umb:TabView ID="TabView1" runat="server" Width="552px" Height="392px">
    </umb:TabView>

     <umb:Pane ID="panChartsOverview" runat="server" Text="">
     <div class="chartsHeader">
        <h1 id="headline" runat="server">[headline]</h1>
        <p>
            <br />
            <asp:Literal ID="liErrors" runat="server" Visible="false" />    
        </p>
        </div>
        
        <table style="float:left; margin-left:20px;">
            <tr>
                <td>
                    <div class="colorbox" style="background:#9ec920;"></div>
                </td>
                <td class="info">
                    <h3><asp:Label ID="lbOpenPercent" runat="server" />% opened the newsletter</h3>
                    <p>
                        <a href="NewsletterAnalyticsDetails.aspx?id=<%=this.NewsletterId %>&status=opened">
                            <asp:Label ID="lbOpenDetails" runat="server" />
                        </a>
                    </p>
                </td>
                <td rowspan="6">
                    <div>
                        <div id="visualization"></div>
                        <div style="width:300px; height:260px; overflow:hidden;" id="chart_div"></div>
                    </div>
                    <div class="clear"></div>
                </td>
                <td rowspan="6" style="padding-left:10px; vertical-align:top;">
                    <p>
                    <strong>Subscribers:</strong> <asp:Label ID="lbTotalRecivers" runat="server" /><br />
                    <strong>Sent:</strong> <asp:Label ID="lbSentDate" runat="server" /><br />
                    <asp:Label ID="lbRecivers" runat="server"/><br />
                    <strong>Subject:</strong> <asp:Label ID="lbSubject" runat="server" /><br />
                    <strong>Sender name:</strong> <asp:Label ID="lbSenderName" runat="server" /><br />
                    <strong>Sender e-mail:</strong> <asp:Label ID="lbSenderEmail" runat="server" /><br />
                    <asp:PlaceHolder runat="server" ID="phContentIdLink" Visible="false"><asp:Literal runat="server" ID="litContentIdLink"></asp:Literal></asp:PlaceHolder>
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="colorbox" style="background:#ffe242;"></div>                    
                </td>
                <td class="info">
                    <h3><asp:Label ID="lbNotopenPercent" runat="server" />% haven't opened</h3>
                    <p>
                        <a href="NewsletterAnalyticsDetails.aspx?id=<%=this.NewsletterId %>&status=notopened">
                        <asp:Label ID="lbNotopenDetails" runat="server" />
                        </a>
                    </p>
                </td>
            </tr>
            <tr>
                <td>
                    <div class="colorbox" style="background:#d02626;"></div>
                </td>
                <td class="info">
                    
                        <h3><asp:Label ID="lbErrorPercent" runat="server" />% delivery failed</h3>
                        <p>
                            <a href="NewsletterAnalyticsDetails.aspx?id=<%=this.NewsletterId %>&status=error"><asp:Literal ID="lbErrorDetails" runat="server" /></a>
                            <asp:Literal ID="lbErrorShowDetails" runat="server" />
                        </p>
                    
                </td>
            </tr>
            <tr>
                <td></td>
                <td class="info">
                        <h3><asp:Label ID="lbClickPercent" runat="server" />% clicked a link</h3>
                        <p><a href="NewsletterAnalyticsDetails.aspx?id=<%=this.NewsletterId %>&status=clicked"><asp:Label ID="lbClickDetails" runat="server" /></a></p>
                </td>
            </tr>
            <tr>
                <td></td>
                <td class="info">
                        <h3><asp:Label ID="lbUnsubscribePercent" runat="server" />% unsubscribed</h3>
                        <p><a href="NewsletterAnalyticsDetails.aspx?id=<%=this.NewsletterId %>&status=unsubscribed"><asp:Label ID="lbUnsubscribeDetails" runat="server" /></a></p>
                </td>
            </tr>
        </table>
        
        
           
    </umb:Pane>

    <umb:Pane ID="panClicks" runat="server" Text="Clicks">
        <asp:Repeater ID="rptClicks" runat="server">
            <HeaderTemplate>
                <table>
                    <thead>
                        <th width="1%">Clicks</th>
                        <th width="1%">Click rate</th>
                        <th width="98%">Url</th>
                    </thead>
            </HeaderTemplate>
            <ItemTemplate>
                    <tr>
                        <td width="1%"><%# Eval("ClickCount")%></td>
                        <td width="1%"><%# Eval("ClickRate")%></td>
                        <td width="98%"><a href='<%#Eval("Url")%>' target="_blank"><%#Eval("Url")%></a></td>
                    </tr>
            </ItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
                    
        </asp:Repeater>

    </umb:Pane>
    <umb:Pane ID="panOpenTimeline" runat="server" Text="Open tracking">
        <p>This chart displays the number of opened newsletters per hour for the first week after sending.</p>
        <iframe src='NewsletterAnalyticsTimeline.aspx?id=<%=NewsletterId%>' width="880" height="400" frameborder="0" style="border:0px; overflow:hidden;"></iframe>
    </umb:Pane>


</asp:Content>