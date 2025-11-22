using ERMS.Data;
using ERMS.Models;

namespace ERMS.Services
{
    public class RegistrationService(AppDbContext context) : IGenericService<Registration, int>
    {
        private readonly AppDbContext _context = context;

        public Task<Registration> InsertAsync(Registration entity)
        {
            throw new NotImplementedException();
        }
        public Task<Registration?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Registration>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Registration entity)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}