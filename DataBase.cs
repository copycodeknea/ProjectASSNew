using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace ProjectASS

{
    public class DataBase
    {
        string conStr = @"Data Source=localhost;Initial Catalog=HotelDB;Integrated Security=True";
        SqlConnection con;
        //reservation
        public DataBase()
        {
            con = new SqlConnection(conStr);
        }

        public DataTable GetReservation()
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Reservation", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public DataTable SearchReservation(string keyword)
        {
            SqlDataAdapter da = new SqlDataAdapter(
                "SELECT * FROM Reservation WHERE UserID LIKE '%" + keyword + "%' OR UserName LIKE '%" + keyword + "%' OR RoomType LIKE '%" + keyword + "%'",
                con);

            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        //client
        public List<client> GetClients()
        {
            List<client> list = new List<client>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sql = "SELECT * FROM Client";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    client c = new client();
                    c.UserID = dr["UserID"].ToString();
                    c.UserName = dr["UserName"].ToString();
                    c.Phone = dr["Phone"].ToString();
                    c.Country = dr["Country"].ToString();

                    list.Add(c);
                }
            }

            return list;
        }

        // INSERT
        public void InsertClient(client c)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sql = "INSERT INTO Client VALUES(@id,@name,@phone,@country)";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id", c.UserID);
                cmd.Parameters.AddWithValue("@name", c.UserName);
                cmd.Parameters.AddWithValue("@phone", c.Phone);
                cmd.Parameters.AddWithValue("@country", c.Country);

                cmd.ExecuteNonQuery();
            }
        }

        // UPDATE
        public void UpdateClient(client c)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sql = "UPDATE Client SET UserName=@name, Phone=@phone, Country=@country WHERE UserID=@id";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id", c.UserID);
                cmd.Parameters.AddWithValue("@name", c.UserName);
                cmd.Parameters.AddWithValue("@phone", c.Phone);
                cmd.Parameters.AddWithValue("@country", c.Country);

                cmd.ExecuteNonQuery();
            }
        }

        // DELETE
        public void DeleteClient(string id)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sql = "DELETE FROM Client WHERE UserID=@id";
                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        // SEARCH
        public List<client> SearchClient(string keyword)
        {
            List<client> list = new List<client>();

            using (SqlConnection con = new SqlConnection(conStr))
            {
                con.Open();
                string sql = "SELECT * FROM Client WHERE UserID LIKE @key OR UserName LIKE @key";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@key", "%" + keyword + "%");

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    client c = new client();
                    c.UserID = dr["UserID"].ToString();
                    c.UserName = dr["UserName"].ToString();
                    c.Phone = dr["Phone"].ToString();
                    c.Country = dr["Country"].ToString();

                    list.Add(c);
                }
            }

            return list;
        }
    
    }
}

