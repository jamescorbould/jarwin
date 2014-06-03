USE [jarwin]
GO

/****** Object:  Table [dbo].[folder]    Script Date: 30/05/2014 7:25:48 a.m. ******/
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

ALTER TABLE [dbo].[folder]  WITH CHECK ADD  CONSTRAINT [FK_folder_folder] FOREIGN KEY([child_folder_id])
REFERENCES [dbo].[folder] ([folder_id])
GO

ALTER TABLE [dbo].[folder] CHECK CONSTRAINT [FK_folder_folder]
GO

