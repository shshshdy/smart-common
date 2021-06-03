using Smart.Shared.Dto;
using Panda.DynamicWebApi;
using Smart.Shared.Interfaces;

namespace Smart.Application.Interfaces
{
    /// <summary>
    /// 服务接口
    /// </summary>
    /// <typeparam name="I">输入对象</typeparam>
    /// <typeparam name="K">主键</typeparam>
    public interface IService<I,K>: IDynamicWebApi
        where I:IInputDto
    {
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IResponseOutput GetAll(I input);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IResponseOutput Get(K id);
    }
}
