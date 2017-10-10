using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain;

namespace Domain.Model
{
    public class Team : IEntity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }

    }
}
