using BankForm.DataAccess.Repository.IRepository;
using BankForm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository;

public class SectionRepository : Repository<Section>, ISectionRepository
{
    private readonly ApplicationDbContext _db;
    public SectionRepository(ApplicationDbContext db):base(db)
    {
        _db = db;
    }

    public void Update(Section obj)
    {
        _db.Sections.Update(obj);
    }
}
