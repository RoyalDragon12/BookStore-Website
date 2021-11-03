using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication2.Models;
using WebApplication2.ModelsAction;
using WebApplication2.ViewModel;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        DBContext db = new DBContext();

        //For testing only
        [Authorize]
        public ActionResult UpdateData()
        {
            if(Session["Roles"].ToString() == "Mod")
            {
                var cartInfoList = db.CartInfos.ToList();
                foreach (var cartInfo in cartInfoList)
                {
                    var bookCost = cartInfo.Books.Cost;
                    cartInfo.SingleCost = bookCost;
                    cartInfo.TotalCost = bookCost * cartInfo.Amount;
                }
                db.SaveChanges();

                var cartList = db.Carts.ToList();
                foreach (var cart in cartList)
                {
                    CartAction.UpdateCart(cart.CartId);
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            User user = db.Users.Where(x => x.UserName == UserName).Where(x => x.Password == Password).FirstOrDefault();
            if (user != null)
            {
                if (!user.isBannedBool)
                {
                    //Roles.CreateRole("Blah"); <-- This doesn't work with Entity Framework Code First tests because you need to run aspnet_regsql EVERY SINGLE TIME you start. NOPE.
                    Session.Add("UserName", UserName);
                    Session.Add("UserID", user.UserId);
                    Session.Add("Roles", user.Roles);
                    FormsAuthentication.SetAuthCookie(UserName, false);
                    ViewBag.Login = UserName;
                    string redirectUrl = "";
                    switch (user.Roles)
                    {
                        case "Mod":
                            redirectUrl = "~/Mod/Index";
                            break;
                        case "Admin":
                            redirectUrl = "~/Admin/Index";
                            break;
                        case "Customer":
                            redirectUrl = "~/Home/Index";
                            break;
                    }
                    return Redirect(redirectUrl);
                }
                else
                {
                    ViewBag.ErrorMessage = "This account has been banned";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Wrong UserName or Password!";
            }
            return View(new {UserName });
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(User user)
        {
            if (TryUpdateModel(user))
            {
                if (UserAction.CreateUser(user,"Customer"))
                {
                    Login(user.UserName, user.Password);
                    return Redirect("~/Home/Index");
                }
            }
            return View(user);
        }

        [Authorize]
        public ActionResult Logoff()
        {
            if (Session["UserName"] != null)
            {
                Session.Remove("UserName");
                Session.Remove("UserID");
                Session.Remove("Roles");
                FormsAuthentication.SignOut();
            }
            return Redirect("~/Home/Index");
        }
        
        [Authorize]
        public ActionResult UserInfo()
        {
            if (Session["Roles"] != null)
            {
                if (Session["Roles"].ToString() != "Mod") //Mods can't change their info, has to rely on Admin to change for them.
                {
                    int userId = Int32.Parse(Session["UserID"].ToString());
                    var user = db.Users.Where(x => x.UserId == userId).FirstOrDefault();
                    UpdateUserViewModel model = new UpdateUserViewModel();
                    model.Name = user.Name;
                    model.Email = user.Email;
                    model.Password = user.Password;
                    model.PhoneNumber = user.PhoneNumber;
                    model.UserId = user.UserId;
                    return View(model);
                }
            }
            return Redirect("~/Home/Login");
        }
        [HttpPost]
        public ActionResult UserInfo(UpdateUserViewModel user)
        {
            if (TryUpdateModel(user))
            {
                string result = UserAction.UpdateUser(user);
                if (result != "")
                {
                    TempData["ErrorMessage"] = result;
                }
            }
            return View(user);
        }

        public ActionResult Notifications()
        {
            if(Session["UserID"] != null)
            {
                int userId = Int32.Parse(Session["UserID"].ToString());
                var notiList = db.Notifications.Where(x => x.UserId == userId).ToList();
                return View(notiList);
            }
            return RedirectToAction("Index");
        }

        #region PartialViews
        [ChildActionOnly]
        public ActionResult _PagingView(string page, int pageCount, string actionName,string controllerName, string sort, bool asc, string mode,string q)
        {
            if (pageCount <= 0)
            {
                pageCount = 1;
            }
            int pageNumber = 1;
            if (page != null && int.TryParse(page, out pageNumber))
            {
                if (pageNumber < 1)
                {
                    pageNumber = 1;
                }
            }
            if(pageNumber > pageCount)
            {
                pageNumber = pageCount;
            }
            ViewBag.Page = pageNumber;
            ViewBag.PageCount = pageCount;
            ViewBag.ActionName = actionName;
            ViewBag.ControllerName = controllerName;
            ViewBag.Sort = sort;
            ViewBag.Asc = asc;
            ViewBag.Mode = mode;
            ViewBag.Search = q;
            return PartialView();
        }
        #endregion

        #region Validation
        public JsonResult CheckUserName(string userName)
        {
            var result = db.Users.Where(x => x.UserName == userName).Count() == 0;
            return Json(result, JsonRequestBehavior.AllowGet); //automatically check duplicate UserName in real time.
        }
        public JsonResult CheckAvailableAmount()
        {
            //This is what I get for trying to pull data out from a list.
            var Amount = Int32.Parse(Request.QueryString[Request.QueryString.AllKeys.First(p => p.Contains("Amount"))]);
            var BookId = Int32.Parse(Request.QueryString[Request.QueryString.AllKeys.First(p => p.Contains("BookId"))]);
            var book = db.Books.Where(x => x.BookId == BookId).FirstOrDefault();
            var result = book.StorageAmount >= Amount;
            return Json(result, JsonRequestBehavior.AllowGet); //automatically check available amount in real time.
        }
        #endregion
    }
}