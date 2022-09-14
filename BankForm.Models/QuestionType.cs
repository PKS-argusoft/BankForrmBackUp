using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models;

public class QuestionType
{
    [Key]
    public int QuestionTypeId { get; set; }
    [Required]
    public string QuestionTypes { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; } = -1;
    public DateTime UpdatedAt { get; set; }
    public int UpdatedBy { get; set; } = -1;

}
