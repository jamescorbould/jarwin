USE [jarwin]
GO

/****** Object:  Table [dbo].[feed]    Script Date: 30/05/2014 7:24:03 a.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feed](
	[feed_id] [int] NOT NULL,
	[feed_uri] [nvarchar](512) NOT NULL,
	[title] [nvarchar](512) NOT NULL,
	[description] [nvarchar](512) NULL,
	[last_build_datetime] [datetime] NOT NULL,
	[last_download_datetime] [datetime] NOT NULL,
	[language] [nvarchar](2) NOT NULL,
	[frequency_id] [int] NULL,
	[site_uri] [nvarchar](512) NOT NULL,
	[status] [nvarchar](16) NULL,
	[type] [nvarchar](16) NULL,
	[folder_id] [int] NULL,
 CONSTRAINT [PK_feed] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[feed]  WITH CHECK ADD  CONSTRAINT [FK_feed_folder] FOREIGN KEY([folder_id])
REFERENCES [dbo].[folder] ([folder_id])
GO

ALTER TABLE [dbo].[feed] CHECK CONSTRAINT [FK_feed_folder]
GO

