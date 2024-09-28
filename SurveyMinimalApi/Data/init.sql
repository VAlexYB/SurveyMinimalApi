CREATE TABLE IF NOT EXISTS Surveys (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    Title VARCHAR(255) NOT NULL,
    Description TEXT
);

CREATE TABLE IF NOT EXISTS Questions (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    SurveyId UUID NOT NULL,
    Text TEXT NOT NULL,
    QueueNumb INT NOT NULL,
    FOREIGN KEY (SurveyId) REFERENCES Surveys(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Answers (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    QuestionId UUID NOT NULL,
    Text TEXT NOT NULL,
    IsCorrect BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Interviews (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    SurveyId UUID NOT NULL,
    UserId UUID NOT NULL,
    StartTime TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    EndTime TIMESTAMP,
    FOREIGN KEY (SurveyId) REFERENCES Surveys(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Results (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    QuestionId UUID NOT NULL,
    InterviewId UUID NOT NULL,
    FOREIGN KEY (QuestionId) REFERENCES Questions(Id) ON DELETE CASCADE,
    FOREIGN KEY (InterviewId) REFERENCES Interviews(Id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ResultAnswers (
    Id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    ResultId UUID NOT NULL,
    AnswerId UUID NOT NULL,
    FOREIGN KEY (ResultId) REFERENCES Results(Id) ON DELETE CASCADE,
    FOREIGN KEY (AnswerId) REFERENCES Answers(Id) ON DELETE CASCADE
);


CREATE INDEX IF NOT EXISTS idx_questions_surveyid ON Questions(SurveyId);
CREATE INDEX IF NOT EXISTS idx_answers_questionid ON Answers(QuestionId);
CREATE INDEX IF NOT EXISTS idx_results_interviewid_questionid ON Results(InterviewId, QuestionId);
CREATE INDEX IF NOT EXISTS idx_resultanswers_resultid ON ResultAnswers(ResultId);


INSERT INTO Surveys (Id, Title, Description)
VALUES 
    (gen_random_uuid(), 'Customer Satisfaction Survey', 'A survey to measure customer satisfaction.');

INSERT INTO Questions (Id, SurveyId, Text, QueueNumb)
VALUES 
    (gen_random_uuid(), (SELECT Id FROM Surveys LIMIT 1), 'How satisfied are you with our service?', 1),
    (gen_random_uuid(), (SELECT Id FROM Surveys LIMIT 1), 'Would you recommend our service to others?', 2),
    (gen_random_uuid(), (SELECT Id FROM Surveys LIMIT 1), 'Any other feedback?', 3);

INSERT INTO Answers (Id, QuestionId, Text, IsCorrect)
VALUES
    (gen_random_uuid(), (SELECT Id FROM Questions WHERE Text = 'How satisfied are you with our service?' LIMIT 1), 'Very satisfied', FALSE),
    (gen_random_uuid(), (SELECT Id FROM Questions WHERE Text = 'How satisfied are you with our service?' LIMIT 1), 'Somewhat satisfied', FALSE),
    (gen_random_uuid(), (SELECT Id FROM Questions WHERE Text = 'How satisfied are you with our service?' LIMIT 1), 'Not satisfied', FALSE);

INSERT INTO Answers (Id, QuestionId, Text, IsCorrect)
VALUES
    (gen_random_uuid(), (SELECT Id FROM Questions WHERE Text = 'Would you recommend our service to others?' LIMIT 1), 'Yes', FALSE),
    (gen_random_uuid(), (SELECT Id FROM Questions WHERE Text = 'Would you recommend our service to others?' LIMIT 1), 'No', FALSE);

INSERT INTO Interviews (Id, SurveyId, UserId, StartTime)
VALUES
    (gen_random_uuid(), (SELECT Id FROM Surveys LIMIT 1), gen_random_uuid(), CURRENT_TIMESTAMP);
