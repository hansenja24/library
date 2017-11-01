using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
    public class AvailableCopies
    {
        private int _id;
        private int _numberCopies;

        public AvailableCopies(int numberCopies, int id = 0)
        {
          _numberCopies = numberCopies;
          _id = id;
        }

        public override int GetHashCode()
        {
          return this.GetId().GetHashCode();
        }

        public string GetNumberOfCopies()
        {
          return _numberCopies;
        }

        public int GetId()
        {
          return _id;
        }

        public static List<AvailableCopies> GetAll()
        {
          List<AvailableCopies> allAvailableCopies = new List<AvailableCopies> {};
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM available_copies;";
          var rdr = cmd.ExecuteReader() as MySqlDataReader;
          while(rdr.Read())
          {
            int AvailableCopiesId = rdr.GetInt32(0);
            int AvailableCopiesNumber = rdr.GetInt32(1);
            AvailableCopies newAvailableCopies = new AvailableCopies(AvailableCopiesNumber, AvailableCopiesId);
            allAvailableCopies.Add(newAvailableCopies);
          }
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return allAvailableCopies;
        }

        public void AddOneCopy(Book newBook)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"UPDATE available_copies SET number_of_copies += 1 WHERE book_id = @searchId;";

          MySqlParameter searchId = new MySqlParameter();
          searchId.ParameterName = "@searchId";
          searchId.Value = newBook.GetId();
          cmd.Parameters.Add(searchId);

          cmd.ExecuteNonQuery();
          _id = (int) cmd.LastInsertedId;
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }

        public void ReduceOneCopy(Book newBook)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"UPDATE available_copies SET number_of_copies -= 1 WHERE book_id = @searchId;";

          MySqlParameter searchId = new MySqlParameter();
          searchId.ParameterName = "@searchId";
          searchId.Value = newBook.GetId();
          cmd.Parameters.Add(searchId);

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
