namespace CONSEGO.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public DateTime TimestampUtc { get; set; }

        public int? UserId { get; set; }
        public string? Username { get; set; }

        // Create, Update, Delete
        public string Action { get; set; } = null!;   
        public string Entity { get; set; } = null!;
        public string EntityId { get; set; } = null!;

        // JSON
        public string? Changes { get; set; }          

        public string? CorrelationId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
    }
}