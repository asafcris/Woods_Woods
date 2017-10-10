using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using FluentNHibernate.Mapping;
using NHibernate.Cfg.XmlHbmBinding;

namespace Repository.NH.Map
{
    public class TeamMap : ClassMap<Team>
    {
        public TeamMap()
        {
            Table("wds_team");
            Id(x => x.Id).Column("idTeam").GeneratedBy.Identity();
            Map(x => x.Name).Column("name");
        }
    }
}
