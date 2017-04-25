var BvlfMemberSearch = {
    _SetupButtonActions: function() {
        $(".GetDetails").click(function() {
            var id = this.id;
            BvlfMemberSearch.ShowMemberDetails(id);
        });
        $(".ChangeStatus").click(function() {
            var id = this.id;

            $("." + id + "_PaimentButton").html("<img src=\"/Media/progress_s.gif\" />updating...");
            BvlfMemberSearch._SetMembershipPaid(id);
        });
        $(".RemoveMember").click(function() {
            var id = this.id;
            $("." + id + "_PaimentButton").html("<img src=\"/Media/progress_s.gif\" />updating...");
            BvlfMemberSearch._RemoveMember(id);
        });
    },

    _SetupModalWindowButtonActions: function() {
        $("#cmdCancelMemberInfo").click(function() {
            $(".memberDetailsDialog").remove();
        });
        $("#cmdModifyMemberInfo").click(function(e) {
            var postdata = $("#UpdatePersonalInfo").serialize();
            var action = "/Umbraco/surface/MemberShipApi/UpdateProfile/";
            $("#LoadingSave").html("<img src=\"/Media/progress_s.gif\" /> loading...");
            $.ajax({
                url: action,
                type: "post",
                dataType: "json",
                data: postdata
            }).done(function(data) {
                e.preventDefault();
                if (data.msg === "success") {
                    $("." + data.userid + "_FullName").html(data.Fullname);
                    $("." + data.userid + "_School").html(data.school);

                    $(".memberDetailsDialog").dialog("destroy");
                    $(".memberDetailsDialog").remove();
                } else {
                    $("#LoadingSave").html(data.msg);
                }
            });
        });
    },

    _SetMembershipPaid: function(id) {
        var postUrl = "/Umbraco/surface/MemberShipApi/SetMembershipPaid/" + id;
        $.ajax({
            url: postUrl,
            cache: false
        }).done(function() {
            $("." + id + "_Status").html("Betaald");
            $("." + id + "_PaimentButton").html(" ");
            $("#" + id + "_Row").removeClass("Alert");
            $("." + id + "_Date").removeClass("Expire");

            $("#" + id + "_Row").addClass("Normal");
        });
    },

    _RemoveMember: function(id) {
        var postUrl = "/Umbraco/surface/MemberShipApi/SetMembershipToSleep/" + id;
        $.ajax({
            url: postUrl,
            cache: false
        }).done(function() {
            $("." + id + "_Status").html("Verwijderd");
            $("." + id + "_PaimentButton").html(" ");
            $("#" + id + "_Row").removeClass("Alert");
            $("." + id + "_Date").removeClass("Expire");

            $("#" + id + "_Row").addClass("Removed");
        });
    },

    ShowMemberDetails: function(memberid) {
        $("#Loading").html("<span class=\"memberDetailsDialog\"><img src=\"/Media/progress_s.gif\" /> loading...</span>");
        $(".memberDetailsDialog")
            .fadeIn("fast");
        var URL = "/Umbraco/surface/MemberShipApi/GetMemberDetails/" + memberid;
        $.ajax({
            url: URL,
            cache: false
        }).done(function(data) {
            $(".memberDetailsDialog").html(data).dialog({
                modal: true,
                closeText: "X",
                draggable: true,
                resizable: false,
                title: "Detail info",
                width: 730,
                height: 450,
                close: function() {
                    $(".memberDetailsDialog").dialog("destroy");
                    $(".memberDetailsDialog").remove();
                }
            });
        });
    },

    _SetupDefaultSearch: function() {
        var action = "GetAllMembers";
        var searchUrl = "/Umbraco/surface/MemberShipApi/";
        $("#SearchResult").html("<p><img src=\"/Media/progress_s.gif\" />searching... </p>");
        $.ajax({
            url: searchUrl + action + "/",
            cache: false
        }).done(function(data) {
            $("#SearchResult").html(data);
            BvlfMemberSearch._SetupButtonActions();
        });
    },

    SetupSearch: function() {
        // Do something

        BvlfMemberSearch._SetupDefaultSearch();


    }
}