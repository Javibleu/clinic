using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class ClinicRepository : IClinicRepository
    {

        private readonly DataContext _context;
        public ClinicRepository(DataContext context)
        {
            _context = context;
        }


        public void AddClinic(Clinic clinic)
        {
            _context.Clinics.Add(clinic);
        }

        public Task<Clinic> GetClinicByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Clinic> GetClinicByNameAsync(string clinicname)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Clinic>> GetClinicsAsync()
        {
            return await _context.Clinics.ToListAsync();
        }

        public void Update(AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
