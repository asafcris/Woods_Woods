using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Domain.Repository;

namespace Domain.Service
{
    public class KeyService : IKeyService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IKeyRepository _keyRepository;

        public KeyService(ITeamRepository teamRepository, IKeyRepository keyRepository)
        {
            _teamRepository = teamRepository;
            _keyRepository = keyRepository;

        }
        // method to generate the championship keys to know the clashes
        // once generated it can not regenerate
        public void GenerateKey()
        {
            try
            {
                //get all teams
                var teams = _teamRepository.GetAll().ToList();

                if (teams.Count() != 4)
                {
                    throw new Exception("it is necessary to have four teams!");
                }
                if (_keyRepository.GetAll().Any())
                {
                    throw new Exception("key has already been generated!");
                }
                //genarete semi-final
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        var semifinal = new Key();

                        if (i != j)
                       
                        {
                            semifinal.Keys = 2;
                            semifinal.TeamOne = teams[i];
                            semifinal.TeamTwo = teams[j];
                            semifinal.Name = "semi-final";
                            _keyRepository.Save(semifinal);
                            _keyRepository.CommitTran();
                            // remove the team that entered the key for the clashes only once so as not to give an error in the negative index
                            if (teams.Count == 4)
                            {
                                teams.Remove(teams[j]);
                                teams.Remove(teams[i]);
                            }
                        }
                    }
                }
                //generate end
                var qtdTeams = 1;
                for (int i = 0; i < qtdTeams; i++)
                {
                    var endTeams = new Key();
                    endTeams.Keys = 1;
                    endTeams.TeamOne = null;
                    endTeams.TeamTwo = null;
                    endTeams.Name = "end";
                    _keyRepository.Save(endTeams);
                    _keyRepository.CommitTran();

                }
                //genarate champion
                var champion = new Key();
                champion.Keys = 0;
                champion.TeamOne = null;
                champion.Name = "end";
                _keyRepository.Save(champion);
                _keyRepository.CommitTran();
            }
            catch (Exception ex)
            {

                throw new Exception("ops! Something went wrong!" + ex.Message);
            }

        }
        // method to save the winner or change, if a change is made to a key, the next key is cleared as it may have changed
        //the winner of the final, and if there is a champion it is withdrawn due to change of the previous result
        public void winner(Key key)
        {
            switch (key.Keys)
            {
                case 2:
                    var obj = _keyRepository.GetKeys(1);
                    if (obj.TeamOne == null || obj.TeamOne == key.TeamOne|| obj.TeamOne == key.TeamTwo)
                    {
                        if (key.TeamGolsOne == key.TeamGolsTwo)
                        {
                            throw new Exception("the result has drawn, please choose a winner");
                        }
                        else if (key.TeamGolsOne > key.TeamGolsTwo)
                        {
                            obj.TeamOne = key.TeamOne;
                            obj.TeamGolsOne = 0;
                            obj.TeamGolsTwo = 0;
                        }
                        else
                        {
                            obj.TeamOne = key.TeamTwo;
                            obj.TeamGolsOne = 0;
                            obj.TeamGolsTwo = 0;
                        }
                        if (_keyRepository.GetKeys(0).TeamOne != null)
                        {
                            var team = _keyRepository.GetKeys(0);
                            team.TeamOne = null;
                            _keyRepository.Save(team);
                            _keyRepository.CommitTran();
                        }
                        _keyRepository.Save(obj);
                        _keyRepository.CommitTran();
                    }
                    else
                    {
                        if (key.TeamGolsOne == key.TeamGolsTwo)
                        {
                            throw new Exception("the result has drawn, please choose a winner");
                        }
                        else if (key.TeamGolsOne > key.TeamGolsTwo)
                        {
                            obj.TeamTwo = key.TeamOne;
                            obj.TeamGolsOne = 0;
                            obj.TeamGolsTwo = 0;
                        }
                        else
                        {
                            obj.TeamTwo = key.TeamTwo;
                            obj.TeamGolsOne = 0;
                            obj.TeamGolsTwo = 0;
                        }
                        if (_keyRepository.GetKeys(0).TeamOne != null)
                        {
                            var team = _keyRepository.GetKeys(0);
                            team.TeamOne = null;
                            _keyRepository.Save(team);
                            _keyRepository.CommitTran();
                        }
                        _keyRepository.Save(obj);
                        _keyRepository.CommitTran();
                    }



                    break;
                case 1:
                    obj = _keyRepository.GetKeys(0);

                    if (key.TeamGolsOne == key.TeamGolsTwo)
                    {
                        throw new Exception("the result has drawn, please choose a winner");
                    }
                    else if (key.TeamGolsOne > key.TeamGolsTwo)
                    {
                        obj.TeamOne = key.TeamOne;
                    }
                    else
                    {
                        obj.TeamOne = key.TeamTwo;
                    }
                    _keyRepository.Save(obj);
                    _keyRepository.CommitTran();
                    break;

            }
        }
    }

    public interface IKeyService
    {
        void GenerateKey();
        void winner(Key key);

    }
}
