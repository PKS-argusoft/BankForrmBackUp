
using BankForm.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankForm.DataAccess;

public class ApplicationDbContext:IdentityDbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) 
    {

    }

    public DbSet<Template>  Templates { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionType> QuestionTypes { get; set; }
    public DbSet<QuestionOption> QuestionOptions { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}
