using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class BookTests : IDisposable
  {
    public BookTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }

    [TestMethod]
       public void GetAll_BooksEmptyAtFirst_0()
       {
         //Arrange, Act
         int result = Book.GetAll().Count;

         //Assert
         Assert.AreEqual(0, result);
       }

  }
}
