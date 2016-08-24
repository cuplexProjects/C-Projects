//-------------------------------------------------------------------------------------------------------------------------------------------
function saveTabState(tabNum)
{
    try
    {
        document.getElementById("<%=TAB_NUM_ID").value = tabNum;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 111');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function NewUserAddressIsEmpty()
{
    try
    {
        var obj = document.getElementById('<%=NewUserAddressID.ClientID%>');
        if( obj.value.trim() == '' ) 
            return true;
        else
        {
            if(!IsEmailAddress(obj, obj.value.trim())) return true;
        }
        return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 112');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function UserIsSelectedInList()
{
    try
    {
        if( document.getElementById('<%=ListMembersDDL.ClientID%>').selectedIndex >= 0 ) 
            return true;
        else
            return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 113');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function DeleteThisUser()
{
    try
    {
        if(!DeleteThisUserAlert())
            return false;
        else
            return true;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 114');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function PasswordIsNotEmpty()
{
    try
    {
        var pass = document.getElementById("<%=UserPassword_PassMode");

        if(pass.className == "wm_input")
        {
            if( pass.value.trim() != '') 
                return true;
            else
                return false;
        }
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 115');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function AccountIsNotEmpty()
{
    try
    {
        var obj = document.getElementById('<%=AccountAliasID.ClientID%>');
        if(obj.value.trim() != '')
        {
            if(!IsEmailAddress(obj, obj.value.trim())) 
            {
                return false;
            }
            return true;
        }
        else
            return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 116');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function AliasIsSelectedInList()
{
    try
    {
        if( document.getElementById('<%=AliasesListDDL.ClientID%>').selectedIndex >= 0 ) 
            return true;
        else
            return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 117');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function RealNameChecking()
{
    try
    {
       if(!RealNameIsOk(document.getElementById('<%=RealNameID.ClientID%>'))) 
       {
           Tip.Show("Incorrect character for a Real Name", document.getElementById('<%=RealNameID.ClientID%>'), '');
       }
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 118');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function HomePageChecking()
{
    try
    {
       if(!HomePageIsOk(document.getElementById('<%=HomePageID.ClientID%>'))) 
       {
           Tip.Show("Some characters entered are not allowed in URL", document.getElementById('<%=HomePageID.ClientID%>'), '');
       }
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 119');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function AllFieldsIsNotEmpty()
{
    try
    {
       var realNameObj = document.getElementById('<%=RealNameID.ClientID%>');
       var homePageObj = document.getElementById('<%=HomePageID.ClientID%>');
       var mailBoxSizeObj = document.getElementById('<%=MaxMailboxSize.ClientID%>');
        
       realNameObj.value    = realNameObj.value.trim().replace(/[|\][/{}@""]/gi, "|");
       homePageObj.value    = homePageObj.value.trim().replace(/[^A-Za-z0-9:/.?%_~@$&#!=-]/gi, '');
       mailBoxSizeObj.value = mailBoxSizeObj.value.trim().replace(/[^0-9]/gi, '');

       if(document.getElementById('<%=RealNameID.ClientID%>').value.trim() != "")
       {
           if(!RealNameIsOk(document.getElementById('<%=RealNameID.ClientID%>'))) 
           {
               Tip.Show("Incorrect character for a Real Name", document.getElementById('<%=RealNameID.ClientID%>'), '');
               return false;
           }
       }
       //------------------------------------------------------------
       if(document.getElementById('<%=HomePageID.ClientID%>').value.trim() != "")
       {
           if(!HomePageIsOk(document.getElementById('<%=HomePageID.ClientID%>'))) 
           {
               Tip.Show("Some characters entered are not allowed in URL", document.getElementById('<%=HomePageID.ClientID%>'), '');
               return false;
           }
       }
       //------------------------------------------------------------
       if(document.getElementById('<%=MaxMailboxSize.ClientID%>').value.trim() == "") 
       {
           Tip.Show("Max Mailbox Size should not be empty", document.getElementById('<%=MaxMailboxSize.ClientID%>'), '');
           return false;
       }
       return true;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 120');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function RedirectionsIsNotEmpty()
{
    try
    {
        var obj = document.getElementById('<%=RedirectionID.ClientID%>');
        if(obj.value.trim() != '')
        {
            if(!IsEmailAddress(obj, obj.value.trim())) return false;
            return true;
        }
        else
        {
            return false;
        }
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 121');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function RedirectionIsSelectedInList()
{
    try
    {
        if( document.getElementById('<%=RedirectionsListDDL.ClientID%>').selectedIndex >= 0 ) 
            return true;
        else
            return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 122');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function LRedirectionsIsNotEmpty()
{
    try
    {
        var obj = document.getElementById('<%=L_RedirectionID.ClientID%>');
        if(obj.value.trim() != '')
        {
            if(!IsEmailAddress(obj, obj.value.trim())) return false;
            return true;
        }
        else
            return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 123');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function LRedirectionIsSelectedInList()
{
    try
    {
        if( document.getElementById('<%=L_RedirectionsListDDL.ClientID%>').selectedIndex >= 0 ) 
            return true;
        else
            return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 124');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function AllExternalFieldsForPOP3IsNotEmpty()
{
    try
    {
       if(!DomainNameIsOk(document.getElementById('<%=ExternalPOP3ServerID.ClientID%>').value.trim())) 
       {
           Tip.Show("Your Domain name or IP address is incorrect", document.getElementById('<%=ExternalPOP3ServerID.ClientID%>'), '');
           return false;
       }
       //------------------------------------------------------------
       if(!IsUserNameOk(document.getElementById('<%=ExternalUserName.ClientID%>'))) 
       {
           Tip.Show("Incorrect character for an External Username", document.getElementById('<%=ExternalUserName.ClientID%>'), '');
           return false;
       }
       //------------------------------------------------------------
       if(document.getElementById('<%=ExternalPOP3ServerID.ClientID%>').value.trim() == "") 
       {
           Tip.Show("External POP3 Server should not be empty", document.getElementById('<%=ExternalPOP3ServerID.ClientID%>'), '');
           return false;
       }
       //------------------------------------------------------------
       if(document.getElementById('<%=ExternalUserName.ClientID%>').value.trim() == "") 
       {
           Tip.Show("External Username should not be empty", document.getElementById('<%=ExternalUserName.ClientID%>'), '');
           return false;
       }
       //------------------------------------------------------------
       if(document.getElementById('<%=ExternalPasswordID.ClientID%>').value.trim() == "") 
       {
           Tip.Show("External password should not be empty", document.getElementById('<%=ExternalPasswordID.ClientID%>'), '');
           return false;
       }
       return true;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 125');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function POP3ExternalLinkIsSelectedInList()
{
    try
    {
        if( document.getElementById('<%=POP3_ExternalLinksListDDL.ClientID%>').selectedIndex >= 0 ) 
            return true;
        else
            return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 126');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function AdvancedFieldIsNotEmpty()
{
    try
    {
        if(document.getElementById('<%=AdvancedID.ClientID%>').value.trim() != '')
            return true;
        else
            return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 127');
    }
}
//-------------------------------------------------------------------------------------------------------------------------------------------
function RealNameIsOk(obj)
{
    try
    {
        var temp_value;
        
        if (obj.value.trim() != '') 
        {
            temp_value = obj.value.trim().replace(/[|\][/{}@""]/gi, "|");
            if (temp_value != obj.value.trim())
            {
                return false;
            }
        }
        return true;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 128');
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function HomePageIsOk(obj)
{
    try
    {
        var temp_value;
        
        if (obj.value.trim() != '') 
        {                                           
            temp_value = obj.value.trim().replace(/[^A-Za-z0-9:/.?%_~@$&#!=-]/gi, "");
            if (temp_value != obj.value.trim())
            {
                return false;
            }
        }
        return true;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 129');
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function AddNewUserShow()
{
    try
    {
        SetDomainFormActive("ADD_NEW_USER");
        HideAllControls();
        document.getElementById("domainID_DomainControlID").style.display = "block";
        ShowAddNewUserAccount();
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 130');
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------

function UserAlreadyExists(userName)
{
    try
    {
        var i = 0;
        for(i = 0; i < GlobalInfo.domains[CurrentDomainIndex].users.length; i++)
        {
            if(GlobalInfo.domains[CurrentDomainIndex].users[i].name == userName)
            {
                Tip.Show('User/Account with the same name already exists. Try to choose another username', document.getElementById('domainID_textUserName'), '');                 
                return true;
            }
        }
        return false;
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 131');
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
function ExternalUsersChecking(obj)
{
    try
    {
        var temp_value;

        document.getElementById('<%=AddExternalLinkButtonID.ClientID%>').disabled = false;
        if (obj.value.trim() != '') 
        {
            temp_value = obj.value.trim().replace(/[|\][/{}@""]/gi, "|");
            if (temp_value != obj.value.trim())
            {
                document.getElementById('<%=AddExternalLinkButtonID.ClientID%>').disabled = true;
                Tip.Show("Incorrect character for an External Username", obj, '');
            }
        }
    }
    catch(errorMsg)
    {
        ErrorMsg.Show(ErrorPrefix + ' 132');
    }
}
//----------------------------------------------------------------------------------------------------------------------------------------------
