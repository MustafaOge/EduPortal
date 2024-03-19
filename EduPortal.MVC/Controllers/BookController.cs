//using EduPortal.Domain.Entities;
//using EduPortal.Persistence.context;
//using Microsoft.AspNetCore.Mvc;

//namespace EduPortal.Controllers
//{
//    public class BookController : Controller
//    {
//        //        private AppDbContext _db = new AppDbContext();


//        //public IActionResult Index()
//        //{
//        //    List<Book> books = _db.Books.ToList();
//        //    return View(books);
//        //}
//        public IActionResult Create()
//        {
//            return View();
//        }

//        //        [HttpGet]
//        //        public IActionResult Create()
//        //        {
//        //            return View();
//        //        }

//        //        //public IActionResult CreateFakeData()
//        //        //{
//        //        //    for (int i = 0; i < 10; i++)
//        //        //    {
//        //        //        Book book = new Book
//        //        //        {
//        //        //            Title = NameData.GetCompanyName(),
//        //        //            Summary = TextData.GetSentences(2),
//        //        //            Author = NameData.GetFullName(),
//        //        //            PageCount = NumberData.GetNumber(10, 1000),
//        //        //            Published = BooleanData.GetBoolean()
//        //        //        };
//        //        //        _db.Books.Add(book);
//        //        //    }
//        //        //    _db.SaveChanges();

//        //        //    return RedirectToAction("Index");
//        //        //}



//        //        [HttpPost]
//        //        public IActionResult Create(Book book)
//        //        {
//        //            if (ModelState.IsValid)
//        //            {
//        //                _db.Books.Add(book);
//        //                _db.SaveChanges();
//        //                return RedirectToAction("Index");
//        //            }


//        //            return View(book);
//        //        }

//        //        [HttpGet]
//        //        public IActionResult Details(int id)
//        //        {
//        //            Book book = _db.Books.FirstOrDefault(p => p.Id == id);
//        //            return View(book);
//        //        }

//        //        [HttpGet]
//        //        public IActionResult Edit(int id)
//        //        {
//        //            Book book = _db.Books.FirstOrDefault(p => p.Id == id);

//        //            return View(book);
//        //        }

//        //        [HttpPost]
//        //        public IActionResult Edit(Book book, int id)
//        //        {
//        //            if (ModelState.IsValid)
//        //            {
//        //                Book bookdb = _db.Books.FirstOrDefault(p => p.Id == id);
//        //                bookdb.Title = book.Title;
//        //                bookdb.Summary = book.Summary;
//        //                bookdb.Author = book.Author;
//        //                bookdb.PageCount = book.PageCount;
//        //                bookdb.Published = book.Published;

//        //                _db.SaveChanges();
//        //                return RedirectToAction("Index");
//        //            }


//        //            return View(book);
//        //        }

//        //        [HttpGet]
//        //        public IActionResult Delete(int id)
//        //        {
//        //            Book book = _db.Books.FirstOrDefault(p => p.Id == id);
//        //            return View(book);
//        //        }

//        //        [ActionName("Delete")]
//        //        [HttpPost]
//        //        public IActionResult DeleteConfirm(int id)
//        //        {
//        //            Book book = _db.Books.FirstOrDefault(p => p.Id == id);
//        //            _db.Books.Remove(book);
//        //            _db.SaveChanges();
//        //            return RedirectToAction("Index");
//        //        }



//        //    }
//    }
//}
