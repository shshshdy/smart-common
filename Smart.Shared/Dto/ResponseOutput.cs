using Smart.Infrastructure.Dto;

namespace Smart.Infrastructure.Dto
{
    /// <summary>
    /// api返回值
    /// </summary>
    public class ResponseOutput : IResponseOutput
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; } = 200;

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; } = "OK";
    }

    /// <summary>
    /// api返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseOutput<T> : ResponseOutput,IResponseOutput<T>
    {
        /// <summary>
        /// data
        /// </summary>
        /// <param name="data"></param>
        public ResponseOutput(T data)
        {
            Data = data;
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }
    }
}
