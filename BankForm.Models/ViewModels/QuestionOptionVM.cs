using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models.ViewModels;

public class QuestionOptionVM
{
    public IEnumerable<QuestionOption> QuestionOptionList { get; set; }
    public int QuestionFkId { get; set; }
}
