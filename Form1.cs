using System;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // 🔐 Password Hash Method
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void Loginbtn_Click_1(object sender, EventArgs e)
        {
            string email = Emailtxt.Text.Trim();
            string password = Passwordtxt.Text;

            // ✅ Input validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            // Hash the entered password
            string hashedPassword = HashPassword(password);

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // ✅ Cleaner SQL query
                    string query = "SELECT 1 FROM Login WHERE Email=@Email AND Password=@Password";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        MessageBox.Show("Login successful!");

                        Mainpagecs mainpagecs = new Mainpagecs();
                        this.Hide();
                        mainpagecs.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid email or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        private void Closebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}