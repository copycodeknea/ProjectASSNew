using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class Reservation : Form
    {
        private int selectedReservationID = 0;

        public Reservation()
        {
            InitializeComponent();
            reservationdatagridview.CellClick += reservationdatagridview_CellClick;

            EditBtn.Enabled = false;
            DeleteBtn.Enabled = false;

            this.Load += Reservation_Load;
        }

        private void Reservation_Load(object sender, EventArgs e)
        {
            ConfigureDataGridView();
            LoadReservations();
        }

        private void ConfigureDataGridView()
        {
            reservationdatagridview.Columns.Clear();
            reservationdatagridview.AutoGenerateColumns = false;

            // Show Reservation ID (read-only)
            reservationdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ReservationId",
                HeaderText = "ID",
                DataPropertyName = "ReservationId",
                ReadOnly = true
            });

            // Client/UserName
            reservationdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "UserName",
                HeaderText = "Client",
                DataPropertyName = "UserName"
            });

            // Room
            reservationdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "RoomId",
                HeaderText = "Room",
                DataPropertyName = "RoomId"
            });

            // Check-in / Check-out
            reservationdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CheckIn",
                HeaderText = "Check In",
                DataPropertyName = "CheckIn"
            });
            reservationdatagridview.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "CheckOut",
                HeaderText = "Check Out",
                DataPropertyName = "CheckOut"
            });
        }

        private void LoadReservations(string search = "")
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();

                string query = "SELECT ReservationId, UserName, RoomId, CheckIn, CheckOut FROM Reservation";

                if (!string.IsNullOrWhiteSpace(search))
                    query += " WHERE UserName LIKE @Search";

                SqlCommand cmd = new SqlCommand(query, conn);

                if (!string.IsNullOrWhiteSpace(search))
                    cmd.Parameters.AddWithValue("@Search", "%" + search + "%");

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                reservationdatagridview.DataSource = dt;
            }
        }

        private void SaveReservation(bool isNew)
        {
            try
            {
                HotelReservation reservation = new HotelReservation
                {
                    UserName = Nametxt.Text.Trim(),
                    RoomId = Roomtxt.Text.Trim(),
                    CheckIn = Checkindatetimepicker.Value,
                    CheckOut = Checkoutdatetimepicker.Value
                };

                reservation.Validate();

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = isNew
                        ? "INSERT INTO Reservation (UserName, RoomId, CheckIn, CheckOut) VALUES (@UserName, @Room, @CheckIn, @CheckOut)"
                        : "UPDATE Reservation SET UserName=@UserName, RoomId=@Room, CheckIn=@CheckIn, CheckOut=@CheckOut WHERE ReservationId=@ID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserName", reservation.UserName);
                    cmd.Parameters.AddWithValue("@Room", reservation.RoomId);
                    cmd.Parameters.AddWithValue("@CheckIn", reservation.CheckIn);
                    cmd.Parameters.AddWithValue("@CheckOut", reservation.CheckOut);

                    if (!isNew)
                        cmd.Parameters.AddWithValue("@ID", selectedReservationID);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show(isNew ? "Reservation added!" : "Reservation updated!");

                ClearFields();
                LoadReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddBtn_Click(object sender, EventArgs e) => SaveReservation(true);

        private void EditBtn_Click(object sender, EventArgs e)
        {
            if (selectedReservationID == 0)
            {
                MessageBox.Show("Select a reservation first.");
                return;
            }

            SaveReservation(false);
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (selectedReservationID == 0)
            {
                MessageBox.Show("Select a reservation first.");
                return;
            }

            if (MessageBox.Show("Delete this reservation?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Reservation WHERE ReservationId=@ID", conn);
                cmd.Parameters.AddWithValue("@ID", selectedReservationID);
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Reservation deleted.");
            ClearFields();
            LoadReservations();
        }

        private void Searchbtn_Click(object sender, EventArgs e) => LoadReservations(Searchtxt.Text.Trim());

        private void BackBtn_Click(object sender, EventArgs e)
        {
            Mainpagecs mainpage = new Mainpagecs();
            mainpage.Show();
            this.Close();
        }

        private void reservationdatagridview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = reservationdatagridview.Rows[e.RowIndex];

            selectedReservationID = Convert.ToInt32(row.Cells["ReservationId"].Value);
            Nametxt.Text = row.Cells["UserName"].Value.ToString();
            Roomtxt.Text = row.Cells["RoomId"].Value.ToString();
            Checkindatetimepicker.Value = Convert.ToDateTime(row.Cells["CheckIn"].Value);
            Checkoutdatetimepicker.Value = Convert.ToDateTime(row.Cells["CheckOut"].Value);

            EditBtn.Enabled = true;
            DeleteBtn.Enabled = true;
        }

        private void ClearFields()
        {
            selectedReservationID = 0;
            Nametxt.Clear();
            Roomtxt.Clear();
            Checkindatetimepicker.Value = DateTime.Now;
            Checkoutdatetimepicker.Value = DateTime.Now.AddDays(1);

            EditBtn.Enabled = false;
            DeleteBtn.Enabled = false;
        }

        private void Checkindatetimepicker_ValueChanged(object sender, EventArgs e)
        {
            if (Checkoutdatetimepicker.Value <= Checkindatetimepicker.Value)
                Checkoutdatetimepicker.Value = Checkindatetimepicker.Value.AddDays(1);
        }

        private void Checkoutdatetimepicker_ValueChanged(object sender, EventArgs e)
        {
            if (Checkoutdatetimepicker.Value <= Checkindatetimepicker.Value)
                MessageBox.Show("Check-out must be after check-in.");
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}