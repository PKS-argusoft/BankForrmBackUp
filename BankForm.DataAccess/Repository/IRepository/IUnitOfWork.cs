using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository.IRepository;

public interface IUnitOfWork 
{
    ITemplateRepository Template { get; }
    ISectionRepository Section { get; }
    IQuestionRepository Question { get; }
    IQuestionTypeRepository QuestionType { get; }
    IQuestionOptionRepository QuestionOption { get; }
    void Save();


}
