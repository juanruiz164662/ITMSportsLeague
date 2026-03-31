using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
    {
        // Registros relacionados a un patrocinador específico
        Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);

        // Registros relacionados a un torneo específico
        Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId);

        // Registro específico por torneo y patrocinador
        Task<TournamentSponsor?> GetByTournamentAndSponsorIdAsync(int tournamentId, int sponsorId);
    }
}
