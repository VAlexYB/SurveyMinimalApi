namespace SurveyMinimalApi.DTOs
{
    public record AnswerSubmissionDto
    (
        Guid QuestionId,
        IEnumerable<Guid> AnswerIds
    );

}
