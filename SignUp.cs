using System;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();
        }

        // 🔐 Password Hash Method (same as login)
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

        private void SignUpbtn_Click(object sender, EventArgs e)
        {
            string email = Semailtxt.Text.Trim();
            string password = Spasswordtxt.Text;

            // ✅ Input validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            // Hash the password
            string hashedPassword = HashPassword(password);

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // ✅ Check if email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Login WHERE Email=@Email";
                    SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Email", email);

                    int userExists = (int)checkCmd.ExecuteScalar();
                    if (userExists > 0)
                    {
                        MessageBox.Show("Email already registered. Please use a different email.");
                        return;
                    }

                    // ✅ Insert new user
                    string insertQuery = "INSERT INTO Login (Email, Password) VALUES (@Email, @Password)";
                    SqlCommand insertCmd = new SqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Password", hashedPassword);

                    int rowsAffected = insertCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Sign up successful! You can now log in.");

                        // Optional: redirect to login page
                        this.Hide();
                        Form1 loginForm = new Form1(); // Replace with your login form class
                        loginForm.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Sign up failed. Please try again.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        private void Sclosebtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}