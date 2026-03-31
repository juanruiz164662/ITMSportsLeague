using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Services
{
    public interface ISponsorService
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task RegisterSponsorAsync (int sponsorId ,int tournamentId ,decimal contractAmount);
        Task UnregisterSponsorAsync(int sponsorId, int tournamentId);
        Task<Sponsor?> GetByIdAsync(int id);
        Task<Sponsor> CreateAsync(Sponsor sponsor);
        Task UpdateAsync(int id, Sponsor sponsor);
        Task DeleteAsync(int id);
        Task<IEnumerable<Tournament>> GetTournamentsBySponsorAsync(int sponsorId);        
    }
}
