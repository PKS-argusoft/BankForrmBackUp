using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using BankForm.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;


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


    public IActionResult Index(int? sectionId)
    {
        
        if(sectionId == null)
        {
           sectionId = _unitOfWork.Section.GetAll().FirstOrDefault().SectionId;
        }
        StartPageVM startPageVM = new()
        {
            templates = _unitOfWork.Template.GetAll(),
            sections = _unitOfWork.Section.GetAll(),
            sectionName = _unitOfWork.Section.GetFirstOrDefault(x=> x.SectionId == sectionId).SectionName,
            questions = _unitOfWork.Question.GetAll().Where(u=> u.FKSectionId == sectionId)

        };

        return View(startPageVM);
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
