namespace Application.DTOs.UserDTOs
{
    public class ManageUserDTO
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }

        public string FullName { get; set; }
    }
}
