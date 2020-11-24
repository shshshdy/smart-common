using System;

namespace Smart.Host.Models
{
    /// <summary>
    /// 错误信息
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// 访问ID
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// 显示访问ID
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
