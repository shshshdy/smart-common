using System.Collections.Generic;

namespace Smart.Domain
{
    public interface IManager<T>
    {
        T Get(long id);

        IEnumerable<T> GetAll();

        long Count();

        IEnumerable<T> GetAll(int pageIndex, int pageSize);


        T CreatOrUpdate(T entity);

    }
}
