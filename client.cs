using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectASS
{
    public abstract class ClientBase
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }

        public abstract string GetClientInfo();
    }

  
    public class Client : ClientBase
    {
        public override string GetClientInfo()
        {
            return $"{ClientName} - {PhoneNumber} - {Country}";
        }
    }
}
