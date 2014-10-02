USE [jarwin]
GO

/****** Object:  Table [dbo].[feed_media]    Script Date: 3/10/2014 7:45:26 a.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[feed_media](
	[feed_id] [int] NOT NULL,
	[feed_item_id] [int] NOT NULL,
	[uri] [nvarchar](512) NOT NULL,
	[description] [nvarchar](512) NULL,
	[data] [varbinary](max) NULL,
	[type] [nvarchar](28) NOT NULL,
 CONSTRAINT [PK_feed_media_1] PRIMARY KEY CLUSTERED 
(
	[feed_id] ASC,
	[feed_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[feed_media]  WITH CHECK ADD  CONSTRAINT [FK_feed_media_feed_item] FOREIGN KEY([feed_id], [feed_item_id])
REFERENCES [dbo].[feed_item] ([feed_id], [feed_item_id])
GO

ALTER TABLE [dbo].[feed_media] CHECK CONSTRAINT [FK_feed_media_feed_item]
GO

