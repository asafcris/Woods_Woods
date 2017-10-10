using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Model;
using Domain.Repository;
using Domain.Service;
using Web.Helpers.Mappers;
using Web.Models;

namespace Web.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamMapper _teamMapper;
        private readonly ITeamService _teamService;

        public TeamController(ITeamRepository teamRepository, ITeamMapper teamMapper, ITeamService teamService)
        {
            _teamRepository = teamRepository;
            _teamMapper = teamMapper;
            _teamService = teamService;
        }

        public ActionResult Register()
        {
            return View();
        }
        public  ActionResult Edit(long id)
        {
            var obj = _teamRepository.GetById(id);

            return View("Register", _teamMapper.Map(obj));
        }
        public ActionResult List()
        {
            var obj = _teamRepository.GetAll().ToList();
            ViewBag.Message = Session["Message"] != null ? Session["Message"].ToString() : "";
            Session["Message"] = null;
            return View("List", _teamMapper.Map(obj));
        }

        public ActionResult Save(TeamViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _teamService.ValidationTeam();

                    var obj = _teamRepository.Save(_teamMapper.Map(viewModel));
                    _teamRepository.CommitTran();
                    Session["Message"] = "Team " + obj.Name + " , Successful registrant!";
                    

                }
                catch (Exception ex)
                {
                    Session["Message"] =  "ops! Errors when performing operation " + ex.Message;
                }
            }
            else
            {
                Session["Message"] = "form validation errors!";
            }

            return RedirectToAction("List");
        }

      
    }
}