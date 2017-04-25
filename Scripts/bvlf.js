eval(function(p, a, c, k, e, d) {
    e = function(c) { return (c < a ? "" : e(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36)) };
    if (!"".replace(/^/, String)) {
        while (c--) {
            d[e(c)] = k[c] || e(c);
        }
        k = [function(e) { return d[e] }];
        e = function() { return "\\w+" };
        c = 1;
    };
    while (c--) {
        if (k[c]) {
            p = p.replace(new RegExp("\\b" + e(c) + "\\b", "g"), k[c]);
        }
    }
    return p;
}("(2($){$.c.f=2(p){p=$.d({g:\"!@#$%^&*()+=[]\\\\\\';,/{}|\\\":<>?~`.- \",4:\"\",9:\"\"},p);7 3.b(2(){5(p.G)p.4+=\"Q\";5(p.w)p.4+=\"n\";s=p.9.z('');x(i=0;i<s.y;i++)5(p.g.h(s[i])!=-1)s[i]=\"\\\\\"+s[i];p.9=s.O('|');6 l=N M(p.9,'E');6 a=p.g+p.4;a=a.H(l,'');$(3).J(2(e){5(!e.r)k=o.q(e.K);L k=o.q(e.r);5(a.h(k)!=-1)e.j();5(e.u&&k=='v')e.j()});$(3).B('D',2(){7 F})})};$.c.I=2(p){6 8=\"n\";8+=8.P();p=$.d({4:8},p);7 3.b(2(){$(3).f(p)})};$.c.t=2(p){6 m=\"A\";p=$.d({4:m},p);7 3.b(2(){$(3).f(p)})}})(C);", 53, 53, "||function|this|nchars|if|var|return|az|allow|ch|each|fn|extend||alphanumeric|ichars|indexOf||preventDefault||reg|nm|abcdefghijklmnopqrstuvwxyz|String||fromCharCode|charCode||alpha|ctrlKey||allcaps|for|length|split|1234567890|bind|jQuery|contextmenu|gi|false|nocaps|replace|numeric|keypress|which|else|RegExp|new|join|toUpperCase|ABCDEFGHIJKLMNOPQRSTUVWXYZ".split("|"), 0, {}));

var bvlf = {
    constraintSessions1: ["1", "2", "3", "4"],
    constraintSessions2: ["1", "2", "3", "4"],
    constraintSessions3: ["1", "2", "3", "4"],
    constraintSessions4: ["1", "2", "3", "4"],
    constraintSessions5: ["2", "3", "4", "5"],
    constraintSessions6: ["6", "7"],
    constraintSessions7: ["6", "7"],
    //constraintSessions8: ["3","5","6","7","8", "9", "10"],
    //constraintSessions9: ["3", "5", "6", "7", "8", "9", "10"],
    //constraintSessions10: ["3", "5", "6", "7", "8", "9", "10"],
    //constraintSessions11: ["11", "12"],
    //constraintSessions12: ["11", "12"],

    _SetUpTabs: function(container) {
        container.tabs({
            select: function(e, ui) {
                if (ui.index === 0 || ui.index === 2) {
                    //    $('#calendar').fullCalendar('destroy');
                } else {
                    $("#calendar").fullCalendar("render");
                }
            }
        });

        $(".ModaliteitenLink").click(function() {
            container.tabs("select", 3); // switch to third tab
            return false;
        });
    },

    _setUpConfirmationMailActions: function(lang) {
        $(".SendConfMail").click(function() {
            var objId = this.id;
            var URL = "/" + lang + "/AJAX/SendExcuseMail/" + objId;
            $.ajax({
                url: URL,
                type: "GET",
                success: function(data) {
                    $("#MailSent_" + objId).html(data);
                },
                error: function(error) {
                    $("#MailSent_" + objId).html(error);
                }
            });
        });
    },
    _getAtelierDetail: function() {
        $("a.AtelierDetail").click(function() {
            var itemToGet = $(this).attr("rel");
            bvlf._getSessionDetail(itemToGet);
        });

    },
    _getSessionDetail: function(sessionnumber) {
        $("#Loading").html("<span class=\"tooltipdialog\"><img alt=\"\"Bezig met laden\" src=\"/Media/progress_s.gif\" />Bezig met laden...</span>");
        $(".tooltipdialog")
            .fadeIn("fast");

        var URL = "/Umbraco/Surface/Studiedag/GetSessionDetails/" + sessionnumber;
        $.ajax({
            url: URL,
            cache: false
        }).done(function(data) {
            $(".tooltipdialog").html(data).dialog({
                modal: true,
                width: 650,
                height: 400,
                closeText: "Sluiten",
                draggable: false,
                title: "Beschrijving van het atelier",
                close: function() {
                    $(".tooltipdialog").dialog("destroy");
                    $(".tooltipdialog").remove();
                }

            });

        });
    },

    _SetupSessionCheckBoxes: function() {
        bvlf.CheckBoxAction("Group1", bvlf.constraintSessions1);
        bvlf.CheckBoxAction("Group2", bvlf.constraintSessions2);
        bvlf.CheckBoxAction("Group3", bvlf.constraintSessions3);
        bvlf.CheckBoxAction("Group4", bvlf.constraintSessions4);
        bvlf.CheckBoxAction("Group5", bvlf.constraintSessions5);
        bvlf.CheckBoxAction("Group6", bvlf.constraintSessions6);
        bvlf.CheckBoxAction("Group7", bvlf.constraintSessions7);
        //bvlf.CheckBoxAction("Group8", bvlf.constraintSessions8);
        //bvlf.CheckBoxAction("Group9", bvlf.constraintSessions9);
        //bvlf.CheckBoxAction("Group10", bvlf.constraintSessions10);
        //bvlf.CheckBoxAction("Group11", bvlf.constraintSessions11);
        //bvlf.CheckBoxAction("Group12", bvlf.constraintSessions12);


        bvlf.CheckHiddenFields();
    },
    CheckBoxAction: function(groupname, constraintList) {
        $("input[type=checkbox]." + groupname).click(function() {
                if (this.checked === true) {
                    $("#Selected" + groupname).val(this.value);
                    // disable other fields
                    $.each(constraintList, function(i, val) {
                        bvlf.SetCBGroupReadonly(".Group" + val);
                    });
                    this.checked = true;
                    $(this).attr("disabled", false);
                } else {
                    $("#Selected" + groupname).val(0);
                    $.each(constraintList, function(i, val) {
                        bvlf.SetCBGroupActive(".Group" + val);
                    });
                }
            })
            .change();
    },

    CheckHiddenFields: function() {
        if ($("#SelectedGroup1").val() !== "0") {
            $.each(bvlf.constraintSessions1, function(i, val) {
                bvlf.SetCBGroupReadonly(".Group" + val);
            });
            bvlf.UpdateCheckbox("1");
        }
        if ($("#SelectedGroup2").val() !== "0") {
            $.each(bvlf.constraintSessions2, function(i, val) {
                bvlf.SetCBGroupReadonly(".Group" + val);
            });
            bvlf.UpdateCheckbox("2");
        }
        if ($("#SelectedGroup3").val() !== "0") {
            $.each(bvlf.constraintSessions3, function(i, val) {
                bvlf.SetCBGroupReadonly(".Group" + val);
            });
            bvlf.UpdateCheckbox("3");
        }
        if ($("#SelectedGroup4").val() !== "0") {
            $.each(bvlf.constraintSessions5, function(i, val) {
                bvlf.SetCBGroupReadonly(".Group" + val);
            });
            bvlf.UpdateCheckbox("4");
        }
        if ($("#SelectedGroup5").val() !== "0") {
            $.each(bvlf.constraintSessions5, function(i, val) {
                bvlf.SetCBGroupReadonly(".Group" + val);
            });
            bvlf.UpdateCheckbox("5");
        }
        if ($("#SelectedGroup6").val() !== "0") {
            $.each(bvlf.constraintSessions6, function(i, val) {
                bvlf.SetCBGroupReadonly(".Group" + val);
            });
            bvlf.UpdateCheckbox("6");
        }
        if ($("#SelectedGroup7").val() !== "0") {
            $.each(bvlf.constraintSessions7, function(i, val) {
                bvlf.SetCBGroupReadonly(".Group" + val);
            });
            bvlf.UpdateCheckbox("7");
        }
        //if ($("#SelectedGroup8").val() !== "0") {
        //    $.each(bvlf.constraintSessions8, function (i, val) {
        //        bvlf.SetCBGroupReadonly(".Group" + val);
        //    });
        //    bvlf.UpdateCheckbox("8");
        //}
        //if ($("#SelectedGroup9").val() !== "0") {
        //    $.each(bvlf.constraintSessions9, function (i, val) {
        //        bvlf.SetCBGroupReadonly(".Group" + val);
        //    });
        //    bvlf.UpdateCheckbox("9");
        //}
        //if ($("#SelectedGroup10").val() !== "0") {
        //    $.each(bvlf.constraintSessions10, function (i, val) {
        //        bvlf.SetCBGroupReadonly(".Group" + val);
        //    });
        //    bvlf.UpdateCheckbox("10");
        //}
        //if ($("#SelectedGroup11").val() !== "0") {
        //    $.each(bvlf.constraintSessions11, function (i, val) {
        //        bvlf.SetCBGroupReadonly(".Group" + val);
        //    });
        //    bvlf.UpdateCheckbox("11");
        //}
        //if ($("#SelectedGroup12").val() !== "0") {
        //    $.each(bvlf.constraintSessions12, function (i, val) {
        //        bvlf.SetCBGroupReadonly(".Group" + val);
        //    });
        //    bvlf.UpdateCheckbox("12");
        //}
    },
    UpdateCheckbox: function(counter) {
        var value = $("#SelectedGroup" + counter).val();
        $("#cbSessionGroup" + counter + "List_" + value).attr("disabled", false);
        $("#cbSessionGroup" + counter + "List_" + value).prop("checked", true);
    },

    SetCBGroupReadonly: function(groupname) {
        $("input[type=checkbox]" + groupname).each(function() {
            this.checked = false;
            $(this).attr("disabled", true);
        });
    },

    SetCBGroupActive: function(groupname) {
        $("input[type=checkbox]" + groupname).each(function() {
            this.checked = false;
            $(this).attr("disabled", false);
        });
    },

    _SetupInputLimitations: function() {
        $(".PhoneNumber").numeric({ allow: "-().+/" });
        $(".DateTime").numeric({ allow: "./:" });
        $(".numericOnly").numeric();
        $(".isreadonly").attr("disabled", "disabled");
    },
    _SetupButtonActions: function() {
        $("#Subscription").submit(function() {
            $("input[type=submit]").click(false);
            $("#PleaseWait").html("<p>Even geduld, uw inschrijving wordt verwerkt</p>");
        });

        $("#cmdStudiedagConfirmStep1").click(function() {
            $("#EvenGeduld").html("<p><img src=\"/Media/progress_s.gif\" /> Even geduld, uw aanvraag wordt verwerkt </p>");
            $("#cmdStudiedagConfirmStep1").attr("disabled", true);
            $("#cmdStudiedagPassStep1").attr("disabled", true);
            var postdata = $("#SubscribeToStudieDagStep1").serialize();
            var action = "/Umbraco/surface/Studiedag/HandleSubscribeTostudiedagStep1/";
            $.ajax({
                url: action,
                type: "post",
                dataType: "json",
                data: postdata
            }).done(function(data) {
                $("#SubscriptionFormContent").html(data.form);
            });
        });
        $("#cmdStudiedagPassStep1").click(function() {
            $("#EvenGeduld").html("<p><img src=\"/Media/progress_s.gif\" /> Even geduld, uw aanvraag wordt verwerkt </p>");
            $("#cmdStudiedagConfirmStep1").attr("disabled", true);
            $("#cmdStudiedagPassStep1").attr("disabled", true);
            var postdata = $("#SubscribeToStudieDagStep1").serialize();
            var action = "/Umbraco/surface/Studiedag/HandleSubscribeTostudiedagStep1Skip/";
            $.ajax({
                url: action,
                type: "post",
                dataType: "json",
                data: postdata
            }).done(function(data) {
                $("#SubscriptionFormContent").html(data.form);
            });
        });

        $("#ShowCalendar").click(function() {
            $("#calendar").fullCalendar({
                render: true,
                weekends: true,
                defaultDate: "2016-03-11",
                defaultView: "agendaDay",
                firstHour: 8,
                allDaySlot: false,
                axisFormat: "HH:mm",
                header: {
                    left: "",
                    center: "title",
                    right: ""
                },
                events: {
                    url: "/Umbraco/surface/Studiedag/GetSessionsForCalendar"

                },
                eventClick: function(event) {
                    if (event.id) {
                        bvlf._getSessionDetail(event.id);
                    }
                }
            });
        });

    },

    _SetupAjaxButtonActions: function() {
        $("#cmdStudiedagConfirmStep2").click(function(e) {
            $("#EvenGeduld").html("<p><img src=\"/Media/progress_s.gif\" /> Even geduld, uw aanvraag wordt verwerkt </p>");
            $("#cmdStudiedagConfirmStep2").attr("disabled", true);
            var postdata = $("#StudiedagSubscribeStep2").serialize();
            var action = "/Umbraco/surface/Studiedag/HandleSubscriptionStudieDagStep2/";
            $.ajax({
                url: action,
                type: "post",
                dataType: "json",
                data: postdata
            }).done(function(data) {
                $("#SubscriptionFormContent").html(data.form);
            });
        });
    },

    _setupDefaultInschrijvingenSearch: function() {
        var action = "GetAllInschrijvingen";
        var searchUrl = "/Umbraco/surface/Studiedag/";
        $("#SearchResult").html("<p><img src=\"/Media/progress_s.gif\" />searching... </p>");
        $.ajax({
            url: searchUrl + action + "/",
            cache: false
        }).done(function(data) {
            $("#SearchResult").html(data);
            bvlf._setupInschrijvingenlistButtonActions();
        });
    },
    _setupInschrijvingenlistButtonActions: function() {
        $(".ConfirmPayment").click(function() {
            var id = this.id;
            var postData = {
                subscriptionid: id
            };
            var action = "/Umbraco/surface/Studiedag/ConfirmStudiedagPayment/";
            $.ajax({
                url: action,
                cache: false,
                data: postData
            }).done(function() {
                $("#status_" + id).html("Betaald");
                $(this).hide();
            });
            $(this).hide();
        });

        $(".CancelSubscription").click(function() {
            //set variables
            var id = this.id;
            var postData = {
                subscriptionid: id
            };
            var action = "/Umbraco/surface/Studiedag/CancelSubscription/";
            var htmlToWrite = "<span style='text-align: center'>U staat op het punt deze inschrijving<br /> voor de studiedag te verwijdeen. <br /><br />Deze actie kan niet ongedaan gemaakt worden. <br /> Wenst u verder te gaan?<span>";

            // Modal Confirm window
            $("#Loading").html(htmlToWrite).dialog({
                modal: true,
                autoOpen: true,
                height: 150,
                width: 350,
                open: function() { $(".ui-dialog-titlebar-close").hide(); },
                close: function() {
                    $(".deletebutton").removeAttr("disabled");
                    $(".cancelbutton").removeAttr("disabled");
                },
                buttons: {
                    "Annuleren": function() {
                        $(this).dialog("close");
                        $(".deletebutton").removeAttr("disabled");
                        $(".cancelbutton").removeAttr("disabled");
                    },
                    "Verder gaan": function() {
                        $(this).dialog("close");
                        $.ajax({
                            url: action,
                            cache: false,
                            data: postData

                        }).done(function() {
                            alert("Deze inschrijving werd verwijderd");
                            bvlf._setupDefaultInschrijvingenSearch();


                        });
                    }
                }
            });

        });
    },

    SetupSiteScripts: function(lang) {
        bvlf._SetupInputLimitations();
        bvlf._SetUpTabs($("#Tabs"), 1, lang);
        bvlf._SetupSessionCheckBoxes();
        bvlf._setUpConfirmationMailActions(lang);
        bvlf._getAtelierDetail();
        bvlf._SetupButtonActions();
        ///  bvlf._SetupCalendar();

    },
    SetupAjaxFunctionalities: function() {
        bvlf._SetupSessionCheckBoxes();
        bvlf._SetupAjaxButtonActions();
    },
    SetupInschrijvingenSearch: function() {
        bvlf._setupDefaultInschrijvingenSearch();
    }
};