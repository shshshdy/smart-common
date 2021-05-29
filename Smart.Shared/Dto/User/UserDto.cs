using Smart.Domain.Entities.Enums;

namespace Smart.Application.Users.Dto
{
    public class UserDto
    {
        public long Id { get; set; }

        public long? Version { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string RealName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Tel { get; set; }

        public Sex Sex { get; set; }
    }
}
