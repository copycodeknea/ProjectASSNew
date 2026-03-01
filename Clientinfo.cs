using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class Clientinfo : Form
    {
        private int selectedClientID = 0;

        public Clientinfo()
        {
            InitializeComponent();
            clientdatagridview.CellClick += clientdatagridview_CellClick;

            EditBtn.Enabled = false;
            DeleteBtn.Enabled = false;

            this.Load += Clientinfo_Load;
        }

        private void Clientinfo_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            LoadClients();
        }

        private void ConfigureDataGridView()
        {
            clientdatagridview.Columns.Clear();
            clientdatagridview.AutoGenerateColumns = false;

            clientdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Client_ID",
                DataPropertyName = "Client_ID",
                Visible = false
            });

            clientdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Client_Name",
                HeaderText = "Name",
                DataPropertyName = "Client_Name"
            });

            clientdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone_Number",
                HeaderText = "Phone",
                DataPropertyName = "Phone_Number"
            });

            clientdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Country",
                HeaderText = "Country",
                DataPropertyName = "Country"
            });
        }

        private void LoadClients(string searchQuery = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT Client_ID, Client_Name, Phone_Number, Country FROM Client";

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        if (int.TryParse(searchQuery, out int id))
                            query += " WHERE Client_ID=@ID OR Client_Name LIKE @Name";
                        else
                            query += " WHERE Client_Name LIKE @Name";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchQuery))
                        {
                            cmd.Parameters.AddWithValue("@Name", "%" + searchQuery + "%");
                            if (int.TryParse(searchQuery, out int id))
                                cmd.Parameters.AddWithValue("@ID", id);
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            clientdatagridview.DataSource = dt;
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
            try
            {
                Client client = new Client
                {
                    ClientName = clientnametxt.Text.Trim(),
                    PhoneNumber = clientphonenumbertxt.Text.Trim(),
                    Country = Countrytxt.Text.Trim()
                };

                client.Validate();

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = isNew
                        ? "INSERT INTO Client (Client_Name, Phone_Number, Country) VALUES (@Name, @Phone, @Country)"
                        : "UPDATE Client SET Client_Name=@Name, Phone_Number=@Phone, Country=@Country WHERE Client_ID=@ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", client.ClientName);
                        cmd.Parameters.AddWithValue("@Phone", client.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Country", client.Country);

                        if (!isNew)
                            cmd.Parameters.AddWithValue("@ID", selectedClientID);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(isNew ? "Client added successfully!" : "Client updated successfully!");

                ClearFields();
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            SaveClient(true);
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (selectedClientID == 0)
            {
                MessageBox.Show("Select a client first.");
                return;
            }

            SaveClient(false);
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (selectedClientID == 0)
            {
                MessageBox.Show("Select a client first.");
                return;
            }

            if (MessageBox.Show("Delete this client?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Client WHERE Client_ID=@ID", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", selectedClientID);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Client deleted successfully!");

            ClearFields();
            LoadClients();
        }

        private void SearchBtn_Click(object sender, EventArgs e)
        {
            LoadClients(Searchtxt.Text.Trim());
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            Searchtxt.Clear();
            ClearFields();
            LoadClients();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            Mainpagecs mainpage = new Mainpagecs();
            mainpage.Show();
            this.Close();
        }

        private void clientdatagridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = clientdatagridview.Rows[e.RowIndex];

            selectedClientID = Convert.ToInt32(row.Cells["Client_ID"].Value);
            clientnametxt.Text = row.Cells["Client_Name"].Value.ToString();
            clientphonenumbertxt.Text = row.Cells["Phone_Number"].Value.ToString();
            Countrytxt.Text = row.Cells["Country"].Value.ToString();

            EditBtn.Enabled = true;
            DeleteBtn.Enabled = true;
        }

        private void ClearFields()
        {
            selectedClientID = 0;

            clientnametxt.Clear();
            clientphonenumbertxt.Clear();
            Countrytxt.Clear();

            EditBtn.Enabled = false;
            DeleteBtn.Enabled = false;
        }

        private void Countrytxt_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}