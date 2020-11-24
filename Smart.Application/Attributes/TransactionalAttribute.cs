using System;
using System.Data;
using System.Threading.Tasks;
using FreeSql;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Smart.Application.Attributes
{
    /// <summary>
    /// 使用事务执行，请在 Program.cs 代码开启动态代理
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionalAttribute : DynamicProxyAttribute, IActionFilter
    {
        /// <summary>
        /// 事务场方式
        /// </summary>
        public Propagation Propagation { get; set; } = Propagation.Required;
        /// <summary>
        /// 
        /// </summary>
        public IsolationLevel IsolationLevel { get => _IsolationLevelPriv.Value; set => _IsolationLevelPriv = value; }
        IsolationLevel? _IsolationLevelPriv;

        [DynamicProxyFromServices]
        UnitOfWorkManager _uowManager = null;
        IUnitOfWork _uow;
        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override Task Before(DynamicProxyBeforeArguments args) => OnBefore(_uowManager);
        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override Task After(DynamicProxyAfterArguments args) => OnAfter(args.Exception);

        /// <summary>
        /// action处理中
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context) => OnBefore(context.HttpContext.RequestServices.GetService(typeof(UnitOfWorkManager)) as UnitOfWorkManager);
        /// <summary>
        /// action处理后
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context) => OnAfter(context.Exception);


        Task OnBefore(UnitOfWorkManager uowm)
        {
            _uow = uowm.Begin(Propagation, _IsolationLevelPriv);
            return Task.FromResult(false);
        }
        Task OnAfter(Exception ex)
        {
            try
            {
                if (ex == null) _uow.Commit();
                else _uow.Rollback();
            }
            finally
            {
                _uow.Dispose();
            }
            return Task.FromResult(false);
        }
    }
}
