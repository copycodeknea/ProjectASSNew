using ProjectASS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;



namespace ProjectASS
{
    public partial class Reservation : Form
    {
        DataBase db = new DataBase();
        public Reservation()
        {
            InitializeComponent();
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            if (search.Text.Trim() == "")
            {
                LoadData();
            }
            else
            {
                dgvReservation.DataSource = db.SearchReservation(search.Text);
            }

        }

        private void Reservation_Load(object sender, EventArgs e)
        {
            LoadRoomType();
            LoadData();
        }
        // LOAD ROOM TYPE
        void LoadRoomType()
        {
            cboRoom.Items.Clear();

            cboRoom.Items.Add("Single ");
            cboRoom.Items.Add("Double ");
            cboRoom.Items.Add("Queen");
            cboRoom.Items.Add("King");

            cboRoom.SelectedIndex = 0;
        }

        // LOAD DATA FROM DATABASE
        void LoadData()
        {
            dgvReservation.DataSource = db.GetReservation();
        }

        private void cboRoom_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvReservation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvReservation.Rows[e.RowIndex];

                textBox1.Text = row.Cells["UserID"].Value.ToString();
                textBox2.Text = row.Cells["UserName"].Value.ToString();
                textBox3.Text = row.Cells["Phone"].Value.ToString();
                cboRoom.Text = row.Cells["RoomType"].Value.ToString();
                CheckIn.Value = Convert.ToDateTime(row.Cells["CheckIn"].Value);
                CheckOut.Value = Convert.ToDateTime(row.Cells["CheckOut"].Value);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
