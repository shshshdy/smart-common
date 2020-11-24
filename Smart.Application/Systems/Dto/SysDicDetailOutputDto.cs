
using Smart.Application.Dtos;

namespace Smart.Application.Systems.Dto
{
    ///<summary>
    ///SysDicDetail输入Dto
    ///</summary>
    public class SysDicDetailOutputDto : BaseOutputDto
    {
        /// <summary>
        /// 字典名
        /// </summary>
        public string Key { get; set; }
    
        /// <summary>
        /// 字典值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 字典分类ID
        /// </summary>
        public long DicId { get; set; }
        /// <summary>
        /// 字典分类名
        /// </summary>
        public string DicName { get; set; }
    
        /// <summary>
        /// 序号
        /// </summary>
        public long Order { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

    }
}
