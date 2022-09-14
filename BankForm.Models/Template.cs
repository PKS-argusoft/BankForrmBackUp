using System.ComponentModel.DataAnnotations;

namespace BankForm.Models;

public class Template
{
    [Key]
    public int TemplateId { get; set; }
    [Required]
    public string TemplateName { get; set; }

    public int Order { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; } = -1;
}
