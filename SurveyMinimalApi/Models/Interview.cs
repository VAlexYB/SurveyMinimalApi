namespace SurveyMinimalApi.Models
{
    public class Interview
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
