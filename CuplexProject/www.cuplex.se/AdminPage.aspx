<%@ Page Title="" Language="C#" MasterPageFile="~/Default.master" AutoEventWireup="true" CodeFile="AdminPage.aspx.cs" Inherits="AdminPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link type="text/css" href="css/adminpage.css" rel="Stylesheet" />
    <link type="text/css" href="css/jquery.ui.base.css" rel="Stylesheet" />
    <link type="text/css" href="css/jquery.ui.theme.css" rel="Stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="AdminPagePanel" Visible="false" runat="server">
    <table cellpadding="0" cellspacing="0" class="adminMenuTable">
        <tr>
            <td><asp:Button ID="LinkSuggestionButton" OnClick="LinkSuggestionButton_Clicked" Text="Föreslagna Länkar" runat="server" /></td>
            <td><asp:Button ID="LinkAdministrationButton" OnClick="LinkAdministrationButton_Clicked" Text="Administrera Länkar" runat="server" /></td>
            <td><asp:Button ID="UserAdministrationButton" OnClick="UserAdministrationButton_Clicked" Text="Administrera Användare" runat="server" /></td>
            <td><asp:Button ID="UpdateFileListButton" OnClick="UpdateFileListButton_Clicked" Text="Uppdatera fil-lista" runat="server" /></td>
        </tr>
    </table>
    
    <div id="AdminUpperPanel" class="adminPagePanel adminFilerPanel" runat="server">
        <div><span class="label">Från datum:&nbsp;</span><asp:TextBox ID="FromDateTextBox" CssClass="dateField" runat="server"></asp:TextBox></div>
        <div><span class="label">Till datum:&nbsp;</span><asp:TextBox ID="UntilDateTextBox" CssClass="dateField" runat="server"></asp:TextBox></div>
        <div><asp:Button ID="LoadDataButton" OnClick="LoadDataButton_Clicked" style="margin-left:45px;" Text="Ladda data" runat="server" /></div>
    </div>
    <div class="adminPagePanel">
        <asp:GridView ID="LinkSuggestionList" AutoGenerateColumns="false" AllowPaging="false" DataKeyNames="LinkSuggestionRef" CssClass="linkSuggestionTable" 
            RowStyle-CssClass="linkSuggestionTableRow" AlternatingRowStyle-CssClass="linkSuggestionTableAlternating" runat="server">
            <Columns>
                <asp:BoundField DataField="LinkSuggestionRef" Visible="false" />
                <asp:HyperLinkField DataTextField="Description" HeaderText="Länkbeskrivning" Target="_blank" ControlStyle-CssClass="link" DataNavigateUrlFields="LinkUrl" />
                <asp:BoundField DataField="CategoryName" HeaderText="Kategori" />
                <asp:BoundField DataField="UserName" HeaderText="Användare" />
                <asp:BoundField DataField="LinkSuggestionDate" HeaderText="Datum" />
                <asp:TemplateField ItemStyle-CssClass="LinkSuggestionListCheckBox" HeaderText="Vald">
                    <ItemTemplate>
                        <input id="SuggestionCheckBox" name='SelCheckBox' value='<%#Eval("LinkSuggestionRef")%>' type="checkbox" checked='<%# Convert.ToBoolean(Eval("IsChecked")) %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <asp:GridView ID="LinkList" AutoGenerateColumns="false" AllowPaging="false" DataKeyNames="LinkRef" CssClass="linkSuggestionTable" 
            RowStyle-CssClass="linkSuggestionTableRow" AlternatingRowStyle-CssClass="linkSuggestionTableAlternating" OnRowDataBound="LinkList_OnRowDataBound" runat="server">
            <Columns>
                <asp:BoundField DataField="LinkRef" Visible="false" />
                <asp:BoundField DataField="LinkDate" HeaderText="LinkDate" />
                <asp:BoundField DataField="LinkName" HeaderText="LinkName" />
                <asp:BoundField DataField="CategoryName" HeaderText="Kategori" />
                <asp:BoundField DataField="UserName" HeaderText="Användare" />
                <asp:BoundField DataField="LinkUrl" HeaderText="Url" />
                <asp:BoundField DataField="Clicks" HeaderText="Klick" />
                <asp:BoundField DataField="Rating" HeaderText="Betyg" />
                <asp:TemplateField ItemStyle-CssClass="LinkSuggestionListCheckBox" HeaderText="Vald">
                    <ItemTemplate>
                        <input id="LinkSelectionCheckBox" name='SelCheckBox' value='<%#Eval("LinkRef")%>' type="checkbox" checked='<%# Convert.ToBoolean(Eval("IsChecked")) %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <asp:GridView ID="UserList" AutoGenerateColumns="false" AllowPaging="false" DataKeyNames="UserRef" CssClass="linkSuggestionTable"
            RowStyle-CssClass="linkSuggestionTableRow" AlternatingRowStyle-CssClass="linkSuggestionTableAlternating" runat="server">
            <Columns>
                <asp:BoundField DataField="UserRef" Visible="false" />
                <asp:BoundField DataField="UserName" HeaderText="Användarnamn" />
                <asp:BoundField DataField="EmailAddress" HeaderText="Epost" />
                <asp:BoundField DataField="LastLoginDate" HeaderText="Senast inloggad" />
                <asp:BoundField DataField="Comments" HeaderText="Kommentarer" />
                <asp:BoundField DataField="Ratings" HeaderText="Betygsättningar" />
                <asp:BoundField DataField="PollVotes" HeaderText="Röstningar" />
                <asp:BoundField DataField="IsAdmin" HeaderText="Admin" />                
            </Columns>
        </asp:GridView>
        
        <asp:Panel ID="ContentPanel" CssClass="adminContentPanel" runat="server"></asp:Panel>
    </div>
    
    <div class="adminPagePanel">        
        <asp:Button ID="AddLinkButton" OnClick="AddLinkButton_Clicked" Text="Lägg till" runat="server" />
        <asp:Button ID="EditButton" OnClick="EditButton_Clicked" Text="Editera" runat="server" />
        <asp:Button ID="DeleteButton" OnClick="DeleteButton_Clicked" Text="Ta bort" runat="server" />
        <asp:Button ID="CreateLinksButton" OnClick="CreateLinks_Clicked" Text="Skapa länkar" runat="server" />
        <span>&nbsp</span><span>&nbsp</span>
        <asp:Button ID="SelectAllButton" Text="Markera alla" OnClientClick="SelectAll(); return false;" runat="server" />
    </div>
    
    <asp:Panel ID="EditLinkSuggestionPanel" CssClass="StandardPopup editLinkSuggestionPanel" Visible="false" runat="server">
        <div class="editLinkSuggestionInputPanel">
            <div>
                <label id="Label1" for="LinkDescriptionTextBox" runat="server"><span>Beskrivning</span></label>
                <asp:TextBox ID="LinkSuggestionDescriptionTextBox" CssClass="editLinkTextBox" runat="server"></asp:TextBox>
            </div>
            <div>
                <label id="Label2" for="LinkUrlTextBox" runat="server"><span>Url</span></label>
                <asp:TextBox ID="LinkSuggestionUrlTextBox" CssClass="editLinkTextBox" runat="server"></asp:TextBox>
            </div>
            <div>
                <label id="Label3" for="CategoryDropdownList" runat="server"><span>Kategori</span></label>
                <asp:DropDownList ID="LinkSuggestionCategoryDropdownList" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="editLinkSuggestionButtonPanel">
            <asp:Button ID="EditOkButton" OnClick="EditOkButton_Clicked" Text="Ok" runat="server" />
            <asp:Button ID="EditCancelButton" OnClick="EditCancelButton_Clicked" Text="Avbryt" runat="server" />
        </div>
    </asp:Panel>
    
    <asp:Panel ID="EditLinkPanel" CssClass="StandardPopup editLinkPanel" Visible="false" runat="server">
        <div class="editLinkSuggestionInputPanel">
            <div>
                <label id="Label6" for="LinkDateTextBox" runat="server"><span>Datum</span></label>
                <asp:TextBox ID="LinkDateTextBox" CssClass="editLinkDateTextBox" runat="server"></asp:TextBox>
                <asp:TextBox ID="LinkTimeTextBox" CssClass="editLinkTimeTextBox" runat="server"></asp:TextBox>
            </div>
            <div>
                <label id="Label4" for="LinkNameTextBox" runat="server"><span>Beskrivning</span></label>
                <asp:TextBox ID="LinkNameTextBox" CssClass="editLinkTextBox" runat="server"></asp:TextBox>
            </div>
            <div>
                <label id="Label8" for="EmailAddressTextBox" runat="server"><span>Kategori</span></label>
                <asp:DropDownList ID="LinkCategoryDropdownList" runat="server"></asp:DropDownList>
            </div>
            <div>
                <label id="Label7" for="UserDropDownList" runat="server"><span>Användare</span></label>
                <asp:DropDownList ID="LinkUserDropDownList" runat="server"></asp:DropDownList>
            </div>
            
            <div>
                <label id="Label5" for="LinkUrlTextBox" runat="server"><span>Url</span></label>
                <asp:TextBox ID="LinkUrlTextBox" CssClass="editLinkTextBox" runat="server"></asp:TextBox>
            </div>
            <div>
                <label id="Label9" for="LinkClicksTextBox" runat="server"><span>Klick</span></label>
                <asp:TextBox ID="LinkClicksTextBox" CssClass="editLinkNumberTextBox" runat="server"></asp:TextBox>
            </div>
            <div>
                <label id="Label10" for="LinkRatingTextBox" runat="server"><span>Betyg</span></label>
                <asp:TextBox ID="LinkRatingTextBox" CssClass="editLinkNumberTextBox" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="editLinkSuggestionButtonPanel">
            <asp:Button ID="Button1" OnClick="EditOkButton_Clicked" Text="Ok" runat="server" />
            <asp:Button ID="Button2" OnClick="EditCancelButton_Clicked" Text="Avbryt" runat="server" />
        </div>
    </asp:Panel>
    
    </asp:Panel>
</asp:Content>

