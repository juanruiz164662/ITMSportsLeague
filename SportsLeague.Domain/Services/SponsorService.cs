using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly ILogger<SponsorService> _logger;

        //Inyección de dependencias a través del constructor
        public SponsorService(
        ISponsorRepository sponsorRepository,
        ITournamentSponsorRepository tournamentSponsorRepository,
        ITournamentRepository tournamentRepository,
        ILogger<SponsorService> logger)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _tournamentRepository = tournamentRepository;
            _logger = logger;
        }
        // Retorna todos los sponsors
        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all sponsors");
            return await _sponsorRepository.GetAllAsync();
        }

        // Retorna un sponsor por el ID
        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving sponsor with ID: {SponsorId}", id);
            var sponsor = await _sponsorRepository.GetByIdAsync(id);
            if (sponsor == null)
                _logger.LogWarning("Sponsor with ID {SponsorId} not found", id);
            return sponsor;
        }

        // Crea un nuevo sponsor
        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            //Validacion de unico nombre
            var exists = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);
            if (exists)
            {
                _logger.LogWarning("Sponsor with name {SponsorName} already exists", sponsor.Name);
                throw new InvalidOperationException($"Sponsor with name {sponsor.Name} already exists.");
            }

            //Validacion de email valido
            if (string.IsNullOrWhiteSpace(sponsor.ContactEmail) && !sponsor.ContactEmail.Contains("@"))
            {
                throw new InvalidOperationException("El email no tiene un formato valido.");
            }
            _logger.LogInformation("Creating new sponsor with name: {SponsorName}", sponsor.Name);
            return await _sponsorRepository.CreateAsync(sponsor);
        }

        // Actualiza un sponsor existente
        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            //Validar que el sponsor exista
            var exist = await _sponsorRepository.GetByIdAsync(id);
            if (exist == null)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found for update", id);
                throw new KeyNotFoundException($"Sponsor with ID {id} not found.");
            }
            //Validacion de unico nombre
            if (exist.Name != sponsor.Name)
            {
                var exist1 = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);
                if (exist1)
                {
                    _logger.LogWarning("Sponsor with name {SponsorName} already exists for update", sponsor.Name);
                    throw new InvalidOperationException($"Sponsor with name {sponsor.Name} already exists.");
                }
            }
            //Validacion de email valido
            if (!string.IsNullOrWhiteSpace(sponsor.ContactEmail) && sponsor.ContactEmail.Contains("@"))
            {
                throw new InvalidOperationException("El email no tiene un formato valido.");
            }
            exist.Name = sponsor.Name;
            exist.ContactEmail = sponsor.ContactEmail;
            exist.Phone = sponsor.Phone;
            exist.WebsiteUrl = sponsor.WebsiteUrl;
            exist.Category = sponsor.Category;

            _logger.LogInformation("Updating sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.UpdateAsync(exist);
        }

        // Elimina un sponsor por el ID
        public async Task DeleteAsync(int id)
        {
            //Validar que el sponsor exista
            var exist = await _sponsorRepository.ExistsAsync(id);
            if (!exist)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found for deletion", id);
                throw new KeyNotFoundException($"Sponsor with ID {id} not found.");
            }
            //Validacion de que no tenga torneos asociados
            var hasTournaments = await _tournamentSponsorRepository.GetBySponsorIdAsync(id);
            if (hasTournaments.Any())
            {
                _logger.LogWarning("Cannot delete sponsor with ID {SponsorId} because it has associated tournaments", id);
                throw new InvalidOperationException("Cannot delete sponsor because it has associated tournaments.");
            }
            _logger.LogInformation("Deleting sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.DeleteAsync(id);
        }

        // Registra un sponsor a un torneo con un monto de contrato
        public async Task RegisterSponsorAsync(int sponsorId, int tournamentId, decimal contractAmount)
        {
            //Validar que el sponsor exista
            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found for registration", sponsorId);
                throw new KeyNotFoundException($"Sponsor with ID {sponsorId} not found.");
            }
            //Validar que el torneo exista
            var tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
            if (tournament == null)
            {
                _logger.LogWarning("Tournament with ID {TournamentId} not found for sponsor registration", tournamentId);
                throw new KeyNotFoundException($"Tournament with ID {tournamentId} not found.");
            }
            //Validar el monto del contrato
            if (contractAmount <= 0)
            {
                _logger.LogWarning("Invalid contract amount {ContractAmount} for sponsor registration", contractAmount);
                throw new InvalidOperationException("El monto del contrato debe ser mayor a 0.");
            }
            //Validar que ya no este registrado
            var exists = await _tournamentSponsorRepository.GetByTournamentAndSponsorIdAsync(tournamentId, sponsorId);
            if (exists != null)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} is already registered for tournament with ID {TournamentId}", sponsorId, tournamentId);
                throw new InvalidOperationException("El sponsor ya esta registrado para este torneo.");
            }
            var tournamentSponsor = new TournamentSponsor
            {
                SponsorId = sponsorId,
                TournamentId = tournamentId,
                ContractAmount = contractAmount,
                JoinedAt = DateTime.UtcNow
            };
            _logger.LogInformation("Registering sponsor with ID {SponsorId} for tournament with ID {TournamentId}", sponsorId, tournamentId);
            await _tournamentSponsorRepository.CreateAsync(tournamentSponsor);
        }

        //Elinima el registro de un sponsor a un torneo
        public async Task UnregisterSponsorAsync(int sponsorId, int tournamentId)
        {
            //Validar que el sponsor exista
            var exist = await _tournamentSponsorRepository.GetByTournamentAndSponsorIdAsync(sponsorId, tournamentId);
            if (exist == null)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} is not registered for tournament with ID {TournamentId} for unregistration", sponsorId, tournamentId);
                throw new KeyNotFoundException("El sponsor no esta registrado para este torneo.");
            }
            _logger.LogInformation("Unregistering sponsor with ID {SponsorId} from tournament with ID {TournamentId}", sponsorId, tournamentId);
            await _tournamentSponsorRepository.DeleteAsync(exist.Id);
        }

        // Retorna los torneos asociados a un sponsor
        public async Task<IEnumerable<Tournament>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            //Validar que el sponsor exista
            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
            if (sponsor == null)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found for retrieving tournaments", sponsorId);
                throw new KeyNotFoundException($"Sponsor with ID {sponsorId} not found.");
            }
            //Validar que tenga torneos asociados
            var registro = await _tournamentSponsorRepository.GetBySponsorIdAsync(sponsorId);
            return registro.Select(r => r.Tournament).ToList();
        }
    }
}

