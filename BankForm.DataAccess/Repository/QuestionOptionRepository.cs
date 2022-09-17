using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository;

public class QuestionOptionRepository : Repository<QuestionOption>, IQuestionOptionRepository
{
    private readonly ApplicationDbContext _db;
    public QuestionOptionRepository(ApplicationDbContext db):base(db)
    {
        _db = db;
    }
    public void Update(QuestionOption obj)
    {
        _db.QuestionOptions.Update(obj);
    }
}
