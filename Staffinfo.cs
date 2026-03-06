using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class Staffinfo : Form
    {
        private int selectedStaffID = 0;

        public Staffinfo()
        {
            InitializeComponent();
            staffdatagridview.CellClick += staffdatagridview_CellClick;

            editbtn.Enabled = false;
            deletebtn.Enabled = false;

            this.Load += Staffinfo_Load;
        }

        private void Staffinfo_Load(object sender, EventArgs e)
        {
            // Initialize gender combo box
            gendercombobox.Items.Clear();
            gendercombobox.Items.AddRange(new string[] { "Male", "Female", "Other" });
            gendercombobox.SelectedIndex = 0;

            ConfigureDataGridView();
            LoadStaff();
        }

        private void ConfigureDataGridView()
        {
            staffdatagridview.Columns.Clear();
            staffdatagridview.AutoGenerateColumns = false;

            staffdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Staff_ID",
                HeaderText = "ID",
                DataPropertyName = "Staff_ID",
                Visible = false
            });
            staffdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Staff_Name",
                HeaderText = "Name",
                DataPropertyName = "Staff_Name"
            });
            staffdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Position",
                HeaderText = "Position",
                DataPropertyName = "Position"
            });
            staffdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Gender",
                HeaderText = "Gender",
                DataPropertyName = "Gender"
            });
            staffdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Phone_Number",
                HeaderText = "Phone",
                DataPropertyName = "Phone_Number"
            });
        }

        private void LoadStaff(string searchQuery = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT Staff_ID, Staff_Name, Position, Gender, Phone_Number FROM Staff";

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        if (int.TryParse(searchQuery, out int staffId))
                            query += " WHERE Staff_ID = @SearchID OR Staff_Name LIKE @SearchName";
                        else
                            query += " WHERE Staff_Name LIKE @SearchName";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchQuery))
                        {
                            cmd.Parameters.AddWithValue("@SearchName", "%" + searchQuery + "%");
                            if (int.TryParse(searchQuery, out int staffId))
                                cmd.Parameters.AddWithValue("@SearchID", staffId);
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            staffdatagridview.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading staff: " + ex.Message);
                staffdatagridview.DataSource = null;
            }
        }

        private void SaveStaff(bool isNew)
        {
            try
            {
                string name = staffnametxt.Text.Trim();
                string phone = phonenumbertxt.Text.Trim();
                string position = positiontxt.Text.Trim();
                string gender = gendercombobox.Text;

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(position))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = isNew
                        ? "INSERT INTO Staff (Staff_Name, Phone_Number, Position, Gender) VALUES (@Name, @Phone, @Position, @Gender)"
                        : "UPDATE Staff SET Staff_Name=@Name, Phone_Number=@Phone, Position=@Position, Gender=@Gender WHERE Staff_ID=@ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@Position", position);
                        cmd.Parameters.AddWithValue("@Gender", gender);

                        if (!isNew)
                            cmd.Parameters.AddWithValue("@ID", selectedStaffID);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(isNew ? "Staff added successfully!" : "Staff updated successfully!");
                ClearFields();
                LoadStaff();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving staff: " + ex.Message);
            }
        }

        private void addbtn_Click(object sender, EventArgs e) => SaveStaff(true);

        private void editbtn_Click(object sender, EventArgs e)
        {
            if (selectedStaffID == 0)
            {
                MessageBox.Show("Select a staff to edit.");
                return;
            }
            SaveStaff(false);
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (selectedStaffID == 0)
            {
                MessageBox.Show("Select a staff first.");
                return;
            }

            if (MessageBox.Show("Delete this staff?", "Confirm Delete", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Staff WHERE Staff_ID=@ID", conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", selectedStaffID);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Staff deleted successfully!");
                ClearFields();
                LoadStaff();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting staff: " + ex.Message);
            }
        }

        private void searchbtn_Click(object sender, EventArgs e)
        {
            string searchText = searchtxt.Text.Trim();
            if (string.IsNullOrWhiteSpace(searchText))
            {
                LoadStaff();
                return;
            }
            LoadStaff(searchText);
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            searchtxt.Clear();
            ClearFields();
            LoadStaff();
        }

        private void staffdatagridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = staffdatagridview.Rows[e.RowIndex];
            selectedStaffID = Convert.ToInt32(row.Cells["Staff_ID"].Value);
            staffnametxt.Text = row.Cells["Staff_Name"].Value.ToString();
            phonenumbertxt.Text = row.Cells["Phone_Number"].Value.ToString();
            positiontxt.Text = row.Cells["Position"].Value.ToString();
            gendercombobox.Text = row.Cells["Gender"].Value.ToString();

            editbtn.Enabled = true;
            deletebtn.Enabled = true;
        }

        private void ClearFields()
        {
            selectedStaffID = 0;
            staffnametxt.Clear();
            phonenumbertxt.Clear();
            positiontxt.Clear();
            gendercombobox.SelectedIndex = 0;

            editbtn.Enabled = false;
            deletebtn.Enabled = false;
        }

        private void backbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
