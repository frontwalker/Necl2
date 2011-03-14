using System.Linq;

namespace Logistikcenter.Domain
{
    public interface IRepository
    {
        IQueryable<T> Query<T>();
        void Save(object entity);
        void Update(object entity);        
        void Delete<T>(long id);
    }
}