<%@ Control Language="C#" AutoEventWireup="true" Inherits="domains_management" Codebehind="domains.ascx.cs" %>
<table class="wm_hide" id="ps_container">
	<tr>
		<td class="wm_inbox_page_switcher_left"></td>
		<td class="wm_inbox_page_switcher_pages" id="ps_pages"></td>
		<td class="wm_inbox_page_switcher_right"></td>
	</tr>
</table>

<input id="HFAction" type="hidden" name="HFAction" runat="server"/>
<input id="HFRequest" type="hidden" name="HFRequest" runat="server"/>
<input id="HFValue" type="hidden" name="HFValue" runat="server"/>
<input id="HFPageInfo" type="hidden" name="HFPageInfo" runat="server"/>
<input id="HFPageSize" type="hidden" name="HFPageSize" runat="server"/>
<input id="HFMaxSize" type="hidden" name="HFMaxSize" runat="server"/>
<input id="HFUID" type="hidden" name="HFUID" runat="server"/>

<div style="display:none">
	<asp:LinkButton ID="PostBackButton" runat="server">PostBackButton</asp:LinkButton>
	<asp:LinkButton ID="PagerButton" runat="server">PagerButton</asp:LinkButton>
</div>

<table class="wm_toolbar" id="toolbar">
<asp:PlaceHolder id="PlaceHolderToolbar" Runat="server"></asp:PlaceHolder>
</table>

<div class="wm_contacts" id="main_contacts">
	<div id="contacts" class="wm_contacts_list">
		<div id="contact_list_div" class="wm_contact_list_div">
		
	        <div class="wm_contact_list_div" style="border-right:0px;border-bottom:0px;padding:8px;">
			    <asp:PlaceHolder ID="Search" Runat="server">
				    <div class="wm_toolbar_search_item" id="search_control">
    			        <span>Search:</span>
					    <input type="text" id="searchdesc" name="searchdesc" class="wm_search_input" value="<%=SearchCondition%>" onkeydown="javascript:if(event.keyCode == 13){ DoSearch(this.value); return false; }" /><span style="background-position: -560px 0px;" class="wm_search_icon_standard" onclick="javascript:DoSearch(document.getElementById('searchdesc').value)">&nbsp;</span>
				    </div>
			    </asp:PlaceHolder>		        
       	        <br />
       	        <br />
	        </div>
	        <div class="wm_contact_list_div" style="border-right:0px;">
       	        <asp:Literal id="LiteralSearchOn" runat="server"></asp:Literal>
	        </div>
            <div style="width: 100%; border-top: 1px solid rgb(195, 195, 195);">
			    <table width="100%" id="list" border="0">
			        <tbody>
		            <tr id="contact_list_headers" class="wm_inbox_headers">
			            <td style="overflow: hidden; text-align: center; padding-top: 0pt; padding-left: 2px; padding-right: 2px;" width="19">
		                    <input type="checkbox" id="allcheck" onclick="Selection.CheckAllBox(this);" />	
			            </td>
			            <td class="wm_inbox_headers_separate_noresize" width="1"></td>
			            <td id="type" class="wm_inbox_headers_from_subject wm_control" width="42" onclick="javascript:Sort(this);">
                            Type<%=si_Type%>
			            </td>
			            <td class="wm_inbox_headers_separate_noresize" width="1"></td>
			            <td id="name" class="wm_inbox_headers_from_subject wm_control" onclick="javascript:Sort(this);">
                            Name<%=si_Name%>
		                </td>
		            </tr>        
			        <asp:Literal id="LiteralContactsGroups" runat="server"></asp:Literal>
			        </tbody>
			    </table>
	        </div>
		</div>
	</div>
	<div class="wm_contacts_view_edit" id="contacts_viewer">
	    <asp:Literal id="LiteralContactsViewer_1" runat="server"></asp:Literal>
			<div>
                <asp:PlaceHolder id="PlaceHolderDomain" Runat="server"></asp:PlaceHolder>&nbsp;
			</div>	    
	    <asp:Literal id="LiteralContactsViewer_2" runat="server"></asp:Literal>
	</div>
</div>
<div id="lowtoolbar" class="wm_lowtoolbar">
    <span class="wm_lowtoolbar_messages"></span>
</div>

<script src="js/resizer.js" type="text/javascript"></script>
<script src="js/pageswitcher.js" type="text/javascript"></script>
<script src="js/selection.js" type="text/javascript"></script>

<script type="text/javascript">
    var ID_PREFIX = "<%=ClientPrefix%>_";

    function Sort(obj)
	{
		HFAction = $("HFAction");
		HFRequest = $("HFRequest");
		HFValue = $("HFValue");
		
		HFAction.value = "";
		HFRequest.value = "";
		HFValue.value = "";

		var sort_field = null;
		var error = false;

		switch(obj.id)
		{
			case "type":
				sort_field = "type";
				break;
			case "name":
				sort_field = "name";
				break;
			default:
				error = true;
				break;
		}

		if(!error)
		{
			HFAction.value = "sort";
			HFRequest.value = "domains";
			HFValue.value = sort_field;

			__doPostBack('PostBackButton','');
		}
	}

	function ViewAddressRecord(lineid)
	{
		HFAction = $("HFAction");
		HFRequest = $("HFRequest");
		HFValue = $("HFValue");
    	uid = $("HFUID").value;
        
		HFAction.value = "get";
    	HFRequest.value = "domains";
		HFValue.value = lineid;
		
		if (uid != HFValue.value)
    	    document.location = "default.aspx?plugin=<%=pluginID%>&mode=edit&uid="+lineid;
	}
			
	function NewDomain(type)
	{
	    document.location = "default.aspx?plugin=<%=pluginID%>&mode=new&type=" + type;
	}

	function DoDelete()
	{
		HFAction = $("HFAction");
		HFRequest = $("HFRequest");
		HFValue = $("HFValue");

		var arr = Selection.GetCheckedLines();
		var temp = "";
		var cg = "";

		if(arr.length > 0)
		{
			confirm_str = "Are you sure to delete these domains?";
			if (arr.length == 1)
    			confirm_str = "Are you sure to delete this domain?";
			
			if(confirm(confirm_str))
			{
				HFAction.value = "";
				HFRequest.value = "";
				HFValue.value = "";

				for(i = 0; arr.length > i; i++)
				{
					if(arr.length != i+1)
					{
						cg += arr[i] + ",";
					}
					else
					{
						cg += arr[i];
					}
				}

				HFAction.value = "delete";
				HFRequest.value = "domain";
				HFValue.value = cg;
				__doPostBack('PostBackButton','');
			}
		}
		else
		{
			alert("No domain(s) selected.");
		}
	}

	function DoSearch(text)
	{
		HFAction = $("HFAction");
		HFRequest = $("HFRequest");
		HFValue = $("HFValue");

		HFAction.value = "search";
		HFRequest.value = "domain";
		HFValue.value = text;
		__doPostBack('PostBackButton','');
	}

	function Pager(id)
	{
		HFPageInfo = $("HFPageInfo");
		HFAction = $("HFAction");

		HFPageInfo.value = "";
		HFAction.value = "page";

		HFPageInfo.value = id;
		
		__doPostBack('PagerButton','');
	}
				
    Browser = new CBrowser();
    PageSwitcher = new CPageSwitcher('');
	PageSwitcher.Build();
	
	var uid = $("HFUID").value;

	var page = $("HFPageInfo").value;
	var pageSaze = $("HFPageSize").value;
	var maxSize = "<%=_domainsCount%>";
	PageSwitcher.Show(page, pageSaze, maxSize, "Pager('", "');");    
    List = new CList();
    var Selection;
    Init_list("list");
    Selection.CheckLine(uid);
    ResizeElements("all");    
</script>