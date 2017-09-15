using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using FluentNHibernate.Mapping;

namespace Repository.NH.Map
{
    public class KeyMap : ClassMap<Key>
    {
        public KeyMap()
        {
            Table("wds_key");
            Id(x => x.Id).Column("idKey").GeneratedBy.Identity();
            Map(x => x.Name).Column("name");
            Map(x => x.TeamGolsOne).Column("teamGolsOne");
            Map(x => x.TeamGolsTwo).Column("teamGolsTwo");
            Map(x => x.Keys).Column("keys");
            References(x => x.TeamOne).Column("idTeamOne");
            References(x => x.TeamTwo).Column("idTeamTwo");
        }
    }
}
