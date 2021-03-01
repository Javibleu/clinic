using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Clinic
    {
        public int Id { get; set; }
        public string ClinicName { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Url { get; set; }

        public string Email { get; set; }

        public string Card { get; set; }

        public AppUser Contact { get; set; }

        public int HealthProviderType { get; set; }
    }
}

