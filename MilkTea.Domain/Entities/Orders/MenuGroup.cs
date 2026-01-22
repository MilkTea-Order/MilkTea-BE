using MilkTea.Domain.Entities.Users;

namespace MilkTea.Domain.Entities.Orders
{
    public class MenuGroup : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public int StatusID { get; set; }

        public Status? Status { get; set; }
    }
}
