using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace Library.Models
{
    public class Author
    {
        private int _id;
        private string _name;

        public Author(string name, int id = 0)
        {
          _name = name;
          _id = id;
        }

        public override bool Equals(System.Object otherAuthor)
        {
          if(!(otherAuthor is Author))
          {
            return false;
          }
          else
          {
            Author newAuthor = (Author) otherAuthor;
            return this.GetId().Equals(newAuthor.GetId());
          }
        }

        public override int GetHashCode()
        {
          return this.GetId().GetHashCode();
        }

        public string GetName()
        {
          return _name;
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
          cmd.CommandText = @"INSERT INTO authors (name) VALUES (@name);";

          MySqlParameter name = new MySqlParameter();
          name.ParameterName = "@name";
          name.Value = this._name;
          cmd.Parameters.Add(name);

          cmd.ExecuteNonQuery();
          _id = (int) cmd.LastInsertedId;
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }

        public static List<Author> GetAll()
        {
          List<Author> allAuthor = new List<Author> {};
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM authors;";
          var rdr = cmd.ExecuteReader() as MySqlDataReader;
          while(rdr.Read())
          {
            int AuthorId = rdr.GetInt32(0);
            string AuthorName = rdr.GetString(1);
            Author newAuthor = new Author(AuthorName, AuthorId);
            allAuthor.Add(newAuthor);
          }
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return allAuthor;
        }

        public static Author Find(int id)
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT * FROM authors WHERE id = (@searchId);";

          MySqlParameter searchId = new MySqlParameter();
          searchId.ParameterName = "@searchId";
          searchId.Value = id;
          cmd.Parameters.Add(searchId);

          var rdr = cmd.ExecuteReader() as MySqlDataReader;
          int AuthorId = 0;
          string AuthorName = "";

          while(rdr.Read())
          {
            AuthorId = rdr.GetInt32(0);
            AuthorName = rdr.GetString(1);
          }
          Author newAuthor = new Author(AuthorName, AuthorId);
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return newAuthor;
        }

        public static void DeleteAll()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM authors;";
          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }

    }
}
