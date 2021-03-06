USE [master]
GO
/****** Object:  Database [MemberManagementData]    Script Date: 4/11/2018 2:53:19 PM ******/
CREATE DATABASE [MemberManagementData]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MemberManagementData', FILENAME = N'C:\Users\mgill\MemberManagementData.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MemberManagementData_log', FILENAME = N'C:\Users\mgill\MemberManagementData_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [MemberManagementData] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MemberManagementData].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MemberManagementData] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MemberManagementData] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MemberManagementData] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MemberManagementData] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MemberManagementData] SET ARITHABORT OFF 
GO
ALTER DATABASE [MemberManagementData] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MemberManagementData] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MemberManagementData] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MemberManagementData] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MemberManagementData] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MemberManagementData] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MemberManagementData] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MemberManagementData] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MemberManagementData] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MemberManagementData] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MemberManagementData] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MemberManagementData] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MemberManagementData] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MemberManagementData] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MemberManagementData] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MemberManagementData] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [MemberManagementData] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MemberManagementData] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MemberManagementData] SET  MULTI_USER 
GO
ALTER DATABASE [MemberManagementData] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MemberManagementData] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MemberManagementData] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MemberManagementData] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MemberManagementData] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MemberManagementData] SET QUERY_STORE = OFF
GO
USE [MemberManagementData]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [MemberManagementData]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4/11/2018 2:53:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Members]    Script Date: 4/11/2018 2:53:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Members](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](12) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](75) NOT NULL,
	[PhoneNumber] [nvarchar](10) NULL,
	[DateOfBirth] [datetime2](7) NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[LastUpdated] [datetime2](7) NOT NULL,
	[Archived] [bit] NOT NULL,
 CONSTRAINT [PK_Members] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20180402185618_initial', N'2.0.2-rtm-10011')
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Members_UserName]    Script Date: 4/11/2018 2:53:19 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Members_UserName] ON [dbo].[Members]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [MemberManagementData] SET  READ_WRITE 
GO
