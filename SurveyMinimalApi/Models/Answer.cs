namespace SurveyMinimalApi.Models
{
    public record Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
