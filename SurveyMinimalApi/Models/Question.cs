namespace SurveyMinimalApi.Models
{
    public class Question
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public string Text { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
