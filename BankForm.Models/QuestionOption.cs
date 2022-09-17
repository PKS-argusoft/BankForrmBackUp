using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models;

public class QuestionOption
{
    [Key]
    public int QuestionOptionId { get; set; }
    [Required]
    public string QuestionOptionName { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; } = -1;
    public DateTime UpdatedAt { get; set; }
    public int UpdatedBy { get; set; } = -1;
    
    public int FKQuestionId { get; set; }
    [ForeignKey("FKQuestionId")]
    [ValidateNever]
    public Question Question { get; set; }
}
