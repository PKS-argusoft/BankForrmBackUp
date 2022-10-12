using BankForm.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Template = new TemplateRepository(_db);
        Section = new SectionRepository(_db);
        Question = new QuestionRepository(_db);
        QuestionType = new QuestionTypeRepository(_db);
        QuestionOption = new QuestionOptionRepository(_db);
        Answer = new AnswerRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);


    }
    public ITemplateRepository Template { get; private set; } 
    public IQuestionTypeRepository QuestionType { get; private set; }
    public IQuestionRepository Question { get; private set; }
    public ISectionRepository Section { get; private set; }
    public IQuestionOptionRepository QuestionOption { get; private set; }
    public IAnswerRepository Answer { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set;}



    public void Save()
    {
        _db.SaveChanges();
    }
}
