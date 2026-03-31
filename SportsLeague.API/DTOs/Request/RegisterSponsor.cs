namespace SportsLeague.API.DTOs.Request
{
   public class RegisterSponsorDTO
   {
        // Se envian los datos necesarios para registrar un nuevo patrocinador en un torneo
        public int SponsorId { get; set; }
        public decimal ContractAmount { get; set; }
    }
}
