using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library;

namespace Library.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [HttpGet("/book/add")]
    public ActionResult AddBook()
    {
      return View();
    }
    [HttpGet("/book/view-all")]
    public ActionResult ViewBooks()
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      List<Book> allBooks = Book.GetAll();

      return View("ViewBooks", allBooks);
    }

    [HttpPost("/book/view-all")]
    public ActionResult AddViewBooks()
    {
      Book newBook = new Book(Request.Form["book-title"], Int32.Parse(Request.Form["book-copies"]));
      newBook.Save();
      int authorValue = Int32.Parse(Request.Form["number-loop"]);
      for(var i=1;i<=authorValue;i++)
      {
        Author newAuthor = new Author(Request.Form["author-name"+i]);
        if(newAuthor.IsNewAuthor() == true)
        {
          newAuthor.Save();
          newBook.AddAuthor(newAuthor);
        }
        else
        {
          Author repeatAuthor = newAuthor.FindAuthor();
          newBook.AddAuthor(repeatAuthor);
        }
      }
      List<Book> allBooks = Book.GetAll();

      return View("ViewBooks", allBooks);
    }

    [HttpPost("/search/authors")]
    public ActionResult SearchAuthors()
    {

        List<Book> booksFound = Author.SearchByAuthor(Request.Form["search-author-name"]);

        return View("ViewBooks", booksFound);
    }

    [HttpPost("/search/book")]
    public ActionResult SearchBook()
    {

        List<Book> booksFound = Book.SearchByTitle(Request.Form["search-book-title"]);

        return View("ViewBooks", booksFound);
    }

    [HttpGet("/patron/add")]
    public ActionResult AddPatron()
    {
      return View();
    }

    [HttpPost("/patron/view-all")]
    public ActionResult AddViewPatron()
    {
      Patron newPatron = new Patron(Request.Form["patron-name"], Request.Form["patron-birth-date"]);
      newPatron.Save();

      return View("Index");
    }

    [HttpGet("/patron/view-all")]
    public ActionResult ViewPatron()
    {
      List<Patron> allPatrons = Patron.GetAll();

      return View("ViewPatrons", allPatrons);
    }

    [HttpGet("/book/{bookId}/checkout")]
    public ActionResult CheckoutBook(int bookId)
    {
      Book checkoutBook = Book.Find(bookId);

      return View("Checkout", checkoutBook);
    }

    [HttpPost("/book/{bookId}/checkout/confirm")]
    public ActionResult CheckoutConfirm(int bookId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Book checkoutBook = Book.Find(bookId);
      Patron checkoutPatron = Patron.Find(Int32.Parse(Request.Form["patron-id"]));
      if (checkoutPatron.GetName() == "")
      {
        return View("CheckoutError", bookId);
      }
      else
      {
        Checkout newCheckout = new Checkout(bookId, Int32.Parse(Request.Form["patron-id"]), Request.Form["date-borrowed"], Request.Form["date-due"]);

        newCheckout.Save();

        model.Add("book", checkoutBook);
        model.Add("patron", checkoutPatron);
        model.Add("checkout", newCheckout);

        return View("CheckoutConfirm", model);
      }
    }

    [HttpGet("/checkout/list")]
    public ActionResult BorrowedList()
    {
      List<Checkout> allCheckouts = Checkout.GetAll();

      return View("BorrowedList", allCheckouts);
    }
  }
}
