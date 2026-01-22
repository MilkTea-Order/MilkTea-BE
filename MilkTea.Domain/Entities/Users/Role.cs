namespace MilkTea.Domain.Entities.Users
{
    public class Role : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string? Note { get; set; }
        public int StatusID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        // Navigation
        public Status? Status { get; set; }
    }
}
