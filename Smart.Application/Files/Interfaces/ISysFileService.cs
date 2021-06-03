
using System.Collections.Generic;
using Smart.Shared.Files.Dto;
using Smart.Shared.Dto;
using Smart.Application.Files.Models;
using System.Threading.Tasks;
using Smart.Application.Interfaces;

namespace Smart.Application.Files.interfaces
{
    /// <summary>
    /// �ļ�����
    /// </summary>
    public interface ISysFileService : IService<SysFileInputDto, long>
    {
        /// <summary>
        /// ��ȡ�ļ�
        /// </summary>
        /// <param name="md5">md5ֵ</param>
        /// <returns></returns>
        IResponseOutput GetFile(string md5);

        /// <summary>
        /// �ϴ��ļ�
        /// </summary>
        /// <param name="chunk">�ļ���</param>
        /// <returns></returns>
        Task<IResponseOutput> Upload(FileChunk chunk);
    }
}
