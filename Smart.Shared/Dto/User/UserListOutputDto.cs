using Smart.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Application.Users.Dto
{
    public class UserListOutputDto : IListOutputDto<UserOutputDto>
    {
        public long TotalPageSize { get; set; }
        public IEnumerable<UserOutputDto> List { get; set; }
    }
}
