using System;
using Smart.Application.Dtos;
using Smart.Domain.Entities.Enums;

namespace Smart.Application.Users.Dto
{
    public class UserOutputDto : BaseOutputDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string RealName { get; set; }

        public string Pinyin { get; set; }

        public string ShortPinyin { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Tel { get; set; }

        public Sex Sex { get; set; }

        public string CreatedUserName { get; set; }

        public DateTime? ModifiedTime { get; set; }
    }
}
