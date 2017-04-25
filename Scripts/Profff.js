var Profff = {
    cfg: {
        ajaxUrl: "/Umbraco/surface/ajax/GetAdditionalMaterial/",
        ajaxOptions: {
            //  dataType: 'json',
            contentType: "application/json; charset=utf-8",
            processData: true,
            cache: false
        },
        dialogwindow: ".tooltipdialog",
        modalWindowOptions: {
            modal: true,
            closeText: "X",
            draggable: true,
            resizable: false,
            width: 730,
            height: 450,
            close: function() {
                $(Profff.cfg.dialogwindow).dialog("destroy");
                $(Profff.cfg.dialogwindow).remove();
            }
        }
    },


    _SetupScripts: function() {
        Profff.GetDetails();
    },

    GetDetails: function() {
        $(".showDetails").click(function(e) {
            e.preventDefault();
            var itemToGet = $(this).attr("data-details");
            Profff.ShowDetails(itemToGet);
        });
    },

    ShowDetails: function(itemtoGet) {
        $("#Loading").html("<span class=\"tooltipdialog\"><img alt=\"\"Bezig met laden\" src=\"/Media/progress_s.gif\" />Bezig met laden...</span>");
        Profff.cfg.ajaxOptions.url = Profff.cfg.ajaxUrl + itemtoGet;
        $.ajax(Profff.cfg.ajaxOptions).done(function(data) {
            $(Profff.cfg.dialogwindow).html(data).dialog(Profff.cfg.modalWindowOptions);

        }).fail(function(jqXhr, textStatus, errorThrow) {
            $("#Loading").html("error : " + jqXhr);
        });
    }
};
jQuery(document).ready(function() {
    Profff._SetupScripts();
});