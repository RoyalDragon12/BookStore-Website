using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.ModelsAction
{
    public class CartAction
    {
        public static void UpdateCart(int CartId)
        {
            using(var db = new DBContext())
            {
                Cart tmp = db.Carts.Where(x => x.CartId == CartId).FirstOrDefault();
                tmp.TotalCost = 0;
                foreach (var cartInfo in tmp.CartInfos)
                {
                    tmp.TotalCost += cartInfo.TotalCost;
                }
                if (tmp.ShippingAddress == "")
                {
                    User user = db.Users.Where(x => x.UserId == tmp.UserId).FirstOrDefault();
                    tmp.ShippingAddress = user.Address;
                }
                db.SaveChanges();
            }
            return;
        }
        public static Cart CreateOrFindLatestCart(int UserId)
        {
            Cart temp;
            using (var db = new DBContext())
            {
                temp = db.Carts.Where(x => x.UserId == UserId && x.Completed == 0).FirstOrDefault();
                if(temp == null)
                {
                    int cartId = db.Carts.FirstOrDefault().CartId;
                    cartId++;
                    temp = new Cart { CartId = cartId, UserId = UserId, Paid = 0, ShipmentProgress = 0, ShippingAddress = "", TotalCost = 0, ShippingCost = 0, PurchaseDate = DateTime.Now,
                        DeliveryDate = DateTime.Now};
                    db.Carts.Add(temp);
                    db.SaveChanges();
                }
            }
            return temp;
        }

        public static bool AddBook(string bookCode,int amount, int UserId)
        {
            using(var db = new DBContext())
            {
                Cart tmp = db.Carts.Where(x => x.UserId == UserId && x.Completed == 0).FirstOrDefault();
                var book = db.Books.Where(x => x.BookCode == bookCode).Where(x => x.isHiddenBool == false).FirstOrDefault();
                var promoDetail = db.PromotionDetails.Where(x => x.BookId == book.BookId).Join(db.Promotions.Where(y => y.PromoState == 1),
                    x => x.PromotionId, y => y.PromotionId, (x, y) => new { x.DiscountedCost}).FirstOrDefault();
                var bookCost = book.Cost;
                if (promoDetail != null)
                {
                    bookCost = promoDetail.DiscountedCost;
                }
                if(book != null)
                {
                    if(amount < 100 && amount <= book.StorageAmount)
                    {
                        if (tmp != null)
                        {
                            var duplicate_book = false;
                            foreach (var cartInfo in tmp.CartInfos)
                            {
                                if (cartInfo.BookId == book.BookId)
                                {
                                    cartInfo.Amount += amount;
                                    duplicate_book = true;
                                    cartInfo.TotalCost = bookCost * cartInfo.Amount;
                                    break;
                                }
                            }
                            if (!duplicate_book)
                            {
                                db.CartInfos.Add(new CartInfo { Amount = amount, Books = book, BookId = book.BookId, Carts = tmp, SingleCost = bookCost, TotalCost = bookCost * amount });
                            }
                        }
                        else
                        {
                            User user = db.Users.Where(x => x.UserId == UserId).FirstOrDefault();
                            tmp = new Cart
                            {
                                Users = user,
                                PurchaseDate = DateTime.Today,
                                ShipmentProgress = 0,
                                ShippingAddress = "",
                                DeliveryDate = DateTime.Today.AddDays(14),
                                Paid = 0,
                                TotalCost = 0,
                                ShippingCost = 0,
                                Completed = 0
                            };
                            db.Carts.Add(tmp);
                            db.CartInfos.Add(new CartInfo { Amount = amount, Books = book, BookId = book.BookId, Carts = tmp, SingleCost = bookCost, TotalCost = bookCost * amount });
                        }
                        db.SaveChanges();
                        UpdateCart(tmp.CartId);
                        return true;
                    }
                }
                return false;
            }
        }


        public static bool RemoveBook(int BookId, int UserId)
        {
            var BookExisted = false;
            using (var db = new DBContext())
            {
                List<Cart> list = db.Carts.Where(x => x.UserId == UserId && x.Completed == 0).ToList();
                foreach(var cart in list)
                {
                    foreach(var cartInfo in cart.CartInfos)
                    {
                        if (cartInfo.BookId == BookId)
                        {
                            db.CartInfos.Remove(cartInfo);
                            db.SaveChanges();
                            UpdateCart(cart.CartId);
                            BookExisted = true;
                            break;
                        }
                    }
                }
                db.Dispose();
            }
            return BookExisted;
        }
    }
}