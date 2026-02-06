using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectASS
{
    public partial class Room : Form
    {
        public Room()
        {
            InitializeComponent();
        }

        private void Room_Load(object sender, EventArgs e)
        {

        }

        private void addbtn_Click(object sender, EventArgs e)
        {
            HotelRoom room = new HotelRoom();
            room.RoomNumber = Convert.ToDouble(roomnumbertxt.Text);
            room.PhoneNumber = phonenumbertxt.Text;

            // Set boolean availability from radio buttons
            room.RoomAvailability = rdyes.Checked;

            roomdatagridview.Rows.Clear();
            roomdatagridview.Rows.Add(
                room.RoomNumber,
                room.PhoneNumber,
                room.RoomAvailability ? "Available" : "Not Available"
            );
            roomnumbertxt.Clear();
            phonenumbertxt.Clear();
            rdyes.Checked = false;
            rdno.Checked = false;
        }
    }
}
