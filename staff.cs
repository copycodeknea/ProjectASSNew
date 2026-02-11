using System;

namespace ProjectASS
{
    // Abstract base class with inheritance + validation
    public abstract class StaffBase
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public string Gender { get; set; }

        // Abstract method for polymorphism
        public abstract string GetStaffType();

        // Validation inside the class
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(StaffName))
                throw new Exception("Staff name cannot be empty.");
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                throw new Exception("Phone number cannot be empty.");
            if (string.IsNullOrWhiteSpace(Position))
                throw new Exception("Position cannot be empty.");
            if (string.IsNullOrWhiteSpace(Gender))
                throw new Exception("Gender must be selected.");
        }
    }

    public class HotelStaff : StaffBase
    {
        public override string GetStaffType()
        {
            return "Hotel Staff";
        }
    }
}
