using System;

namespace WebMail
{
    public enum SupportedDatabase
    {
        MsSqlServer = 1,
        MsAccess = 2,
        MySql = 3,
        PostgreSql = 4
    }

	public enum IncomingMailProtocol
	{
		Pop3 = 0,
		Imap4 = 1,
		WMServer = 2
	}

    public enum TimeFormats
    {
        F24 = 0,
        F12 = 1
    }

	public enum DefaultOrder
	{
		DateDesc = 0,
		Date = 1,
		FromDesc = 2,
		From = 3,
		ToDesc = 4,
		To = 5,
		SizeDesc = 6,
		Size = 7,
		SubjDesc = 8,
		Subj = 9,
		AttachmentDesc = 10,
        Attachment = 11,
		FlagDesc = 12,
		Flag = 13
	}

    public enum AddressBookStorageType
    {
        DataBase = 0
    }
    
    [Flags]
	public enum MailMode
	{
		DeleteMessagesFromServer = 0,
		LeaveMessagesOnServer = 1,
		KeepMessagesOnServer = 2,
        DeleteMessageWhenItsRemovedFromTrash = 3,
        KeepMessagesOnServerAndDeleteMessageWhenItsRemovedFromTrash = 4
	}

	public enum SignatureType
	{
		Plain = 0,
		Html = 1
	}

	[Flags]
	public enum SignatureOptions
	{
		DontAddSignature = 0,
		AddSignatureToAllOutgoingMessages = 1,
		DontAddSignatureToRepliesAndForwards = 2
	}

	public enum FolderType
	{
		Inbox = 1,
		SentItems = 2,
		Drafts = 3,
		Trash = 4,
        Spam = 5,
        Quarantine = 6,
        Custom = 10
	}

	public enum FolderSyncType
	{
		DontSync = 0,
		NewHeadersOnly = 1,
		AllHeadersOnly = 2,
		NewEntireMessages = 3,
		AllEntireMessages = 4,
		DirectMode = 5
	}

	public enum FilterField
	{
		From = 0,
		To = 1,
		Subject = 2,
		XSpamHeader = 3,
        XVirusHeader = 4
	}

	public enum FilterCondition
	{
		ContainSubstring = 0,
		ContainExactPhrase = 1,
		NotContainSubstring = 2,
		BeginsWithSubstring = 3
	}

	public enum FilterAction
	{
		DoNothing = 0,
		DeleteFromServerImmediately = 1,
		MarkGrey = 2,
		MoveToFolder = 3
	}

	public enum ContactPrimaryEmail
	{
		Personal = 0,
		Business = 1,
        Other = 2
	}

	[Flags]
	public enum MessageMode
	{
		None = 0,
        Headers = 1,
		HtmlBody = 2,
		PlainBody = 4,
		HtmlReply = 8,
		PlainReply = 16,
        HtmlForward = 32,
		PlainForward = 64,
		FullHeaders = 128,
		Attachments = 256,
		PlainBodyUnmodified = 512
	}

	public enum LoginMode
	{
		Default = 0, // (show all)
		HideLoginFieldLoginIsAccount = 10, // hide login field, login is account name from email
		HideLoginFieldLoginIsEmail = 11,
		HideEmailField = 20, // hide email field, email as "login + '@' + DefaultDomainOptional"
		HideEmailFieldDisplayDomainAfterLogin = 21,
		HideEmailFieldLoginIsLoginAndDomain = 22, // Login as concatenation of "Login" field + "@" + domain
		HideEmailFieldDisplayDomainAfterLoginAndLoginIsLoginAndDomain = 23 // Display domain after login field & Login as concatenation of "Login" field + "@" + domain		
	}

	[Flags]
	public enum ViewMode
	{
		WithoutPreviewPane = 0,
		WithPreviewPane = 1,
		AlwaysShowPictures = 2
	}

    public enum WebMailSensitivity
    {
        None = 0,
        Confidential = 1,
        Private = 2,
        Personal = 3
    }

	public enum GlobalAddressBookEnum
	{
		Off = 0,
		DomainWide = 1,
		SystemWide = 2
	}

    public enum SaveMail
    {
        Always = 0,
        DefaultOn = 1,
        DefaultOff = 2
    }
}
