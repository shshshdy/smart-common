using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart.Shared.Dto
{
    public class FileOutput
    {
        /// <summary>
        /// 是否上传结束
        /// </summary>
        public bool IsUploaded { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }
}
