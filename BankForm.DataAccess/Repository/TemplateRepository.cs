using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository;

public class TemplateRepository : Repository<Template>, ITemplateRepository
{
    private ApplicationDbContext _db;

    public TemplateRepository(ApplicationDbContext db):base(db)
    {
        _db = db;
    }


    public void Update(Template obj)
    {
        _db.Templates.Update(obj);
    }
}
