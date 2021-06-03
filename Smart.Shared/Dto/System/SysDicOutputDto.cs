
using Smart.Shared.Dtos;

namespace Smart.Shared.Systems.Dto
{
    ///<summary>
    ///SysDic输入Dto
    ///</summary>
    public class SysDicOutputDto : BaseOutputDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public string Code { get; set; }
    }
}
