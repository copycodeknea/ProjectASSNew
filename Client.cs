using System;

namespace ProjectASS
{
    public class Client
    {
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ClientName))
                throw new ArgumentException("Client name cannot be empty.");

            if (string.IsNullOrWhiteSpace(PhoneNumber))
                throw new ArgumentException("Phone number cannot be empty.");

            if (string.IsNullOrWhiteSpace(Country))
                throw new ArgumentException("Country must be selected.");
        }
    }
}