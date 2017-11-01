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
    private int _returnstatus;

    public Checkout(int bookid, int patronid, string dateborrowed, string duedate, int returnstatus = 0, int id = 0)
    {
      _id = id;
      _bookid = bookid;
      _patronid = patronid;
      _dateborrowed = dateborrowed;
      _duedate = duedate;
      _returnstatus = returnstatus;
    }

    public string GetId()
    {
      return _id;
    }

    public string GetBookId()
    {
      return _bookid;
    }

    public string GetPatronId()
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

    public string GetReturnStatus()
    {
      return _returnstatus;
    }

    
