USE [jarwin]
GO

/****** Object:  Table [dbo].[feed_item]    Script Date: 3/10/2014 7:45:02 a.m. ******/
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
	[last_download_datetime] [datetime] NOT NULL,
 CONSTRAINT [PK_feed_item] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC,
	[feed_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[feed_item]  WITH CHECK ADD  CONSTRAINT [FK_feed_item_feed] FOREIGN KEY([feed_id])
REFERENCES [dbo].[feed] ([feed_id])
GO

ALTER TABLE [dbo].[feed_item] CHECK CONSTRAINT [FK_feed_item_feed]
GO

