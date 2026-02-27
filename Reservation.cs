using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class Reservation : Form
    {
        private int selectedReservationId = 0;

        public Reservation()
        {
            InitializeComponent();
            reservationdatagridview.CellClick += reservationdatagridview_CellClick;
            this.Load += Reservation_Load;

            editbtn.Enabled = false;
            deletebtn.Enabled = false;
        }

        private void Reservation_Load(object sender, EventArgs e)
        {
            LoadReservations();
        }

        private void LoadReservations(int? roomIdSearch = null)
        {
            try
            {
                using (SqlConnection conn =
                    new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM Reservation";

                    if (roomIdSearch.HasValue)
                        query += " WHERE RoomId = @roomId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (roomIdSearch.HasValue)
                            cmd.Parameters.AddWithValue("@roomId", roomIdSearch.Value);

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataTable display = new DataTable();
                            display.Columns.Add("ReservationId", typeof(int));
                            display.Columns.Add("Client ID", typeof(string));
                            display.Columns.Add("Name", typeof(string));
                            display.Columns.Add("Room ID", typeof(string));
                            display.Columns.Add("Check In", typeof(string));
                            display.Columns.Add("Check Out", typeof(string));
                            display.Columns.Add("Total Days", typeof(int));

                            foreach (DataRow row in dt.Rows)
                            {
                                int reservationId = Convert.ToInt32(row["ReservationId"]);
                                string clientIdStr = row["ClientId"]?.ToString();
                                string clientName = row["UserName"]?.ToString();
                                string roomIdStr = row["RoomId"]?.ToString();

                                DateTime checkIn = Convert.ToDateTime(row["CheckIn"]);
                                DateTime checkOut = Convert.ToDateTime(row["CheckOut"]);

                                int totalDays = (checkOut - checkIn).Days;

                                display.Rows.Add(
                                    reservationId,
                                    clientIdStr,
                                    clientName,
                                    roomIdStr,
                                    checkIn.ToShortDateString(),
                                    checkOut.ToShortDateString(),
                                    totalDays
                                );
                            }

                            reservationdatagridview.DataSource = display;

                            if (reservationdatagridview.Columns["ReservationId"] != null)
                                reservationdatagridview.Columns["ReservationId"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading reservations: " + ex.Message);
            }
        }

        // ================================
        // SAVE (ADD / UPDATE)
        // ================================
        private void SaveReservation(bool isNew)
        {
            if (!int.TryParse(clientidtxt.Text, out int clientId))
            {
                MessageBox.Show("Client ID must be numeric.");
                return;
            }

            if (string.IsNullOrWhiteSpace(usernametxt.Text))
            {
                MessageBox.Show("User Name is required.");
                return;
            }

            if (!int.TryParse(roomidtxt.Text, out int roomId))
            {
                MessageBox.Show("Room ID must be numeric.");
                return;
            }

            if (checkoutdate.Value <= checkindate.Value)
            {
                MessageBox.Show("Check-out must be after check-in.");
                return;
            }

            try
            {
                using (SqlConnection conn =
                    new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = isNew
                        ? @"INSERT INTO Reservation
                           (ClientId, UserName, RoomId, CheckIn, CheckOut)
                           VALUES (@clientId, @userName, @roomId, @checkIn, @checkOut)"
                        : @"UPDATE Reservation SET
                           ClientId = @clientId,
                           UserName = @userName,
                           RoomId = @roomId,
                           CheckIn = @checkIn,
                           CheckOut = @checkOut
                           WHERE ReservationId = @id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientId", clientId);
                        cmd.Parameters.AddWithValue("@userName", usernametxt.Text.Trim());
                        cmd.Parameters.AddWithValue("@roomId", roomId);
                        cmd.Parameters.AddWithValue("@checkIn", checkindate.Value.Date);
                        cmd.Parameters.AddWithValue("@checkOut", checkoutdate.Value.Date);

                        if (!isNew)
                            cmd.Parameters.AddWithValue("@id", selectedReservationId);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(isNew ? "Reservation added!"
                                      : "Reservation updated!");

                ClearFields();
                LoadReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Error: " + ex.Message);
            }
        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            SaveReservation(true);
        }

        private void editbtn_Click(object sender, EventArgs e)
        {
            if (selectedReservationId == 0)
            {
                MessageBox.Show("Select a reservation first.");
                return;
            }

            SaveReservation(false);
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            if (selectedReservationId == 0)
            {
                MessageBox.Show("Select a reservation first.");
                return;
            }

            if (MessageBox.Show("Delete this reservation?",
                "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (SqlConnection conn =
                new SqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd =
                    new SqlCommand("DELETE FROM Reservation WHERE ReservationId=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", selectedReservationId);
                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Deleted successfully!");
            ClearFields();
            LoadReservations();
        }

        private void reservationdatagridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row =
                reservationdatagridview.Rows[e.RowIndex];

            if (int.TryParse(row.Cells["ReservationId"].Value?.ToString(), out int rid))
                selectedReservationId = rid;

            clientidtxt.Text = row.Cells["Client ID"].Value?.ToString();
            usernametxt.Text = row.Cells["Name"].Value?.ToString();
            roomidtxt.Text = row.Cells["Room ID"].Value?.ToString();

            DateTime.TryParse(row.Cells["Check In"].Value?.ToString(), out DateTime ci);
            DateTime.TryParse(row.Cells["Check Out"].Value?.ToString(), out DateTime co);

            checkindate.Value = ci;
            checkoutdate.Value = co;

            editbtn.Enabled = true;
            deletebtn.Enabled = true;
        }

        private void ClearFields()
        {
            selectedReservationId = 0;
            clientidtxt.Clear();
            usernametxt.Clear();
            roomidtxt.Clear();
            checkindate.Value = DateTime.Today;
            checkoutdate.Value = DateTime.Today.AddDays(1);

            editbtn.Enabled = false;
            deletebtn.Enabled = false;
        }
    }
}