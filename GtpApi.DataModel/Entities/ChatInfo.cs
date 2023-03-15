using System.ComponentModel.DataAnnotations;

namespace GtpApi.DataModel.Entities
{
    public class ChatInfo
    {
        [Key]
        public Guid Id { get; set; }
        public string ChatRequest { get; set; }
        public string? ChatSetupMessage { get; set; }
        public string ChatResponse { get; set; }
        public string Model { get; set; }
        public double Temperature { get; set; }
        public int MaxTokens { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public int? QuestionTokenAmount { get; set; }
        public int? ResponseTokenAmount { get; set; }
        public int? TotalTokenAmount { get; set; }
        public DateTime RequestDateTime { get; set; }
    }
}
