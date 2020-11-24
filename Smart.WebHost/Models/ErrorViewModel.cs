using System;

namespace Smart.Host.Models
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// ����ID
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// ��ʾ����ID
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
