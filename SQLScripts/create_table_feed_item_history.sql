USE [jarwin]
GO

/****** Object:  Table [dbo].[feed_item_history]    Script Date: 29/07/2014 7:26:29 a.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[feed_item_history](
	[feed_id] [int] NOT NULL,
	[feed_item_id] [int] NOT NULL,
	[title] [nvarchar](512) NULL,
	[item_uri] [nvarchar](512) NULL,
	[comments] [nvarchar](1024) NOT NULL,
	[published_datetime] [datetime] NOT NULL,
	[creator] [nvarchar](128) NULL,
	[description] [nvarchar](max) NULL,
	[content] [nvarchar](max) NULL,
	[last_download_datetime] [datetime] NOT NULL,
 CONSTRAINT [PK_feed_item_history] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC,
	[feed_item_id] ASC,
	[last_download_datetime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

