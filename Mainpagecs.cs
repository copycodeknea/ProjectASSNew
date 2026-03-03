using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class Mainpagecs : Form
    {
        public Mainpagecs()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clientinfo clientinfo = new Clientinfo();
            clientinfo.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Staffinfo staffinfo = new Staffinfo();  
            staffinfo.Show();
            this.Hide();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Room room = new Room();
            room.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reservation resevation =new Reservation();
            resevation.Show();
            this.Hide();
        }
    }
}
