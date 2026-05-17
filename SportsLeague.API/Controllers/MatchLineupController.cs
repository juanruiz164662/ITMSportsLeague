using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;    
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/match/{matchId}")]
    public class MatchLineupController : ControllerBase
    {
        private readonly IMatchLineupService _matchLineupService;
        private readonly IMapper _mapper;

        public MatchLineupController(IMatchLineupService matchLineupService, IMapper mapper)
        {
            _matchLineupService = matchLineupService;
            _mapper = mapper;
        }

        // Obtener la alineacion completa del partido
        [HttpGet("lineup")]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetByMatch(int matchId)
        {
            var lineups = await _matchLineupService.GetByMatchAsync(matchId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineups));
        }

        // Obtener alineacion filtrada por equipo
        [HttpGet("lineup/team/{teamId}")]
        public async Task<ActionResult<IEnumerable<MatchLineupResponseDTO>>> GetByMatchAndTeam(
            int matchId, int teamId)
        {
            var lineups = await _matchLineupService.GetByMatchAndTeamAsync(matchId, teamId);
            return Ok(_mapper.Map<IEnumerable<MatchLineupResponseDTO>>(lineups));
        }

        // Agregar un jugador a la alineacion
        [HttpPost("lineup")]
        public async Task<ActionResult<MatchLineupResponseDTO>> AddPlayer(
            int matchId, MatchLineupRequestDto dto)
        {
            try
            {
                var created = await _matchLineupService.AddPlayerAsync(
                    matchId, dto.PlayerId, dto.IsStarter, dto.Position);

                var response = _mapper.Map<MatchLineupResponseDTO>(created);
                return CreatedAtAction(nameof(GetByMatch), new { matchId }, response);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // Eliminar un jugador de la alineacion
        [HttpDelete("lineup/{id}")]
        public async Task<ActionResult> Delete(int matchId, int id)
        {
            try
            {
                await _matchLineupService.DeleteAsync(matchId, id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }

}
