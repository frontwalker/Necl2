using System.Linq;

namespace Logistikcenter.Domain
{
    public interface IRepository
    {
        IQueryable<T> Query<T>();        
    }
}