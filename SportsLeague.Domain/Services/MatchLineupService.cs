using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Helpers;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class MatchLineupService : IMatchLineupService
    {
        private readonly IMatchLineupRepository _lineupRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly MatchValidationHelper _validationHelper;
        private readonly ILogger<MatchLineupService> _logger;

        public MatchLineupService(
            IMatchLineupRepository lineupRepository,
            IMatchRepository matchRepository,
            MatchValidationHelper validationHelper,
            ILogger<MatchLineupService> logger)
        {
            _lineupRepository = lineupRepository;
            _matchRepository = matchRepository;
            _validationHelper = validationHelper;
            _logger = logger;
        }

        // GET alineacion completa del partido
        public async Task<IEnumerable<MatchLineup>> GetByMatchAsync(int matchId)
            => await _lineupRepository.GetByMatchAsync(matchId);

        // GET alineacion filtrada por equipo
        public async Task<IEnumerable<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
            => await _lineupRepository.GetByMatchAndTeamAsync(matchId, teamId);

        // POST agregar jugador a la alineacion con las 6 validaciones
        public async Task<MatchLineup> AddPlayerAsync(int matchId, int playerId, bool isStarter, string position)
        {
            // El partido debe existir
            var match = await _matchRepository.GetByIdAsync(matchId);
            if (match == null)
                throw new KeyNotFoundException($"No se encontro el partido con ID {matchId}");

            // El jugador debe existir y pertenecer al HomeTeam o AwayTeam
            var player = await _validationHelper.ValidatePlayerInMatchAsync(playerId, match);

            // El jugador no puede estar registrado dos veces en la misma alineacion
            if (await _lineupRepository.ExistsByMatchAndPlayerAsync(matchId, playerId))
                throw new InvalidOperationException("El jugador ya esta registrado en la alineacion de este partido");

            // Maximo 11 titulares por equipo por partido (solo aplica IsStarter = true)
            if (isStarter)
            {
                var starterCount = await _lineupRepository.CountStartersByMatchAndTeamAsync(matchId, player.TeamId);
                if (starterCount >= 11)
                    throw new InvalidOperationException("El equipo ya tiene 11 titulares registrados en este partido");
            }

            // El partido debe estar en estado Scheduled (Programado)
            if (match.Status != MatchStatus.Scheduled)
                throw new InvalidOperationException("Solo se pueden registrar alineaciones en partidos Scheduled");

            var lineup = new MatchLineup
            {
                MatchId = matchId,
                PlayerId = playerId,
                IsStarter = isStarter,
                Position = position
            };

            _logger.LogInformation(
                "Se añade el jugador {PlayerId} a la alineación del partido {MatchId}, IsStarter: {IsStarter}",
                playerId, matchId, isStarter);

            var created = await _lineupRepository.CreateAsync(lineup);

            // Recargar con includes para retornar PlayerName y TeamName
            var lineups = await _lineupRepository.GetByMatchAsync(matchId);
            return lineups.First(l => l.Id == created.Id);
        }

        // DELETE eliminar jugador de la alineacion
        public async Task DeleteAsync(int matchId, int lineupId)
        {
            var lineup = await _lineupRepository.GetByIdAsync(lineupId);
            if (lineup == null || lineup.MatchId != matchId)
                throw new KeyNotFoundException($"No se encontro la alineacion con ID {lineupId}");

            _logger.LogInformation(
                "Se elimina la alineación con ID {LineupId} del partido {MatchId}", lineupId, matchId);

            await _lineupRepository.DeleteAsync(lineupId);
        }
    }

}
