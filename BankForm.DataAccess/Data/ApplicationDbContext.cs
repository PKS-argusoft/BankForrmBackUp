
using BankForm.Models;
using Microsoft.EntityFrameworkCore;

namespace BankForm.DataAccess;

public class ApplicationDbContext:DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
    {

    }

    public DbSet<Template>  Templates { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
}
