using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository;

public class QuestionRepository : Repository<Question>, IQuestionRepository
{
    private readonly ApplicationDbContext _db;
    public QuestionRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Question obj)
    {
        _db.Questions.Update(obj);
    }

}
