using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Shared.Dtos
{
    /// <summary>
    /// 输出dto
    /// </summary>
    public class OutPutDto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        public OutPutDto(bool success=false)
        {
            this.Success = success;
        }
        /// <summary>
        /// 成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 消息
        /// </summary>

        public string Msg { get; set; }
    }
}
