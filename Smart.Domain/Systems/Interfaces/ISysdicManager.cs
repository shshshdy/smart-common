using Smart.Infrastructure.Freesql.Entities;

namespace Smart.Domain.Systems.Interfaces
{
    public interface ISysDicManager : IManager<SysDic>
    {
        SysDic GetDicByCode(string code);
    }
}
