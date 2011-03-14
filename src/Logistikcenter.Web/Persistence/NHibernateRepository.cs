using System;
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

        public void Save(object entity)
        {
            _session.Save(entity);
        }

        public void Update(object entity)
        {
            _session.Update(entity);
        }

        public void Delete<T>(long id)
        {
            var entity = _session.Get<T>(id);
            _session.Delete(entity);
            _session.Flush();
        }
    }
}