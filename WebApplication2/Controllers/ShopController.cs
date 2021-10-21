using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.ModelsAction;

namespace WebApplication2.Controllers
{
    public class ShopController : Controller
    {
        DBContext db = new DBContext();


        public void UpdateCartViewBag(Cart cart)
        {
            CartAction.UpdateCart(cart.CartId);
        }
        public ActionResult Index(string id, string q) //id = books' category, like Genre or Publisher, etc. , q = the category's searching code.
        {
            List<Book> BookList = new List<Book>();
            if (q != null)
            {
                switch (id)
                {
                    case "genre":
                        Genre genre = db.Genres.Where(x => x.GenreCode == q).FirstOrDefault();
                        if(genre == null || genre.isHiddenBool)
                        {
                            break;
                        }
                        List<int> book_Id_List = new List<int>();
                        var list = db.Book_Genre_Junctions.Where(x => x.GenreId == genre.GenreId).ToList();
                        foreach(var item in list)
                        {
                            book_Id_List.Add(item.BookId);
                        }
                        foreach(int book_Id in book_Id_List)
                        {
                            var book = db.Books.Where(x => x.BookId == book_Id).Where(x => x.isHiddenBool == false).FirstOrDefault();
                            BookList.Add(book);
                        }
                        TempData["Category"] = genre.GenreName;
                        break;
                    case "publisher":
                        Publisher publisher = db.Publishers.Where(x => x.PublisherCode == q).FirstOrDefault();
                        if (publisher == null || publisher.isHiddenBool)
                        {
                            break;
                        }
                        BookList = db.Books.Where(x => x.PublisherId == publisher.PublisherId).Where(x => x.isHiddenBool == false).ToList();
                        TempData["Category"] = publisher.PublisherName;
                        break;
                    case "author":
                        Author author = db.Authors.Where(x => x.AuthorName == q).FirstOrDefault();
                        if (author == null || author.isHiddenBool)
                        {
                            break;
                        }
                        BookList = db.Books.Where(x => x.AuthorId == author.AuthorId).Where(x => x.isHiddenBool == false).ToList();
                        TempData["Category"] = author.AuthorName;
                        break;
                    case "promotion":
                        Promotion promo = db.Promotions.Where(x => x.PromotionId.ToString() == q).FirstOrDefault();
                        if(promo == null || promo.PromoState == 0)
                        {
                            break;
                        }
                        BookList.Clear();
                        foreach(var promoDetails in promo.PromotionDetails)
                        {
                            var book = promoDetails.Books;
                            BookList.Add(book);
                        }
                        TempData["Category"] = promo.PromoName;
                        break;
                    default:
                        BookList = db.Books.Where(x => x.BookName.Contains(q)).Where(x => x.isHiddenBool == false).Take(10).ToList();
                        TempData["Category"] = q;
                        break;
                }
            }
            else
            {
                BookList = db.Books.Where(x => x.isHiddenBool == false).ToList();
            }
            return View(BookList);
        }

        [Authorize]
        public ActionResult Cart(int? id, int? notiId)
        {
            if (Session["UserID"] != null)
            {
                Cart cart = db.Carts.FirstOrDefault();
                int UserId = Int32.Parse(Session["UserID"].ToString());
                if (id == null)
                {
                    cart = CartAction.CreateOrFindLatestCart(UserId);
                    UpdateCartViewBag(cart);
                    return RedirectToAction("Cart", new { id = cart.CartId });
                }
                else
                {
                    cart = db.Carts.Where(x => x.CartId == id).FirstOrDefault();
                    if(cart.UserId == UserId)
                    {
                        if (notiId != null)
                        {
                            var noti = db.Notifications.Where(x => x.NotificationId == notiId).FirstOrDefault();
                            noti.NotiState = 1;
                            db.SaveChanges();
                        }
                        TempData["Completed"] = cart.Completed.ToString();
                        UpdateCartViewBag(cart);
                        return View(cart);
                    }
                    else
                    {
                        //prevent user from messing other users's cart.
                        return new HttpStatusCodeResult(statusCode: 403);
                    }
                }
            }
            else
                return Redirect("~/Home/Login");
        }

        public ActionResult CartList()
        {
            List<Cart> cartList;
            if(Session["UserID"] != null)
            {
                int UserId = Int32.Parse(Session["UserID"].ToString());
                cartList = db.Carts.Where(x => x.UserId == UserId).ToList();
            }
            else
            {
                return Redirect("~/Home/Login");
            }
            return View(cartList);
        }

        public ActionResult BookDetails(string id)
        {
            var book = db.Books.Where(x => x.isHiddenBool == false).FirstOrDefault();
            if(id != null & id != "")
            {
                book = db.Books.Where(x => x.BookCode == id).FirstOrDefault();
                if(book == null || book.isHiddenBool)
                {
                    return Redirect("~/Home/Index");
                }
            }
            return View(book);
        }
        public ActionResult PublisherDetails(string id)
        {
            Publisher publisher = db.Publishers.FirstOrDefault();
            if(id != "")
            {
                publisher = db.Publishers.Where(x => x.PublisherCode == id).FirstOrDefault();
                if(publisher == null || publisher.isHiddenBool)
                {
                    return Redirect("~/Home/Index");
                }
            }
            return View(publisher);
        }
        public ActionResult PromotionDetails(string id)
        {
            if (id != null & int.TryParse(id, out var new_id))
            {
                var promo = db.Promotions.Where(x => x.PromotionId == new_id).FirstOrDefault();
                if (promo != null)
                {
                    return View(promo);
                }
            }
            return Redirect("~/Home/Index");
        }

        #region  Actions
        public ActionResult Search()
        {
            return Redirect("~Shop/Index");
        }

        [HttpPost]
        public ActionResult Search(string q)
        {
            return RedirectToAction("Index", new { id = "tim-kiem", q } );
        }


        //There should be another page to reconfirm the order, but to keep it simplified, we'll skip it
        [HttpPost]
        public ActionResult CheckoutCart(Cart cart)
        {
            //TO BE ADDED: Notify the user if they have any Hidden book in their cart and tell them to remove it ?
            Cart userCart = db.Carts.Where(x => x.CartId == cart.CartId).FirstOrDefault();
            var valid = true;
            foreach(var cartInfo in userCart.CartInfos)
            {
                var book = db.Books.Where(x => x.BookId == cartInfo.BookId).FirstOrDefault();
                if (cartInfo.Amount > book.StorageAmount)
                {
                    valid = false;
                    TempData["Error"] = cartInfo.Books.BookName + " có số lượng đặt lớn hơn hàng tồn kho";
                }
            }
            if (valid)
            {
                userCart.PurchaseDate = DateTime.Now;
                userCart.DeliveryDate = DateTime.Now.AddDays(14);
                userCart.Completed = 1;
                if (cart.PaidBool)
                {
                    userCart.Paid = 1;
                }
                else
                {
                    userCart.Paid = 0;
                }
                userCart.ShippingAddress = cart.ShippingAddress;
                db.SaveChanges();
            }
            return RedirectToAction("Cart", new { id = cart.CartId });
        }
        public ActionResult ClearCart(int id)
        {
            Cart cart = db.Carts.Where(x => x.CartId == id).FirstOrDefault();
            int userId = 0;
            if(Session["UserId"].ToString() == cart.UserId.ToString())
            {
                userId = cart.UserId;
                List<CartInfo> cartInfos = db.CartInfos.Where(x => x.CartId == cart.CartId).ToList();
                foreach(var item in cartInfos)
                {
                    CartAction.RemoveBook(item.CartInfoId, userId);
                }
            }
            else
            {
                return Redirect("~Shop/Index");
            }
            return RedirectToAction("Cart", new {id});
        }

        public ActionResult Add(string id, string amount)
        {
            if (Session["UserID"] != null)
            {
                //This is to prevent people purposely give anything other than int into the "amount" parameter
                if (string.IsNullOrWhiteSpace(amount))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                if (!int.TryParse(amount,out var new_amount))
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                int UserId = Int32.Parse(Session["UserID"].ToString());
                var result = CartAction.AddBook(id,new_amount, UserId);
                if (result)
                {
                    return Redirect("~/Shop/Cart");
                }
                else
                {
                    TempData["ErrorMessage"] = "Số Lượng Đặt Quá Lớn";
                    return RedirectToAction("BookDetails", new { id });
                }
            }
            else
                return Redirect("~/Home/Login");
        }

        public ActionResult Remove(int id)
        {
            if (Session["UserID"] != null)
            {
                int UserId = Int32.Parse(Session["UserID"].ToString());
                if (CartAction.RemoveBook(id, UserId))
                {
                    ViewBag.CartMessage = "Removed Successfully";
                }
                else
                {
                    ViewBag.CartMessage = "Book not found";
                }
                return Redirect("~/Shop/Cart");
            }
            else
                return Redirect("~/Home/Login");
        }

        [HttpPost]
        public ActionResult Update(List<CartInfo> cartInfoList)
        {
            if(cartInfoList != null)
            {
                foreach (var item in cartInfoList)
                {
                    var cartInfo = db.CartInfos.Where(x => x.CartInfoId == item.CartInfoId).FirstOrDefault();
                    if (item.Amount == 0)
                    {
                        db.CartInfos.Remove(cartInfo);
                    }
                    else
                    {
                        cartInfo.Amount = item.Amount;
                        cartInfo.TotalCost = cartInfo.Books.Cost * item.Amount;
                    }
                    if (TryUpdateModel(cartInfo))
                    {
                        db.SaveChanges();
                    }
                }
            }

            return Redirect("~/Shop/Cart");
        }

        public ActionResult CancelCart(int id)
        {
            Cart cart = db.Carts.Where(x => x.CartId == id).FirstOrDefault();
            if (Session["UserId"].ToString() == cart.UserId.ToString())
            {
                cart.Completed = -1;
                if (cart.ShipmentProgress == 1)
                {
                    cart.ShipmentProgress = 0;
                    foreach (var cartInfo in cart.CartInfos)
                    {
                        var book = db.Books.Where(x => x.BookId == cartInfo.BookId).FirstOrDefault();
                        book.StorageAmount += cartInfo.Amount;
                    }
                }
                db.SaveChanges();
                return Redirect("~Shop/CartList");
            }
            else
            {
                return Redirect("~Shop/Index");
            }
        }
        #endregion
        #region PartialViews

        [ChildActionOnly]
        public ActionResult _GenrePartial()
        {
            return PartialView(db.Genres.Where(x => x.isHidden == 0).ToList());
        }

        [ChildActionOnly]
        public ActionResult _PublisherPartial()
        {
            return PartialView(db.Publishers.Where(x => x.isHidden == 0).ToList());
        }

        [ChildActionOnly]
        public ActionResult _AuthorPartial()
        {
            return PartialView(db.Authors.Where(x => x.isHidden == 0).ToList());
        }

        [ChildActionOnly]
        public ActionResult _PromotionPartial()
        {
            return PartialView(db.Promotions.Where(x => x.PromoState == 1).ToList());
        }

        [ChildActionOnly]
        public ActionResult _BookPartial(int id)
        {
            var book = db.Books.Where(x => x.BookId == id).FirstOrDefault();
            if (book.isHiddenBool)
            {
                //Gives a random non-hidden book instead
                var bookCount = db.Books.Where(x=> x.isHiddenBool == false).Count();
                Random rng = new Random();
                int random = rng.Next(0, bookCount);
                book = db.Books.Where(x => x.isHiddenBool == false).ToList().ElementAt(random);
            }
            return PartialView(book);
        }

        [ChildActionOnly]
        public ActionResult _BookListPartial(int id, string type)
        {
            var book = db.Books.ToList();
            switch (type)
            {
                case "publisher":
                    book = db.Books.Where(x => x.PublisherId == id && x.isHiddenBool == false).ToList();
                    break;
                case "promotion":
                    book.Clear();
                    var promoList = db.PromotionDetails.Where(x => x.PromotionId == id).ToList();
                    foreach (var promoDetails in promoList)
                    {
                        var promoBook = db.Books.Where(x => x.BookId == promoDetails.BookId && x.isHiddenBool == false).FirstOrDefault();
                        if(promoBook != null)
                        {
                            book.Add(promoBook);
                        }
                    }
                    break;
                default:
                    break;
            }
            return PartialView(book);
        }

        [ChildActionOnly]
        public ActionResult _BookCategoryPartial()
        {
            //Informs the user of what category the user is searching from
            string output = "";
            if (RouteData.Values["id"] != null)
            {
                string value = RouteData.Values["id"].ToString();
                switch (value)
                {
                    case "genre":
                        output = "Theo Thể Loại: ";
                        break;
                    case "publisher":
                        output = "Của NXB: ";
                        break;
                    case "author":
                        output = "Của Tác Giả: ";
                        break;
                    case "promotion":
                        output = "Theo Khuyến Mãi: ";
                        break;
                    default:
                        output = "Tìm Theo: ";
                        break;
                }
            }
            if (TempData["Category"] != null)
            {
                string category = TempData["Category"].ToString();
                if(category.Length >= 15) //take maximum 15 chars from a search string
                {
                    category = category.Substring(0, 14);
                }
                output = output + category;
            }
            return PartialView((object)output);
        }

        [ChildActionOnly]
        public ActionResult _CartPartial(int? id)
        {
            if (Session["UserID"] != null && id != null)
            {
                var cart = db.Carts.Where(x => x.CartId == id).FirstOrDefault();
                return PartialView(cart);
            }
            return PartialView(new Cart());
        }

        [ChildActionOnly]
        public ActionResult _CartInfoPartial(int? id)
        {
            if (Session["UserID"] != null && id != null)
            {
                var cartInfo = db.CartInfos.Where(x => x.CartId == id).ToList();
                return PartialView(cartInfo);
            }
            return PartialView(new List<CartInfo>());
        }

        [ChildActionOnly]
        public ActionResult _BookDetailsGenrePartial(int id)
        {
            List<Genre> genreList = new List<Genre>();
            List<int> genres_Id_List = new List<int>();
            var all_genres = db.Book_Genre_Junctions.Where(x => x.BookId == id);
            foreach (var item in all_genres)
            {   genres_Id_List.Add(item.GenreId);
            }
            foreach(int genre_id in genres_Id_List)
            {
                var genre = db.Genres.Where(x => x.GenreId == genre_id).FirstOrDefault();
                genreList.Add(genre);
            }
            return PartialView(genreList);
        }
        [ChildActionOnly]
        public ActionResult _BookCostPartial(int id)
        {
            var book = db.Books.Where(x => x.BookId == id).FirstOrDefault();
            var promoDetail = db.PromotionDetails.Where(x => x.BookId == id).Join(db.Promotions.Where(y => y.PromoState == 1),x => x.PromotionId, y => y.PromotionId, 
                (x,y) => new {
                    x.PromotionId,
                    x.DiscountedCost
            }).FirstOrDefault();
            if(promoDetail != null)
            {
                ViewBag.DiscountedCost = promoDetail.DiscountedCost;
                double discountPercent = (book.Cost - promoDetail.DiscountedCost) / book.Cost * 100;
                discountPercent = Math.Round(discountPercent, 0);
                ViewBag.DiscountPercent = "-" + discountPercent + "%";
            }
            return PartialView(book);
        }
        #endregion
    }
}