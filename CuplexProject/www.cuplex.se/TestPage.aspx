<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="TestPage.aspx.cs" Inherits="TestPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<script type="text/javascript">
    function testCallback(result) {
        alert(result);
    }
</script>

<asp:Panel ID="ContainerPanel" runat="server">

    <asp:Button ID="TransferButton" Text="Transfer to loginpage" OnClick="TransferButton_Clicked" runat="server" />
    
    <asp:Panel ID="StarVotingPanel" runat="server">    
    </asp:Panel>
    
    <asp:Panel ID="TestPanel" runat="server">
        <asp:Label ID="TestLabel" runat="server"></asp:Label><br />
        <object width="400" height="225"><param name="allowfullscreen" value="true" /><param name="allowscriptaccess" value="always" /><param name="movie" value="http://vimeo.com/moogaloop.swf?clip_id=10692284&amp;server=vimeo.com&amp;show_title=1&amp;show_byline=1&amp;show_portrait=0&amp;color=&amp;fullscreen=1" /><embed src="http://vimeo.com/moogaloop.swf?clip_id=10692284&amp;server=vimeo.com&amp;show_title=1&amp;show_byline=1&amp;show_portrait=0&amp;color=&amp;fullscreen=1" type="application/x-shockwave-flash" allowfullscreen="true" allowscriptaccess="always" width="400" height="225"></embed></object><p><a href="http://vimeo.com/10692284">ENVISION : Step into the sensory box</a> from <a href="http://vimeo.com/user606055">SUPERBIEN</a> on <a href="http://vimeo.com">Vimeo</a>.</p>
    </asp:Panel>
    <asp:TextBox ID="LookupIpTextBox" runat="server"></asp:TextBox>
    <asp:Button ID="LookupIpButton" Text="Lookup ip" OnClick="LookupIpButton_Clicked" runat="server" />
    <asp:Label ID="IpLookupResultLabel" runat="server"></asp:Label>
    
    
    <asp:Button ID="TestButton" Text="Testa funktion" OnClientClick="CuplexService.HelloWorld(testCallback);return false;" runat="server" />
</asp:Panel>
</asp:Content>