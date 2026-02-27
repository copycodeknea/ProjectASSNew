using System;
using System.Data;
using System.Data.SqlClient;
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

        // ─── LOAD ────────────────────────────────────────────────────
        private void Clientinfo_Load(object sender, EventArgs e)
        {
            countrycombobox.Items.Clear();
            countrycombobox.Items.AddRange(new string[]
            {
                "Cambodia", "Laos", "Vietnam", "China"
            });
            countrycombobox.SelectedIndex = 0;

            try
            {
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading clients: " + ex.Message);
            }
        }

        // ─── LOAD / SEARCH ───────────────────────────────────────────
        private void LoadClients(string searchQuery = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    // Ensure Client table exists before querying
                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Client'", conn))
                    {
                        int tblCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (tblCount == 0)
                        {
                            var result = MessageBox.Show("Table 'Client' does not exist in the database. Create it now?",
                                "Create Table", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            if (result == DialogResult.Yes)
                            {
                                string createSql = @"CREATE TABLE dbo.Client (
                                    ClientId INT IDENTITY(1,1) PRIMARY KEY,
                                    Client_Name NVARCHAR(200) NOT NULL,
                                    Phone_Number NVARCHAR(50) NULL,
                                    Country NVARCHAR(100) NULL
                                );";
                                using (SqlCommand createCmd = new SqlCommand(createSql, conn))
                                {
                                    createCmd.ExecuteNonQuery();
                                }

                                MessageBox.Show("Table 'Client' created. Reloading clients...");
                            }
                            else
                            {
                                // User declined to create table; abort loading
                                return;
                            }
                        }
                    }

                    string query = "SELECT ClientId, Client_Name, Phone_Number, Country FROM Client";

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                        query += " WHERE Client_Name LIKE @SearchName";

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
                            displayTable.Columns.Add("Status", typeof(string));

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
                                    client.GetClientStatus()  // Polymorphism used
                                );
                            }

                            clientdatagridview.DataSource = null;
                            clientdatagridview.Columns.Clear();
                            clientdatagridview.DataSource = displayTable;
                            clientdatagridview.AutoGenerateColumns = true; if (clientdatagridview.Columns["ClientId"] != null)
                                clientdatagridview.Columns["ClientId"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading clients: " + ex.Message);
            }
        }

        // ─── SAVE (ADD or EDIT) ──────────────────────────────────────
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
                        ? "git INTO Client (Client_Name, Phone_Number, Country) VALUES (@name, @phone, @country)"
                        : "UPDATE Client SET Client_Name=@name, Phone_Number=@phone, Country=@country WHERE ClientId=@id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", clientnametxt.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", phonetxt.Text.Trim());
                        cmd.Parameters.AddWithValue("@country", countrycombobox.Text);

                        if (!isNew)
                            cmd.Parameters.AddWithValue("@id", selectedClientId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(isNew ? "Client added successfully!" : "Client updated successfully!");

                ClearAllFields();
                clientdatagridview.DataSource = null;
                clientdatagridview.Columns.Clear();
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving client: " + ex.Message);
            }
        }

        // ─── ADD ─────────────────────────────────────────────────────
        private void addbtn_Click(object sender, EventArgs e)
        {
            SaveClient(true);
        }

        // ─── EDIT ────────────────────────────────────────────────────
        private void editbtn_Click(object sender, EventArgs e)
        {
            if (selectedClientId == 0)
            {
                MessageBox.Show("Please select a client to edit.");
                return;
            }

            SaveClient(false);
        }

        // ─── DELETE ──────────────────────────────────────────────────
        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (selectedClientId == 0)
            {
                MessageBox.Show("Please select a client to delete.");
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this client?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                != DialogResult.Yes) return;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Client WHERE ClientId = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", selectedClientId);
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Client deleted successfully!");
                ClearAllFields();
                clientdatagridview.DataSource = null;
                clientdatagridview.Columns.Clear();
                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting client: " + ex.Message);
            }
        }

        // ─── SEARCH ──────────────────────────────────────────────────
        private void searchbtn_Click(object sender, EventArgs e) => LoadClients(searchtxt.Text.Trim());

        // ─── REFRESH ─────────────────────────────────────────────────
        private void refreshbtn_Click(object sender, EventArgs e)
        {
            searchtxt.Clear();
            ClearAllFields();
            clientdatagridview.DataSource = null;
            clientdatagridview.Columns.Clear();
            LoadClients();
        }

        // ─── GRID CLICK ──────────────────────────────────────────────
        private void clientdatagridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = clientdatagridview.Rows[e.RowIndex];

            selectedClientId = Convert.ToInt32(row.Cells["ClientId"].Value);
            clientnametxt.Text = row.Cells["Client Name"].Value.ToString();
            phonetxt.Text = row.Cells["Phone Number"].Value.ToString();
            countrycombobox.Text = row.Cells["Country"].Value.ToString();

            editbtn.Enabled = true;
            deletebtn.Enabled = true;
        }

        // ─── CLEAR ───────────────────────────────────────────────────
        private void clearbtn_Click(object sender, EventArgs e) => ClearAllFields();

        private void ClearAllFields()
        {
            selectedClientId = 0;
            clientnametxt.Clear();
            phonetxt.Clear();
            countrycombobox.SelectedIndex = 0;

            editbtn.Enabled = false;
            deletebtn.Enabled = false;
        }
    }
}