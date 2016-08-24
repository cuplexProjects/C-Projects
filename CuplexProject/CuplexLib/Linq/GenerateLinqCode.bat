sqlmetal /server:localhost\SQLEXPRESS /database:CuplexDB /user:Cuplex /password:SRqElKALxIPyjILkgH6BF2ixdlYKb2UZ /dbml:temp.dbml /namespace:CuplexLib.Linq /context:CuplexDataClassesDataContext /pluralize /functions
cscs CSScript.cs temp.dbml CuplexDataClasses.dbml
sqlmetal /code:CuplexDataClasses.designer.cs CuplexDataClasses.dbml