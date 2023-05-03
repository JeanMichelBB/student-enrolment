
-- StName, CName, ProgId, ProgName, Cid, StId, FinalNote

-- DROP DATABASE College1en;


IF db_id('College1en') IS NULL CREATE DATABASE College1en;
GO

USE College1en;
GO
CREATE TABLE Programs(ProgId VARCHAR (5) NOT NULL,
ProgName VARCHAR (50) NOT NULL,
PRIMARY KEY (ProgId));

CREATE TABLE Courses(CId VARCHAR (7) NOT NULL,
CName VARCHAR (50) NOT NULL,
ProgId VARCHAR (5) NOT NULL,
PRIMARY KEY (CId),
FOREIGN KEY (ProgId) REFERENCES
Programs(ProgId)
ON DELETE NO ACTION
ON UPDATE CASCADE);

CREATE TABLE Students(StId VARCHAR (10) NOT NULL,
StName VARCHAR (50) NOT NULL,
ProgId VARCHAR (5) NOT NULL,
PRIMARY KEY (StId),
FOREIGN KEY (ProgId) REFERENCES
Programs(ProgId)
ON DELETE CASCADE
ON UPDATE CASCADE);

CREATE TABLE Enrollments(StId VARCHAR (10) NOT NULL,
CId VARCHAR (7) NOT NULL,
FinalNote INT,
PRIMARY KEY (StId,CId),
FOREIGN KEY (StId) REFERENCES
Students(StId)
ON DELETE CASCADE
ON UPDATE CASCADE,
FOREIGN KEY (CId) REFERENCES
Courses(CId)
ON DELETE NO ACTION
ON UPDATE NO ACTION);
GO
INSERT INTO Programs (ProgId, ProgName) 
VALUES ( 'P0010', 'Program1'),
( 'P0020', 'Program2'),
( 'P0030', 'Program3'),
( 'P0040', 'Program4') ;

INSERT INTO Courses (CId, CName, ProgId) 
VALUES ( 'C000011', 'Course11','P0010'),
( 'C000012', 'Course12','P0010'),
( 'C000013', 'Course13','P0010'),
( 'C000014', 'Course14','P0010'),
( 'C000021', 'Course21','P0020'),
( 'C000022', 'Course22','P0020'),
( 'C000023', 'Course23','P0020'),
( 'C000024', 'Course24','P0020'),
( 'C000031', 'Course31','P0030'),
( 'C000032', 'Course32','P0030'),
( 'C000033', 'Course33','P0030'),
( 'C000034', 'Course34','P0030'),
( 'C000041', 'Course41','P0040'),
( 'C000042', 'Course42','P0040'),
( 'C000043', 'Course43','P0040'),
( 'C000044', 'Course44','P0040');

INSERT INTO Students (StId, StName, ProgId) 
VALUES ( 'S000001', 'Jean','P0010'),
( 'S000002', 'Jean-Michel','P0020'),
( 'S000003', 'Michel','P0030'),
( 'S000004', 'Michel-Jean','P0040') ;

INSERT INTO Enrollments (StId, CId, FinalNote)
VALUES ( 'S000001', 'C000011', 10),
( 'S000002', 'C000021', 20),
( 'S000003', 'C000031', 30),
( 'S000004', 'C000041', 40) ;
GO