USE [master]
GO

/****** Object:  Login [IIS APPPOOL\KokaarQrCoder]    Script Date: 2022-03-14 16:22:27 ******/
CREATE LOGIN [IIS APPPOOL\KokaarQrCoder] FROM WINDOWS WITH DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[Français]
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [IIS APPPOOL\KokaarQrCoder]
GO

ALTER SERVER ROLE [dbcreator] ADD MEMBER [IIS APPPOOL\KokaarQrCoder]
GO