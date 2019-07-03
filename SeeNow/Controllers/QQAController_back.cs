using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SeeNow.Models;
using SeeNow.ViewModels;

namespace SeeNow.Controllers
{
    
    public class QQAController : Controller
    {
        SeeNowEntities db = new SeeNowEntities();

        // GET: QQA
        public ActionResult Index(int id=12)
        {
            QQA qqa = new QQA()
            {
                quizzes = db.quizzes.ToList(),
                answers=db.quiz_answer.Where(m=>m.quiz_guid==id).ToList()
            };

            return View(qqa);
        }
    }
}