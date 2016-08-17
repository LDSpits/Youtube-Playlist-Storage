CREATE TABLE [dbo].[DataTable]
(
	[VideoID] INT NOT NULL PRIMARY KEY, 
    [VideoTitle] NCHAR(50) NOT NULL, 
    [wasDeleted] BIT NOT NULL
)
