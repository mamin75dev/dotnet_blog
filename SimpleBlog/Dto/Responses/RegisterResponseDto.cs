namespace SimpleBlog.Dto.Responses
{
    public class RegisterResponseDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public RegisterResponseDto(Guid id, string userName, string email, string phoneNumber)
        {
            Id = id;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
