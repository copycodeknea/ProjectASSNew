using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class Room : Form
    {
        private int selectedRoomId = 0;

        public Room()
        {
            InitializeComponent();
            roomdatagridview.CellClick += roomdatagridview_CellClick;

            // Disable Edit/Delete buttons initially
            editbtn.Enabled = false;
            deletebtn.Enabled = false;

            this.Load += Room_Load;
        }

        private void Room_Load(object sender, EventArgs e)
        {
            rdyes.Checked = true;

            // Initialize Bed Type combobox
            bedtypecombobox.Items.Clear();
            bedtypecombobox.Items.AddRange(new string[] { "Single", "Double", "Queen", "King" });
            bedtypecombobox.SelectedIndex = 0;

            try
            {
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message);
            }
        }

        private void LoadRooms(string searchQuery = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT room_id, room_number, price_per_night, floor_number, bed_type, roomavailability FROM Room";

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        if (int.TryParse(searchQuery, out int roomNum))
                        {
                            query += " WHERE room_number = @SearchRoom";
                        }
                        else
                        {
                            MessageBox.Show("Room number must be numeric.");
                            return;
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchQuery))
                            cmd.Parameters.Add("@SearchRoom", SqlDbType.Int).Value = int.Parse(searchQuery);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // Prepare display table
                            DataTable displayTable = new DataTable();
                            displayTable.Columns.Add("room_id", typeof(int));
                            displayTable.Columns.Add("Room Number", typeof(int));
                            displayTable.Columns.Add("Price per Night", typeof(decimal));
                            displayTable.Columns.Add("Floor Number", typeof(int));
                            displayTable.Columns.Add("Bed Type", typeof(string));
                            displayTable.Columns.Add("Availability", typeof(string));

                            foreach (DataRow row in dt.Rows)
                            {
                                // Create HotelRoom object for polymorphism
                                HotelRoom room = new HotelRoom
                                {
                                    RoomId = Convert.ToInt32(row["room_id"]),
                                    RoomNumber = Convert.ToInt32(row["room_number"]),
                                    FloorNumber = Convert.ToInt32(row["floor_number"]),
                                    RoomAvailability = Convert.ToBoolean(row["roomavailability"])
                                };

                                string bedType = row["bed_type"].ToString();
                                // Fallback if bed type is invalid
                                if (!bedtypecombobox.Items.Contains(bedType))
                                    bedType = "Single";

                                displayTable.Rows.Add(
                                    room.RoomId,
                                    room.RoomNumber,
                                    Convert.ToDecimal(row["price_per_night"]),
                                    room.FloorNumber,
                                    bedType,
                                    room.GetAvailabilityStatus()  // Polymorphism used
                                );
                            }

                            roomdatagridview.DataSource = displayTable;
                            roomdatagridview.AutoGenerateColumns = true;

                            // Hide internal ID column
                            if (roomdatagridview.Columns["room_id"] != null)
                                roomdatagridview.Columns["room_id"].Visible = false;

                            // Format Price as currency
                            if (roomdatagridview.Columns["Price per Night"] != null)
                                roomdatagridview.Columns["Price per Night"].DefaultCellStyle.Format = "C2";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message);
                roomdatagridview.DataSource = null;
            }
        }

        private void SaveRoom(bool isNew)
        {
            // Validate numeric inputs
            if (!int.TryParse(roomnumbertxt.Text, out int roomNumber) || roomNumber <= 0 ||
                !int.TryParse(floornumbertxt.Text, out int floorNumber) || floorNumber <= 0 ||
                !decimal.TryParse(pricetxt.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Enter valid numeric values for Room Number, Floor, and Price.");
                return;
            }

            if (isNew && RoomNumberExists(roomNumber))
            {
                MessageBox.Show("Room number already exists.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = isNew
                        ? "INSERT INTO Room (room_number, price_per_night, floor_number, bed_type, roomavailability) " +
                          "VALUES (@room_number, @price, @floor_number, @bed_type, @roomavailability)"
                        : "UPDATE Room SET room_number=@room_number, price_per_night=@price, floor_number=@floor_number, " +
                          "bed_type=@bed_type, roomavailability=@roomavailability WHERE room_id=@room_id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@room_number", roomNumber);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@floor_number", floorNumber);
                        cmd.Parameters.AddWithValue("@bed_type", bedtypecombobox.Text);
                        cmd.Parameters.AddWithValue("@roomavailability", rdyes.Checked);

                        if (!isNew)
                            cmd.Parameters.AddWithValue("@room_id", selectedRoomId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(isNew ? "Room added successfully!" : "Room updated successfully!");
                ClearFields();
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving room: " + ex.Message);
            }
        }

        private bool RoomNumberExists(int roomNumber)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Room WHERE room_number = @room_number", conn))
                    {
                        cmd.Parameters.AddWithValue("@room_number", roomNumber);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        private void btnadd_Click(object sender, EventArgs e) => SaveRoom(true);

        private void editbtn_Click(object sender, EventArgs e)
        {
            if (selectedRoomId == 0)
            {
                MessageBox.Show("Select a room to edit.");
                return;
            }
            SaveRoom(false);
        }

        private void roomdatagridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = roomdatagridview.Rows[e.RowIndex];
            selectedRoomId = Convert.ToInt32(row.Cells["room_id"].Value);
            roomnumbertxt.Text = row.Cells["Room Number"].Value.ToString();
            pricetxt.Text = row.Cells["Price per Night"].Value.ToString();
            floornumbertxt.Text = row.Cells["Floor Number"].Value.ToString();

            string bedType = row.Cells["Bed Type"].Value.ToString();
            if (bedtypecombobox.Items.Contains(bedType))
                bedtypecombobox.Text = bedType;
            else
                bedtypecombobox.SelectedIndex = 0;

            rdyes.Checked = row.Cells["Availability"].Value.ToString() == "Available";
            rdno.Checked = !rdyes.Checked;

            editbtn.Enabled = true;
            deletebtn.Enabled = true;
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (selectedRoomId == 0)
            {
                MessageBox.Show("Select a room first.");
                return;
            }

            if (MessageBox.Show("Delete this room?", "Confirm Delete", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Room WHERE room_id = @room_id", conn))
                    {
                        cmd.Parameters.AddWithValue("@room_id", selectedRoomId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Room deleted successfully!");
                ClearFields();
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting room: " + ex.Message);
            }
        }

        private void searchbtn_Click(object sender, EventArgs e)
        {
            LoadRooms(searchtxt.Text.Trim());
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            searchtxt.Clear();
            ClearFields();
            LoadRooms();
        }

        private void ClearFields()
        {
            selectedRoomId = 0;
            roomnumbertxt.Clear();
            pricetxt.Clear();
            floornumbertxt.Clear();
            bedtypecombobox.SelectedIndex = 0;
            rdyes.Checked = true;
            rdno.Checked = false;

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
