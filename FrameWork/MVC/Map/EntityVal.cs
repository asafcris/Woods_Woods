using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MVC.Map
{
    public class EntityVal
    {
        public EntityVal() { }
        public EntityVal(string id) : this(id, "") { }
        public EntityVal(string id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public string Id { get; set; }
        public string Nome { get; set; }
    }
}
