using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Domain.Repository;
using Framework.NHibernate;

namespace Repository.NH.Impl
{
   public class KeyRepository : Repository<Key>,IKeyRepository
   {
        public Key GetKeys(int keys)
        {
            return Find(x => x.Keys == keys).FirstOrDefault();
        }
    }
}
