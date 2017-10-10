using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Domain.Repository;

namespace Domain.Service
{
   public class TeamService : ITeamService
   {
       private readonly ITeamRepository _teamRepository;

       public TeamService(ITeamRepository teamRepository)
       {
           _teamRepository = teamRepository;

       }

       public void ValidationTeam()
       {
           var teams = _teamRepository.GetAll().Count();
           if (teams == 4)
           {
                throw new Exception("Registration is restricted to 4 teams!");
            }
       }
   }

    public interface ITeamService
    {
        void ValidationTeam();

    }
}
