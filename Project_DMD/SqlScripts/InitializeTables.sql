/***** PUBLICATION SYSTEM *****/
CREATE TABLE Client
(
	UserID SERIAL PRIMARY KEY,
	Email VARCHAR(254) NOT NULL,
	FirstName VARCHAR(35),
	LastName VARCHAR(35),
	PasswordHash CHAR(32)
);

-- Create table with all authors
/*CREATE TABLE Author
(
	AuthorID SERIAL PRIMARY KEY,
	AuthorName VARCHAR(64) NOT NULL
);

-- Create table with all article's categories
CREATE TABLE Category
(
	CategoryID SERIAL PRIMARY KEY,
	CategoryName VARCHAR(32) NOT NULL
);

-- Create table with keywords
CREATE TABLE Keyword
(
	KeywordID SERIAL PRIMARY KEY,
	KeywordName VARCHAR(100) NOT NULL
);

-- Create table with articles
CREATE TABLE Article
(
	ArticleID SERIAL PRIMARY KEY,
	Title VARCHAR(512) NOT NULL,
	Summary VARCHAR(2048),
	Published DATE NOT NULL,
	Updated DATE,
	Views INTEGER,
	URL VARCHAR(65356),
	DOI VARCHAR(512),
	JournalReference VARCHAR(512)
);

-- Create table for connection between Articles and Categories
CREATE TABLE ArticleCategories
(
	ArticleID INTEGER REFERENCES Article(ArticleID),
	CategoryID INTEGER REFERENCES Category(CategoryID)
);

-- Create table for connection between Articles and Authors
CREATE TABLE ArticleAuthors
(
	ArticleID INTEGER REFERENCES Article(ArticleID),
	AuthorID INTEGER REFERENCES Author(AuthorID)
);


-- Create table for connection between Articles and Keywords
CREATE TABLE ArticleKeywords
(
	ArticleID INTEGER REFERENCES Article(ArticleID),
	KeywordID INTEGER REFERENCES Keyword(KeywordID)
);

/***** *****/


/***** ACCOUNT SYSTEM *****/

-- Create tabe with users
CREATE TABLE Client
(
	UserID SERIAL PRIMARY KEY,
	Email VARCHAR(254) NOT NULL,
	FirstName VARCHAR(35),
	LastName VARCHAR(35),
	PasswordHash CHAR(32)
);

-- Create table with favorite articles
CREATE TABLE Favorite
(
	ArticleID INTEGER REFERENCES Article(ArticleID),
	UserID INTEGER REFERENCES Client(UserID),
	AdditionDate DATE
);

-- Create table with all visits
CREATE TABLE Visit
(
	-- Why we don't use Foreign Key? Beacause we want to store information about visited page even though this page would be removed
	ArticleID INTEGER,
	-- The same for UserID
	UserID INTEGER,
	VisitDate TIMESTAMP
);

DROP TYPE ActionType;
-- Create enum to indentify actions that're done on articles
CREATE  TYPE ActionType AS ENUM ('Edit', 'Delete', 'Add');

-- Create table for saving history of articles
CREATE TABLE ActionHistory
(
	ActionHistoryID SERIAL PRIMARY KEY,
	ActionDate TIMESTAMP NOT NULL,
	ActionDone ActionType NOT NULL,
	-- Why we don't use Foreign Key? Beacause we want to store information about action history on the page even though this page would be removed
	ArticleID INTEGER,
	-- The same for UserID
	UserID INTEGER
);
*/
/***** *****/

