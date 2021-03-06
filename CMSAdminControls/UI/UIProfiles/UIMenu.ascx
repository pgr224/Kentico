<%@ Control Language="C#" AutoEventWireup="true" Inherits="CMSAdminControls_UI_UIProfiles_UIMenu" CodeFile="UIMenu.ascx.cs" %>
<%@ Register Src="~/CMSAdminControls/UI/Trees/UniTree.ascx" TagName="UniTree" TagPrefix="cms" %>
<cms:UniTree runat="server" ID="treeElem" ShortID="t" Localize="true" IsLiveSite="false" />

<script type="text/javascript" language="javascript">
    //<![CDATA[
    function SelectNode(elementName) {
        // Set selected item in tree
        $j('span[name=treeNode]').each(function() {
            var jThis = $j(this);
            jThis.removeClass('ContentTreeSelectedItem');
            if (!jThis.hasClass('ContentTreeItem')) {
                jThis.addClass('ContentTreeItem');
            }
            if (this.id == 'node_' + elementName) {
                jThis.addClass('ContentTreeSelectedItem');
            }
        });
    }
    //]]>
</script>

