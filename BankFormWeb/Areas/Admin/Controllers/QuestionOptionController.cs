using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using Microsoft.AspNetCore.Mvc;
using BankForm.Models.ViewModels;
using System.Collections.Generic;

namespace BankFormWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class QuestionOptionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public QuestionOptionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index(int QuestionId)
    {
        QuestionOptionVM questionOptionVM = new()
        {
            QuestionOptionList = _unitOfWork.QuestionOption.GetAll().Where(u => u.FKQuestionId == QuestionId).OrderBy(k => k.Order),
            QuestionFkId = QuestionId 
        };
       
        return View(questionOptionVM);
    }


    public IActionResult upward(int id, int fk)
    {
        var selectedOnes = _unitOfWork.QuestionOption.GetAll().Where(u => u.FKQuestionId == fk);
        var upwardOperation = selectedOnes.FirstOrDefault(u => u.Order == id);
        var aboveElement = selectedOnes.FirstOrDefault(u => u.Order == id - 1);
        upwardOperation.Order -= 1;
        aboveElement.Order += 1;
        _unitOfWork.Save();
        return RedirectToAction("Index", new { questionid = upwardOperation.FKQuestionId });
    }

    public IActionResult downward(int id, int fk)
    {
        var selectedOnes = _unitOfWork.QuestionOption.GetAll().Where(u => u.FKQuestionId == fk);
        var downwardOperation = selectedOnes.FirstOrDefault(u => u.Order == id);
        var belowElement = selectedOnes.FirstOrDefault(u => u.Order == id + 1);
        downwardOperation.Order += 1;
        belowElement.Order -= 1;
        _unitOfWork.Save();
        return RedirectToAction("Index", new { questionid = downwardOperation.FKQuestionId });
    }



    //GET
    public IActionResult Create(int id)
    {
        var obj = new QuestionOption();
        obj.FKQuestionId = id;
        TempData["storeFKQuestionId"] = id;
        return View(obj);
    }

    //Post
    [HttpPost]
    [ValidateAntiforgeryToken]
    public IActionResult Create(QuestionOption obj)
    {
        //Get the highest order value and set the current by adding 1 into it
        if (ModelState.IsValid)
        {
            var selectedOnes = _unitOfWork.QuestionOption.GetAll().Where(U=>U.FKQuestionId == obj.FKQuestionId);
            var orderSet = 0;
            if (selectedOnes.Count() != 0)
            {
                orderSet = selectedOnes.Max(u => u.Order);
            }
            obj.Order = orderSet+1;
            /*obj.FKQuestionId = Convert.ToInt32(TempData["storeFKQuestionId"]);*/
            _unitOfWork.QuestionOption.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = obj.QuestionOptionName + " is added successfully .";
            return RedirectToAction("Index", new { questionid = obj.FKQuestionId });
        }
        return View(obj);
    }

    //Get
    public IActionResult Edit(int? id)
    {
        if(id== 0 || id==null)
        {
            TempData["Error"] = "QuestionOption does not exists .";
            return NotFound();
        }
        var QuestionOptionObjFetch = _unitOfWork.QuestionOption.GetFirstOrDefault(u => u.QuestionOptionId == id);

        if (QuestionOptionObjFetch == null)
        {
            return NotFound();
        }
   

        TempData["QuestionOptionOldName"] = QuestionOptionObjFetch.QuestionOptionName;
        return View(QuestionOptionObjFetch);
    }

    //Post
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(QuestionOption obj)
    {
        if(ModelState.IsValid)
        {
            _unitOfWork.QuestionOption.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = TempData["QuestionOptionOldName"] + " changed to " + obj.QuestionOptionName+ " .";
            return RedirectToAction("Index", new { questionid = obj.FKQuestionId });
        }

        return View(obj);
    }


    //GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            TempData["Error"] = "QuestionOption does not exists .";
            return NotFound();
        }
        var QuestionOptionObjFetch = _unitOfWork.QuestionOption.GetFirstOrDefault(u => u.QuestionOptionId == id);
        if (QuestionOptionObjFetch == null)
        {
            return NotFound();
        }
        return View(QuestionOptionObjFetch);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.QuestionOption.GetFirstOrDefault(u => u.QuestionOptionId == id);
        var tempfkstore = obj.FKQuestionId;
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.QuestionOption.Remove(obj);
        _unitOfWork.Save();

        var reorderList = _unitOfWork.QuestionOption.GetAll().Where(u=>u.FKQuestionId == tempfkstore).OrderBy(u => u.Order);
        var i = 1;
        foreach (var inObj in reorderList)
        {
            inObj.Order = i;
            i++;
        }
        _unitOfWork.Save();
        TempData["success"] = "QuestionOption deleted successfully";
        return RedirectToAction("Index", new { questionid = tempfkstore });

    }



}

