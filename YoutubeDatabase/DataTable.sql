CREATE TABLE [dbo].[DataTable]
(
	[ID] INT NOT NULL PRIMARY KEY, 
    [VideoID] NCHAR(50) NOT NULL, 
    [VideoTitle] NCHAR(50) NOT NULL, 
    [wasDeleted] BIT NOT NULL
)
