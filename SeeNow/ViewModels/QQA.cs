using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeeNow.Models;

namespace SeeNow.ViewModels
{
    public class QQA
    {
        public List<quizzes> quizzes { get; set; }
        public List<quiz_answer> answers { get; set; }
        public List<quizzes> quiz { get; set; }

        public List<quiz_group> quiz_Groups { get; set; }
    }
}