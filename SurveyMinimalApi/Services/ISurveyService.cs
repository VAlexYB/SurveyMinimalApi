using SurveyMinimalApi.DTOs;
using SurveyMinimalApi.Models;

namespace SurveyMinimalApi.Services
{
    public interface ISurveyService
    {
        Task<QuestionWithAnswersDto?> GetQuestionWithAnswersAsync(Guid questionId);
        Task<Guid?> SaveAnswerAsync(Guid interviewId, Guid questionId, IEnumerable<Guid> answerIds);
    }
}
