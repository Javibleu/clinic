using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IClinicRepository
    {

        void AddClinic(Clinic clinic);
        Task<IEnumerable<Clinic>> GetClinicsAsync();
        Task<Clinic> GetClinicByIdAsync(int id);
        Task<Clinic> GetClinicByNameAsync(string clinicname);

       //Task<ClinicDto> GetAsync(string username);

        void Update(AppUser user);

    }
}
