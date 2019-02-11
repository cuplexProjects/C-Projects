SET Path=C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools
sqlmetal /server:localhost /database:SecureChat /dbml:SecureChatDataClasses.dbml /namespace:Toyota.TMHE.TPC.DAL.Linq /context:SecureChatDataClassesDataContext /pluralize /functions
sqlmetal /code:SecureChatDataClasses.designer.cs SecureChatDataClasses.dbml
pause