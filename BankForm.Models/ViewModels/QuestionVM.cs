using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models.ViewModels;

public class QuestionVM
{
    public IEnumerable<Question> QuestionList { get; set; }
    public int SectionFkId { get; set; }

    public IEnumerable<QuestionType> QuestionTypeList { get; set; }

}
