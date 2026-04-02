using System;
using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Net.Mail;

namespace ProjectASS
{
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();

            // Ensure the click event is wired (designer may not have wired it)
            try
            {
                this.Signupbtn.Click -= SignUpbtn_Click;
            }
            catch { }
            this.Signupbtn.Click += SignUpbtn_Click;
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

            // ✅ Validation
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both email and password.");
                return;
            }

            // Validate email format
            try
            {
                var _ = new MailAddress(email);
            }
            catch
            {
                MessageBox.Show("Please enter a valid email address.");
                Semailtxt.Focus();
                return;
            }

            string hashedPassword = HashPassword(password);

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // ✅ CHECK if email already exists
                    string checkQuery = "SELECT COUNT(*) FROM Signup WHERE Email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists > 0)
                        {
                            // ✅ User already has an account → ask to go to login
                            var answer = MessageBox.Show(
                                "This email is already registered.\nWould you like to go to the Login page?",
                                "Account Exists",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);

                            if (answer == DialogResult.Yes)
                            {
                                this.DialogResult = DialogResult.Retry; // signal: go to login
                                this.Close();
                            }
                            return;
                        }
                    }

                    // ✅ INSERT into dbo.Signup
                    string insertSignup = "INSERT INTO Signup (Email, Password) VALUES (@Email, @Password)";
                    using (SqlCommand signupCmd = new SqlCommand(insertSignup, conn))
                    {
                        signupCmd.Parameters.AddWithValue("@Email", email);
                        signupCmd.Parameters.AddWithValue("@Password", hashedPassword);
                        signupCmd.ExecuteNonQuery();
                    }

                    // ✅ INSERT into dbo.Login
                    string insertLogin = "INSERT INTO Login (Email, Password) VALUES (@Email, @Password)";
                    using (SqlCommand loginCmd = new SqlCommand(insertLogin, conn))
                    {
                        loginCmd.Parameters.AddWithValue("@Email", email);
                        loginCmd.Parameters.AddWithValue("@Password", hashedPassword);
                        loginCmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Sign up successful! You can now log in.");

                    // Signal success to caller (Program.cs) and close
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message);
            }
        }

        private void Sclosebtn_Click(object sender, EventArgs e)
        {
            // treat close as cancel when shown modally
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}