using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.ViewModel;
using static WebApplication2.ModelsAction.ModAction;

namespace WebApplication2.Controllers
{
    [Authorize]
    //Because Roles don't go along with Entity Framework Code First, we'll need to manually add Roles checking condition in every single ActionResult :)
    public class ModController : Controller
    {
        DBContext db = new DBContext();



        public ActionResult Index()
        {
            if (RoleChecker(Session["Roles"]))
            {
                var id = Int32.Parse(Session["UserID"].ToString());
                User model = db.Users.Where(x => x.UserId == id).FirstOrDefault();
                return View(model);
            }
            return Redirect("~/Home/Index");
        }

        #region BookManagement
        public ActionResult BookManager(string sort, bool? asc, string mode, string q, string searchType, string page)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //This is for the Toolbars Partial
                ViewBag.DataName = "Sách";
                ViewBag.DataType = "Book";

                //TempData after using ActionLink in _PagingView
                if(TempData["SearchType"]!= null && searchType == null)
                {
                    searchType = TempData["SearchType"].ToString();
                }

                //Determines if the list is Ascending or Descending.
                bool newAsc = true;
                if (asc.HasValue)
                {
                    newAsc = asc.Value;
                }
                ViewBag.Asc = newAsc;
                IEnumerable<Book> bookList = db.Books.ToList();

                //If Search is in play, then search only books within the query
                if(q != null && q != "")
                {
                    ViewBag.Search = q;
                    ViewBag.SearchType = searchType;
                    switch (searchType)
                    {
                        case "Author":
                            bookList = bookList.Where(x => x.Authors.AuthorName.Contains(q));
                            break;
                        case "Publisher":
                            bookList = bookList.Where(x => x.Publishers.PublisherName.Contains(q));
                            break;
                        case "Genre":
                            var genre = db.Genres.Where(x => x.GenreName.Contains(q)).FirstOrDefault();
                            if(genre != null)
                            {
                                var junctionList = db.Book_Genre_Junctions.Where(x => x.GenreId == genre.GenreId);
                                bookList = bookList.Join(junctionList, x => x.BookId, y => y.BookId, (x, y) => x);
                            }
                            break;
                        default: //search using Book's Name
                            bookList = bookList.Where(x => x.BookName.Contains(q));
                            break;
                    }
                }

                ViewBag.Mode = mode;
                //Returns the list of book that is either Hidden or Not, or both.
                if (mode == null)
                {
                    mode = "All";
                }
                switch (mode)
                {
                    case "Available":
                        bookList = bookList.Where(x => x.isHiddenBool == false);
                        break;
                    case "Hidden":
                        bookList = bookList.Where(x => x.isHiddenBool == true);
                        break;
                    default:
                        mode = "All";
                        break;
                }

                //Determines which data to sort
                if (sort == null)
                {
                    sort = "BookName";
                }
                else
                {
                    switch (sort)
                    {
                        case "AuthorName":
                            if (newAsc)
                            {
                                bookList = bookList.OrderBy(x => x.Authors.AuthorName);
                            }
                            else
                            {
                                bookList = bookList.OrderByDescending(x => x.Authors.AuthorName);
                            }
                            break;
                        case "PublisherName":
                            if (newAsc)
                            {
                                bookList = bookList.OrderBy(x => x.Publishers.PublisherName);
                            }
                            else
                            {
                                bookList = bookList.OrderByDescending(x => x.Publishers.PublisherName);
                            }
                            break;
                        case "UpdatedDate":
                            if (newAsc)
                            {
                                bookList = bookList.OrderBy(x => x.UpdatedDate);
                            }
                            else
                            {
                                bookList = bookList.OrderByDescending(x => x.UpdatedDate);
                            }
                            break;
                        case "Cost":
                            if (newAsc)
                            {
                                bookList = bookList.OrderBy(x => x.Cost);
                            }
                            else
                            {
                                bookList = bookList.OrderByDescending(x => x.Cost);
                            }
                            break;
                        case "StorageAmount":
                            if (newAsc)
                            {
                                bookList = bookList.OrderBy(x => x.StorageAmount);
                            }
                            else
                            {
                                bookList = bookList.OrderByDescending(x => x.StorageAmount);
                            }
                            break;
                        default: //sorts using BookName by default
                            sort = "BookName";
                            if (newAsc)
                            {
                                bookList = bookList.OrderBy(x => x.BookName);
                            }
                            else
                            {
                                bookList = bookList.OrderByDescending(x => x.BookName);
                            }
                            break;
                    }
                }
                ViewBag.Sort = sort;

                //TempData to work with _PagingView
                TempData["SearchType"] = searchType;

                //Paging settings
                int numOfItems = 20;
                int pageCount = (bookList.Count() + numOfItems - 1) / numOfItems; //suffers from overflow bug, but shouldn't be critical.
                //Exchanging back and forth to double is said to be inefficient, thus the reason why I used the above calculation.
                bookList = ListByPage<Book>(page,numOfItems,bookList);
                ViewBag.Page = page;
                ViewBag.PageCount = pageCount;

                //The .ToList() is necessary to avoid error "There is already an open DataReader associated with this Command which must be closed first"
                return View(bookList.ToList());
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult CreateBook()
        {
            if (RoleChecker(Session["Roles"]))
            {
                //These are for the DropDownLists
                ViewBag.PublisherId = new SelectList(db.Publishers.OrderBy(x => x.PublisherName), "PublisherId", "PublisherName", default);
                ViewBag.AuthorId = new SelectList(db.Authors.OrderBy(x => x.AuthorName), "AuthorId", "AuthorName", default);
                return View();
            }
            return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult CreateBook(Book book, List<GenreViewModel> genres)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //These are for the DropDownLists
                ViewBag.PublisherId = new SelectList(db.Publishers.OrderBy(x => x.PublisherName), "PublisherId", "PublisherName", default);
                ViewBag.AuthorId = new SelectList(db.Authors.OrderBy(x => x.AuthorName), "AuthorId", "AuthorName", default);
                book.UpdatedDate = DateTime.Now;
                var id = db.Books.Count() + 1;
                book.BookId = id;
                if (book.BookCode == "" || book.BookCode == null)
                {
                    book.BookCode = DeAccent(book.BookName);
                }
                if (book.isHiddenBool)
                {
                    book.isHidden = 1;
                }
                else
                {
                    book.isHidden = 0;
                }

                db.Books.Add(book);
                if (ModelState.IsValid)
                {
                    //Add the uploaded image into database and assign it to the book
                    HttpPostedFileBase photo = Request.Files["photo"];
                    UploadImage(photo, book.BookCode);

                    foreach (var genre in genres)
                    {
                        if (genre.GenreIsChecked)
                        {
                            db.Book_Genre_Junctions.Add(new Book_Genre_Junction { BookId = id, GenreId = genre.GenreId });
                        }
                    }

                    db.SaveChanges();
                    return Redirect("~/Shop/BookDetails/" + book.BookCode);
                }
                return View(book);
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult EditBook(int id)
        {
            if (RoleChecker(Session["Roles"]))
            {
                var book = db.Books.Where(x => x.BookId == id).FirstOrDefault();
                //These are for the DropDownLists
                int publisherValue = db.Publishers.ToList().IndexOf(book.Publishers) + 1; //option value starts at 1, but Index starts at 0. Hence the "+"
                int authorValue = db.Authors.ToList().IndexOf(book.Authors) + 1;
                ViewBag.PublisherId = new SelectList(db.Publishers.OrderBy(x => x.PublisherName), "PublisherId", "PublisherName",publisherValue);
                ViewBag.AuthorId = new SelectList(db.Authors.OrderBy(x => x.AuthorName), "AuthorId", "AuthorName", authorValue);
                var genreList = new List<Genre>();
                var junctionList = db.Book_Genre_Junctions.Where(x => x.BookId == id).ToList();
                foreach(var item in junctionList)
                {
                    var genre = db.Genres.Where(x => x.GenreId == item.GenreId).FirstOrDefault();
                    genreList.Add(genre);
                }
                TempData["genreList"] = genreList;
                return View(book);
            }
            return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult EditBook(Book bookModel, List<GenreViewModel> genres)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //Mods cannot change the publisher's name and code.
                var book = db.Books.Where(x => x.BookId == bookModel.BookId).FirstOrDefault();
                //These are for the DropDownLists
                ViewBag.PublisherId = new SelectList(db.Publishers.OrderBy(x => x.PublisherName), "PublisherId", "PublisherName", default);
                ViewBag.AuthorId = new SelectList(db.Authors.OrderBy(x => x.AuthorName), "AuthorId", "AuthorName", default);
                book.UpdatedDate = DateTime.Now;
                if (bookModel.isHiddenBool)
                {
                    book.isHidden = 1;
                }
                else
                {
                    book.isHidden = 0;
                }
                if (TryUpdateModel(book))
                {
                    //Add the uploaded image into database and assign it to the publisher
                    HttpPostedFileBase photo = Request.Files["photo"];
                    UploadImage(photo, book.BookCode);
                    var junctionList = db.Book_Genre_Junctions.Where(x => x.BookId == book.BookId);
                    foreach (var item in junctionList)
                    {
                        db.Book_Genre_Junctions.Remove(item);
                    }

                    foreach (var genre in genres)
                    {
                        if (genre.GenreIsChecked)
                        {
                            db.Book_Genre_Junctions.Add(new Book_Genre_Junction { BookId = book.BookId, GenreId = genre.GenreId });
                        }
                    }
                    db.SaveChanges();
                    return Redirect("~/Shop/BookDetails/" + book.BookCode);
                }
                return View(book);
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult HideBook(int id)
        {
            var book = db.Books.Where(x => x.BookId == id).FirstOrDefault();
            if (book.isHiddenBool)
            {
                book.isHidden = 0;
            }
            else
            {
                book.isHidden = 1;
            }
            book.UpdatedDate = DateTime.Now;
            db.SaveChanges();
            return Redirect("~/Mod/BookManager");
        }

        #endregion

        #region PublisherManagement
        public ActionResult PublisherManager(string sort, bool? asc, string mode, string q, string page)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //This is for the Toolbars Partial
                ViewBag.DataName = "NXB";
                ViewBag.DataType = "Publisher";

                bool newAsc = true;
                if (asc.HasValue)
                {
                    newAsc = asc.Value;
                }
                ViewBag.Asc = newAsc;
                IEnumerable<Publisher> publisherList = db.Publishers.ToList();

                //If Search is in play, then search all Publishers that matches.
                //Of course there could be another SelectList for Publisher and others, but I don't see the point of that.
                if (q != null && q != "")
                {
                    publisherList = publisherList.Where(x => x.PublisherName.Contains(q));
                    ViewBag.Search = q;
                }

                //Returns the list of publisher that is either Hidden or Not, or both.
                if (mode == null)
                {
                    mode = "All";
                }
                switch (mode)
                {
                    case "Available":
                        publisherList = publisherList.Where(x => x.isHiddenBool == false);
                        break;
                    case "Hidden":
                        publisherList = publisherList.Where(x => x.isHiddenBool == true);
                        break;
                    default:
                        mode = "All";
                        break;
                }
                ViewBag.Mode = mode;

                //Counts book from each publisher
                var publisherModelList = new List<PublisherBookCountViewModel>();
                foreach (var publisher in publisherList)
                {
                    int bookCount = db.Books.Where(x => x.PublisherId == publisher.PublisherId).Count();
                    publisherModelList.Add(new PublisherBookCountViewModel { Publisher = publisher, bookCount = bookCount });
                }

                if (sort != null && sort != "")
                {
                    ViewBag.Sort = sort;
                }
                else
                {
                    ViewBag.Sort = "PublisherName";
                }
                switch (sort)
                {
                    case "BookCount":
                        if (newAsc)
                        {
                            publisherModelList = publisherModelList.OrderBy(x => x.bookCount).ToList();
                        }
                        else
                        {
                            publisherModelList = publisherModelList.OrderByDescending(x => x.bookCount).ToList();
                        }
                        break;
                    default: //Publisher's Name
                        if (newAsc)
                        {
                            publisherModelList = publisherModelList.OrderBy(x => x.Publisher.PublisherName).ToList();
                        }
                        else
                        {
                            publisherModelList = publisherModelList.OrderByDescending(x => x.Publisher.PublisherName).ToList();
                        }
                        break;
                }

                //Paging settings
                int numOfItems = 20;
                int pageCount = (publisherModelList.Count() + numOfItems - 1) / numOfItems; //suffers from overflow bug, but shouldn't be critical.
                //Exchanging back and forth to double is said to be inefficient, thus the reason why I used the above calculation.
                publisherModelList = ListByPage<PublisherBookCountViewModel>(page, numOfItems, publisherModelList).ToList();
                ViewBag.Page = page;
                ViewBag.PageCount = pageCount;

                return View(publisherModelList);
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult CreatePublisher()
        {
            if (RoleChecker(Session["Roles"]))
            {
                return View();
            }
            return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult CreatePublisher(Publisher publisher)
        {
            if (RoleChecker(Session["Roles"]))
            {
                publisher.UpdatedDate = DateTime.Now;
                var id = db.Publishers.Count() + 1;
                publisher.PublisherId = id;
                if (publisher.PublisherCode == "" || publisher.PublisherCode == null)
                {
                    publisher.PublisherCode = DeAccent(publisher.PublisherName);
                }
                if (publisher.isHiddenBool)
                {
                    publisher.isHidden = 1;
                }
                else
                {
                    publisher.isHidden = 0;
                }

                db.Publishers.Add(publisher);
                if (ModelState.IsValid)
                {
                    //Add the uploaded image into database and assign it to the book
                    HttpPostedFileBase photo = Request.Files["photo"];
                    UploadImage(photo, publisher.PublisherCode);

                    db.SaveChanges();
                    return Redirect("~/Shop/PublisherDetails/" + publisher.PublisherCode);
                }
                return View(publisher);
            }
            return Redirect("~/Home/Index");
        }
        public ActionResult EditPublisher(int id)
        {
            if (RoleChecker(Session["Roles"]))
            {
                var publisher = db.Publishers.Where(x => x.PublisherId == id).FirstOrDefault();
                return View(publisher);
            }
            return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult EditPublisher(Publisher publisherModel)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //Mods cannot change the publisher's name and code. If they mess up while creating, hide the publisher and create new.
                var publisher = db.Publishers.Where(x => x.PublisherId == publisherModel.PublisherId).FirstOrDefault();
                publisher.UpdatedDate = DateTime.Now;
                if (publisherModel.isHiddenBool)
                {
                    publisher.isHidden = 1;
                }
                else
                {
                    publisher.isHidden = 0;
                }
                if (TryUpdateModel(publisher))
                {
                    //Hiding a publisher of many books will set the publisher for those books to the default "Đang Cập Nhật" publisher / aka 1st publisher.
                    //TO BE ADDED: A Warning if the mods attempt to do so.
                    if (publisher.isHidden == 1)
                    {
                        List<Book> bookList = db.Books.Where(x => x.PublisherId == publisher.PublisherId).ToList();
                        foreach (var book in bookList)
                        {
                            book.PublisherId = 1;
                        }
                    }
                    //Add the uploaded image into database and assign it to the Publisher
                    HttpPostedFileBase photo = Request.Files["photo"];
                    UploadImage(photo, publisher.PublisherCode);

                    db.SaveChanges();
                    return Redirect("~/Shop/PublisherDetails/" + publisher.PublisherCode);
                }
                return View(publisher);
            }
            return Redirect("~/Home/Index");
        }
        public ActionResult HidePublisher(int id)
        {
            if (RoleChecker(Session["Roles"]))
            {
                if(id > 1)
                {
                    var publisher = db.Publishers.Where(x => x.PublisherId == id).FirstOrDefault();
                    if (publisher.isHiddenBool)
                    {
                        publisher.isHidden = 0;
                    }
                    else
                    {
                        publisher.isHidden = 1;
                        List<Book> bookList = db.Books.Where(x => x.PublisherId == publisher.PublisherId).ToList();
                        foreach (var book in bookList)
                        {
                            book.PublisherId = 1;
                        }
                    }
                    publisher.UpdatedDate = DateTime.Now;
                    db.SaveChanges();
                }
                return Redirect("~/Mod/PublisherManager");

            }
            return Redirect("~/Home/Index");
        }

        #endregion

        #region AuthorManagement
        public ActionResult AuthorManager(string sort, bool? asc, string mode, string q, string page)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //This is for the Toolbars Partial
                ViewBag.DataName = "Tác Giả";
                ViewBag.DataType = "Author";

                IEnumerable<Author> authorList = db.Authors.ToList();
                //Sort ascending or descending
                bool newAsc = true;
                if (asc.HasValue)
                {
                    newAsc = asc.Value;
                }
                ViewBag.Asc = newAsc;

                //If Search is in play, then search all Authors that matches.
                //Of course there could be another SelectList for Author and others, but I don't see the point of that.
                if (q != null && q != "")
                {
                    authorList = authorList.Where(x => x.AuthorName.Contains(q));
                    ViewBag.Search = q;
                }

                //View Mode
                if (mode == null)
                {
                    mode = "All";
                }
                switch (mode)
                {
                    case "Available":
                        authorList = db.Authors.Where(x => x.isHiddenBool == false);
                        break;
                    case "Hidden":
                        authorList = db.Authors.Where(x => x.isHiddenBool == true);
                        break;
                    default:
                        mode = "All";
                        break;
                }
                ViewBag.Mode = mode;

                //Counts book from each author
                var authorModelList = new List<AuthorBookCountViewModel>();
                foreach (var author in authorList)
                {
                    int bookCount = db.Books.Where(x => x.AuthorId == author.AuthorId).Count();
                    authorModelList.Add(new AuthorBookCountViewModel { Author = author, bookCount = bookCount });
                }
                if(sort != null && sort != "")
                {
                    ViewBag.Sort = sort;
                }
                else
                {
                    ViewBag.Sort = "AuthorName";
                }
                switch (sort)
                {
                    case "BookCount":
                        if (newAsc)
                        {
                            authorModelList = authorModelList.OrderBy(x => x.bookCount).ToList();
                        }
                        else
                        {
                            authorModelList = authorModelList.OrderByDescending(x => x.bookCount).ToList();
                        }
                        break;
                    default: //Author's Name
                        if (newAsc)
                        {
                            authorModelList = authorModelList.OrderBy(x => x.Author.AuthorName).ToList();
                        }
                        else
                        {
                            authorModelList = authorModelList.OrderByDescending(x => x.Author.AuthorName).ToList();
                        }
                        break;
                }

                //Paging settings
                int numOfItems = 20;
                int pageCount = (authorModelList.Count() + numOfItems - 1) / numOfItems; //suffers from overflow bug, but shouldn't be critical.
                //Exchanging back and forth to double is said to be inefficient, thus the reason why I used the above calculation.
                authorModelList = ListByPage<AuthorBookCountViewModel>(page, numOfItems, authorModelList).ToList();
                ViewBag.Page = page;
                ViewBag.PageCount = pageCount;

                return View(authorModelList);
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult CreateAuthor()
        {
            if (RoleChecker(Session["Roles"]))
            {
                return View();
            }
            return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult CreateAuthor(Author author)
        {
            if (RoleChecker(Session["Roles"]))
            {
                var id = db.Authors.Count() + 1;
                author.AuthorId = id;
                if (author.isHiddenBool)
                {
                    author.isHidden = 1;
                }
                else
                {
                    author.isHidden = 0;
                }
                db.Authors.Add(author);
                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                    return Redirect("~/Mod/AuthorManager");
                }
                return View(author);
            }
            return Redirect("~/Home/Index");
        }
        public ActionResult EditAuthor(int id)
        {
            if (RoleChecker(Session["Roles"]))
            {
                var author = db.Authors.Where(x => x.AuthorId == id).FirstOrDefault();
                return View(author);
            }
            return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult EditAuthor(Author authorModel)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //Mods cannot change the author's name. If they mess up while creating, hide the author and create new.
                var author = db.Authors.Where(x => x.AuthorId == authorModel.AuthorId).FirstOrDefault();
                if (author.isHiddenBool)
                {
                    author.isHidden = 1;
                }
                else
                {
                    author.isHidden = 0;
                }
                if (TryUpdateModel(author))
                {
                    //Hiding an author of many books will set the author for those books to the default "Đang Cập Nhật" author / aka 1st author.
                    //TO BE ADDED: A Warning if the mods attempt to do so.
                    if (author.isHidden == 1)
                    {
                        List<Book> bookList = db.Books.Where(x => x.AuthorId == author.AuthorId).ToList();
                        foreach (var book in bookList)
                        {
                            book.AuthorId = 1;
                        }
                    }
                    db.SaveChanges();
                    return Redirect("~/Mod/AuthorManager");
                }
                return View(author);
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult HideAuthor(int id)
        {
            if (RoleChecker(Session["Roles"]))
            {
                if(id > 1)
                {
                    var author = db.Authors.Where(x => x.AuthorId == id).FirstOrDefault();
                    if (author.isHiddenBool)
                    {
                        author.isHidden = 0;
                    }
                    else
                    {
                        author.isHidden = 1;
                        List<Book> bookList = db.Books.Where(x => x.AuthorId == author.AuthorId).ToList();
                        foreach (var book in bookList)
                        {
                            book.AuthorId = 1;
                        }
                    }
                    db.SaveChanges();
                }
                return Redirect("~/Mod/AuthorManager");
            }
            return Redirect("~/Home/Index");
        }

        #endregion

        #region GenreManagement
        public ActionResult GenreManager(string sort, bool? asc, string mode, string q, string page)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //This is for the Toolbars Partial
                ViewBag.DataName = "Thể Loại";
                ViewBag.DataType = "Genre";

                bool newAsc = true;
                if (asc.HasValue)
                {
                    newAsc = asc.Value;
                }
                ViewBag.Asc = newAsc;
                List<Genre> genreList = db.Genres.ToList();

                //Returns the list of genre that is either Hidden or Not, or both.
                if (mode == null)
                {
                    mode = "All";
                }
                switch (mode)
                {
                    case "Available":
                        genreList = db.Genres.Where(x => x.isHiddenBool == false).ToList();
                        break;
                    case "Hidden":
                        genreList = db.Genres.Where(x => x.isHiddenBool == true).ToList();
                        break;
                    default:
                        mode = "All";
                        break;
                }
                ViewBag.Mode = mode;

                //If Search is in play, then search all Publishers that matches.
                //Of course there could be another SelectList for Publisher and others, but I don't see the point of that.
                if (q != null && q != "")
                {
                    genreList = genreList.Where(x => x.GenreName.Contains(q)).ToList();
                    ViewBag.Search = q;
                }

                //Counts book from each genre
                var genreModelList = new List<GenreBookCountViewModel>();
                foreach (var genre in genreList)
                {
                    int bookCount = db.Book_Genre_Junctions.Where(x => x.GenreId == genre.GenreId).Count(); 
                    genreModelList.Add(new GenreBookCountViewModel { Genre = genre, bookCount = bookCount });
                }

                if (sort != null && sort != "")
                {
                    ViewBag.Sort = sort;
                }
                else
                {
                    ViewBag.Sort = "GenreName";
                }
                switch (sort)
                {
                    case "BookCount":
                        if (newAsc)
                        {
                            genreModelList = genreModelList.OrderBy(x => x.bookCount).ToList();
                        }
                        else
                        {
                            genreModelList = genreModelList.OrderByDescending(x => x.bookCount).ToList();
                        }
                        break;
                    default: //Genre's Name
                        if (newAsc)
                        {
                            genreModelList = genreModelList.OrderBy(x => x.Genre.GenreName).ToList();
                        }
                        else
                        {
                            genreModelList = genreModelList.OrderByDescending(x => x.Genre.GenreName).ToList();
                        }
                        break;
                }

                //Paging settings
                int numOfItems = 20;
                int pageCount = (genreModelList.Count() + numOfItems - 1) / numOfItems; //suffers from overflow bug, but shouldn't be critical.
                //Exchanging back and forth to double is said to be inefficient, thus the reason why I used the above calculation.
                genreModelList = ListByPage<GenreBookCountViewModel>(page, numOfItems, genreModelList).ToList();
                ViewBag.Page = page;
                ViewBag.PageCount = pageCount;

                return View(genreModelList);
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult CreateGenre()
        {
            if (RoleChecker(Session["Roles"]))
            {
                return View();
            }
            return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult CreateGenre(Genre genre)
        {
            if (RoleChecker(Session["Roles"]))
            {
                if (genre.isHiddenBool)
                {
                    genre.isHidden = 1;
                }
                else
                {
                    genre.isHidden = 0;
                }
                db.Genres.Add(genre);
                if (ModelState.IsValid)
                {
                    db.SaveChanges();
                    return Redirect("~/Mod/GenreManager");
                }
                return View(genre);
            }
            return Redirect("~/Home/Index");
        }
        public ActionResult EditGenre(int id)
        {
            if (RoleChecker(Session["Roles"]))
            {
                var genre = db.Genres.Where(x => x.GenreId == id).FirstOrDefault();
                return View(genre);
            }
            return Redirect("~/Home/Index");
        }

        [HttpPost]
        public ActionResult EditGenre(Genre genreModel)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //Mods cannot change the genre's name. If they mess up while creating, hide the Genre and create new.
                var genre = db.Genres.Where(x => x.GenreId == genreModel.GenreId).FirstOrDefault();
                if (genre.isHiddenBool)
                {
                    genre.isHidden = 1;
                }
                else
                {
                    genre.isHidden = 0;
                }
                if (TryUpdateModel(genre))
                {
                    //Hiding a genre will remove its junctions with the associated books.
                    //TO BE ADDED: A Warning if the mods attempt to do so.
                    if (genre.isHidden == 1)
                    {
                        var junctionList = db.Book_Genre_Junctions.Where(x => x.GenreId == genre.GenreId).ToList();
                        foreach (var junction in junctionList)
                        {
                            db.Book_Genre_Junctions.Remove(junction);
                        }
                    }
                    db.SaveChanges();
                    return Redirect("~/Mod/GenreManager");
                }
                return View(genre);
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult HideGenre(int id)
        {
            var genre = db.Genres.Where(x => x.GenreId == id).FirstOrDefault();
            if (genre.isHiddenBool)
            {
                genre.isHidden = 0;
            }
            else
            {
                genre.isHidden = 1;
                var junctionList = db.Book_Genre_Junctions.Where(x => x.GenreId == genre.GenreId).ToList();
                foreach (var junction in junctionList)
                {
                    db.Book_Genre_Junctions.Remove(junction);
                }
            }
            db.SaveChanges();
            return Redirect("~/Mod/GenreManager");
        }
        #endregion

        #region CustomerManagement

        public ActionResult CustomerManager(string sort, bool? asc, string mode, string q, string searchType, string page)
        {
            if (RoleChecker(Session["Roles"]))
            {
                bool newAsc = true;
                if (asc.HasValue)
                {
                    newAsc = asc.Value;
                }
                ViewBag.Asc = newAsc;
                IEnumerable<User> customerList = db.Users.Where(x=> x.Roles == "Customer").ToList();

                //TempData after using ActionLink in _PagingView
                if (TempData["SearchType"] != null && searchType == null)
                {
                    searchType = TempData["SearchType"].ToString();
                }

                //If Search is in play, then search all Users that matches.
                if (q != null && q != "")
                {
                    ViewBag.Search = q;
                    ViewBag.SearchType = searchType;
                    customerList = customerList.Where(x => x.UserName.Contains(q));
                    switch (searchType)
                    {
                        case "UserId":
                            customerList = customerList.Where(x => x.UserId.ToString().Contains(q));
                            break;
                        case "Name":
                            customerList = customerList.Where(x => x.Name.Contains(q));
                            break;
                        default: //search using UserName
                            customerList = customerList.Where(x => x.UserName.Contains(q));
                            break;
                    }
                }

                //Returns the list of Users that is either Active or Banned, or both.
                if (mode == null)
                {
                    mode = "All";
                }
                switch (mode)
                {
                    case "Available":
                        customerList = customerList.Where(x => x.isBannedBool == false);
                        break;
                    case "Banned":
                        customerList = customerList.Where(x => x.isBannedBool == true);
                        break;
                    default:
                        mode = "All";
                        break;
                }
                ViewBag.Mode = mode;

                if (sort != null && sort != "")
                {
                    ViewBag.Sort = sort;
                }
                else
                {
                    ViewBag.Sort = "UserName";
                }
                switch (sort)
                {
                    case "UserId":
                        if (newAsc)
                        {
                            customerList = customerList.OrderBy(x => x.UserId).ToList();
                        }
                        else
                        {
                            customerList = customerList.OrderByDescending(x => x.UserId).ToList();
                        }
                        break;
                    case "Name":
                        if (newAsc)
                        {
                            customerList = customerList.OrderBy(x => x.Name).ToList();
                        }
                        else
                        {
                            customerList = customerList.OrderByDescending(x => x.Name).ToList();
                        }
                        break;
                    default: //Customer's UserName
                        if (newAsc)
                        {
                            customerList = customerList.OrderBy(x => x.UserName).ToList();
                        }
                        else
                        {
                            customerList = customerList.OrderByDescending(x => x.UserName).ToList();
                        }
                        break;
                }

                //TempData to work with _PagingView
                TempData["SearchType"] = searchType;

                //Paging settings
                int numOfItems = 20;
                int pageCount = (customerList.Count() + numOfItems - 1) / numOfItems; //suffers from overflow bug, but shouldn't be critical.
                //Exchanging back and forth to double is said to be inefficient, thus the reason why I used the above calculation.
                customerList = ListByPage<User>(page, numOfItems, customerList);
                ViewBag.Page = page;
                ViewBag.PageCount = pageCount;

                return View(customerList);
            }
            return Redirect("~/Home/Index");
        }
        public ActionResult CustomerDetails(string id)
        {
            if (RoleChecker(Session["Roles"]))
            {
                User user = db.Users.Where(x => x.UserId.ToString() == id).Where(x=> x.Roles == "Customer").FirstOrDefault();
                if(user!= null)
                {
                    return View(user);
                }
                else
                {
                    return Redirect("CustomerManager");
                }
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        public ActionResult BanCustomer(int id)
        {
            var user = db.Users.Where(x => x.UserId == id).FirstOrDefault();
            if (user.isBannedBool)
            {
                user.isBanned = 0;
            }
            else
            {
                user.isBanned = 1;
            }
            db.SaveChanges();
            return Redirect("~/Mod/CustomerManager");
        }

        #endregion

        #region CartManagement

        public ActionResult CartManager(string sort, bool? asc, string mode, string q, string searchType, string page)
        {
            if (RoleChecker(Session["Roles"]))
            {
                //Determines if the list is Ascending or Descending.
                bool newAsc = true;
                if (asc.HasValue)
                {
                    newAsc = asc.Value;
                }
                ViewBag.Asc = newAsc;
                IEnumerable<Cart> cartList = db.Carts.ToList();

                //TempData after using ActionLink in _PagingView
                if (TempData["SearchType"] != null && searchType == null)
                {
                    searchType = TempData["SearchType"].ToString();
                }

                //If Search is in play, then search only Carts within the query
                if (q != null && q != "")
                {
                    ViewBag.Search = q;
                    ViewBag.SearchType = searchType;
                    switch (searchType)
                    {
                        case "UserId":
                            cartList = cartList.Where(x => x.UserId.ToString().Contains(q));
                            break;
                        case "UserName":
                            cartList = cartList.Where(x => x.Users.UserName.Contains(q));
                            break;
                        default: //search using Cart's Id
                            cartList = cartList.Where(x => x.CartId.ToString().Contains(q));
                            break;
                    }
                }

                ViewBag.Mode = mode;
                //Returns the list of Carts that is either Pending, Shipping, Shipped, Cancelled, or All.
                if (mode == null)
                {
                    mode = "Pending";
                }
                switch (mode)
                {
                    case "All":
                        cartList = cartList.Where(x => x.Completed != 0);
                        break;
                    case "Shipping":
                        cartList = cartList.Where(x => x.Completed == 1).Where(x => x.ShipmentProgress == 1);
                        break;
                    case "Shipped":
                        cartList = cartList.Where(x => x.Completed == 1).Where(x => x.ShipmentProgress == 2);
                        break;
                    case "Cancelled":
                        cartList = cartList.Where(x => x.Completed == -1);
                        break;
                    default:
                        mode = "Pending"; //Default is pending, because for sure that's what needed first.
                        cartList = cartList.Where(x => x.Completed == 1).Where(x => x.ShipmentProgress == 0);
                        break;
                }

                //Determines which data to sort
                switch (sort)
                {
                    case "UserId":
                        if (newAsc)
                        {
                            cartList = cartList.OrderBy(x => x.UserId);
                        }
                        else
                        {
                            cartList = cartList.OrderByDescending(x => x.UserId);
                        }
                        break;
                    case "PurchaseDate":
                        if (newAsc)
                        {
                            cartList = cartList.OrderBy(x => x.PurchaseDate);
                        }
                        else
                        {
                            cartList = cartList.OrderByDescending(x => x.PurchaseDate);
                        }
                        break;
                    case "UserName":
                        if (newAsc)
                        {
                            cartList = cartList.OrderBy(x => x.Users.UserName);
                        }
                        else
                        {
                            cartList = cartList.OrderByDescending(x => x.Users.UserName);
                        }
                        break;
                    case "Completed":
                        if (newAsc)
                        {
                            cartList = cartList.OrderBy(x => x.Completed).OrderBy(x => x.ShipmentProgress);
                        }
                        else
                        {
                            cartList = cartList.OrderByDescending(x => x.Completed).OrderByDescending(x => x.ShipmentProgress);
                        }
                        break;
                    default: //sorts using CartId by default
                        sort = "CartId";
                        if (newAsc)
                        {
                            cartList = cartList.OrderBy(x => x.CartId);
                        }
                        else
                        {
                            cartList = cartList.OrderByDescending(x => x.CartId);
                        }
                        break;
                }
                ViewBag.Sort = sort;

                //TempData to work with _PagingView
                TempData["SearchType"] = searchType;

                //Paging settings
                int numOfItems = 20;
                int pageCount = (cartList.Count() + numOfItems - 1) / numOfItems; //suffers from overflow bug, but shouldn't be critical.
                //Exchanging back and forth to double is said to be inefficient, thus the reason why I used the above calculation.
                cartList = ListByPage<Cart>(page, numOfItems, cartList);
                ViewBag.Page = page;
                ViewBag.PageCount = pageCount;

                //The .ToList() is necessary to avoid error "There is already an open DataReader associated with this Command which must be closed first"
                return View(cartList.ToList());
            }
            return Redirect("~/Home/Index");
        }

        public ActionResult CartDetails(string id)
        {
            if (RoleChecker(Session["Roles"]))
            {
                Cart cart = db.Carts.Where(x => x.CartId.ToString() == id).Where(x => x.Completed != 0).FirstOrDefault();
                if (cart != null)
                {
                    TempData["Completed"] = 1;
                    return View(cart);
                }
                else
                {
                    return Redirect("CustomerManager");
                }
            }
            else
            {
                return Redirect("~/Home/Index");
            }
        }

        [HttpPost]
        public ActionResult ApproveCart(Cart inputCart)
        {
            var cart = db.Carts.Where(x => x.CartId == inputCart.CartId).FirstOrDefault();
            cart.ShipmentProgress = 1;
            var valid = true;
            if(DateTime.Compare(inputCart.DeliveryDate, cart.PurchaseDate) > 0)
            {
                foreach (var cartInfo in cart.CartInfos)
                {
                    var book = cartInfo.Books;
                    if (book.StorageAmount < cartInfo.Amount)
                    {
                        TempData["ErrorMessage"] = "Sách " + book.BookName + " chỉ còn lại " + book.StorageAmount + " trong kho.";
                        valid = false;
                        break;
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Ngày Giao Hàng bé hơn Ngày Đặt Hàng";
                valid = false;
            }
            if (TryUpdateModel(cart) && valid)
            {
                foreach (var cartInfo in cart.CartInfos)
                {
                    var book = cartInfo.Books;
                    book.StorageAmount -= cartInfo.Amount;
                }
                var user = db.Users.Where(x => x.UserId == cart.UserId).FirstOrDefault();
                var noti = new Notification{NotiDescription = "Đơn Hàng Của Bạn Vừa Được Xác Nhận", NotiState = 0, NotiDate = DateTime.Now, UserId = user.UserId, CartId = cart.CartId };
                db.Notifications.Add(noti);
                db.SaveChanges();
            }
            return RedirectToAction("CartDetails",new { id = cart.CartId });
        }

        [HttpPost]
        public ActionResult UpdateCart(Cart inputCart)
        {
            var cart = db.Carts.Where(x => x.CartId == inputCart.CartId).FirstOrDefault();
            var valid = true;
            if (DateTime.Compare(inputCart.DeliveryDate, cart.PurchaseDate) < 0)
            {
                TempData["ErrorMessage"] = "Ngày Giao Hàng bé hơn Ngày Đặt Hàng";
                valid = false;
            }
            if (TryUpdateModel(cart) && valid)
            {
                var user = db.Users.Where(x => x.UserId == cart.UserId).FirstOrDefault();
                var noti = new Notification { NotiDescription = "Đơn Hàng Của Bạn Vừa Được Cập Nhật", NotiState = 0, NotiDate = DateTime.Now, UserId = user.UserId, CartId = cart.CartId };
                db.Notifications.Add(noti);
                db.SaveChanges();
            }
            return RedirectToAction("CartDetails", new { id = cart.CartId });
        }

        [HttpPost]
        public ActionResult DeclineCart(Cart inputCart)
        {
            var cart = db.Carts.Where(x => x.CartId == inputCart.CartId).FirstOrDefault();
            cart.Completed = -1;
            if (TryUpdateModel(cart))
            {
                if (cart.ShipmentProgress == 1)
                {
                    cart.ShipmentProgress = 0;
                    foreach (var cartInfo in cart.CartInfos)
                    {
                        var book = db.Books.Where(x => x.BookId == cartInfo.BookId).FirstOrDefault();
                        book.StorageAmount += cartInfo.Amount;
                    }
                }
                var user = db.Users.Where(x => x.UserId == cart.UserId).FirstOrDefault();
                var noti = new Notification { NotiDescription = "Đơn Hàng Của Bạn Vừa Bị Hủy", NotiState = 0, NotiDate = DateTime.Now, UserId = user.UserId, CartId = cart.CartId };
                db.Notifications.Add(noti);
                db.SaveChanges();
            }
            return RedirectToAction("CartDetails", new { id = cart.CartId });
        }

        [HttpPost]
        public ActionResult FinalizeCart(Cart inputCart)
        {
            var cart = db.Carts.Where(x => x.CartId == inputCart.CartId).FirstOrDefault();
            cart.ShipmentProgress = 2;
            var valid = true;
            if (DateTime.Compare(inputCart.DeliveryDate, cart.PurchaseDate) < 0)
            {
                TempData["ErrorMessage"] = "Ngày Giao Hàng bé hơn Ngày Đặt Hàng";
                valid = false;
            }
            if (TryUpdateModel(cart) && valid)
            {
                var user = db.Users.Where(x => x.UserId == cart.UserId).FirstOrDefault();
                var noti = new Notification { NotiDescription = "Đơn Hàng Của Bạn Vừa Được Cập Nhật", NotiState = 0, NotiDate = DateTime.Now, UserId = user.UserId, CartId = cart.CartId };
                db.Notifications.Add(noti);
                db.SaveChanges();
            }
            return RedirectToAction("CartDetails", new { id = cart.CartId });
        }
        #endregion

        #region PartialViews


        [ChildActionOnly]
        public ActionResult _CreateBookGenrePartial()
        {
            var genreListViewModel = new List<GenreViewModel>();
            var genreList = db.Genres.ToList();
            foreach (var genre in genreList)
            {
                var newGenreViewModel = new GenreViewModel() { GenreId = genre.GenreId, GenreCode = genre.GenreCode, GenreName = genre.GenreName, GenreIsChecked = false };
                genreListViewModel.Add(newGenreViewModel);
            }
            if (TempData["genreList"] != null)
            {
                foreach (var genre in TempData["genreList"] as List<Genre>)
                {
                    genreListViewModel.Where(x => x.GenreId == genre.GenreId).FirstOrDefault().GenreIsChecked = true;
                }
            }
            return PartialView(genreListViewModel);
        }

        [ChildActionOnly]
        public ActionResult _ModLeftSideBarCategoryPartial(string name,string type)
        {
            ViewBag.CategoryName = name;
            ViewBag.CategoryType = type;
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult _CustomerDetailsCartPartial(string id)
        {
            int amount = 0;
            var cartList = db.Carts.Where(x => x.UserId.ToString() == id).Where(x=> x.Completed != 0).OrderByDescending(x => x.PurchaseDate).ToList();
            if(cartList.Count() >= 10)
            {
                amount = 10;
                cartList = cartList.Take(10).ToList();
            }
            else
            {
                if(cartList.Count() > 0)
                {
                    amount = cartList.Count();
                }
            }
            ViewBag.Amount = amount;
            return PartialView(cartList);
        }

        [ChildActionOnly]
        public ActionResult _ModCartInfoPartial(int? id)
        {
            var cartInfo = db.CartInfos.Where(x => x.CartId == id).ToList();
            return PartialView(cartInfo);
        }

        #endregion
    }
}