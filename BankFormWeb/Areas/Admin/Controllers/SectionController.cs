using BankForm.DataAccess.Repository;
using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using BankForm.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace BankFormWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class SectionController : Controller
{

    private readonly IUnitOfWork _unitOfWork;

    public SectionController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }

    public IActionResult Index(int Templateid)
    {
        SectionVM sectionVM = new()
        {
            TemplateId = Templateid,
            SectionList = _unitOfWork.Section.GetAll().Where(u => u.FKTemplateId == Templateid)
        };

        return View(sectionVM);
    }

    //GET
    public IActionResult Create(int id)
    {
        TempData["storeFKTemplate"] = id;
        return View();
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Section obj)
    {
        
        var objNamevalid = _unitOfWork.Section.GetFirstOrDefault(u => u.SectionName == obj.SectionName);
        if (objNamevalid != null)
        {
            TempData["Error"] = obj.SectionName + " already exists .";
            return View(obj);
        }
        if (ModelState.IsValid)
        {
            obj.FKTemplateId = Convert.ToInt32(TempData["storeFKTemplate"]);
            _unitOfWork.Section.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = obj.SectionName + " is added successfully .";
            return RedirectToAction("Index", new {templateid= obj.FKTemplateId});
        }

        return View(obj);
    }


    //GET
    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
        {
            TempData["Error"] = "Section does not exists .";
            return NotFound();
        }
        var SectionObjFetch = _unitOfWork.Section.GetFirstOrDefault(u => u.SectionId == id);

        if (SectionObjFetch == null)
        {
            return NotFound();
        }
        /*TempData["storeFKTemplateEdit"] = SectionObjFetch.FKTemplateId;*/

        TempData["SectionOldName"] = SectionObjFetch.SectionName;
        return View(SectionObjFetch);
    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Section obj)
    {
        if (ModelState.IsValid)
        {
            /*obj.FKTemplateId = Convert.ToInt32(TempData["storeFKTemplate"]);*/
            _unitOfWork.Section.Update(obj);
            _unitOfWork.Save();
            TempData["Success"] = TempData["SectionOldName"] + " changed to " + obj.SectionName + " .";
            return RedirectToAction("Index", new { templateid = obj.FKTemplateId });
        }
        return View(obj);
    }


    //GET
    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
        {
            TempData["Error"] = "Section does not exists .";
            return NotFound();
        }
        var SectionObjFetch = _unitOfWork.Section.GetFirstOrDefault(u => u.SectionId == id);
        if (SectionObjFetch == null)
        {
            return NotFound();
        }
        return View(SectionObjFetch);
    }

    //POST
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(int? id)
    {
        var obj = _unitOfWork.Section.GetFirstOrDefault(u => u.SectionId == id);
        var tempfkstore = obj.FKTemplateId;
        if (obj == null)
        {
            return NotFound();
        }

        _unitOfWork.Section.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Section deleted successfully";
        return RedirectToAction("Index", new { templateid = tempfkstore });

    }

}
