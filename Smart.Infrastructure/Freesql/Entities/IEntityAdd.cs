using System;

namespace Smart.Infrastructure.Freesql.Entities
{
    public interface IEntityAdd
    {
        long? CreatedUserId { get; set; }
        string CreatedUserName { get; set; }
        DateTime? CreatedTime { get; set; }
    }
}
