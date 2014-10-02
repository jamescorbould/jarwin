USE [jarwin]
GO

/****** Object:  Table [dbo].[feed_category]    Script Date: 3/10/2014 7:44:35 a.m. ******/
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

ALTER TABLE [dbo].[feed_category]  WITH CHECK ADD  CONSTRAINT [FK_feed_category_feed_item] FOREIGN KEY([feed_id], [feed_item_id])
REFERENCES [dbo].[feed_item] ([feed_id], [feed_item_id])
GO

ALTER TABLE [dbo].[feed_category] CHECK CONSTRAINT [FK_feed_category_feed_item]
GO

