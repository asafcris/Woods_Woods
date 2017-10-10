using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Domain.Model;
using Domain.Repository;
using Framework.Helpers;
using Framework.MVC.Map;
using Web.Models;

namespace Web.Helpers.Mappers
{
    public class TeamMapper : ITeamMapper
    {
        private readonly ITeamRepository _teamRepository;

        public TeamMapper(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public Team Map(TeamViewModel viewModel)
        {
            Mapper.CreateMap<TeamViewModel, Team>();

            Team time;

            if (viewModel.Id > 0)
            {
                time = _teamRepository.GetById(viewModel.Id);
            }
            else
            {
                time = new Team();
                time.Name = viewModel.Name;
            }
            Mapper.Map(viewModel, time);

            return time;
        }

        public TeamViewModel Map(Team domainModel)
        {
            Mapper.CreateMap<Team, TeamViewModel>()
                .ForMember(a => a.Name, opt => opt.MapFrom(src => src.Name))
                ;

            var viewModel = new TeamViewModel();

            Mapper.Map(domainModel, viewModel);

            return viewModel;
        }

        public IList<TeamViewModel> Map(IList<Team> domainModelList)
        {
            return domainModelList.Select(Map).ToList();
        }
    }

    public interface ITeamMapper : IMapper<TeamViewModel, Team>
    {
    }
}