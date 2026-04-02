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

        // 🔐 Password Hash Method (must match SignupForm.HashPassword)
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

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // Check Login table first, then Signup table as fallback
                    string storedPassword = null;

                    // Try dbo.Login
                    using (SqlCommand cmd = new SqlCommand("SELECT Password FROM Login WHERE Email = @Email", conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                            storedPassword = result.ToString();
                    }

                    // Fallback: try dbo.Signup if not found in Login
                    if (storedPassword == null)
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT Password FROM Signup WHERE Email = @Email", conn))
                        {
                            cmd.Parameters.AddWithValue("@Email", email);
                            object result = cmd.ExecuteScalar();
                            if (result != null && result != DBNull.Value)
                                storedPassword = result.ToString();
                        }
                    }

                    if (storedPassword == null)
                    {
                        // ✅ User has no account → ask to go to signup
                        var answer = MessageBox.Show(
                            "This email is not registered.\nWould you like to go to the Sign Up page?",
                            "No Account Found",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                        if (answer == DialogResult.Yes)
                        {
                            this.DialogResult = DialogResult.Retry; // signal: go to signup
                            this.Close();
                        }
                        return;
                    }

                    // Compare hashed password
                    if (string.Equals(storedPassword, hashedPassword, StringComparison.OrdinalIgnoreCase))
                    {
                        // ✅ Login success → go to main menu
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid password. Please try again.");
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