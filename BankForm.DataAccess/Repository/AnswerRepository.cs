using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private ApplicationDbContext _db;
        public AnswerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Answer obj)
        {
            _db.Answers.Update(obj);
        }

      
    }

}
