namespace SurveyMinimalApi.DTOs
{
    public record QuestionWithAnswersDto
    (
        Guid QuestionId,
        string QuestionText,
        IEnumerable<AnswerDto> Answers
    );
}
