using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ISponsorRepository : IGenericRepository<Sponsor>
    {
        // Verificar si existe un patrocinador por nombre
        Task<bool> ExistsByNameAsync(string name);

        // Obtener un patrocinador por nombre
        Task<Sponsor?> GetByNameAsync(string name);
    }
}
