using System;
namespace Smart.Application.Users.Dto
{
    public class AuthenticateDto
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string RealName { get; set; }

        public string[] Permissions { get; set; }

        public string Token { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}