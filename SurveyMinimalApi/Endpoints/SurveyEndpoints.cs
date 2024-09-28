using SurveyMinimalApi.DTOs;
using SurveyMinimalApi.Services;

namespace SurveyMinimalApi.Endpoints
{
    public static class SurveyEndpoints
    {
        public static void MapSurveyEndpoints(this WebApplication app)
        {
            app.MapGet("/questions/{questionId}", async (Guid questionId, ISurveyService surveyService) =>
            {
                var question = await surveyService.GetQuestionWithAnswersAsync(questionId);
                if (question == null)
                    return Results.NotFound("Question not found");

                return Results.Ok(question);
            });

            app.MapPost("/interviews/{interviewId}/answer", async (Guid interviewId, AnswerSubmissionDto submission, ISurveyService surveyService) =>
            {

                var nextQuestionId = await surveyService.SaveAnswerAsync(interviewId, submission.QuestionId, submission.AnswerIds);

                if (nextQuestionId == Guid.Empty)
                    return Results.NotFound("No more questions available");

                return Results.Ok(new { NextQuestionId = nextQuestionId });
            });

        }
    }
}
