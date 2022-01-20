using Microsoft.EntityFrameworkCore.Migrations;

namespace WebProjekat.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aerodromi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeAerodroma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImeGrada = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aerodromi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kompanije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeKompanije = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BrSedista = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kompanije", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Putnik",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(55)", maxLength: 55, nullable: false),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    BrLicneKarte = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    BrTelefona = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Putnik", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Destinacije",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kontinent = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Drzava = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Grad = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DatumiVreme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IDKompanijeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Destinacije", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Destinacije_Kompanije_IDKompanijeID",
                        column: x => x.IDKompanijeID,
                        principalTable: "Kompanije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Letovi",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipSedista = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cena = table.Column<int>(type: "int", nullable: false),
                    PutnikID = table.Column<int>(type: "int", nullable: true),
                    DestinacijaID = table.Column<int>(type: "int", nullable: true),
                    AerodromID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Letovi", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Letovi_Aerodromi_AerodromID",
                        column: x => x.AerodromID,
                        principalTable: "Aerodromi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Letovi_Destinacije_DestinacijaID",
                        column: x => x.DestinacijaID,
                        principalTable: "Destinacije",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Letovi_Putnik_PutnikID",
                        column: x => x.PutnikID,
                        principalTable: "Putnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Destinacije_IDKompanijeID",
                table: "Destinacije",
                column: "IDKompanijeID");

            migrationBuilder.CreateIndex(
                name: "IX_Letovi_AerodromID",
                table: "Letovi",
                column: "AerodromID");

            migrationBuilder.CreateIndex(
                name: "IX_Letovi_DestinacijaID",
                table: "Letovi",
                column: "DestinacijaID");

            migrationBuilder.CreateIndex(
                name: "IX_Letovi_PutnikID",
                table: "Letovi",
                column: "PutnikID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Letovi");

            migrationBuilder.DropTable(
                name: "Aerodromi");

            migrationBuilder.DropTable(
                name: "Destinacije");

            migrationBuilder.DropTable(
                name: "Putnik");

            migrationBuilder.DropTable(
                name: "Kompanije");
        }
    }
}
