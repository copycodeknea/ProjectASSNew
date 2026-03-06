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

            editbtn.Enabled = false;
            deletebtn.Enabled = false;
            this.Load += Room_Load;
        }

        private void Room_Load(object sender, EventArgs e)
        {
            rdyes.Checked = true;

            bedtypecombobox.Items.Clear();
            bedtypecombobox.Items.AddRange(new string[] { "Single", "Double", "Queen", "King" });
            bedtypecombobox.SelectedIndex = 0;

            LoadRooms();
        }

        private void LoadRooms(string searchQuery = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT RoomId, Room_Number, Price_per_Night, floor, Bed_type, Room_Availability FROM Room";

                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        if (!int.TryParse(searchQuery, out _))
                        {
                            MessageBox.Show("Room number must be numeric.");
                            return;
                        }
                        query += " WHERE Room_Number = @SearchRoom";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrWhiteSpace(searchQuery))
                            cmd.Parameters.Add("@SearchRoom", SqlDbType.Int).Value = int.Parse(searchQuery);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataTable displayTable = new DataTable();
                            displayTable.Columns.Add("RoomId", typeof(int));
                            displayTable.Columns.Add("Room Number", typeof(int));
                            displayTable.Columns.Add("Price per Night", typeof(decimal));
                            displayTable.Columns.Add("Floor", typeof(int));
                            displayTable.Columns.Add("Bed Type", typeof(string));
                            displayTable.Columns.Add("Availability", typeof(string));

                            foreach (DataRow row in dt.Rows)
                            {
                                HotelRoom room = new HotelRoom
                                {
                                    RoomId = Convert.ToInt32(row["RoomId"]),
                                    RoomNumber = Convert.ToInt32(row["Room_Number"]),
                                    FloorNumber = Convert.ToInt32(row["floor"]),
                                    RoomAvailability = Convert.ToBoolean(row["Room_Availability"])
                                };

                                displayTable.Rows.Add(
                                    room.RoomId,
                                    room.RoomNumber,
                                    Convert.ToDecimal(row["Price_per_Night"]),
                                    room.FloorNumber,
                                    row["Bed_type"].ToString(),
                                    room.GetAvailabilityStatus()
                                );
                            }

                            // ❗ CLEAR OLD GRID STATE BEFORE BINDING
                            roomdatagridview.DataSource = null;
                            roomdatagridview.Columns.Clear();

                            roomdatagridview.DataSource = displayTable;
                            roomdatagridview.AutoGenerateColumns = true;

                            if (roomdatagridview.Columns["RoomId"] != null)
                                roomdatagridview.Columns["RoomId"].Visible = false;

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
                        ? "INSERT INTO Room (Room_Number, Price_per_Night, floor, Bed_type, Room_Availability) " +
                          "VALUES (@room_number, @price, @floor_number, @bed_type, @roomavailability)"
                        : "UPDATE Room SET Room_Number=@room_number, Price_per_Night=@price, floor=@floor_number, " +
                          "Bed_type=@bed_type, Room_Availability=@roomavailability WHERE RoomId=@room_id";

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

                // ❗ force reload with correct binding
                roomdatagridview.DataSource = null;
                roomdatagridview.Columns.Clear();
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
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Room WHERE Room_Number = @room_number", conn))
                    {
                        cmd.Parameters.AddWithValue("@room_number", roomNumber);
                        return (int)cmd.ExecuteScalar() > 0;
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
            selectedRoomId = Convert.ToInt32(row.Cells["RoomId"].Value);
            roomnumbertxt.Text = row.Cells["Room Number"].Value.ToString();
            pricetxt.Text = row.Cells["Price per Night"].Value.ToString();
            floornumbertxt.Text = row.Cells["Floor"].Value.ToString();

            string bedType = row.Cells["Bed Type"].Value.ToString();
            bedtypecombobox.Text = bedType;

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
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM Room WHERE RoomId = @room_id", conn))
                    {
                        cmd.Parameters.AddWithValue("@room_id", selectedRoomId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Room deleted successfully!");
                ClearFields();

                roomdatagridview.DataSource = null;
                roomdatagridview.Columns.Clear();
                LoadRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting room: " + ex.Message);
            }
        }

        private void searchbtn_Click(object sender, EventArgs e) => LoadRooms(searchtxt.Text.Trim());

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            searchtxt.Clear();
            ClearFields();
            roomdatagridview.DataSource = null;
            roomdatagridview.Columns.Clear();
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
            this.Close();
        }
    }
}
