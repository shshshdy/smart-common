using System;
namespace Smart.Application.Dtos
{
    /// <summary>
    /// 基础dto
    /// </summary>
    public class BaseOutputDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public long? Version { get; set; }
    }
}
