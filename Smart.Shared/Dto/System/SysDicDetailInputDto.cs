
using Smart.Shared.Dtos;

namespace Smart.Shared.Systems.Dto
{
    ///<summary>
    ///SysDicDetail输入Dto
    ///</summary>
    public class SysDicDetailInputDto:InputDto
    {
        /// <summary>
        /// 字典类型
        /// </summary>
        public long Type { get; set; }
        /// <summary>
        /// 字典值
        /// </summary>
        public string Key { get; set; }
    }
}
