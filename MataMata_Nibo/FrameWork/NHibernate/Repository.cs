using NHibernate;
using Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NHibernate;
using NHibernate.Linq;

namespace Framework.NHibernate
{
    public class Repository<T> : IRepository<T> where T : IEntity
    {

        public T GetById(long id)
        {
            T entity;

            entity = (T)NHibernateSession.Load(persitentType, id, LockMode.Read);

            return entity;
        }

        object IRepository.GetById(long id)
        {
            return GetById(id);
        }

        public IQueryable<T> GetAll()
        {
            return NHibernateSession.Query<T>();
        }

        public IQueryable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> where)
        {
            return NHibernateSession.Query<T>().Where(where);
        }

        public T Save(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);

            return entity;
        }

        public void Save(IList<T> entities)
        {

            foreach (T item in entities)
            {
                Save(item);
            }

        }

        public void Delete(T entity)
        {
            NHibernateSession.Delete(entity);
        }

        public void BeginTran()
        {
            NHibernateSession.BeginTransaction();
        }

        public void CommitTran()
        {
            if (NHibernateHelper.Instance.HasOpenTransaction())
            {
                NHibernateHelper.Instance.CommitTransaction();
            }
            else
            {
                NHibernateHelper.Instance.GetSession().Flush();
            }
        }

        public void RollbackTran()
        {
            NHibernateHelper.Instance.RollbackTransaction();
        }

        public void Evict(Object obj)
        {
            NHibernateSession.Evict(obj);
        }

        #region Privados

        protected ISession NHibernateSession
        {
            get
            {
                return NHibernateHelper.Instance.GetSession();
            }
        }

        protected Type persitentType = typeof(T);

        #endregion


    }
}
