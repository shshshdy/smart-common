using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Smart.Application.Files
{
    /// <summary>
    /// 文件流
    /// </summary>
    public class FileStreamResult : IActionResult
    {
        readonly Stream _stream;
        readonly string _mediaType;
        readonly string _fileName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="mediaType"></param>
        public FileStreamResult(Stream stream, string mediaType) : this(stream, mediaType, null) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="mediaType"></param>
        /// <param name="fileName"></param>
        public FileStreamResult(Stream stream, string mediaType, string fileName)
        {
            _stream = stream;
            _mediaType = mediaType;
            _fileName = fileName;
        }

        /// <summary>
        /// ExecuteResultAsync
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task ExecuteResultAsync(ActionContext context)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            try
            {
                httpResponseMessage.Content = new StreamContent(_stream);
                httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue(_mediaType);
                if (!string.IsNullOrEmpty(_fileName))
                {
                    httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = HttpUtility.UrlEncode(_fileName, Encoding.UTF8),
                    };
                }
                return httpResponseMessage;
            }
            catch
            {
                httpResponseMessage.Dispose();
                throw;
            }
        }
    }
}
