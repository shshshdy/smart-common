using System;

namespace Smart.Infrastructure.Freesql.Entities
{
    public interface IEntityUpdate
    {
        long? ModifiedUserId { get; set; }
        string ModifiedUserName { get; set; }
        DateTime? ModifiedTime { get; set; }
    }
}
