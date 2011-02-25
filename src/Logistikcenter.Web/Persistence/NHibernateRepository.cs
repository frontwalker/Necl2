using System.Linq;
using Logistikcenter.Domain;
using NHibernate;
using NHibernate.Linq;

namespace Logistikcenter.Web.Persistence
{
    public class NHibernateRepository : IRepository
    {
        private readonly ISession _session;

        public NHibernateRepository(ISession session)
        {
            _session = session;
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }
    }
}