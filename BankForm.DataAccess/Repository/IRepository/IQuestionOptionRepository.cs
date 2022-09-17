using BankForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository.IRepository;

public interface IQuestionOptionRepository : IRepository<QuestionOption> 
{
    void Update(QuestionOption obj);
}
