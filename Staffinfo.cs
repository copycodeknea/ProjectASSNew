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

            LoadStaff();
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

                            // Add StaffType column via polymorphism
                            if (!dt.Columns.Contains("StaffType"))
                                dt.Columns.Add("StaffType", typeof(string));

                            foreach (DataRow row in dt.Rows)
                            {
                                HotelStaff staff = new HotelStaff
                                {
                                    StaffName = row["Staff_Name"].ToString(),
                                    PhoneNumber = row["Phone_Number"].ToString(),
                                    Position = row["Position"].ToString(),
                                    Gender = row["Gender"].ToString()
                                };
                                row["StaffType"] = staff.GetStaffType();
                            }

                            staffdatagridview.DataSource = dt;
                            staffdatagridview.AutoGenerateColumns = false;

                            // Configure columns
                            ConfigureColumn("Staff_ID", "ID", false, false);
                            ConfigureColumn("Staff_Name", "Name");
                            ConfigureColumn("Position", "Position");
                            ConfigureColumn("Gender", "Gender");
                            ConfigureColumn("Phone_Number", "Phone");
                            ConfigureColumn("StaffType", "Staff Type", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading staff: " + ex.Message);
            }
        }

        private void ConfigureColumn(string name, string header, bool readOnly = false, bool visible = true)
        {
            if (!staffdatagridview.Columns.Contains(name))
            {
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = header,
                    DataPropertyName = name,
                    ReadOnly = readOnly,
                    Visible = visible
                };
                staffdatagridview.Columns.Add(col);
            }
        }

        private void SaveStaff(bool isNew)
        {
            try
            {
                HotelStaff staff = new HotelStaff
                {
                    StaffID = selectedStaffID,
                    StaffName = staffnametxt.Text.Trim(),
                    PhoneNumber = phonenumbertxt.Text.Trim(),
                    Position = positiontxt.Text.Trim(),
                    Gender = gendercombobox.Text
                };

                // Validation
                staff.Validate();

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = isNew
                        ? @"INSERT INTO Staff (Staff_Name, Phone_Number, Position, Gender)
                            VALUES (@StaffName, @PhoneNumber, @Position, @Gender)"
                        : @"UPDATE Staff 
                            SET Staff_Name=@StaffName, Phone_Number=@PhoneNumber, Position=@Position, Gender=@Gender 
                            WHERE Staff_ID=@StaffID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffName", staff.StaffName);
                        cmd.Parameters.AddWithValue("@PhoneNumber", staff.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Position", staff.Position);
                        cmd.Parameters.AddWithValue("@Gender", staff.Gender);

                        if (!isNew)
                            cmd.Parameters.AddWithValue("@StaffID", staff.StaffID);

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
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Staff WHERE Staff_ID=@StaffID", conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffID", selectedStaffID);
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
                MessageBox.Show("Please enter a StaffID or Name to search.");
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
            Mainpagecs mainpage = new Mainpagecs();
            mainpage.Show();
            this.Close();
        }
    }
}
