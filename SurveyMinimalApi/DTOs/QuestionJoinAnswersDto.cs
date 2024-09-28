namespace SurveyMinimalApi.DTOs
{
    public record QuestionJoinAnswersDto
    (
        Guid QuestionId,
        string QuestionText,
        Guid AnswerId,
        string AnswerText
    );
}
