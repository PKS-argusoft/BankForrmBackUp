using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using BankForm.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BankFormWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class QuestionController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public QuestionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }




    public IActionResult Index(int Sectionid)
    {
        QuestionVM questionVM = new()
        {
            SectionFkId = Sectionid,
            QuestionList = _unitOfWork.Question.GetAll().Where(u => u.FKSectionId == Sectionid).OrderBy(k=> k.Order),
            QuestionTypeList = _unitOfWork.QuestionType.GetAll(),
            QuestionOptionList = _unitOfWork.QuestionOption.GetAll()
        };
        
        
        return View(questionVM);
    }



    public IActionResult upward(int id, int fk)
    {
        var selectedOnes = _unitOfWork.Question.GetAll().Where(u => u.FKSectionId == fk);
        var upwardOperation = selectedOnes.FirstOrDefault(u => u.Order == id);
        var aboveElement = selectedOnes.FirstOrDefault(u => u.Order == id - 1);
        upwardOperation.Order -= 1;
        aboveElement.Order += 1;
        _unitOfWork.Save();
        return RedirectToAction("Index", new { sectionid = upwardOperation.FKSectionId });
    }

    public IActionResult downward(int id, int fk)
    {
        var selectedOnes = _unitOfWork.Question.GetAll().Where(u => u.FKSectionId == fk);
        var downwardOperation = selectedOnes.FirstOrDefault(u => u.Order == id);
        var belowElement = selectedOnes.FirstOrDefault(u => u.Order == id + 1);
        downwardOperation.Order += 1;
        belowElement.Order -= 1;
        _unitOfWork.Save();
        return RedirectToAction("Index", new { sectionid = downwardOperation.FKSectionId });
    }



    //GET
    public IActionResult Create(int id)
    {
        QuestionUpsertVM questionUpsertVM = new()
        {
            Questions = new()
            {
                FKSectionId = id
            },
            QuestionTypes = _unitOfWork.QuestionType.GetAll().Select(i => new SelectListItem
            {
                Text = i.QuestionTypes,
                Value = i.QuestionTypeId.ToString()
            })
        };
        
        TempData["storeFKSectionId"] = id;
        return View(questionUpsertVM);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Question Questions)
    {
  
  

        QuestionUpsertVM questionUpsertVM = new()
        {
            Questions = Questions,
            QuestionTypes = _unitOfWork.QuestionType.GetAll().Select(i => new SelectListItem
            {
                Text = i.QuestionTypes,
                Value = i.QuestionTypeId.ToString()
            })
        };

        questionUpsertVM.Questions = _unitOfWork.Question.GetFirstOrDefault(u => u.QuestionName == Questions.QuestionName);
        if (questionUpsertVM.Questions != null)
        {
            TempData["Error"] = questionUpsertVM.Questions.QuestionName + " already exists .";
            return View(questionUpsertVM);
        }
        //Get the highest order value and set the current by adding 1 into it
        

        if (ModelState.IsValid)
        {
            Questions.FKSectionId = Convert.ToInt32(TempData["storeFKSectionId"]);
            var selectedOnes = _unitOfWork.Question.GetAll().Where(u => u.FKSectionId == Questions.FKSectionId);
            var orderSet = 0;
            if (selectedOnes.Count() != 0)
            {
                orderSet = selectedOnes.Max(u => u.Order);
            }
            Questions.Order = orderSet+1;
            _unitOfWork.Question.Add(Questions);
            _unitOfWork.Save();
            TempData["Success"] = Questions.QuestionName + " is added successfully .";
            return RedirectToAction("Index", new { sectionid = Questions.FKSectionId });
        }

        return View(questionUpsertVM);
    }













    //GET
    public IActionResult Edit(int? id)
    {
        QuestionUpsertVM questionUpsertVM = new()
        {
            Questions = new(),
            QuestionTypes = _unitOfWork.QuestionType.GetAll().Select(i => new SelectListItem
            {
                Text = i.QuestionTypes,
                Value = i.QuestionTypeId.ToString()
            })
        };
        if (id == null || id == 0)
        {
            TempData["Error"] = "Question does not exists .";
            return NotFound();
        }
        questionUpsertVM.Questions = _unitOfWork.Question.GetFirstOrDefault(u => u.QuestionId == id);

       
        /*TempData["storeFKTemplateEdit"] = SectionObjFetch.FKTemplateId;*/

        TempData["QuestionOldName"] = questionUpsertVM.Questions.QuestionName;
        return View(questionUpsertVM);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Question Questions)
    {
        QuestionUpsertVM questionUpsertVM = new()
        {
            Questions = Questions,
            QuestionTypes = _unitOfWork.QuestionType.GetAll().Select(i => new SelectListItem
            {
                Text = i.QuestionTypes,
                Value = i.QuestionTypeId.ToString()
            })
        };
        if (ModelState.IsValid)
        {
            /*obj.FKTemplateId = Convert.ToInt32(TempData["storeFKTemplate"]);*/
            _unitOfWork.Question.Update(Questions);
            _unitOfWork.Save();
            TempData["Success"] = TempData["QuestionOldName"] + " changed to " + Questions.QuestionName + " .";
            return RedirectToAction("Index", new { sectionid = Questions.FKSectionId });
        }
        return View(questionUpsertVM);
    }

















    //GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            TempData["Error"] = "Question does not exists .";
            return NotFound();
        }
        var QuestionObjFetch = _unitOfWork.Question.GetFirstOrDefault(u => u.QuestionId == id);
        if (QuestionObjFetch == null)
        {
            return NotFound();
        }
        return View(QuestionObjFetch);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Question.GetFirstOrDefault(u => u.QuestionId == id);
        var tempfkstore = obj.FKSectionId;
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Question.Remove(obj);

        _unitOfWork.Save();
        var reorderList = _unitOfWork.Question.GetAll().Where(u => u.FKSectionId == tempfkstore).OrderBy(u => u.Order);
        var i = 1;
        foreach(var inObj in reorderList)
        {
            inObj.Order = i;
            i++;
        }
        _unitOfWork.Save();
        TempData["success"] = "Question deleted successfully";
        return RedirectToAction("Index", new { sectionid = tempfkstore });

    }
}
