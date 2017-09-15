using Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Framework.NHibernate
{
    public interface IRepository<T> : IRepository where T : IEntity
    {

        T GetById(long id);
        IQueryable<T> GetAll();
        IQueryable<T> Find(Expression<Func<T, bool>> where);
        T Save(T entity);
        void Save(IList<T> entities);
        void Delete(T entity);
        void BeginTran();
        void CommitTran();
        void RollbackTran();

        void Evict(Object obj);

    }

    public interface IRepository
    {
        object GetById(long id);
    }
}
