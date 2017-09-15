using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Repository;
using Domain.Service;
using Web.Helpers.Mappers;
using Web.Models;

namespace Web.Controllers
{
    public class KeyController : Controller
    {

        private readonly ITeamRepository _teamRepository;
        private readonly IKeyRepository _keyRepository;
        private readonly ITeamMapper _teamMapper;
        private readonly IKeyService _keyService;
        private readonly IKeyMapper _keyMapper;


        public KeyController(ITeamRepository teamRepository, ITeamMapper teamMapper, IKeyService keyService, IKeyRepository keyRepository, IKeyMapper keyMapper)
        {
            _teamRepository = teamRepository;
            _teamMapper = teamMapper;
            _keyService = keyService;
            _keyRepository = keyRepository;
            _keyMapper = keyMapper;
        }



        public ActionResult List()
        {
            var obj = _keyMapper.Map(_keyRepository.GetAll().ToList());
            ViewBag.Message = Session["Message"] != null ? Session["Message"].ToString() : "";

            ViewBag.KeysTems = obj;
            Session["Message"] = null;
            return View("List");
        }

        public ActionResult Generate()
        {
            try
            {
                _keyService.GenerateKey();
                Session["Message"] = "Key Generated Successfully!";
            }
            catch (Exception ex)
            {

                Session["Message"] = ex.Message;
            }
            

            return RedirectToAction("List");
        }
        public ActionResult winner(KeyViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var obj = _keyRepository.GetById(viewModel.Id);
                    obj.TeamGolsOne = viewModel.TeamGolsOne;
                    obj.TeamGolsTwo = viewModel.TeamGolsTwo;


                    _keyRepository.Save(obj);
                    _keyRepository.CommitTran();

                    _keyService.winner(obj);

                   
                    Session["Message"] = " Successful registrant!";


                }
                catch (Exception ex)
                {
                    Session["Message"] = "ops! Errors when performing operation " + ex.Message;
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