
using AutoMapper;
using Smart.Application.Attributes;
using Smart.Application.Files.Dto;
using Smart.Application.Files.interfaces;
using Smart.Application.Files.Models;
using Smart.Domain.Files.Interfaces;
using Smart.Infrastructure.Authenticate;
using Smart.Infrastructure.Configs;
using Smart.Shared.Dto;
using Smart.Infrastructure.Freesql.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Panda.DynamicWebApi.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Polly;

namespace Smart.Application.Files
{
    /// <summary>
    /// 文件服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "Smart")]
    [DynamicWebApi]
    [SMAuthorize]
    public class SysFileService : BaseService<SysFileService>, ISysFileService
    {
        private const string TEMP_FOLDER = "temps";
        private readonly HttpContext context;
        private readonly ISysFileManager _sysFileManager;
        private readonly AppConfig _appConfig;
        private readonly IUser _user;
        private readonly IMapper _mapper;

        private readonly object _uploading;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessor"></param>
        /// <param name="sysFileManager"></param>
        /// <param name="appConfig"></param>
        /// <param name="user"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public SysFileService(IHttpContextAccessor accessor, ISysFileManager sysFileManager, AppConfig appConfig, IUser user, IMapper mapper, ILogger<SysFileService> logger) : base(logger)
        {
            context = accessor?.HttpContext ?? throw new ArgumentNullException(nameof(accessor));
            _sysFileManager = sysFileManager;
            _appConfig = appConfig;
            _user = user;
            _uploading = true;
            _mapper = mapper;
            var path = Path.Combine(_appConfig.FilePath, TEMP_FOLDER);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }
        /// <summary>
        /// 获取指定ID
        ///</summary>
        ///<param name="id"></param>
        /// <returns></returns>
        public IResponseOutput Get(long id)
        {
            var model = _sysFileManager.Get(id);
            var result = _mapper.Map<SysFileOutputDto>(model);
            return new ResponseOutput<SysFileOutputDto>(result);
        }
        /// <summary>
        /// 获取所有
        ///</summary>
        ///<param name="input"></param>
        /// <returns></returns>
        public IResponseOutput GetAll(SysFileInputDto input)
        {
            var result = _sysFileManager.GetAll(input.PageIndex, input.PageSize);
            var dtos = new List<SysFileOutputDto>();
            foreach (var item in result)
            {
                dtos.Add(_mapper.Map<SysFileOutputDto>(item));
            }
            var count = _sysFileManager.Count();
            var output = new SysFileListOutputDto { List = dtos, TotalPageSize = count };
            return new ResponseOutput<SysFileListOutputDto>(output);
        }

        /// <summary>
        /// 编辑
        ///</summary>
        ///<param name="dto"></param>
        /// <returns></returns>
        public SysFile RegisterOrUpdate(SysFileOutputDto dto)
        {
            var result = _sysFileManager.CreatOrUpdate(_mapper.Map<SysFile>(dto));
            return result;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="md5">md5</param>
        /// <returns></returns>
        public IResponseOutput GetFile(string md5)
        {
            var sysFile = _sysFileManager.GetForMd5(md5);
            var result = _mapper.Map<SysFileOutputDto>(sysFile);
            return Ok(result);
        }

        #region js前端上传
        /// <summary>
        /// 上传文件分片
        /// </summary>
        /// <param name="chunk">分片</param>
        /// <returns></returns>
        [DisableFormValueModelBinding]
        public async Task<IResponseOutput> Upload([FromQuery] FileChunk chunk)
        {
            //TODO 定时任务删除date=1天前的，used=flase无用的文件
            if (!IsMultipartContentType(context.Request.ContentType))
            {
                return Error("类型异常");
            }

            var boundary = GetBoundary();
            if (string.IsNullOrEmpty(boundary))
            {
                return Error("空文件");
            }

            SysFile sysFile;
            lock (_uploading)
            {
                sysFile = _sysFileManager.GetForMd5(chunk.Md5);
                if (sysFile != null)
                {
                    if (sysFile.Uploaded)
                    {
                        //已经上传，无需再上传
                        return new ResponseOutput<FileOutput>(new FileOutput { IsUploaded = true, Msg = $"{chunk.FileName},秒传成功!" });
                    }
                }
                else
                {
                    sysFile = _sysFileManager.CreatOrUpdate(new SysFile
                    {
                        Md5 = chunk.Md5,
                        FileName = chunk.FileName,
                        FileDate = DateTime.Now.ToString("yyyyMMdd"),
                        Uploaded = false
                    });
                }
            }


            var reader = new MultipartReader(boundary, context.Request.Body);

            var section = await reader.ReadNextSectionAsync();
            while (section != null)
            {
                var buffer = new byte[chunk.Size];
                var path = Path.Combine(_appConfig.FilePath, TEMP_FOLDER, GetPartName(chunk));
                using (var stream = new FileStream(path, FileMode.Append))
                {
                    int bytesRead;
                    do
                    {
                        bytesRead = await section.Body.ReadAsync(buffer, 0, buffer.Length);
                        stream.Write(buffer, 0, bytesRead);

                    } while (bytesRead > 0);
                }

                section = await reader.ReadNextSectionAsync();
            }

            //合并文件（可能涉及转码等）
            if (chunk.PartNumber == chunk.Chunks)
            {
                await MergeChunkFile(sysFile, chunk);
                lock (_uploading)
                {
                    sysFile = _sysFileManager.GetForMd5(chunk.Md5);
                    //已有用户成功上传
                    if (sysFile.Uploaded)
                    {
                        var fileFullPath = GetFilePath(sysFile, chunk);
                        if (File.Exists(fileFullPath))
                        {
                            File.Delete(fileFullPath);
                        }
                        return new ResponseOutput<FileOutput>(new FileOutput { IsUploaded = true, Msg = $"{chunk.FileName},上传成功!" });
                    }
                    else
                    {
                        sysFile.Uploaded = true;
                        _sysFileManager.CreatOrUpdate(sysFile);
                    }
                }
                return new ResponseOutput<FileOutput>(new FileOutput { IsUploaded = true, Msg = $"{chunk.FileName},上传成功!" });
            }
            return new ResponseOutput<FileOutput>(new FileOutput { IsUploaded = false, Msg = "文件处理中..." });
        }

        #region 文件分段储存、合并
        /// <summary>
        /// 分片文件名
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        private string GetPartName(FileChunk chunk)
        {
            //用户ID,防止用户并发上传同一文件
            return $"{_user.Id}.{chunk.Md5}{Path.GetExtension(chunk.FileName)}{FileSort.PART_NUMBER}{chunk.PartNumber}";
        }

        private string GetFullPath(SysFile file)
        {
            var userId = file.CreatedUserId == null ? _user.Id : file.CreatedUserId;
            var path = Path.Combine(_appConfig.FilePath, file.FileDate, userId.ToString());
            return path;
        }
        private string GetFilePath(SysFile file, FileChunk chunk)
        {
            var path = Path.Combine(GetFullPath(file),  $"{chunk.Md5}{Path.GetExtension(chunk.FileName)}");
            return path;
        }
        private bool IsMultipartContentType(string contentType)
        {
            return
                !string.IsNullOrEmpty(contentType) &&
                contentType.IndexOf("multipart/", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string GetBoundary()
        {
            var mediaTypeHeaderContentType = MediaTypeHeaderValue.Parse(context.Request.ContentType);

            return HeaderUtilities.RemoveQuotes(mediaTypeHeaderContentType.Boundary).Value;
        }

        private string GetFileName(string contentDisposition)
        {
            return contentDisposition
                .Split(';')
                .SingleOrDefault(part => part.Contains("filename"))
                .Split('=')
                .Last()
                .Trim('"');
        }

        private async Task MergeChunkFile(SysFile sysFile,FileChunk chunk)
        {
            var uploadDirectoryName = Path.Combine(_appConfig.FilePath, TEMP_FOLDER);

            var partToken = FileSort.PART_NUMBER;
            var partName = GetPartName(chunk);
            var baseFileName = partName.Substring(0, partName.IndexOf(partToken));

            var searchpattern = $"{Path.GetFileName(baseFileName)}{partToken}*";

            var filesList = Directory.GetFiles(uploadDirectoryName, searchpattern);
            if (!filesList.Any()) { return; }

            var mergeFiles = new List<FileSort>();

            foreach (string fileName in filesList)
            {
                var fileNameNumber = fileName.Substring(fileName.IndexOf(FileSort.PART_NUMBER)
                    + FileSort.PART_NUMBER.Length);

                int.TryParse(fileNameNumber, out var number);
                if (number <= 0)
                {
                    continue;
                }

                mergeFiles.Add(new FileSort
                {
                    FileName = fileName,
                    PartNumber = number
                });
            }

            // 按照分片排序
            var mergeFileSorts = mergeFiles.OrderBy(s => s.PartNumber).ToArray();

            // 合并文件
            var fileFullPath = GetFilePath(sysFile, chunk);
            if (!Directory.Exists(GetFullPath(sysFile)))
            {
                Directory.CreateDirectory(GetFullPath(sysFile));
            }
            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
            }
            await Policy.Handle<IOException>()
                    .RetryForeverAsync()
                    .ExecuteAsync(async () =>
                    {
                        using (var fileStream = new FileStream(fileFullPath, FileMode.Create))
                        {
                            foreach (FileSort fileSort in mergeFileSorts)
                            {
                                using (FileStream fileChunk = new FileStream(fileSort.FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    await fileChunk.CopyToAsync(fileStream);
                                }
                            }
                        };
                    });


            //删除分片文件
            Parallel.ForEach(mergeFiles, f =>
            {
                System.IO.File.Delete(f.FileName);
            });
        }
        #endregion

        #endregion

        #region blazor前端 上传
        /// <summary>
        /// blazor上传附件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> Upload2Async(FileInput input)
        {
            SysFile sysFile;
            lock (_uploading)
            {
                sysFile = _sysFileManager.GetForMd5(input.Md5);
                if (sysFile != null)
                {
                    if (sysFile.Uploaded)
                    {
                        //已经上传，无需再上传
                        return new ResponseOutput<FileOutput>(new FileOutput { IsUploaded = true, Msg = "秒传成功!" });
                    }
                }
                else
                {
                    sysFile = _sysFileManager.CreatOrUpdate(new SysFile
                    {
                        Md5 = input.Md5,
                        FileName = GetFileName(input),
                        FileDate = DateTime.Now.ToString("yyyyMMdd"),
                        Uploaded = false
                    });
                }
            }

            if (input.IsComplate)
            {
                //合并切片文件
                var filesList = Directory.GetFiles(GetTempPath(input));

                using (var fileStream = new FileStream(GetFileFullPath(input), FileMode.Create))
                {
                    for (var i = 0; i < filesList.Length; i++)
                    {
                        using (FileStream fileChunk = new FileStream(GetTempFullPath(input, i), FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            await fileChunk.CopyToAsync(fileStream);
                        }
                    }
                };
                Directory.Delete(GetTempPath(input), true);

                //上传完成
                sysFile.Uploaded = true;
                _sysFileManager.CreatOrUpdate(sysFile);
                return new ResponseOutput<FileOutput>(new FileOutput { IsUploaded = true, Msg = "上传成功!" });
            }
            else
            {
                //保存切片
                using (var stream = new FileStream(GetTempFullPath(input, input.ChunkIndex), FileMode.Create))
                {
                    stream.Write(input.Stream, 0, input.Stream.Length);
                }
            }
            return new ResponseOutput<FileOutput>(new FileOutput { IsUploaded = false, Msg = "上传中!" });
        }
        /// <summary>
        /// 切片文件
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetTempFullPath(FileInput input, int index)
        {
            return Path.Combine(GetTempPath(input), index + ".tmp");
        }

        /// <summary>
        /// 切片目录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetTempPath(FileInput input)
        {
            var path = Path.Combine(_appConfig.FilePath,TEMP_FOLDER, input.Guid);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
        /// <summary>
        /// 完整文件路径
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetFileFullPath(FileInput input)
        {
            return Path.Combine(_appConfig.FilePath, input.Md5 + Path.GetExtension(input.FileName));
        }
        /// <summary>
        /// 文件名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetFileName(FileInput input)
        {
            return Path.Combine(input.Guid + Path.GetExtension(input.FileName));
        }
        #endregion
    }
}
