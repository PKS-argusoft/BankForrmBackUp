using BankForm.Models;
using BankForm.DataAccess;
using Microsoft.AspNetCore.Mvc;
using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models.ViewModels;

namespace BankFormWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class TemplateController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public TemplateController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult upward(int id)
    {
        var upwardOperation = _unitOfWork.Template.GetFirstOrDefault(u => u.Order == id);
        var aboveElement = _unitOfWork.Template.GetFirstOrDefault(u => u.Order == id - 1);
        upwardOperation.Order -= 1;
        aboveElement.Order += 1;
        _unitOfWork.Save();
        return RedirectToAction("Index");
    }

    public IActionResult downward(int id)
    {
        var downwardOperation = _unitOfWork.Template.GetFirstOrDefault(u => u.Order == id);
        var belowElement = _unitOfWork.Template.GetFirstOrDefault(u => u.Order == id + 1);
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
    public IActionResult Create(Template obj)
    {
        var templateFromDbnCheck = _unitOfWork.Template.GetFirstOrDefault(x => x.TemplateName == obj.TemplateName);
        if (templateFromDbnCheck == null)
        {
            var orderSet = _unitOfWork.Template.GetAll().Max(u => u.Order);

            if (ModelState.IsValid)
            {
                obj.Order = orderSet + 1;
                obj.CreatedAt = DateTime.Now;
                _unitOfWork.Template.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = obj.TemplateName + " is added successfully .";
                return RedirectToAction("Index");
            }

        }
        else 
            TempData["Error"] = obj.TemplateName + "  already exists .";
        return View(obj);
    }

    //get
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        var templateFromDb = _unitOfWork.Template.GetFirstOrDefault(u => u.TemplateId == id);

        if (templateFromDb == null)
        {
            return NotFound();
        }

        TempData["old_name_store"] = templateFromDb.TemplateName;

        return View(templateFromDb);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Template obj)
    {
        if (ModelState.IsValid)
        {
            obj.CreatedAt = obj.CreatedAt;
            obj.UpdatedAt = DateTime.Now;

            _unitOfWork.Template.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = TempData["old_name_store"] + " has been edited to " + obj.TemplateName + " successfully . ";
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
        var TemplateFromDb = _unitOfWork.Template.GetFirstOrDefault(u => u.TemplateId == id);
        //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

        if (TemplateFromDb == null)
        {
            return NotFound();
        }

        return View(TemplateFromDb);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Template.GetFirstOrDefault(u => u.TemplateId == id);
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Template.Remove(obj);
        _unitOfWork.Save();
        var reorderList = _unitOfWork.Template.GetAll().OrderBy(u => u.Order);
        if (reorderList.Count() != 0)
        {
            var i = 1;
            foreach (var inObj in reorderList)
            {
                inObj.Order = i;
                i++;
            }
            _unitOfWork.Save();
        }
        TempData["success"] = "Template deleted successfully";
        return RedirectToAction("Index");

    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var templateList = _unitOfWork.Template.GetAll();
        return Json( new { data =  templateList });
    }

    #endregion


}
