﻿CREATE TABLE [dbo].[Tracks]
(
	[Id] INT NOT NULL IDENTITY (1, 1),
	[Performer] NVARCHAR(256) NOT NULL,
	[Name] NVARCHAR(512) NOT NULL,
	[Genre] NVARCHAR(128) NOT NULL,
	[Year] DATETIME2 NOT NULL
	CONSTRAINT [PK_Tracks_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
)
