using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
namespace WebApplication2.Controllers
{
    public class InfoController : Controller
    {
        DBContext db = new DBContext();
        // GET: Info
        public ActionResult _BookInfoPartial()
        {
            var info = db.Publishers.ToList();
            return PartialView(info);
        }
    }
}