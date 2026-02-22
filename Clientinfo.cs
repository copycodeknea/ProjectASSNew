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

namespace ProjectASS
{
    public partial class Clientinfo : Form
    {
        private int selectedClientId = 0;
        public Clientinfo()
        {
            InitializeComponent();

            clientdatagridview.CellClick += clientdatagridview_CellClick;

            editbtn.Enabled = false;
            deletebtn.Enabled = false;

            this.Load += Clientinfo_Load;
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            SaveClient(true);
        }

        private void Clientinfo_Load(object sender, EventArgs e)
        {

            LoadClients();

        }
        private void LoadClients(string searchQuery = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = @"SELECT ClientId, Client_Name, Phone_Number, Country 
                                     FROM Client";

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        query += " WHERE Client_Name LIKE @SearchName";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchQuery))
                            cmd.Parameters.AddWithValue("@SearchName", "%" + searchQuery + "%");

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataTable displayTable = new DataTable();
                            displayTable.Columns.Add("ClientId", typeof(int));
                            displayTable.Columns.Add("Client Name", typeof(string));
                            displayTable.Columns.Add("Phone Number", typeof(string));
                            displayTable.Columns.Add("Country", typeof(string));
                            displayTable.Columns.Add("Info", typeof(string));

                            foreach (DataRow row in dt.Rows)
                            {
                                Client client = new Client
                                {
                                    ClientId = Convert.ToInt32(row["ClientId"]),
                                    ClientName = row["Client_Name"].ToString(),
                                    PhoneNumber = row["Phone_Number"].ToString(),
                                    Country = row["Country"].ToString()
                                };

                                displayTable.Rows.Add(
                                    client.ClientId,
                                    client.ClientName,
                                    client.PhoneNumber,
                                    client.Country,
                                    client.GetClientInfo()
                                );
                            }

                            clientdatagridview_CellClick.DataSource = null;
                            clientdatagridview_CellClick.Columns.Clear();

                            clientdatagridview_CellClick.DataSource = displayTable;

                            if (clientdatagridview_CellClick.Columns["ClientId"] != null)
                                clientdatagridview_CellClick.Columns["ClientId"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading clients: " + ex.Message);
            }
        }
        private void SaveClient(bool isNew)
        {
            if (string.IsNullOrWhiteSpace(clientnametxt.Text))
            {
                MessageBox.Show("Client name is required.");
                return;
            }

            if (!long.TryParse(phonetxt.Text, out _))
            {
                MessageBox.Show("Phone number must be numeric.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = isNew
                        ? @"INSERT INTO Client (Client_Name, Phone_Number, Country)
                           VALUES (@name, @phone, @country)"
                        : @"UPDATE Client SET 
                           Client_Name=@name,
                           Phone_Number=@phone,
                           Country=@country
                           WHERE ClientId=@id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", clientnametxt.Text);
                        cmd.Parameters.AddWithValue("@phone", phonetxt.Text);
                        cmd.Parameters.AddWithValue("@country", countrycombobox.Text);

                        if (!isNew)
                            cmd.Parameters.AddWithValue("@id", selectedClientId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(isNew ? "Client added successfully!" : "Client updated successfully!");

                
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving client: " + ex.Message);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            LoadClients(searchtxt.Text.Trim());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedClientId == 0)
            {
                MessageBox.Show("Select a client to edit.");
                return;
            }

            SaveClient(false);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (selectedClientId == 0)
            {
                MessageBox.Show("Select a client first.");
                return;
            }

            if (MessageBox.Show("Delete this client?", "Confirm Delete",
                MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Client WHERE ClientId=@id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedClientId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Client deleted successfully!");
                
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting client: " + ex.Message);
            }
        }

        private void dgvClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = clientdatagridview_CellClick.Rows[e.RowIndex];

            selectedClientId = Convert.ToInt32(row.Cells["ClientId"].Value);
            clientnametxt.Text = row.Cells["Client Name"].Value.ToString();
            phonetxt.Text = row.Cells["Phone Number"].Value.ToString();
            countrycombobox.Text = row.Cells["Country"].Value.ToString();

            editbtn.Enabled = true;
            deletebtn.Enabled = true;
        }

        private void ClearFields_Click(object sender, EventArgs e)
        {
            selectedClientId = 0;
            clientnametxt.Clear();
            phonetxt.Clear();
            countrycombobox.SelectedIndex = -1;

            editbtn.Enabled = false;
            deletebtn.Enabled = false;
        }
    }
}
