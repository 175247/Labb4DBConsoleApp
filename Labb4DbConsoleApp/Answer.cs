using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Labb4DbConsoleApp
{
    public class Answer
    {
        public string id { get; set; }
        public string TheAnswer { get; set; }
        public bool IsCorrectAnswer { get; set; } 
        [ForeignKey("id")]
        public string QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
