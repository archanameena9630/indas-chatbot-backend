using System.Collections.Generic;

namespace JobCardBackend.Models
{
    public class CreateJobCardDto
    {
        public string ClientName { get; set; }
        public string JobName { get; set; }
        public string OrderNo { get; set; }
        public string PaperDetails { get; set; }
        public string? JobSize { get; set; }
        public List<string> Processes { get; set; } = new();
        public int UserId { get; set; }
    }
}
