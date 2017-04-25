<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsletterAnalyticsTimeline.aspx.cs" Inherits="NewsletterStudio.Pages.NewsletterAnalyticsTimeline" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">


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

          var data2 = new google.visualization.DataTable();
          data2.addColumn('string', 'Hour');
          data2.addColumn('number', 'Opens');
          //data.addColumn('number', 'Expenses');
          data2.addRows([
          <%=ChartData %>
          ]);

          // Create and draw the visualization.
          new google.visualization.AreaChart(document.getElementById('visualization'))
            .draw(data2, {
                title: 'Opens first week after send out',
                isStacked: true,
                width: 880,
                height: 250,
                chartArea:{left:60, top:30},
                vAxis: { title: ''},
                hAxis: { title: ''}
            });
      }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="visualization">
        <img src="../images/ajax.gif" />
    </div>
    </form>
</body>
</html>
