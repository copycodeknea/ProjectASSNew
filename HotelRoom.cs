using System;

namespace ProjectASS
{
    // Abstract base class (Abstraction + Inheritance)
    public abstract class RoomBase
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int FloorNumber { get; set; }
        public bool RoomAvailability { get; set; }

        // Polymorphism
        public abstract string GetAvailabilityStatus();
    }

    public class HotelRoom : RoomBase
    {
        public override string GetAvailabilityStatus()
        {
            return RoomAvailability ? "Available" : "Occupied";
        }
    }
}
