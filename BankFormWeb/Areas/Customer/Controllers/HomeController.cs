using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using BankForm.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using static System.Collections.Specialized.BitVector32;

namespace BankFormWeb.Areas.Customer.Controllers;
[Area("Customer")]
public class HomeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger , IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [Authorize]
    public IActionResult Index(int? sectionId)
    {

        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        
        if(sectionId == null)
        {
           sectionId = _unitOfWork.Section.GetAll().FirstOrDefault().SectionId;
        }
        StartPageVM startPageVM = new()
        {
            templates = _unitOfWork.Template.GetAll(),
            sections = _unitOfWork.Section.GetAll(),
            section = _unitOfWork.Section.GetFirstOrDefault(x => x.SectionId == sectionId),
            questions = _unitOfWork.Question.GetAll().Where(u => u.FKSectionId == sectionId),
            answers = _unitOfWork.Answer.GetAll().Where(u=> u.ApplicationUserId == claim.Value)
            
        };
            foreach(var q in startPageVM.questions)
            {
                var checkAnswer = _unitOfWork.Answer.GetAll().Where(u => u.ApplicationUserId == claim.Value).Where(u => u.QuestionId == q.QuestionId);
                if (!checkAnswer.Any())
                {
                    Answer answer = new()
                    {
         
                        QuestionId = q.QuestionId,
                        ApplicationUserId = claim.Value
                    };
                    _unitOfWork.Answer.Add(answer);
                    _unitOfWork.Save();

                }


            }

        return View(startPageVM);
    }






    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult IndexPost(IEnumerable<Answer> answer)
    {
        var sectionId = _unitOfWork.Section.GetAll().FirstOrDefault().SectionId;
        StartPageVM startPageVM = new()
        {
            templates = _unitOfWork.Template.GetAll(),
            sections = _unitOfWork.Section.GetAll(),
            section = _unitOfWork.Section.GetFirstOrDefault(x => x.SectionId == sectionId),
            questions = _unitOfWork.Question.GetAll().Where(u => u.FKSectionId == sectionId),
            answers = answer

        };
        foreach(var Ans in startPageVM.answers)
        {
            _unitOfWork.Answer.Update(Ans);
            _unitOfWork.Save();
        }

        return RedirectToAction("Index", new {sectionId = sectionId});
    }

 
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
