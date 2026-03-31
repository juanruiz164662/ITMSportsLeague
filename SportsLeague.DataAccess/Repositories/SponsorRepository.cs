using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class SponsorRepository : GenericRepository<Sponsor>, ISponsorRepository
    {
        public SponsorRepository(LeagueDbContext context) : base(context)
        {
        }
        // Se verifica si existe un sponsor con el mismo nombre
        public async Task<bool> ExistsByNameAsync(string name)
        {
            return await _dbSet
                .Where(s => s.Name.ToLower() == name.ToLower())
                .AnyAsync();
        }
        // Se obtiene un sponsor por su nombre, ignorando mayúsculas y minúsculas
        public async Task<Sponsor?> GetByNameAsync(string name)
        {
            return await _dbSet
                .Where(s => s.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
        }
    }
}
