using System.Collections.Generic;
using Smart.Shared.Interfaces;

namespace Smart.Shared.Files.Dto
{
    /// <summary>
    /// �ļ����
    /// </summary>
    public class SysFileListOutputDto : IListOutputDto<SysFileOutputDto>
    {
        /// <summary>
        /// ��ҳ��
        /// </summary>
        public long TotalPageSize { get; set; }
        /// <summary>
        /// ���ݼ�
        /// </summary>
        public IEnumerable<SysFileOutputDto> List { get; set; }
    }
}
