
CREATE TABLE [ChatMessage] (
	Id int IDENTITY(1,1) NOT NULL,
	TimeStamp datetime NOT NULL,
	Message varchar(8000) NOT NULL,
	ReceiverUserId int NOT NULL,
	SenderUserId int NOT NULL,
	Deleted bit NOT NULL, 
	CONSTRAINT PK_ChatMessage PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [ChatUser] (
	Id int IDENTITY(1,1) NOT NULL,
	Nickname varchar(255) NOT NULL,
	CreateDate datetime NOT NULL,
	LastOnline datetime NOT NULL,
	GUID char(36) NOT NULL,
	Status int NOT NULL,
	Deleted bit NOT NULL,
	Hidden bit NOT NULL, 
	CONSTRAINT PK_ChatUser PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [ChatUserAccount] (
	UserId int NOT NULL,
	UserAccountData varchar(8000) NOT NULL,
	Salt varchar(64) NOT NULL,
	RestoreKey varchar(250) NOT NULL, 
	CONSTRAINT PK_ChatUserAccount PRIMARY KEY CLUSTERED (UserId)
);

CREATE TABLE [EventLog] (
	Id int IDENTITY(1,1) NOT NULL,
	TimeStamp datetime NOT NULL,
	InnerException varchar(8000),
	StackTrace varchar(8000),
	Text varchar(8000) NOT NULL,
	ChatUserId int,
	EventLevel int NOT NULL,
	EventType int NOT NULL, 
	CONSTRAINT PK_EventLog PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [ServerSetting] (
	KeyName varchar(50) NOT NULL,
	Description varchar(250),
	Value varchar(8000) NOT NULL,
	DataType int NOT NULL, 
	CONSTRAINT PK_ServerSetting PRIMARY KEY CLUSTERED (KeyName)
);

/*
SELECT 'FK_ChatMessage_ChatUser_Receiver' constraint_name FROM [ChatMessage] WHERE NOT EXISTS(SELECT 1 FROM [ChatUser] WHERE [ChatMessage].ReceiverUserId = Id) AND ReceiverUserId IS NOT NULL
UNION
SELECT 'FK_ChatMessage_ChatUser_Sender' constraint_name FROM [ChatMessage] WHERE NOT EXISTS(SELECT 1 FROM [ChatUser] WHERE [ChatMessage].SenderUserId = Id) AND SenderUserId IS NOT NULL
UNION
SELECT 'FK_ChatUserAccount_ChatUser' constraint_name FROM [ChatUserAccount] WHERE NOT EXISTS(SELECT 1 FROM [ChatUser] WHERE [ChatUserAccount].UserId = Id) AND UserId IS NOT NULL
UNION
SELECT 'FK_EventLog_ChatUser' constraint_name FROM [EventLog] WHERE NOT EXISTS(SELECT 1 FROM [ChatUser] WHERE [EventLog].ChatUserId = Id) AND ChatUserId IS NOT NULL
*/
ALTER TABLE [ChatMessage] ADD CONSTRAINT FK_ChatMessage_ChatUser_Receiver FOREIGN KEY (ReceiverUserId) REFERENCES [ChatUser] (Id);
ALTER TABLE [ChatMessage] ADD CONSTRAINT FK_ChatMessage_ChatUser_Sender FOREIGN KEY (SenderUserId) REFERENCES [ChatUser] (Id);
ALTER TABLE [ChatUserAccount] ADD CONSTRAINT FK_ChatUserAccount_ChatUser FOREIGN KEY (UserId) REFERENCES [ChatUser] (Id);
ALTER TABLE [EventLog] ADD CONSTRAINT FK_EventLog_ChatUser FOREIGN KEY (ChatUserId) REFERENCES [ChatUser] (Id);


