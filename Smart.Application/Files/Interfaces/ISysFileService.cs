
using System.Collections.Generic;
using Smart.Application.Interfaces;
using Smart.Application.Files.Dto;
using Smart.Infrastructure.Dto;
using Smart.Application.Files.Models;
using System.Threading.Tasks;

namespace Smart.Application.Files.interfaces
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public interface ISysFileService : IService<SysFileInputDto, long>
    {
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="md5">md5值</param>
        /// <returns></returns>
        IResponseOutput GetFile(string md5);

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="chunk">文件块</param>
        /// <returns></returns>
        Task<IResponseOutput> Upload(FileChunk chunk);
    }
}
