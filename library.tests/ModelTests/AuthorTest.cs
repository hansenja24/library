using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using Library.Models;

namespace Library.Tests
{
  [TestClass]
  public class AuthorTests : IDisposable
  {
    public AuthorTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=library_test;";
    }
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }

    [TestMethod]
    public void Equals_OverrideTrueForSameName_Author()
    {
      //Arrange, Act
      Author firstAuthor = new Author("Jack London");
      Author secondAuthor = new Author("Jack London");

      //Assert
      Assert.AreEqual(firstAuthor, secondAuthor);
    }
  }
}
