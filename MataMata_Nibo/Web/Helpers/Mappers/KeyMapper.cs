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
    public class KeyMapper : IKeyMapper
    {
        private readonly IKeyRepository _keyRepository;
        private readonly ITeamRepository _teamRepository;

        public KeyMapper(IKeyRepository keyRepository, ITeamRepository teamRepository)
        {
            _keyRepository = keyRepository;
            _teamRepository = teamRepository;
        }

        public Key Map(KeyViewModel viewModel)
        {
            Mapper.CreateMap<KeyViewModel, Key>()

               .ForMember(a => a.TeamOne, opt => opt.Ignore())
               .ForMember(a => a.TeamTwo, opt => opt.Ignore());

            Key key;

            if (viewModel.Id > 0)
            {
                key = _keyRepository.GetById(viewModel.Id);
            }
            else
            {
                key = new Key();
            }
            Mapper.Map(viewModel, key);
            key.TeamOne = viewModel.TeamOneId == 0 ? null : new Team() { Id = viewModel.TeamOneId };
            key.TeamTwo = viewModel.TeamTwoId == 0 ? null : new Team() { Id = viewModel.TeamTwoId };

           

            return key;
        }

        public KeyViewModel Map(Key domainModel)
        {
            Mapper.CreateMap<Key, KeyViewModel>()
                .ForMember(a => a.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(a => a.TeamOne, opt => opt.MapFrom(src => src.TeamOne.Name))
                .ForMember(a => a.TeamOneId, opt => opt.MapFrom(src => src.TeamOne.Id))
                 .ForMember(a => a.TeamTwo, opt => opt.MapFrom(src => src.TeamTwo.Name))
                .ForMember(a => a.TeamTwoId, opt => opt.MapFrom(src => src.TeamTwo.Id))
                .ForMember(a => a.Keys, opt => opt.MapFrom(src => src.Keys))
                .ForMember(a => a.TeamGolsOne, opt => opt.MapFrom(src => src.TeamGolsOne))
                .ForMember(a => a.TeamGolsTwo, opt => opt.MapFrom(src => src.TeamGolsTwo))
                ;

            var viewModel = new KeyViewModel();

            Mapper.Map(domainModel, viewModel);

            return viewModel;
        }

        public IList<KeyViewModel> Map(IList<Key> domainModelList)
        {
            return domainModelList.Select(Map).ToList();
        }
    }

    public interface IKeyMapper : IMapper<KeyViewModel, Key>
    {
    }
}