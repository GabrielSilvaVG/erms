using ERMS.Data;
using ERMS.Models;


namespace ERMS.Services
{
    public class UserService(AppDbContext context) : IGenericService<User, int>
    {
        private readonly AppDbContext _context = context;

        public Task<User> InsertAsync(User entity)
        {
            throw new NotImplementedException();
        }
        public Task<User?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User entity)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}