USE [master]
GO
/****** Object:  Database [testknorish]    Script Date: 09/16/2020 00:43:25 ******/
CREATE DATABASE [testknorish] ON  PRIMARY 
( NAME = N'testknorish', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\testknorish.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'testknorish_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\testknorish_log.LDF' , SIZE = 576KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [testknorish] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [testknorish].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [testknorish] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [testknorish] SET ANSI_NULLS OFF
GO
ALTER DATABASE [testknorish] SET ANSI_PADDING OFF
GO
ALTER DATABASE [testknorish] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [testknorish] SET ARITHABORT OFF
GO
ALTER DATABASE [testknorish] SET AUTO_CLOSE ON
GO
ALTER DATABASE [testknorish] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [testknorish] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [testknorish] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [testknorish] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [testknorish] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [testknorish] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [testknorish] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [testknorish] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [testknorish] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [testknorish] SET  ENABLE_BROKER
GO
ALTER DATABASE [testknorish] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [testknorish] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [testknorish] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [testknorish] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [testknorish] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [testknorish] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [testknorish] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [testknorish] SET  READ_WRITE
GO
ALTER DATABASE [testknorish] SET RECOVERY SIMPLE
GO
ALTER DATABASE [testknorish] SET  MULTI_USER
GO
ALTER DATABASE [testknorish] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [testknorish] SET DB_CHAINING OFF
GO
USE [testknorish]
GO
/****** Object:  Table [dbo].[tblStatusMaster]    Script Date: 09/16/2020 00:43:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblStatusMaster](
	[StatusCode] [varchar](2) NULL,
	[StatusDesc] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblRequest]    Script Date: 09/16/2020 00:43:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblRequest](
	[RequestId] [int] IDENTITY(1,1) NOT NULL,
	[BoatName] [varchar](50) NULL,
	[Status] [varchar](2) NULL,
PRIMARY KEY CLUSTERED 
(
	[RequestId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblLogin]    Script Date: 09/16/2020 00:43:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblLogin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblBoatInfo]    Script Date: 09/16/2020 00:43:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblBoatInfo](
	[BoatId] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NULL,
	[HourlyRate] [int] NULL,
	[CustomerName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[BoatId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[spInsertData]    Script Date: 09/16/2020 00:43:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spInsertData]
(
@reqId int,
@cName varchar(50),
@hRate int,
@BoatName varchar(50),
@BoatId int output
--@BoatId varchar(max) output
)
as begin
begin try
begin tran
set xact_abort on
insert into tblBoatInfo(RequestId,CustomerName,HourlyRate)
values(@reqId,@cName,@hRate)

insert into tblRequest(BoatName,[Status])
values(@BoatName, 'NC')
commit tran
Select @BoatId = (Select SCOPE_IDENTITY())
end try
begin catch
if xact_state()= -1
rollback tran
Select @BoatId = -1
--Select @BoatId = (Select ERROR_MESSAGE())
end catch
end
GO
/****** Object:  StoredProcedure [dbo].[spGetData]    Script Date: 09/16/2020 00:43:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[spGetData]
(
@reqId int,
@cName varchar(50),
@BoatNo int
)
as begin
Select
	tblRequest.Status as [Status]
from 
tblBoatInfo
	inner join 
tblRequest
on tblBoatInfo.RequestId = tblRequest.RequestId
where
	tblBoatInfo.RequestId = @reqId
	and tblBoatInfo.CustomerName = @cName
	and tblBoatInfo.BoatId = @BoatNo
end
GO
