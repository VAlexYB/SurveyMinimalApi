using Dapper;
using SurveyMinimalApi.DTOs;
using SurveyMinimalApi.Models;
using System.Data;

namespace SurveyMinimalApi.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<SurveyService> _logger;

        public SurveyService(IDbConnection dbConnection, ILogger<SurveyService> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves question with associated answers by given question's ID
        /// </summary>
        /// <param name="questionId"> Unique identifier for question to retrieve</param>
        /// <returns><see cref="QuestionWithAnswersDto"/> contaian question details and list of answers, or null if no question found</returns>
        public async Task<QuestionWithAnswersDto?> GetQuestionWithAnswersAsync(Guid questionId)
        {
            var sql = @"
                SELECT 
                    q.Id AS QuestionId, 
                    q.Text AS QuestionText, 
                    a.Id AS AnswerId, 
                    a.Text AS AnswerText
                FROM Questions q
                LEFT JOIN Answers a ON q.Id = a.QuestionId
                WHERE q.Id = @QuestionId
            ";

            try
            {
                var result = await _dbConnection.QueryAsync<QuestionJoinAnswersDto>(sql, new { QuestionId = questionId });

                if (!result.Any())
                {
                    _logger.LogInformation($"No question found for Id = {questionId}");
                    return null;
                }

                var questionWithAnswersDto = new QuestionWithAnswersDto(
                    result.First().QuestionId,
                    result.First().QuestionText,
                    result.Select(r => new AnswerDto(r.AnswerId, r.AnswerText)).ToList()
                );

                return questionWithAnswersDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured while retrieving question with Id = {questionId}");
                throw;
            }
        }

        /// <summary>
        /// Saves user's answer for specific question at interviw and retrieves next question's Id
        /// </summary>
        /// <param name="interviewId"> Identifier of interview</param>
        /// <param name="questionId"> Identifier of question</param>
        /// <param name="answerIds"> Collection of identifiers choosed answers</param>
        /// <returns>Next question's ID or null if no question found</returns>
        public async Task<Guid?> SaveAnswerAsync(Guid interviewId, Guid questionId, IEnumerable<Guid> answerIds)
        {
            using (var connection = _dbConnection)
            {
                connection.Open();

                using (var transaction = _dbConnection.BeginTransaction())
                {
                    try
                    {
                        var resultSql = @"
                            INSERT INTO Results (Id, InterviewId, QuestionId) 
                            VALUES (gen_random_uuid(), @InterviewId, @QuestionId)
                            RETURNING Id;
                        ";

                        var resultId = await _dbConnection.QuerySingleOrDefaultAsync<Guid>(resultSql, new { InterviewId = interviewId, QuestionId = questionId }, transaction);

                        var resultAnswersSql = @"
                            INSERT INTO ResultAnswers (Id, ResultId, AnswerId) 
                            VALUES (gen_random_uuid(), @ResultId, @AnswerId);
                        ";

                        foreach (var answerId in answerIds)
                        {
                            await _dbConnection.ExecuteAsync(resultAnswersSql, new { ResultId = resultId, AnswerId = answerId }, transaction);
                        }

                        var nextQuestionSql = @"
                            SELECT Id FROM Questions
                            WHERE SurveyId = (SELECT SurveyId FROM Questions WHERE Id = @QuestionId)
                            AND QueueNumb > (SELECT QueueNumb FROM Questions WHERE Id = @QuestionId)
                            ORDER BY QueueNumb
                            LIMIT 1;
                        ";

                        var nextQuestionId = await _dbConnection.QuerySingleOrDefaultAsync<Guid?>(nextQuestionSql, new { QuestionId = questionId }, transaction);

                        transaction.Commit();
                        return nextQuestionId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, $"Error occured in transaction while saving answers for interview with Id = {interviewId} (questionId = {questionId}) and retrieving next question's Id");
                        throw;
                    }
                }
            }
         
        }
    }
}
