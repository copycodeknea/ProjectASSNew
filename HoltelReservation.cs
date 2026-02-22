using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectASS
{
    public abstract class ReservationBase
    {
        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        public int RoomId { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        // Polymorphism
        public abstract int GetTotalDays();
    }

    // Inheritance
    public class HotelReservation : ReservationBase
    {
        public override int GetTotalDays()
        {
            return (CheckOut - CheckIn).Days;
        }
    }
}
