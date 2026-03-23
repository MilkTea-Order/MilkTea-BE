namespace MilkTea.API.RestfulAPI.DTOs.User.Responses
{
    public class GetUserListResponseDto
    {
        public List<UserDto> Users { get; set; } = new List<UserDto>();
    }

    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
    }
}
