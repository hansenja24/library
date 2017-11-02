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
  }
}
