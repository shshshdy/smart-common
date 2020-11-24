using System.Collections.Generic;
using Smart.Application.Interfaces;

namespace Smart.Application.Files.Dto
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
