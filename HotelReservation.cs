using System;

namespace ProjectASS
{
    public class HotelReservation
    {
        public int ReservationID { get; set; }   // Auto-generated in DB
        public string UserName { get; set; }     // Client name
        public string RoomId { get; set; }       // Room number or ID
        public DateTime CheckIn { get; set; }    // Check-in date
        public DateTime CheckOut { get; set; }   // Check-out date

        // Validation method to ensure data is correct
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(UserName))
                throw new ArgumentException("Client name cannot be empty.");

            if (string.IsNullOrWhiteSpace(RoomId))
                throw new ArgumentException("Room number cannot be empty.");

            if (CheckOut <= CheckIn)
                throw new ArgumentException("Check-out date must be after check-in date.");
        }
    }
}