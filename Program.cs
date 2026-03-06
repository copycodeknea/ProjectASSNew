using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            // Show login dialog first. If login succeeds, start main menu as application root.
            using (var login = new Form1())
            {
                var dr = login.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Application.Run(new Mainpagecs());
                }
            }
        }
    }
}
