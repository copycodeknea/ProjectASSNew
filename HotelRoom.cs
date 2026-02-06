using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectASS
{
    public class HotelRoom
    {
        public double RoomNumber { get; set; }
        public string PhoneNumber { get; set; }
        public bool RoomAvailability { get; set; }

        public void Number()
        {
            MessageBox.Show("Room Number: " + RoomNumber);
        }
        public void Phone()
        {
            MessageBox.Show("Phone Number: " + PhoneNumber);
        }
        public void Availability()
        {
            MessageBox.Show("Room Availability: " + (RoomAvailability ? "Available" : "Not Available"));
        }
    }
}
