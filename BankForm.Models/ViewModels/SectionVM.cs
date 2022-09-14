using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models.ViewModels
{
    public class SectionVM
    {
        public IEnumerable<Section> SectionList { get; set; }
        public int TemplateId { get; set; }
    }
}
