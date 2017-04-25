tinyMCEPopup.requireLangPack();

var baseurl, plugin_url;

var ExampleDialog = {
    init: function () {


        plugin_url = tinyMCEPopup.getWindowArg('plugin_url');
        baseurl = tinyMCEPopup.getWindowArg('baseurl');

        var f = document.forms[0];

        // Get the selected contents as text and place it in the input
        var test = tinyMCEPopup.editor.selection.getNode();

        if (test != null && test.attributes["data-url"] != null) {
            // has data.
            f.txtUrl.value = test.attributes["data-url"].value;
            f.insert.value = 'Ok';

        }
        else {
            // Loding without an selected element

        }

    },

    insert: function () {
        var f = document.forms[0];

        // Check the value
        var value = f.txtUrl.value;
        var strImageUrl = plugin_url + '/img/trans.gif';

        // Insert the contents from the input into the document
        var tag = '<img src="' + strImageUrl + '" class="mceNewsletterStudioUrlContent mceItemNoResize" data-url="' + value + '" />'
        
        tinyMCEPopup.editor.execCommand('mceInsertContent', false, tag);
        tinyMCEPopup.close();
    }
};

tinyMCEPopup.onInit.add(ExampleDialog.init, ExampleDialog);
