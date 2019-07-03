using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SeeNow.Models;

namespace SeeNow.ViewModels
{
    public class setting_menuViewModel
    {
        public int Id { get; set; }
        public string TopMenu { get; set; }
        public string SecondMenu { get; set; }
        public string View { get; set; }
        public string Page { get; set; }
        public string OrderNum { get; set; }
    }
}