using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository;

internal class QuestionTypeRepository : Repository<QuestionType>, IQuestionTypeRepository
{
    private readonly ApplicationDbContext _context;
    public QuestionTypeRepository(ApplicationDbContext context):base(context)
    {
        _context = context;
    }

    public void Update(QuestionType obj)
    {
        _context.QuestionTypes.Update(obj);
    }
}
