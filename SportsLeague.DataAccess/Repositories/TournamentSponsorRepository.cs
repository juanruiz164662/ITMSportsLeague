using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }
        // Se otienen los registros del sponsor, incluyendo la información del torneo
        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorID)
        {
            return await _dbSet
                .Where(ts => ts.SponsorId == sponsorID)
                .Include(ts => ts.Tournament)
                .ToListAsync();
        }
        // Se otienen los registros del torneo, incluyendo la información del sponsor
        public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentID)
        {
            return await _dbSet
                .Where(ts => ts.TournamentId == tournamentID)
                .Include(ts => ts.Sponsor)
                .ToListAsync();
        }
        // Se obtiene un registro específico de la relación entre torneo y sponsor
        public async Task<TournamentSponsor?> GetByTournamentAndSponsorIdAsync(int tournamentID, int sponsorID)
        {
            return await _dbSet
                .Where(ts => ts.TournamentId == tournamentID && ts.SponsorId == sponsorID)
                .FirstOrDefaultAsync();
        }
    }
}
