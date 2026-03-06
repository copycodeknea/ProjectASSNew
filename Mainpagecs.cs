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

        private void Clientbtn_Click(object sender, EventArgs e)
        {
            Clientinfo clientInfoForm = new Clientinfo();
            clientInfoForm.ShowDialog();
        }

        private void Staffbtn_Click(object sender, EventArgs e)
        {
            Staffinfo staffInfoForm = new Staffinfo();
            staffInfoForm.ShowDialog();
        }

        private void Roombtn_Click(object sender, EventArgs e)
        {
            Room Roomform = new Room();
            Roomform.ShowDialog();
        }

        private void Reservationbtn_Click(object sender, EventArgs e)
        {
            Reservation Reservationform = new Reservation();
            Reservationform.ShowDialog();
        }

        private void Closebtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
