using System;
using System.Collections.Generic;
using System.Text;

namespace Labb4DbConsoleApp
{
    public class Question
    {
        public string id { get; set; }
        public string TheQuestion { get; set; }
        public virtual Answer CorrectAnswer { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
