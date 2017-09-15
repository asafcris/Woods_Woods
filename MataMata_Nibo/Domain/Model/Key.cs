using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Domain;

namespace Domain.Model
{
    public class Key : IEntity
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Team TeamOne { get; set; }
        public virtual Team TeamTwo { get; set; }
        public virtual int TeamGolsOne { get; set; }
        public virtual int TeamGolsTwo { get; set; }
        public virtual int Keys { get; set; }
        
    }
}
