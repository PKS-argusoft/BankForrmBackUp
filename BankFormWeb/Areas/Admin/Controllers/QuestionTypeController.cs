using BankForm.DataAccess.Repository;
using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using BankForm.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BankFormWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class QuestionTypeController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public QuestionTypeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
     
        IEnumerable<QuestionType> objQuestionTypeList = _unitOfWork.QuestionType.GetAll().OrderBy(k => k.Order);
    
        return View(objQuestionTypeList);
    }


    public IActionResult upward(int id)
    {
        var upwardOperation = _unitOfWork.QuestionType.GetFirstOrDefault(u => u.Order == id);
        var aboveElement = _unitOfWork.QuestionType.GetFirstOrDefault(u => u.Order == id - 1);
        upwardOperation.Order -= 1;
        aboveElement.Order += 1;
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }

    public IActionResult downward(int id)
    {
        var downwardOperation = _unitOfWork.QuestionType.GetFirstOrDefault(u => u.Order == id);
        var belowElement = _unitOfWork.QuestionType.GetFirstOrDefault(u => u.Order == id + 1);
        downwardOperation.Order += 1;
        belowElement.Order -= 1;
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }



    //GET
    public IActionResult Create()
    {
        return View();
    }

    //post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(QuestionType obj)
    {
        var QuestionTypesFromDbnCheck = _unitOfWork.QuestionType.GetFirstOrDefault(x => x.QuestionTypes == obj.QuestionTypes);
        if (QuestionTypesFromDbnCheck == null)
        {
            var orderSet = _unitOfWork.QuestionType.GetAll().Max(u => u.Order);
            if (ModelState.IsValid)
            {
                obj.Order = orderSet+1;
                obj.CreatedAt = DateTime.Now;
                _unitOfWork.QuestionType.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = obj.QuestionTypes + " is added successfully .";
                return RedirectToAction("Index");
            }

        }
        return View(obj);
    }

    //get
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var QuestionTypeFromDb = _unitOfWork.QuestionType.GetFirstOrDefault(u => u.QuestionTypeId == id);

        if (QuestionTypeFromDb == null)
        {
            return NotFound();
        }

        TempData["old_name_store"] = QuestionTypeFromDb.QuestionTypes;

        return View(QuestionTypeFromDb);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(QuestionType obj)
    {
        if (ModelState.IsValid)
        {
            obj.CreatedAt = obj.CreatedAt;
            obj.UpdatedAt = DateTime.Now;

            _unitOfWork.QuestionType.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = TempData["old_name_store"] + " has been edited to " + obj.QuestionTypes + " successfully . ";
            return RedirectToAction("Index");

        }
        return View(obj);
    }



    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var QuestionTypeFromDb = _unitOfWork.QuestionType.GetFirstOrDefault(u => u.QuestionTypeId == id);
        //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if (QuestionTypeFromDb == null)
        {
            return NotFound();
        }

        return View(QuestionTypeFromDb);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.QuestionType.GetFirstOrDefault(u => u.QuestionTypeId == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.QuestionType.Remove(obj);
        _unitOfWork.Save();
        var reorderList = _unitOfWork.QuestionType.GetAll().OrderBy(u=> u.Order);
        var i = 1;
        foreach (var inObj in reorderList)
        {
            inObj.Order = i;
            i++;
        }
        _unitOfWork.Save();

        TempData["success"] = "QuestionType deleted successfully";
        return RedirectToAction("Index");

    }
}
