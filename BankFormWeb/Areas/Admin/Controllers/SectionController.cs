using BankForm.DataAccess.Repository;
using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using BankForm.Models.ViewModels;
using BankForm.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace BankFormWeb.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
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
            SectionList = _unitOfWork.Section.GetAll().Where(u => u.FKTemplateId == Templateid).OrderBy(u=> u.Order)
        };

        return View(sectionVM);
    }



    public IActionResult upward(int id , int fk)
    {
        var selectedOnes = _unitOfWork.Section.GetAll().Where(u => u.FKTemplateId == fk);
        var upwardOperation = selectedOnes.FirstOrDefault(u=> u.Order == id);
        var aboveElement = selectedOnes.FirstOrDefault(u => u.Order == id - 1);
        upwardOperation.Order -= 1;
        aboveElement.Order += 1;
        _unitOfWork.Save();
        return RedirectToAction("Index", new { templateid = upwardOperation.FKTemplateId });
    }

    public IActionResult downward(int id, int fk)
    {
        var selectedOnes = _unitOfWork.Section.GetAll().Where(u => u.FKTemplateId == fk);
        var downwardOperation = selectedOnes.FirstOrDefault(u => u.Order == id);
        var belowElement = selectedOnes.FirstOrDefault(u => u.Order == id +1 );
        downwardOperation.Order += 1;
        belowElement.Order -= 1;
        _unitOfWork.Save();
        return RedirectToAction("Index", new { templateid = downwardOperation.FKTemplateId });
    }






    //GET
    public IActionResult Create(int id)
    {
        Section section = new Section();
        section.FKTemplateId = id;
        TempData["storeFKTemplate"] = id;
        return View(section);
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
            var selectedOnes = _unitOfWork.Section.GetAll().Where(u => u.FKTemplateId ==  obj.FKTemplateId);
            var orderSet = 0;
            if(selectedOnes.Count() != 0)
            {
                orderSet = selectedOnes.Max(u => u.Order);
            }
            obj.Order= orderSet+1;
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
        var reorderList = _unitOfWork.Section.GetAll().Where(u=> u.FKTemplateId == tempfkstore).OrderBy(u => u.Order);
        var i = 1;
        foreach (var inObj in reorderList)
        {
            inObj.Order = i;
            i++;
        }
        _unitOfWork.Save();
        TempData["success"] = "Section deleted successfully";
        return RedirectToAction("Index", new { templateid = tempfkstore });

    }

}
