tinyMCEPopup.requireLangPack();

var newsletterstudioDialog = {
	
    init: function() {
		
        //f = document.forms[0];
        // Get the selected contents as text and place it in the input
        //f.someval.value = tinyMCEPopup.editor.selection.getContent({format : 'text'});
		
    },

    insert: function() {
        // Insert the contents from the input into the document
		var txtCaption = document.getElementById('txtCaption');
		
		// Validate
		if(this.shouldHaveCaption() && txtCaption.value == '')
		{
			alert('Caption is required');
			return false;
		}
		
		var ddlAction = document.getElementById('ddlAction');
		var insertText = '';
		
		switch(ddlAction.value)
		{
		 	case 'Name':
				insertText = '[name]';
				break;
		 	case 'Email':
				insertText = '[email]';
				break;
		 	case 'Unsubscribe':
				insertText = '<a href="/[unsubscribe]" title="' + txtCaption.value + '">' + txtCaption.value + '</a>';
				break;				
		 	case 'ReadOnline':
				insertText = '<a href="/[readonline]" title="' + txtCaption.value + '">' + txtCaption.value + '</a>';
				break;				
		}
		
        tinyMCEPopup.editor.execCommand('mceInsertContent', false, insertText);
        tinyMCEPopup.close();

    },
	 
	ddlValueChanged : function(sender) {
		
		// Set visability on "divCaption", it will only be displayed if the selected action needs a caption.
		document.getElementById('divCaption').style.display = (this.shouldHaveCaption()) ? '' : 'none';
		
	},
	
	shouldHaveCaption : function()
	{

		var ddl = document.getElementById('ddlAction');
		
		return (ddl.value == 'Unsubscribe' || ddl.value == 'ReadOnline');
		
	}
	
	
};




function GetFormatedCode() {
    var strCode = document.forms[0].txtCode.value;

    strCode = strCode.replace(/</gi,"&lt;");
    strCode = strCode.replace(/>/gi, "&gt;");
    //strCode = strCode.replace(/&gt;/gi, ">");
    var strCodeText = '<div id="CodeDiv" dir="ltr"><pre  class="brush: ' + document.forms[0].selctLanguage.value + '">';
    strCodeText += strCode;
    strCodeText += '</pre></div><br/>'    
    return strCodeText;
    //alert("done");
}

tinyMCEPopup.onInit.add(newsletterstudioDialog.init, newsletterstudioDialog);
