using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
  public class Checkout
  {
    private int _id;
    private int _bookid;
    private int _patronid;
    private string _dateborrowed;
    private string _duedate;
    private bool _returnstatus;

    public Checkout(int bookid, int patronid, string dateborrowed, string duedate, bool returnstatus = false, int id = 0)
    {
      _id = id;
      _bookid = bookid;
      _patronid = patronid;
      _dateborrowed = dateborrowed;
      _duedate = duedate;
      _returnstatus = returnstatus;
    }

    public int GetId()
    {
      return _id;
    }

    public int GetBookId()
    {
      return _bookid;
    }

    public int GetPatronId()
    {
      return _patronid;
    }

    public string GetDateBorrowed()
    {
      return _dateborrowed;
    }

    public string GetDueDate()
    {
      return _duedate;
    }

    public bool GetReturnStatus()
    {
      return _returnstatus;
    }

    public static List<Checkout> GetAll()
    {
      List<Checkout> allCheckout = new List<Checkout> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM checkouts;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int CheckoutId = rdr.GetInt32(0);
        int CheckoutBookId = rdr.GetInt32(1);
        int CheckoutPatronId = rdr.GetInt32(2);
        string CheckoutDateBorrow = rdr.GetString(3);
        string CheckoutDueDate = rdr.GetString(4);
        bool CheckoutReturnStatus = rdr.GetBoolean(5);

        Checkout newCheckout = new Checkout(CheckoutBookId, CheckoutPatronId, CheckoutDateBorrow, CheckoutDueDate, CheckoutReturnStatus, CheckoutId);
        allCheckout.Add(newCheckout);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCheckout;
    }

    public string GetBookTitle()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT title FROM books WHERE books.id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = this._bookid;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      string bookTitle = "";

      while(rdr.Read())
      {
        bookTitle = rdr.GetString(0);
      }
      string title = bookTitle;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return title;
    }

    public string GetPatronName()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT name FROM patrons WHERE patrons.id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = this._patronid;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      string patronName = "";

      while(rdr.Read())
      {
        patronName = rdr.GetString(0);
      }
      string name = patronName;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return name;
    }

    public void Save()
    {

      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO checkouts (book_id, patron_id, date_borrowed, date_due, return_status) VALUES (@bookId, @patronId, @dateBorrowed, @dateDue, @returnStatus);";

      MySqlParameter bookId = new MySqlParameter();
      bookId.ParameterName = "@bookId";
      bookId.Value = this._bookid;
      cmd.Parameters.Add(bookId);

      MySqlParameter patronId = new MySqlParameter();
      patronId.ParameterName = "@patronId";
      patronId.Value = this._patronid;
      cmd.Parameters.Add(patronId);

      MySqlParameter dateBorrowed = new MySqlParameter();
      dateBorrowed.ParameterName = "@dateBorrowed";
      dateBorrowed.Value = this._dateborrowed;
      cmd.Parameters.Add(dateBorrowed);

      MySqlParameter dateDue = new MySqlParameter();
      dateDue.ParameterName = "@dateDue";
      dateDue.Value = this._duedate;
      cmd.Parameters.Add(dateDue);

      MySqlParameter returnStatus = new MySqlParameter();
      returnStatus.ParameterName = "@returnStatus";
      returnStatus.Value = this._returnstatus;
      cmd.Parameters.Add(returnStatus);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
}
}
