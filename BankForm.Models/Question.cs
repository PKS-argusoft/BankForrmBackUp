using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models;

public class Question
{
    [Key]
    public int QuestionId { get; set; }
    [Required]
    public string QuestionName { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; } = -1;

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; } = -1;
    public string QuestionTypeString { get; set; }

    [Required]
    public int QuestionTypeId { get; set; }
    [ForeignKey("QuestionTypeId")]
    [ValidateNever]
    public QuestionType QuestionType { get; set; }

    [Required]
    public int FKSectionId { get; set; }
    [ForeignKey("FKSectionId")]
    [ValidateNever]
    public Section Section { get; set; }

}
