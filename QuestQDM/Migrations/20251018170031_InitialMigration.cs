using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Quest.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectQualities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProjectTitle = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ProjectId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectQualities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentQualities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DocumentType = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    DocumentTitle = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ProjectQualityId = table.Column<int>(type: "INTEGER", nullable: false),
                    QualityId = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    IsRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsAssessed = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentQualities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentQualities_ProjectQualities_ProjectQualityId",
                        column: x => x.ProjectQualityId,
                        principalTable: "ProjectQualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QualityFactorType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Colors = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    ProjectQualityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityFactorType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityFactorType_ProjectQualities_ProjectQualityId",
                        column: x => x.ProjectQualityId,
                        principalTable: "ProjectQualities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QualityGrade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", maxLength: 4, nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false),
                    Meaning = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ProjectQualityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityGrade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityGrade_ProjectQualities_ProjectQualityId",
                        column: x => x.ProjectQualityId,
                        principalTable: "ProjectQualities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QualityNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Ord = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    ParentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 21, nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: true),
                    FactorTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    DocumentQualityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualityNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QualityNodes_DocumentQualities_DocumentQualityId",
                        column: x => x.DocumentQualityId,
                        principalTable: "DocumentQualities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QualityNodes_QualityFactorType_FactorTypeId",
                        column: x => x.FactorTypeId,
                        principalTable: "QualityFactorType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QualityNodes_QualityNodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "QualityNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentQualities_ProjectQualityId",
                table: "DocumentQualities",
                column: "ProjectQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityFactorType_ProjectQualityId",
                table: "QualityFactorType",
                column: "ProjectQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityGrade_ProjectQualityId",
                table: "QualityGrade",
                column: "ProjectQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityNodes_DocumentQualityId",
                table: "QualityNodes",
                column: "DocumentQualityId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityNodes_FactorTypeId",
                table: "QualityNodes",
                column: "FactorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_QualityNodes_ParentId",
                table: "QualityNodes",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualityGrade");

            migrationBuilder.DropTable(
                name: "QualityNodes");

            migrationBuilder.DropTable(
                name: "DocumentQualities");

            migrationBuilder.DropTable(
                name: "QualityFactorType");

            migrationBuilder.DropTable(
                name: "ProjectQualities");
        }
    }
}
