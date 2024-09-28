namespace SurveyMinimalApi.Models
{
    public class Survey
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
