USE [jarwin]
GO

/****** Object:  Table [dbo].[feed_history]    Script Date: 3/10/2014 7:44:49 a.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feed_history](
	[feed_id] [int] NOT NULL,
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
	[archived_datetime] [datetime] NOT NULL,
 CONSTRAINT [PK_feed_history_1] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC,
	[archived_datetime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[feed_history]  WITH CHECK ADD  CONSTRAINT [FK_feed_history_folder] FOREIGN KEY([folder_id])
REFERENCES [dbo].[folder] ([folder_id])
GO

ALTER TABLE [dbo].[feed_history] CHECK CONSTRAINT [FK_feed_history_folder]
GO

