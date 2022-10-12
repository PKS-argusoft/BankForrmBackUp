using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankForm.Models;

public class Answer
{
    [Key]
    public int AnswerId { get; set; }
    public string AnswerName { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; } = -1;
    public DateTime UpdatedAt { get; set; }
    public int UpdatedBy { get; set; } = -1;

    public int QuestionId { get; set; }
    [ForeignKey("QuestionId")]
    [ValidateNever]
    public Question Question { get; set; }

    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }
}
