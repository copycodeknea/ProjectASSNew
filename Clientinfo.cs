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
    public partial class Clientinfo : Form
    {
        DataBase db = new DataBase();
        public Clientinfo()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client c = new client()
            {
                UserID = textBox1.Text,
                UserName = textBox2.Text,
                Phone = textBox3.Text,
                Country = cboCountry.Text
            };

            db.InsertClient(c);
            LoadData();
            ClearData();
            MessageBox.Show("Added Successfully");
        }

        private void Clientinfo_Load(object sender, EventArgs e)
        {
            LoadCountry();
            LoadData();
        }
        void LoadCountry()
        {
            cboCountry.Items.Add("Cambodia");
            cboCountry.Items.Add("Laos");
            cboCountry.Items.Add("Vietnam");
            cboCountry.Items.Add("China");
            cboCountry.SelectedIndex = 0;
        }

        void LoadData()
        {
            dgvClient.DataSource = db.GetClients();
        }

        void ClearData()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            cboCountry.SelectedIndex = 0;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            dgvClient.DataSource = db.SearchClient(txtSearch.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client c = new client() {
                UserID = textBox1.Text,
                UserName = textBox2.Text,
                Phone = textBox3.Text,
                Country = cboCountry.Text
            };

            db.UpdateClient(c);
            LoadData();
            MessageBox.Show("Updated Successfully");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            db.DeleteClient(textBox1.Text);
            LoadData();
            ClearData();
            MessageBox.Show("Deleted Successfully");
        }

        private void dgvClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvClient.Rows[e.RowIndex];

                textBox1.Text = row.Cells["UserID"].Value.ToString();
                textBox2.Text = row.Cells["UserName"].Value.ToString();
                textBox3.Text = row.Cells["Phone"].Value.ToString();
                cboCountry.Text = row.Cells["Country"].Value.ToString();
            }
        }
    }
}
