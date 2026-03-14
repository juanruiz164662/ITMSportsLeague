using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsLeague.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tournamentTeams_Teams_TeamId",
                table: "tournamentTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_tournamentTeams_tournaments_TournamentId",
                table: "tournamentTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tournamentTeams",
                table: "tournamentTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tournaments",
                table: "tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_referees",
                table: "referees");

            migrationBuilder.RenameTable(
                name: "tournamentTeams",
                newName: "TournamentTeams");

            migrationBuilder.RenameTable(
                name: "tournaments",
                newName: "Tournaments");

            migrationBuilder.RenameTable(
                name: "referees",
                newName: "Referees");

            migrationBuilder.RenameIndex(
                name: "IX_tournamentTeams_TournamentId_TeamId",
                table: "TournamentTeams",
                newName: "IX_TournamentTeams_TournamentId_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_tournamentTeams_TeamId",
                table: "TournamentTeams",
                newName: "IX_TournamentTeams_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentTeams",
                table: "TournamentTeams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Referees",
                table: "Referees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeams_Teams_TeamId",
                table: "TournamentTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentTeams_Tournaments_TournamentId",
                table: "TournamentTeams",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeams_Teams_TeamId",
                table: "TournamentTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentTeams_Tournaments_TournamentId",
                table: "TournamentTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentTeams",
                table: "TournamentTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tournaments",
                table: "Tournaments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Referees",
                table: "Referees");

            migrationBuilder.RenameTable(
                name: "TournamentTeams",
                newName: "tournamentTeams");

            migrationBuilder.RenameTable(
                name: "Tournaments",
                newName: "tournaments");

            migrationBuilder.RenameTable(
                name: "Referees",
                newName: "referees");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentTeams_TournamentId_TeamId",
                table: "tournamentTeams",
                newName: "IX_tournamentTeams_TournamentId_TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentTeams_TeamId",
                table: "tournamentTeams",
                newName: "IX_tournamentTeams_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tournamentTeams",
                table: "tournamentTeams",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tournaments",
                table: "tournaments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_referees",
                table: "referees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tournamentTeams_Teams_TeamId",
                table: "tournamentTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tournamentTeams_tournaments_TournamentId",
                table: "tournamentTeams",
                column: "TournamentId",
                principalTable: "tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
