IF NOT EXISTS(SELECT * FROM sysobjects WHERE name='toDoItems' and xtype='U') 
CREATE TABLE toDoItems (
    id INT NOT NULL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    isComplete BIT NOT NULL
);