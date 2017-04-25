<%@ Page Language="C#" MasterPageFile="../../masterpages/umbracoPage.Master" AutoEventWireup="true" CodeBehind="NewsletterSendout.aspx.cs" Inherits="NewsletterStudio.Pages.NewsletterSendout" %>

<%@ Register TagPrefix="umb" Namespace="umbraco.uicontrols" Assembly="controls" %>
<%@ Register TagPrefix="umb1" Namespace="umbraco.uicontrols.DatePicker" Assembly="controls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="Stylesheet" type="text/css" href="/umbraco/newsletterstudio/css/style.css" />
    <script src="/umbraco/newsletterstudio/js/ui.dropdownchecklist-1.3.1-min.js" type="text/javascript"></script>

       <script type="text/javascript">
           $(function () {
               $("select").dropdownchecklist({ icon: { placement: 'right', toOpen: 'ui-icon-arrowthick-1-s'
                                            , toClose: 'ui-icon-arrowthick-1-n'}
            								, maxDropHeight: 150, width: 250,
                                            emptyText: "Please select..."
               });
           });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    

<umb:Pane ID="panValidation" runat="server" Visible="false">
    <p>The following errors was detected</p>
    <div id="divValidationErrors" runat="server" />
    <p><a href="#" style="color: blue"  onclick="UmbClientMgr.closeModalWindow()">Close window</a></p>

</umb:Pane>

<umb:Pane ID="panSendout" runat="server" Text="">
    <p>Please select what you want to do</p>
    <umb:PropertyPanel ID="PropertyPanel6" runat="server" Text="">
    
        <table class="form select sendout" cellspacing="0">
        <tr>
            <td class="0" width="30"><p><input type="radio" id="id_send_type_0" value="test" name="send_type" /></p></td>
            <td class="0">
                <p><label for="id_send_type_0"><h3>Send out a test mail</h3></label><br />
                Use this to test the newsletter in a real e-mail client.</p>
                <div id="id_div_0" class="extrainfo" style="display:none; margin-top:15px;">
                    <asp:Label runat="server" AssociatedControlID="txtTestEmails"><strong>Send to this email</strong></asp:Label><br />
                    <asp:TextBox ID="txtTestEmails" runat="server" style="margin-top:5px;width:300px;" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="1" width="30"><p><input type="radio" id="id_send_type_1" value="newsletter" name="send_type" /></p></td>
            <td class="1">
                <p><label for="id_send_type_1"><h3>Send the newsletter now</h3></label><br />
                Sends out the newsletter right away to a selected mailing list.</p>
                <div id="id_div_1" class="extrainfo" style="display:none; margin-top:15px;">
                    <strong>Select mailing list:</strong> <br />
                    <asp:ListBox runat="server" ID="lbSendNowReceivers" SelectionMode="Multiple" />
                    <%-- <asp:DropDownList runat="server" ID="ddlSendNowReceivers" /> --%>
                </div>
            </td>
        </tr>
        <tr class="<%=this.GetClientCode("ScheduledTrClass") %>">
            <td class="2" width="30"><p><input type="radio" id="id_send_type_2" value="scheduled" name="send_type" <%=this.GetClientCode("ScheduleInputChecked") %> /></p></td>
            <td class="2"><p><label for="id_send_type_2">
                <h3>Schedule a mail out</h3></label><br />
                Select recipients and schedule a mail out for a specific time and date.</p>
                <div id="id_div_2" class="extrainfo" style="margin-top:15px; <%=this.GetClientCode("ScheduleStyleForDiv")%>">
                    <div style="float:left;">
                        <strong>Select mailing list:</strong> <br />
                        <%-- <asp:DropDownList runat="server" ID="ddlScheduleReceivers" /> --%>
                        <asp:ListBox runat="server" ID="lbScheduleReceivers" SelectionMode="Multiple" />
                    </div>
                    <div style="float:left; padding-left:15px;">
                        <strong>Send date/time:</strong><br />
                        <umb1:DateTimePicker ID="dpSendOutDate" runat="server" />
                    </div>
                    <div class="clear"></div>
                </div>
            </td>
        </tr>
        <tr style="<%=this.GetClientCode("UnscheduleVisible") %>">
           <td class="3" width="30"><p><input type="radio" id="id_send_type_3" value="unschedule" name="send_type" /></p></td>
            <td class="3"><p><label for="id_send_type_3">
                <h3>Cancel scheduled mail </h3></label><br />
                This will cancel scheduled mail.</p>
            </td>
        </tr>

    </table>
    <br />
    <asp:HiddenField ID="hiddenSelectedAction" Value="" runat="server" />
    <asp:Button ID="btnConfirm" OnClick="btnConfirm_Click" runat="server" OnClientClick="return ValidateForm();" style="width:100px; height:50px;" Text="Confirm" />
    &nbsp; <em>or</em> &nbsp;
   <a href="#" style="color: blue"  onclick="UmbClientMgr.closeModalWindow()">Cancel</a>
    
    </umb:PropertyPanel>
</umb:Pane>


<div id="divServiceStatus" class="divServiceStatus" runat="server"></div>


<script type="text/javascript">

        function ValidateForm() {
            if ($("#<%=hiddenSelectedAction.ClientID%>").val() == '2' && $(".hasDatepicker").val() == '') {
                alert('Choose a date');
                return false;
            }

            if ($("#<%=hiddenSelectedAction.ClientID%>").val() == '1') {

                var confirmed = confirm('This will send the newsletter right away. Are you sure?');
                if (confirmed) {

                    // Hide pan
                    $('.propertypane').hide();
                    $('.divServiceStatus').html('<div class="sendoutAjax"><p>Checking settings</p></div>');

                    return true;
                }
                return false;
            }
        
            return true;
        }

        var currentSelectedClass = '<%=this.hiddenSelectedAction.Value %>';
        $(document).ready(function () {
            $(".select tr > td").click(function (event) {
                 
                $('.select .active-row').removeClass("active-row");
                $(this).parent().addClass("active-row");
                $("input#id_send_type_" + this.className).attr('checked', true);
                if (currentSelectedClass != this.className) {
                    $(".extrainfo").slideUp();
                    $("#id_div_" + this.className).slideDown();
                    $("#<%=hiddenSelectedAction.ClientID%>").val(this.className);
                }
                
                currentSelectedClass = this.className;
                return true;
            });
        });


        function updateAjax() {
            $.ajax({
                type: "POST",
                url: "NewsletterSendout.aspx/TestWeb",

                data: "{'newsletterId':'<%=NewsletterId %>'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    // Replace the div's content with the page method's return.
                    $(".divServiceStatus").html(msg.d);
                }
            });
        }
</script>

</asp:Content>
