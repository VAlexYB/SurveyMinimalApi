namespace SurveyMinimalApi.Models
{
    public class Result
    {
        public Guid Id { get; set; }
        public Guid InterviewId { get; set; }
        public Guid QuestionId { get; set; }
        public Guid AnswerId { get; set; }
    }
}
