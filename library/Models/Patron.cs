using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Patron
  {
    private int _id;
    private string _name;
    private string _dateofbirth;

    public Patron(string name, string dateofbirth, int id = 0)
    {
      _id = id;
      _name = name;
      _dateofbirth = dateofbirth;
    }

    public override int GetHashCode()
    {
      return this.GetName().GetHashCode();
    }

    public string GetName()
    {
      return _name;
    }

    public string GetDateOfBirth()
    {
      return _dateofbirth;
    }

    public int GetId()
    {
      return _id;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO patrons (name, date_of_birth) VALUES (@name, @dateofbirth);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      MySqlParameter dateofbirth = new MySqlParameter();
      dateofbirth.ParameterName = "@dateofbirth";
      dateofbirth.Value = this._dateofbirth;
      cmd.Parameters.Add(dateofbirth);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
       conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Patron> GetAll()
    {
      List<Patron> allPatrons = new List<Patron> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int patronId = rdr.GetInt32(0);
        string patronName = rdr.GetString(1);
        string patronDOB = rdr.GetString(2);
        Patron newPatron = new Patron(patronName, patronDOB, patronId);
        allPatron.Add(newPatron);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allPatrons;
    }

    public static Patron Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM patrons WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int patronId = 0;
      string patronName = "";
      string patronDOB = "";

      while(rdr.Read())
      {
        patronId = rdr.GetInt32(0);
        patronName = rdr.GetString(1);
        patronDOB = rdr.GetString(2);
      }
      Patron newPatron = new Patron(patronName, patronDOB, patronId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newPatron;
    }

    public void CheckoutBook(Checkout newCheckout)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO checkouts (book_id, patron_id, date_borrowed, due_date, return_status) VALUES (@bookId, @patronId, @dateBorrowed, @dueDate, @returnStatus);";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@bookId";
      bookId.Value = newCheckout.GetBookId();
      cmd.Parameters.Add(bookId);

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@patronId";
      patronId.Value = newCheckout.GetPatronId();
      cmd.Parameters.Add(patronId);

      MySqlParameter dateBorrowed = new MySqlParameter();
      dateBorrowed.ParameterName = "@dateBorrowed";
      dateBorrowed.Value = newCheckout.GetDateBorrowed();
      cmd.Parameters.Add(dateBorrowed);

      MySqlParameter dueDate = new MySqlParameter();
      dueDate.ParameterName = "@dueDate";
      dueDate.Value = newCheckout.GetDueDate();
      cmd.Parameters.Add(dueDate);

      MySqlParameter returnStatus = new MySqlParameter();
      returnStatus.ParameterName = "@returnStatus";
      returnStatus.Value = newCheckout.GetReturnStatus();
      cmd.Parameters.Add(returnStatus);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
       conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void ReturnBook(Checkout returnBook)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE checkouts SET return_status =1 WHERE id = @checkoutId;";

      MySqlParameter checkoutId = new MySqlParameter();
      checkoutId.ParameterName = "@checkoutId";
      checkoutId.Value = returnBook.GetId();
      cmd.Parameters.Add(checkoutId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Book> GetBorrowHistory()
    {
      List<Book> allBook = new List<Book> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT books.* FROM checkouts JOIN books ON (checkouts.book_id = books.id) WHERE checkouts.patron_id = @patronId;";

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@patronId";
      patronId.Value = this._id;
      cmd.Parameters.Add(patronId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int BookId = rdr.GetInt32(0);
        string BookName = rdr.GetString(1);
        int bookCopies = rdr.GetInt32(2);
        Book newBook = new Book(BookName, bookCopies, BookId);
        allBook.Add(newBook);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allBook;
    }

    public static void DeletePatron()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      MySqlCommand cmd = new MySqlCommand("DELETE FROM patrons WHERE id = @patronId; DELETE FROM checkouts WHERE patron_id = @PatronId;", conn);
      MySqlParameter PatronIdParameter = new MySqlParameter();
      PatronIdParameter.ParameterName = "@PatronId";
      PatronIdParameter.Value = this.GetId();

      cmd.Parameters.Add(PatronIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

  }
}
