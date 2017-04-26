

// hover function for tr's
$(document).ready(function () {
    $('table.data tr').hover(function () { $(this).addClass('hovered'); $(this).find('div.editcommands').show(); }, function () { $(this).removeClass('hovered'); $(this).find('div.editcommands').hide(); });
});


function NewNewsletterFromCurrent() {


    if (UmbClientMgr.mainTree().getActionNode().nodeId != '-1' && UmbClientMgr.mainTree().getActionNode().nodeType != '') {

        UmbClientMgr.openModalWindow('create.aspx?nodeId=' + UmbClientMgr.mainTree().getActionNode().nodeId + '&nodeType=newsletterstudio_newsletters&nodeName=Newsletters&rnd=52.7&rndo=53.8', 'Copy to new Newsletter', true, 420, 270);

    }
    else 
    {
        alert('No Newsletter selected');
    }

}


function ToggleExpand(node) {
    if (!$(node).hasClass('open')) {

        // Load nodes
        UmbClientMgr.mainTree()._loadChildNodes(node);
    }
    else 
    {
        // close node
        UmbClientMgr.mainTree()._tree.close_branch(node);
    }
}

function ExpandNewslettersOnce() {
    var node = $('#rootNewsletters');
    if ($(node).hasClass('closed'))
    {
        UmbClientMgr.mainTree()._loadChildNodes(node);
    }
    javascript: UmbClientMgr.appActions().openDashboard('newsletterstudio');

}