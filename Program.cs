using System;
using System.Windows.Forms;

namespace ProjectASS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start on Signup page
            bool showSignup = true;

            while (true)
            {
                if (showSignup)
                {
                    // ✅ STEP 1: Show Signup
                    using (var signup = new SignupForm())
                    {
                        var result = signup.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            // Signup succeeded → go to login
                            showSignup = false;
                            continue;
                        }
                        else if (result == DialogResult.Retry)
                        {
                            // User already has account → go to login
                            showSignup = false;
                            continue;
                        }
                        else
                        {
                            // User cancelled → exit app
                            return;
                        }
                    }
                }
                else
                {
                    // ✅ STEP 2: Show Login
                    using (var login = new Form1())
                    {
                        var result = login.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            // Login succeeded → go to main menu
                            break;
                        }
                        else if (result == DialogResult.Retry)
                        {
                            // User has no account → go back to signup
                            showSignup = true;
                            continue;
                        }
                        else
                        {
                            // User cancelled → exit app
                            return;
                        }
                    }
                }
            }

            // ✅ STEP 3: Login succeeded — show main menu page
            Application.Run(new Mainpagecs());
        }
    }
}