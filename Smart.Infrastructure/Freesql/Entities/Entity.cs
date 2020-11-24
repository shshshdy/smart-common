﻿using FreeSql.DataAnnotations;

namespace Smart.Infrastructure.Freesql.Entities
{
    public interface IEntity
    {
    }

    public class Entity<TKey> : IEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Column(Position = 1, IsIdentity = true)]
        public virtual TKey Id { get; set; }
    }

    public class Entity : Entity<long>
    {

    }
}
