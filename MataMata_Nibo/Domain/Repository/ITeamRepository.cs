﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Framework.NHibernate;

namespace Domain.Repository
{
    public interface ITeamRepository : IRepository<Team>
    {
    }
}