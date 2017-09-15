using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain;
using Framework.NHibernate;

namespace Framework.MVC.Map
{
    public abstract class Mapper<TViewModel, TDomainModel> : IMapper<TViewModel, TDomainModel>
        where TDomainModel : IEntity
    {

        protected readonly IRepository<TDomainModel> _repository;

        protected Mapper() { }

        protected Mapper(IRepository<TDomainModel> repository)
            : this()
        {
            this._repository = repository;
        }

        public abstract TDomainModel Map(TViewModel viewModel);
        public abstract TViewModel Map(TDomainModel domainModel);
        public IList<TViewModel> Map(IList<TDomainModel> domainModelList)
        {
            return domainModelList.Select(Map).ToList();
        }
        public T GetNullOrObject<T>(string id) where T : IEntity, new()
        {
            var r = new Repository<T>();

            var obj = new T();

            if (!string.IsNullOrEmpty(id))
                obj = r.GetById(long.Parse(id));
            else
                obj = default(T);

            return obj;

        }

        public EntityVal ConvertToEntityVal<T>(T obj) where T : IEntity
        {
            return obj == null ? new EntityVal() : new EntityVal(obj.Id.ToString(), obj.ToString());
        }

    }

    public interface IMapper<TViewModel, TDomainModel> where TDomainModel : IEntity
    {

        TDomainModel Map(TViewModel viewModel);
        TViewModel Map(TDomainModel domainModel);
        IList<TViewModel> Map(IList<TDomainModel> domainModelList);

    }
}
