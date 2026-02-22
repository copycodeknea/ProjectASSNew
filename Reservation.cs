using ProjectASS;
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
using System.Xml.Linq;



namespace ProjectASS
{
    public partial class Reservation : Form
    {
        private int selectedReservationId = 0;
        public Reservation()
        {
            InitializeComponent();
            reservationdatagridview.CellClick += reservationdatagridview;
            this.Load += Reservation_Load;
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(searchtxt.Text, out _))
            {
                MessageBox.Show("Room ID must be numeric.");
                return;
            }

            try
            {
                using (SqlConnection conn =
                    new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = @"SELECT ReservationId, ClientId, RoomId,
                                     CheckIn, CheckOut
                                     FROM Reservation
                                     WHERE RoomId = @roomId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@roomId",
                            int.Parse(searchtxt.Text));

                        using (SqlDataAdapter adapter =
                            new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DataTable display = new DataTable();
                            display.Columns.Add("ReservationId", typeof(int));
                            display.Columns.Add("Client ID");
                            display.Columns.Add("Room ID");
                            display.Columns.Add("Check In");
                            display.Columns.Add("Check Out");
                            display.Columns.Add("Total Days");

                            foreach (DataRow row in dt.Rows)
                            {
                                HotelReservation res =
                                    new HotelReservation
                                    {
                                        ReservationId =
                                            Convert.ToInt32(row["ReservationId"]),
                                        ClientId =
                                            Convert.ToInt32(row["ClientId"]),
                                        RoomId =
                                            Convert.ToInt32(row["RoomId"]),
                                        CheckIn =
                                            Convert.ToDateTime(row["CheckIn"]),
                                        CheckOut =
                                            Convert.ToDateTime(row["CheckOut"])
                                    };

                                display.Rows.Add(
                                    res.ReservationId,
                                    res.ClientId,
                                    res.RoomId,
                                    res.CheckIn.ToShortDateString(),
                                    res.CheckOut.ToShortDateString(),
                                    res.GetTotalDays()
                                );
                            }

                            reservationdatagridview.DataSource = display;
                            reservationdatagridview.Columns["ReservationId"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message);
            }

        }
        private void reservationdatagridview_CellClick(object sender,
            DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row =
                reservationdatagridview.Rows[e.RowIndex];

            selectedReservationId =
                Convert.ToInt32(row.Cells["ReservationId"].Value);

            useridtxt.Text = row.Cells["Client ID"].Value.ToString();
            usernametxt.Text = row.Cells["Name"].Value.ToString();
            checkindate.Value =
                Convert.ToDateTime(row.Cells["Check In"].Value);
            checkoutdate.Value =
                Convert.ToDateTime(row.Cells["Check Out"].Value);
        }
        private void Reservation_Load(object sender, EventArgs e)
        {
            LoadReservations();
        }
        private void LoadReservations()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.ConnectionString))
                {
                    conn.Open();

                    string query = @"SELECT ReservationId, ClientId, RoomId, CheckIn, CheckOut 
                                     FROM Reservation";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataTable displayTable = new DataTable();
                        displayTable.Columns.Add("ReservationId", typeof(int));
                        displayTable.Columns.Add("Client ID", typeof(int));
                        displayTable.Columns.Add("Room ID", typeof(int));
                        displayTable.Columns.Add("Check In", typeof(DateTime));
                        displayTable.Columns.Add("Check Out", typeof(DateTime));
                        displayTable.Columns.Add("Days", typeof(int));

                        foreach (DataRow row in dt.Rows)
                        {
                            HotelReservation reservation = new HotelReservation
                            {
                                ReservationId = Convert.ToInt32(row["ReservationId"]),
                                ClientId = Convert.ToInt32(row["ClientId"]),
                                RoomId = Convert.ToInt32(row["RoomId"]),
                                CheckIn = Convert.ToDateTime(row["CheckIn"]),
                                CheckOut = Convert.ToDateTime(row["CheckOut"])
                            };

                            displayTable.Rows.Add(
                                reservation.ReservationId,
                                reservation.ClientId,
                                reservation.RoomId,
                                reservation.CheckIn,
                                reservation.CheckOut,
                                reservation.GetTotalDays()
                            );
                        }

                        reservationdatagridview.DataSource = displayTable;

                        if (reservationdatagridview.Columns["ReservationId"] != null)
                            reservationdatagridview.Columns["ReservationId"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading reservations: " + ex.Message);
            }
        }

        private void cboRoom_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgvReservation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }
    }
}
