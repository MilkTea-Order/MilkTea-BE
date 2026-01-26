namespace MilkTea.Domain.Users.Entities;


public class UserAndRole
{
    public int UserID { get; set; }
    public int RoleID { get; set; }

    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }


}
