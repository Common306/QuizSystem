USE [master]
GO
CREATE DATABASE QuizSystemDatabase
GO
USE QuizSystemDatabase

/* ------------------ Script create table ------------------ */
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NULL
)
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](250) NOT NULL,
	[RoleId] [int] NOT NULL,
	[FullName] [nvarchar](250) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[IsEnable] [bit] NULL,
	[CreateAt] [datetime] default CURRENT_TIMESTAMP,
	[UpdateAt] [datetime] default CURRENT_TIMESTAMP,
	FOREIGN KEY (RoleId) REFERENCES [Role](RoleId) ON DELETE CASCADE
)
GO
CREATE TABLE [dbo].[Quiz](
	[QuizId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](250) NULL,
	[Description] [text] NULL,
	[IsPublish] [bit] NULL,
	[StartAt] [datetime] NULL,
	[EndAt] [datetime] NULL,
	[PassScore] [float] NULL,
	[QuizCode] [nvarchar](50) NULL,
	[CreatorId] [int] NULL,
	[CreateAt] [datetime] default CURRENT_TIMESTAMP,
	[UpdateAt] [datetime] default CURRENT_TIMESTAMP,
	FOREIGN KEY (CreatorId) REFERENCES [User](UserId) ON DELETE CASCADE
)
GO
CREATE TABLE [dbo].[TakeQuiz](
	[TakeQuizId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[QuizId] [int] NULL,
	[StartAt] [datetime] NULL,
	[EndAt] [datetime] NULL,
	[Score] [float] NULL,
	FOREIGN KEY (UserId) REFERENCES [User](UserId) ON DELETE CASCADE,
	FOREIGN KEY (QuizId) REFERENCES [Quiz](QuizId) ON DELETE NO ACTION
) 
GO
CREATE TABLE [dbo].[Question](
	[QuestionId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](500) NULL,
	[Score] [float] NULL,
	[MultipleChoice] [bit] NULL,
	[QuizId] [int] NULL,
	[IsActive] [bit] NULL,
	FOREIGN KEY (QuizId) REFERENCES [Quiz](QuizId) ON DELETE CASCADE
)
GO
CREATE TABLE [dbo].[Answer](
	[AnswerId] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Content] [nvarchar](500) NULL,
	[IsCorrect] [bit] NULL,
	[IsActive] [bit] NULL,
	[QuestionId] [int] NULL,
	FOREIGN KEY (QuestionId) REFERENCES [Question](QuestionId) ON DELETE CASCADE
)
GO
CREATE TABLE [dbo].[TakeAnswer](
	[TakeQuizId] [int] NOT NULL,
	[AnswerId] [int] NOT NULL,
	[QuestionId] [int] NULL,
	PRIMARY KEY(TakeQuizId, AnswerId),
	FOREIGN KEY (TakeQuizId) REFERENCES [TakeQuiz](TakeQuizId) ON DELETE CASCADE,
	FOREIGN KEY (AnswerId) REFERENCES [Answer](AnswerId) ON DELETE NO ACTION
)
/* ------------------ Script insert data ------------------ */
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
INSERT [dbo].[Role] ([RoleId], [RoleName], [Description]) VALUES (1, N'Admin', NULL)
INSERT [dbo].[Role] ([RoleId], [RoleName], [Description]) VALUES (2, N'Teacher', NULL)
INSERT [dbo].[Role] ([RoleId], [RoleName], [Description]) VALUES (3, N'Student', NULL)
SET IDENTITY_INSERT [dbo].[Role] OFF 
GO
INSERT INTO [dbo].[User] ([Username], [Password], [RoleId], [FullName], [PhoneNumber], [IsEnable])
VALUES
	(N'admin', N'123456', 1, N'Admin System', N'0912564788', 1),
	(N'teacher1', N'123456', 2, N'Teacher Number 1', N'0328451552', 1),
	(N'teacher2', N'123456', 2, N'Teacher Number 2', N'0315654112', 1),
	(N'student1', N'123456', 3, N'Student Number 1', N'0315654112', 1),
    ('john_doe', 'password123', 1, 'John Doe', '1234567890', 1),
    ('jane_smith', 'pass456', 2, 'Jane Smith', '9876543210', 1),
    ('alex_green', 'qwerty', 1, 'Alex Green', '5555555555', 1),
    ('emily_wilson', 'abc123', 3, 'Emily Wilson', '9999999999', 1),
    ('michael_brown', 'pass321', 2, 'Michael Brown', '1111111111', 1),
    ('sarah_jones', 'password1', 1, 'Sarah Jones', '5555555555', 1),
    ('peter_williams', 'pass123', 2, 'Peter Williams', '1234567890', 1),
    ('lisa_johnson', 'abcd1234', 1, 'Lisa Johnson', '9876543210', 1),
    ('kevin_lee', 'password123', 3, 'Kevin Lee', '9999999999', 1),
    ('mary_wilson', 'password1', 2, 'Mary Wilson', '1111111111', 1),
    ('robert_smith', 'pass456', 1, 'Robert Smith', '5555555555', 1),
    ('olivia_davis', 'qwerty', 2, 'Olivia Davis', '1234567890', 1),
    ('william_jones', 'abc123', 1, 'William Jones', '9876543210', 1),
    ('ava_johnson', 'pass321', 3, 'Ava Johnson', '5555555555', 1),
    ('ethan_brown', 'password1', 2, 'Ethan Brown', '1111111111', 1);
GO
INSERT INTO [dbo].[Quiz] ([Title], [Description], [IsPublish], [StartAt], [EndAt], [PassScore], [QuizCode], [CreatorId])
VALUES
    ('History Quiz', 'Test your knowledge of historical events.', 1, '2023-07-25 10:00:00', '2023-07-25 12:00:00', 70.0, 'HISTQUIZ001', 2),
    ('Math Quiz', 'Solve some challenging math problems.', 1, '2023-07-26 15:00:00', '2023-07-26 17:00:00', 60.0, 'MATHQUIZ001', 3),
    ('Science Quiz', 'Questions about various scientific concepts.', 1, '2023-07-27 14:00:00', '2023-07-27 16:00:00', 75.0, 'SCIQUIZ001', 2),
    ('Literature Quiz', 'Test your knowledge of classic literature.', 1, '2023-07-28 18:00:00', '2023-07-28 20:00:00', 65.0, 'LITQUIZ001', 3),
    ('Language Quiz', 'Questions about different languages and linguistics.', 1, '2023-07-29 12:00:00', '2023-07-29 14:00:00', 80.0, 'LANGQUIZ001', 2),
    ('Geography Quiz', 'Test your knowledge of world geography.', 1, '2023-07-30 11:00:00', '2023-07-30 13:00:00', 70.0, 'GEOQUIZ001', 3),
    ('Music Quiz', 'Questions about famous musicians and compositions.', 1, '2023-07-31 16:00:00', '2023-07-31 18:00:00', 75.0, 'MUSICQUIZ001', 2),
    ('Sports Quiz', 'Test your knowledge of various sports.', 1, '2023-08-01 19:00:00', '2023-08-01 21:00:00', 65.0, 'SPORTQUIZ001', 3),
    ('Technology Quiz', 'Questions about modern technology and inventions.', 1, '2023-08-02 13:00:00', '2023-08-02 15:00:00', 70.0, 'TECHQUIZ001', 2),
    ('Biology Quiz', 'Test your knowledge of biology and life sciences.', 1, '2023-08-03 17:00:00', '2023-08-03 19:00:00', 75.0, 'BIOQUIZ001', 2),
    ('Movie Quiz', 'Questions about famous movies and directors.', 1, '2023-08-04 20:00:00', '2023-08-04 22:00:00', 80.0, 'MOVIEQUIZ001', 2),
    ('Art Quiz', 'Test your knowledge of famous artists and art movements.', 1, '2023-08-05 14:00:00', '2023-08-05 16:00:00', 70.0, 'ARTQUIZ001', 2),
    ('General Knowledge Quiz', 'A mix of questions covering various topics.', 1, '2023-08-06 12:00:00', '2023-08-06 14:00:00', 65.0, 'GKQUIZ001', 3),
    ('Computer Science Quiz', 'Questions about computer programming and algorithms.', 1, '2023-08-07 15:00:00', '2023-08-07 17:00:00', 70.0, 'CSQUIZ001', 3),
    ('Food and Cuisine Quiz', 'Test your knowledge of world cuisines and food.', 1, '2023-08-08 18:00:00', '2023-08-08 20:00:00', 75.0, 'FOODQUIZ001', 3);
GO
-- Quiz 1 - History Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('When did World War II end?', 5.0, 1, 1, 1),
    ('Who was the first president of the United States?', 5.0, 1, 1, 1),
    ('What year did Christopher Columbus arrive in the Americas?', 5.0, 1, 1, 1),
    ('Which famous battle took place in 1066 and changed the course of English history?', 5.0, 1, 1, 1),
    ('Who was the leader of the Soviet Union during World War II?', 5.0, 1, 1, 1);

-- Quiz 2 - Math Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('What is the value of pi (π)?', 5.0, 1, 2, 1),
    ('Solve: 2x + 5 = 15', 5.0, 1, 2, 1),
    ('What is the square root of 144?', 5.0, 1, 2, 1),
    ('Simplify: (3 + 4) * (8 - 2)', 5.0, 1, 2, 1),
    ('Find the area of a triangle with base = 6 and height = 8.', 5.0, 1, 2, 1);

-- Quiz 3 - Science Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('What is the chemical symbol for water?', 5.0, 1, 3, 1),
    ('Who proposed the theory of relativity?', 5.0, 1, 3, 1),
    ('What gas do plants absorb during photosynthesis?', 5.0, 1, 3, 1),
    ('What is the hardest mineral on the Mohs scale?', 5.0, 1, 3, 1),
    ('What is the process by which liquid water turns into water vapor?', 5.0, 1, 3, 1);

-- Quiz 4 - Literature Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('Who wrote the play "Romeo and Juliet"?', 5.0, 1, 4, 1),
    ('In which novel is the character "Sherlock Holmes" featured?', 5.0, 1, 4, 1),
    ('What is the famous opening line of Charles Dickens'' novel "A Tale of Two Cities"?', 5.0, 1, 4, 1),
    ('Which author is known for the book "Pride and Prejudice"?', 5.0, 1, 4, 1),
    ('In "The Lord of the Rings," who is the creator of the One Ring?', 5.0, 1, 4, 1);

-- Quiz 5 - Language Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('Which language is spoken in Japan?', 5.0, 1, 5, 1),
    ('What is the official language of Brazil?', 5.0, 1, 5, 1),
    ('How do you say "hello" in Spanish?', 5.0, 1, 5, 1),
    ('Which country is known for the language "French"?', 5.0, 1, 5, 1),
    ('What is the alphabet used in the Russian language called?', 5.0, 1, 5, 1);

-- Quiz 6 - Geography Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('What is the capital of France?', 5.0, 1, 6, 1),
    ('Which river is the longest in the world?', 5.0, 1, 6, 1),
    ('What is the largest desert in the world?', 5.0, 1, 6, 1),
    ('In which country is the Great Barrier Reef located?', 5.0, 1, 6, 1),
    ('Which mountain range stretches across much of South America?', 5.0, 1, 6, 1);

-- Quiz 7 - Music Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('Who is known as the "King of Pop"?', 5.0, 1, 7, 1),
    ('Which musical instrument has black and white keys?', 5.0, 1, 7, 1),
    ('What is the national anthem of the United States?', 5.0, 1, 7, 1),
    ('Which band is known for the song "Bohemian Rhapsody"?', 5.0, 1, 7, 1),
    ('Who composed the famous symphony "Symphony No. 9" (Ode to Joy)?', 5.0, 1, 7, 1);

-- Quiz 8 - Sports Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('Which sport is played in the Super Bowl?', 5.0, 1, 8, 1),
    ('In which country were the first Olympic Games held?', 5.0, 1, 8, 1),
    ('How many players are there in a standard soccer team?', 5.0, 1, 8, 1),
    ('Which sport uses a shuttlecock?', 5.0, 1, 8, 1),
    ('Who is the fastest man in the world, known for his speed in track and field events?', 5.0, 1, 8, 1);

-- Quiz 9 - Technology Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('Who is the co-founder of Microsoft?', 5.0, 1, 9, 1),
    ('What is the programming language used to create web pages?', 5.0, 1, 9, 1),
    ('Which company developed the iPhone?', 5.0, 1, 9, 1),
    ('What does CPU stand for?', 5.0, 1, 9, 1),
    ('What is the name for a small piece of code that performs a specific task and is designed to be reusable?', 5.0, 1, 9, 1);

-- Quiz 10 - Biology Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('What is the largest organ in the human body?', 5.0, 1, 10, 1),
    ('What is the basic unit of life?', 5.0, 1, 10, 1),
    ('Which gas do plants give off during photosynthesis?', 5.0, 1, 10, 1),
    ('What is the process by which organisms produce offspring?', 5.0, 1, 10, 1),
    ('Which scientist is known for discovering the laws of inheritance?', 5.0, 1, 10, 1);

-- Quiz 11 - Movie Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('Who directed the movie "Titanic"?', 5.0, 1, 11, 1),
    ('In which movie does the character "Forrest Gump" appear?', 5.0, 1, 11, 1),
    ('Which film won the Academy Award for Best Picture in 2020?', 5.0, 1, 11, 1),
    ('Who played the role of "Harry Potter" in the Harry Potter film series?', 5.0, 1, 11, 1),
    ('Which movie features the quote "You can''t handle the truth!"?', 5.0, 1, 11, 1);

-- Quiz 12 - Art Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('Who painted the "Mona Lisa"?', 5.0, 1, 12, 1),
    ('Which art movement is characterized by its use of geometric shapes and primary colors?', 5.0, 1, 12, 1),
    ('Who is known for creating the sculpture "David"?', 5.0, 1, 12, 1),
    ('Which artist is famous for his "Campbell''s Soup Cans" artwork?', 5.0, 1, 12, 1),
    ('In which country was Vincent van Gogh born?', 5.0, 1, 12, 1);

-- Quiz 13 - General Knowledge Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('What is the capital of Australia?', 5.0, 1, 13, 1),
    ('Who wrote the play "Hamlet"?', 5.0, 1, 13, 1),
    ('What is the chemical symbol for gold?', 5.0, 1, 13, 1),
    ('What is the currency of Japan?', 5.0, 1, 13, 1),
    ('Which planet is known as the "Red Planet"?', 5.0, 1, 13, 1);

-- Quiz 14 - Computer Science Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('What is an IP address used for?', 5.0, 1, 14, 1),
    ('Which programming language is used for building Android apps?', 5.0, 1, 14, 1),
    ('What is the full form of HTML?', 5.0, 1, 14, 1),
    ('What is the main function of a compiler?', 5.0, 1, 14, 1),
    ('What does CSS stand for in web development?', 5.0, 1, 14, 1);

-- Quiz 15 - Food and Cuisine Quiz
INSERT INTO [dbo].[Question] ([Content], [Score], [MultipleChoice], [QuizId], [IsActive])
VALUES
    ('What is the main ingredient in guacamole?', 5.0, 1, 15, 1),
    ('Which country is known for inventing pizza?', 5.0, 1, 15, 1),
    ('What is the traditional Japanese drink made from fermented rice?', 5.0, 1, 15, 1),
    ('Which spice is known as "the king of spices"?', 5.0, 1, 15, 1),
    ('What type of pasta is shaped like small rice grains?', 5.0, 1, 15, 1);

GO
-- Câu hỏi 1 (Quiz 1 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('1945', 1, 1, 1), -- Câu trả lời đúng
    ('1918', 0, 1, 1),
    ('1955', 0, 1, 1),
    ('1939', 0, 1, 1);

-- Câu hỏi 2 (Quiz 1 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('George Washington', 1, 1, 2), -- Câu trả lời đúng
    ('Abraham Lincoln', 0, 1, 2),
    ('Thomas Jefferson', 0, 1, 2),
    ('John Adams', 0, 1, 2);

-- Câu hỏi 3 (Quiz 1 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('1492', 1, 1, 3), -- Câu trả lời đúng
    ('1521', 0, 1, 3),
    ('1607', 0, 1, 3),
    ('1620', 0, 1, 3);

-- Câu hỏi 4 (Quiz 1 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Battle of Hastings', 1, 1, 4), -- Câu trả lời đúng
    ('Battle of Agincourt', 0, 1, 4),
    ('Battle of Thermopylae', 0, 1, 4),
    ('Battle of Waterloo', 0, 1, 4);

-- Câu hỏi 5 (Quiz 1 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Joseph Stalin', 1, 1, 5), -- Câu trả lời đúng
    ('Adolf Hitler', 0, 1, 5),
    ('Winston Churchill', 0, 1, 5),
    ('Franklin D. Roosevelt', 0, 1, 5);

-- Câu hỏi 6 (Quiz 2 - Math Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('3.14', 1, 1, 6), -- Câu trả lời đúng
    ('2.71', 0, 1, 6),
    ('1.41', 0, 1, 6),
    ('2.22', 0, 1, 6);

-- Câu hỏi 7 (Quiz 2 - Math Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('x = 5', 1, 1, 7), -- Câu trả lời đúng
    ('x = 7', 0, 1, 7),
    ('x = 4', 0, 1, 7),
    ('x = 9', 0, 1, 7);

-- Câu hỏi 8 (Quiz 2 - Math Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('12', 1, 1, 8), -- Câu trả lời đúng
    ('10', 0, 1, 8),
    ('15', 0, 1, 8),
    ('9', 0, 1, 8);

-- Câu hỏi 9 (Quiz 2 - Math Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('35', 1, 1, 9), -- Câu trả lời đúng
    ('40', 0, 1, 9),
    ('30', 0, 1, 9),
    ('25', 0, 1, 9);

-- Câu hỏi 10 (Quiz 2 - Math Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('24', 1, 1, 10), -- Câu trả lời đúng
    ('32', 0, 1, 10),
    ('20', 0, 1, 10),
    ('18', 0, 1, 10);

-- Câu hỏi 11 (Quiz 3 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('H2O', 1, 1, 11), -- Câu trả lời đúng
    ('CO2', 0, 1, 11),
    ('N2', 0, 1, 11),
    ('O2', 0, 1, 11);

-- Câu hỏi 12 (Quiz 3 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Albert Einstein', 1, 1, 12), -- Câu trả lời đúng
    ('Isaac Newton', 0, 1, 12),
    ('Galileo Galilei', 0, 1, 12),
    ('Marie Curie', 0, 1, 12);

-- Câu hỏi 13 (Quiz 3 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('CO2', 1, 1, 13), -- Câu trả lời đúng
    ('O2', 0, 1, 13),
    ('N2', 0, 1, 13),
    ('H2O', 0, 1, 13);

-- Câu hỏi 14 (Quiz 3 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Diamond', 1, 1, 14), -- Câu trả lời đúng
    ('Quartz', 0, 1, 14),
    ('Topaz', 0, 1, 14),
    ('Sapphire', 0, 1, 14);

-- Câu hỏi 15 (Quiz 3 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Evaporation', 1, 1, 15), -- Câu trả lời đúng
    ('Condensation', 0, 1, 15),
    ('Melting', 0, 1, 15),
    ('Freezing', 0, 1, 15);

-- Câu hỏi 16 (Quiz 4 - Literature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('William Shakespeare', 1, 1, 16), -- Câu trả lời đúng
    ('Charles Dickens', 0, 1, 16),
    ('Mark Twain', 0, 1, 16),
    ('Jane Austen', 0, 1, 16);

-- Câu hỏi 17 (Quiz 4 - Literature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Sherlock Holmes', 1, 1, 17), -- Câu trả lời đúng
    ('Harry Potter', 0, 1, 17),
    ('Huckleberry Finn', 0, 1, 17),
    ('Tom Sawyer', 0, 1, 17);

-- Câu hỏi 18 (Quiz 4 - Literature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('It was the best of times, it was the worst of times.', 1, 1, 18), -- Câu trả lời đúng
    ('Once upon a time...', 0, 1, 18),
    ('In a hole in the ground there lived a hobbit.', 0, 1, 18),
    ('Call me Ishmael.', 0, 1, 18);

-- Câu hỏi 19 (Quiz 4 - Literature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Jane Austen', 1, 1, 19), -- Câu trả lời đúng
    ('Emily Brontë', 0, 1, 19),
    ('Charlotte Brontë', 0, 1, 19),
    ('Louisa May Alcott', 0, 1, 19);

-- Câu hỏi 20 (Quiz 4 - Literature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Sauron', 1, 1, 20), -- Câu trả lời đúng
    ('Gandalf', 0, 1, 20),
    ('Frodo Baggins', 0, 1, 20),
    ('Aragorn', 0, 1, 20);

-- Câu hỏi 21 (Quiz 5 - Language Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Japanese', 1, 1, 21), -- Câu trả lời đúng
    ('Chinese', 0, 1, 21),
    ('Korean', 0, 1, 21),
    ('Thai', 0, 1, 21);

-- Câu hỏi 22 (Quiz 5 - Language Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Portuguese', 1, 1, 22), -- Câu trả lời đúng
    ('Spanish', 0, 1, 22),
    ('Italian', 0, 1, 22),
    ('French', 0, 1, 22);

-- Câu hỏi 23 (Quiz 5 - Language Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Hola', 1, 1, 23), -- Câu trả lời đúng
    ('Bonjour', 0, 1, 23),
    ('Hello', 0, 1, 23),
    ('Ciao', 0, 1, 23);

-- Câu hỏi 24 (Quiz 5 - Language Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('France', 1, 1, 24), -- Câu trả lời đúng
    ('Germany', 0, 1, 24),
    ('Spain', 0, 1, 24),
    ('China', 0, 1, 24);

-- Câu hỏi 25 (Quiz 5 - Language Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Cyrillic', 1, 1, 25), -- Câu trả lời đúng
    ('Latin', 0, 1, 25),
    ('Katakana', 0, 1, 25),
    ('Hangul', 0, 1, 25);

-- Câu hỏi 26 (Quiz 6 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Amazon River', 1, 1, 26), -- Câu trả lời đúng
    ('Nile River', 0, 1, 26),
    ('Mississippi River', 0, 1, 26),
    ('Yangtze River', 0, 1, 26);

-- Câu hỏi 27 (Quiz 6 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Mount Everest', 1, 1, 27), -- Câu trả lời đúng
    ('K2', 0, 1, 27),
    ('Kilimanjaro', 0, 1, 27),
    ('Matterhorn', 0, 1, 27);

-- Câu hỏi 28 (Quiz 6 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Australia', 1, 1, 28), -- Câu trả lời đúng
    ('South America', 0, 1, 28),
    ('Africa', 0, 1, 28),
    ('Asia', 0, 1, 28);

-- Câu hỏi 29 (Quiz 6 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Canada', 1, 1, 29), -- Câu trả lời đúng
    ('United States', 0, 1, 29),
    ('Russia', 0, 1, 29),
    ('China', 0, 1, 29);

-- Câu hỏi 30 (Quiz 6 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Egypt', 1, 1, 30), -- Câu trả lời đúng
    ('Brazil', 0, 1, 30),
    ('India', 0, 1, 30),
    ('Mexico', 0, 1, 30);

-- Câu hỏi 31 (Quiz 7 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Soccer', 1, 1, 31), -- Câu trả lời đúng
    ('Basketball', 0, 1, 31),
    ('Tennis', 0, 1, 31),
    ('Baseball', 0, 1, 31);

-- Câu hỏi 32 (Quiz 7 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('New England Patriots', 1, 1, 32), -- Câu trả lời đúng
    ('Dallas Cowboys', 0, 1, 32),
    ('Green Bay Packers', 0, 1, 32),
    ('Kansas City Chiefs', 0, 1, 32);

-- Câu hỏi 33 (Quiz 7 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('NBA', 1, 1, 33), -- Câu trả lời đúng
    ('NFL', 0, 1, 33),
    ('MLB', 0, 1, 33),
    ('NHL', 0, 1, 33);

-- Câu hỏi 34 (Quiz 7 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Serena Williams', 1, 1, 34), -- Câu trả lời đúng
    ('Maria Sharapova', 0, 1, 34),
    ('Venus Williams', 0, 1, 34),
    ('Simona Halep', 0, 1, 34);

-- Câu hỏi 35 (Quiz 7 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Muhammad Ali', 1, 1, 35), -- Câu trả lời đúng
    ('Mike Tyson', 0, 1, 35),
    ('Floyd Mayweather', 0, 1, 35),
    ('George Foreman', 0, 1, 35);

-- Câu hỏi 36 (Quiz 8 - Music Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Ludwig van Beethoven', 1, 1, 36), -- Câu trả lời đúng
    ('Wolfgang Amadeus Mozart', 0, 1, 36),
    ('Johann Sebastian Bach', 0, 1, 36),
    ('Pyotr Ilyich Tchaikovsky', 0, 1, 36);

-- Câu hỏi 37 (Quiz 8 - Music Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Guitar', 1, 1, 37), -- Câu trả lời đúng
    ('Piano', 0, 1, 37),
    ('Violin', 0, 1, 37),
    ('Drums', 0, 1, 37);

-- Câu hỏi 38 (Quiz 8 - Music Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Michael Jackson', 1, 1, 38), -- Câu trả lời đúng
    ('Elvis Presley', 0, 1, 38),
    ('Madonna', 0, 1, 38),
    ('Whitney Houston', 0, 1, 38);

-- Câu hỏi 39 (Quiz 8 - Music Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Rock', 1, 1, 39), -- Câu trả lời đúng
    ('Pop', 0, 1, 39),
    ('Hip-hop', 0, 1, 39),
    ('Country', 0, 1, 39);

-- Câu hỏi 40 (Quiz 8 - Music Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Bohemian Rhapsody', 1, 1, 40), -- Câu trả lời đúng
    ('Thriller', 0, 1, 40),
    ('Imagine', 0, 1, 40),
    ('Billie Jean', 0, 1, 40);

-- Câu hỏi 41 (Quiz 9 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('United Kingdom', 1, 1, 41), -- Câu trả lời đúng
    ('France', 0, 1, 41),
    ('Germany', 0, 1, 41),
    ('Spain', 0, 1, 41);

-- Câu hỏi 42 (Quiz 9 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('World War II', 1, 1, 42), -- Câu trả lời đúng
    ('World War I', 0, 1, 42),
    ('Vietnam War', 0, 1, 42),
    ('Korean War', 0, 1, 42);

-- Câu hỏi 43 (Quiz 9 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Nelson Mandela', 1, 1, 43), -- Câu trả lời đúng
    ('Mahatma Gandhi', 0, 1, 43),
    ('Martin Luther King Jr.', 0, 1, 43),
    ('Winston Churchill', 0, 1, 43);

-- Câu hỏi 44 (Quiz 9 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Leonardo da Vinci', 1, 1, 44), -- Câu trả lời đúng
    ('Michelangelo', 0, 1, 44),
    ('Pablo Picasso', 0, 1, 44),
    ('Vincent van Gogh', 0, 1, 44);

-- Câu hỏi 45 (Quiz 9 - History Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Berlin Wall', 1, 1, 45), -- Câu trả lời đúng
    ('Great Wall of China', 0, 1, 45),
    ('Hadrian''s Wall', 0, 1, 45),
    ('Western Wall', 0, 1, 45);

-- Câu hỏi 46 (Quiz 10 - General Knowledge Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Venus', 1, 1, 46), -- Câu trả lời đúng
    ('Mars', 0, 1, 46),
    ('Jupiter', 0, 1, 46),
    ('Saturn', 0, 1, 46);

-- Câu hỏi 47 (Quiz 10 - General Knowledge Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Mona Lisa', 1, 1, 47), -- Câu trả lời đúng
    ('Starry Night', 0, 1, 47),
    ('The Last Supper', 0, 1, 47),
    ('Girl with a Pearl Earring', 0, 1, 47);

-- Câu hỏi 48 (Quiz 10 - General Knowledge Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Marie Curie', 1, 1, 48), -- Câu trả lời đúng
    ('Albert Einstein', 0, 1, 48),
    ('Isaac Newton', 0, 1, 48),
    ('Galileo Galilei', 0, 1, 48);

-- Câu hỏi 49 (Quiz 10 - General Knowledge Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Monaco', 1, 1, 49), -- Câu trả lời đúng
    ('Liechtenstein', 0, 1, 49),
    ('Andorra', 0, 1, 49),
    ('San Marino', 0, 1, 49);

-- Câu hỏi 50 (Quiz 10 - General Knowledge Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Albert Einstein', 1, 1, 50), -- Câu trả lời đúng
    ('Isaac Newton', 0, 1, 50),
    ('Marie Curie', 0, 1, 50),
    ('Nikola Tesla', 0, 1, 50);

-- Câu hỏi 51 (Quiz 11 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('DNA', 1, 1, 51), -- Câu trả lời đúng
    ('RNA', 0, 1, 51),
    ('ATP', 0, 1, 51),
    ('NAD+', 0, 1, 51);

-- Câu hỏi 52 (Quiz 11 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Protons and neutrons', 1, 1, 52), -- Câu trả lời đúng
    ('Electrons', 0, 1, 52),
    ('Neutrons and electrons', 0, 1, 52),
    ('Protons and electrons', 0, 1, 52);

-- Câu hỏi 53 (Quiz 11 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Carbon dioxide (CO2)', 1, 1, 53), -- Câu trả lời đúng
    ('Oxygen (O2)', 0, 1, 53),
    ('Nitrogen (N2)', 0, 1, 53),
    ('Methane (CH4)', 0, 1, 53);

-- Câu hỏi 54 (Quiz 11 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Galaxy', 1, 1, 54), -- Câu trả lời đúng
    ('Planet', 0, 1, 54),
    ('Nebula', 0, 1, 54),
    ('Star', 0, 1, 54);

-- Câu hỏi 55 (Quiz 11 - Science Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Photosynthesis', 1, 1, 55), -- Câu trả lời đúng
    ('Cellular respiration', 0, 1, 55),
    ('Fermentation', 0, 1, 55),
    ('Transpiration', 0, 1, 55);

-- Câu hỏi 56 (Quiz 12 - Technology Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Steve Jobs', 1, 1, 56), -- Câu trả lời đúng
    ('Bill Gates', 0, 1, 56),
    ('Mark Zuckerberg', 0, 1, 56),
    ('Elon Musk', 0, 1, 56);

-- Câu hỏi 57 (Quiz 12 - Technology Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Google', 1, 1, 57), -- Câu trả lời đúng
    ('Facebook', 0, 1, 57),
    ('Amazon', 0, 1, 57),
    ('Apple', 0, 1, 57);

-- Câu hỏi 58 (Quiz 12 - Technology Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('RAM', 1, 1, 58), -- Câu trả lời đúng
    ('ROM', 0, 1, 58),
    ('CPU', 0, 1, 58),
    ('GPU', 0, 1, 58);

-- Câu hỏi 59 (Quiz 12 - Technology Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Internet Protocol', 1, 1, 59), -- Câu trả lời đúng
    ('HyperText Transfer Protocol', 0, 1, 59),
    ('Simple Mail Transfer Protocol', 0, 1, 59),
    ('File Transfer Protocol', 0, 1, 59);

-- Câu hỏi 60 (Quiz 12 - Technology Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Artificial Intelligence', 1, 1, 60), -- Câu trả lời đúng
    ('Virtual Reality', 0, 1, 60),
    ('Augmented Reality', 0, 1, 60),
    ('Machine Learning', 0, 1, 60);

-- Câu hỏi 61 (Quiz 13 - Nature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Polar bear', 1, 1, 61), -- Câu trả lời đúng
    ('Grizzly bear', 0, 1, 61),
    ('Black bear', 0, 1, 61),
    ('Koala bear', 0, 1, 61);

-- Câu hỏi 62 (Quiz 13 - Nature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('The Great Barrier Reef', 1, 1, 62), -- Câu trả lời đúng
    ('The Galapagos Islands', 0, 1, 62),
    ('Yellowstone National Park', 0, 1, 62),
    ('The Serengeti', 0, 1, 62);

-- Câu hỏi 63 (Quiz 13 - Nature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Photosynthesis', 1, 1, 63), -- Câu trả lời đúng
    ('Respiration', 0, 1, 63),
    ('Transpiration', 0, 1, 63),
    ('Pollination', 0, 1, 63);

-- Câu hỏi 64 (Quiz 13 - Nature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Coral', 1, 1, 64), -- Câu trả lời đúng
    ('Plant', 0, 1, 64),
    ('Mammal', 0, 1, 64),
    ('Bird', 0, 1, 64);

-- Câu hỏi 65 (Quiz 13 - Nature Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Tsunami', 1, 1, 65), -- Câu trả lời đúng
    ('Earthquake', 0, 1, 65),
    ('Volcano eruption', 0, 1, 65),
    ('Hurricane', 0, 1, 65);

-- Câu hỏi 66 (Quiz 14 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Amazon River', 1, 1, 66), -- Câu trả lời đúng
    ('Nile River', 0, 1, 66),
    ('Yangtze River', 0, 1, 66),
    ('Mississippi River', 0, 1, 66);

-- Câu hỏi 67 (Quiz 14 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Mount Everest', 1, 1, 67), -- Câu trả lời đúng
    ('Kilimanjaro', 0, 1, 67),
    ('Mount McKinley (Denali)', 0, 1, 67),
    ('Mount Fuji', 0, 1, 67);

-- Câu hỏi 68 (Quiz 14 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Asia', 1, 1, 68), -- Câu trả lời đúng
    ('Africa', 0, 1, 68),
    ('North America', 0, 1, 68),
    ('Europe', 0, 1, 68);

-- Câu hỏi 69 (Quiz 14 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Sahara Desert', 1, 1, 69), -- Câu trả lời đúng
    ('Arabian Desert', 0, 1, 69),
    ('Gobi Desert', 0, 1, 69),
    ('Kalahari Desert', 0, 1, 69);

-- Câu hỏi 70 (Quiz 14 - Geography Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Australia', 1, 1, 70), -- Câu trả lời đúng
    ('Antarctica', 0, 1, 70),
    ('South America', 0, 1, 70),
    ('Africa', 0, 1, 70);

-- Câu hỏi 71 (Quiz 15 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Michael Jordan', 1, 1, 71), -- Câu trả lời đúng
    ('LeBron James', 0, 1, 71),
    ('Kobe Bryant', 0, 1, 71),
    ('Magic Johnson', 0, 1, 71);

-- Câu hỏi 72 (Quiz 15 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('FIFA World Cup', 1, 1, 72), -- Câu trả lời đúng
    ('Super Bowl', 0, 1, 72),
    ('Olympic Games', 0, 1, 72),
    ('NBA Finals', 0, 1, 72);

-- Câu hỏi 73 (Quiz 15 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Roger Federer', 1, 1, 73), -- Câu trả lời đúng
    ('Rafael Nadal', 0, 1, 73),
    ('Novak Djokovic', 0, 1, 73),
    ('Serena Williams', 0, 1, 73);

-- Câu hỏi 74 (Quiz 15 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Baseball', 1, 1, 74), -- Câu trả lời đúng
    ('Soccer', 0, 1, 74),
    ('Basketball', 0, 1, 74),
    ('Hockey', 0, 1, 74);

-- Câu hỏi 75 (Quiz 15 - Sports Quiz)
INSERT INTO [dbo].[Answer] ([Content], [IsCorrect], [IsActive], [QuestionId])
VALUES
    ('Usain Bolt', 1, 1, 75), -- Câu trả lời đúng
    ('Carl Lewis', 0, 1, 75),
    ('Michael Johnson', 0, 1, 75),
    ('Jesse Owens', 0, 1, 75);









