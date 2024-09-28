namespace SurveyMinimalApi.DTOs
{
    public record AnswerDto(
        Guid AnswerId,
        string AnswerText
    );
}
