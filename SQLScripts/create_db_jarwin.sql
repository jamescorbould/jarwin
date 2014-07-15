USE [master]
GO
/****** Object:  Database [jarwin]    Script Date: 15/07/2014 5:35:42 p.m. ******/
CREATE DATABASE [jarwin]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'jarwin', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\jarwin.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'jarwin_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLEXPRESS\MSSQL\DATA\jarwin_log.ldf' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [jarwin] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [jarwin].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [jarwin] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [jarwin] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [jarwin] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [jarwin] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [jarwin] SET ARITHABORT OFF 
GO
ALTER DATABASE [jarwin] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [jarwin] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [jarwin] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [jarwin] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [jarwin] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [jarwin] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [jarwin] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [jarwin] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [jarwin] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [jarwin] SET  DISABLE_BROKER 
GO
ALTER DATABASE [jarwin] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [jarwin] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [jarwin] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [jarwin] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [jarwin] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [jarwin] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [jarwin] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [jarwin] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [jarwin] SET  MULTI_USER 
GO
ALTER DATABASE [jarwin] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [jarwin] SET DB_CHAINING OFF 
GO
ALTER DATABASE [jarwin] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [jarwin] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [jarwin] SET DELAYED_DURABILITY = DISABLED 
GO
USE [jarwin]
GO
/****** Object:  Table [dbo].[feed]    Script Date: 15/07/2014 5:35:43 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[feed](
	[feed_id] [int] IDENTITY(0,1) NOT NULL,
	[feed_uri] [nvarchar](512) NOT NULL,
	[title] [nvarchar](512) NOT NULL,
	[description] [nvarchar](512) NULL,
	[last_build_datetime] [datetime] NOT NULL,
	[last_download_datetime] [datetime] NOT NULL,
	[language] [nvarchar](8) NOT NULL,
	[update_frequency] [int] NULL,
	[site_uri] [nvarchar](512) NOT NULL,
	[status] [nvarchar](16) NULL,
	[type] [nvarchar](16) NULL,
	[folder_id] [int] NULL,
	[update_period] [nchar](10) NULL,
 CONSTRAINT [PK_feed] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[feed_category]    Script Date: 15/07/2014 5:35:43 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[feed_category](
	[feed_id] [int] NOT NULL,
	[feed_item_id] [int] NOT NULL,
	[description] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_feed_category] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC,
	[feed_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[feed_item]    Script Date: 15/07/2014 5:35:43 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[feed_item](
	[feed_id] [int] NOT NULL,
	[feed_item_id] [int] NOT NULL,
	[title] [nvarchar](512) NULL,
	[item_uri] [nvarchar](512) NULL,
	[comments] [nvarchar](1024) NOT NULL,
	[published_datetime] [datetime] NOT NULL,
	[creator] [nvarchar](128) NULL,
	[description] [nvarchar](max) NULL,
	[content] [nvarchar](max) NULL,
 CONSTRAINT [PK_feed_item] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC,
	[feed_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[feed_media]    Script Date: 15/07/2014 5:35:43 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[feed_media](
	[feed_id] [int] NOT NULL,
	[feed_item_id] [int] NOT NULL,
	[uri] [nvarchar](512) NOT NULL,
	[desciption] [nvarchar](512) NOT NULL,
 CONSTRAINT [PK_feed_media] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC,
	[feed_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[folder]    Script Date: 15/07/2014 5:35:43 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[folder](
	[folder_id] [int] NOT NULL,
	[name] [nvarchar](48) NOT NULL,
	[child_folder_id] [int] NULL,
 CONSTRAINT [PK_folder] PRIMARY KEY CLUSTERED 
(
	[folder_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[feed]  WITH CHECK ADD  CONSTRAINT [FK_feed_folder] FOREIGN KEY([folder_id])
REFERENCES [dbo].[folder] ([folder_id])
GO
ALTER TABLE [dbo].[feed] CHECK CONSTRAINT [FK_feed_folder]
GO
ALTER TABLE [dbo].[feed_category]  WITH CHECK ADD  CONSTRAINT [FK_feed_category_feed_item] FOREIGN KEY([feed_id], [feed_item_id])
REFERENCES [dbo].[feed_item] ([feed_id], [feed_item_id])
GO
ALTER TABLE [dbo].[feed_category] CHECK CONSTRAINT [FK_feed_category_feed_item]
GO
ALTER TABLE [dbo].[feed_item]  WITH CHECK ADD  CONSTRAINT [FK_feed_item_feed] FOREIGN KEY([feed_id])
REFERENCES [dbo].[feed] ([feed_id])
GO
ALTER TABLE [dbo].[feed_item] CHECK CONSTRAINT [FK_feed_item_feed]
GO
ALTER TABLE [dbo].[feed_media]  WITH CHECK ADD  CONSTRAINT [FK_feed_media_feed_item] FOREIGN KEY([feed_id], [feed_item_id])
REFERENCES [dbo].[feed_item] ([feed_id], [feed_item_id])
GO
ALTER TABLE [dbo].[feed_media] CHECK CONSTRAINT [FK_feed_media_feed_item]
GO
ALTER TABLE [dbo].[folder]  WITH CHECK ADD  CONSTRAINT [FK_folder_folder] FOREIGN KEY([child_folder_id])
REFERENCES [dbo].[folder] ([folder_id])
GO
ALTER TABLE [dbo].[folder] CHECK CONSTRAINT [FK_folder_folder]
GO
USE [master]
GO
ALTER DATABASE [jarwin] SET  READ_WRITE 
GO
