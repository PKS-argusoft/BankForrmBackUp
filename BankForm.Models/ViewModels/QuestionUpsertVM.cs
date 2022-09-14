using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models.ViewModels;

public class QuestionUpsertVM
{
    public Question Questions { get; set; }
    public IEnumerable<SelectListItem> QuestionTypes { get; set; }
}
