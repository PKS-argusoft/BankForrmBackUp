using BankForm.DataAccess.Repository;
using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankFormWeb.Areas.Admin.Controllers;
[Area("Admin")]

public class QuestionTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public QuestionTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
     
        IEnumerable<QuestionType> objQuestionTypeList = _unitOfWork.QuestionType.GetAll();
    
        return View(objQuestionTypeList);
    }
}
