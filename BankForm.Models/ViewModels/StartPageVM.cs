using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models.ViewModels;

public class StartPageVM
{
    public IEnumerable<Template> templates { get; set; }
    public IEnumerable<Section> sections { get; set; }
    public IEnumerable<Question> questions { get; set; }
    public IEnumerable<Answer> answers { get; set; }
    public Section section { get; set; }

}
