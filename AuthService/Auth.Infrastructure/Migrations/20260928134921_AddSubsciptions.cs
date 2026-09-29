using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Auth.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSubsciptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TeacherProfiles_SubscriptionPlan",
                table: "TeacherProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TeacherProfiles_SubscriptionPlan_SubscriptionExpiresAt",
                table: "TeacherProfiles");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlan",
                table: "TeacherProfiles");

            migrationBuilder.AddColumn<int>(
                name: "SubscriptionPlanId",
                table: "TeacherProfiles",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DurationDays = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPopular = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SubscriptionPlans",
                columns: new[] { "Id", "Code", "Description", "DurationDays", "IsActive", "IsPopular", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Оплата каждый месяц", 30, true, false, "Monthly", 19.99m },
                    { 2, 2, "Оплата раз в год. Экономия $60", 365, true, true, "Yearly", 179.99m },
                    { 3, 3, "Премиум доступ со всеми функциями", 30, true, false, "VIP", 39.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherProfiles_SubscriptionPlanId",
                table: "TeacherProfiles",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherProfiles_SubscriptionPlanId_SubscriptionExpiresAt",
                table: "TeacherProfiles",
                columns: new[] { "SubscriptionPlanId", "SubscriptionExpiresAt" });

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_Code",
                table: "SubscriptionPlans",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherProfiles_SubscriptionPlans_SubscriptionPlanId",
                table: "TeacherProfiles",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherProfiles_SubscriptionPlans_SubscriptionPlanId",
                table: "TeacherProfiles");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropIndex(
                name: "IX_TeacherProfiles_SubscriptionPlanId",
                table: "TeacherProfiles");

            migrationBuilder.DropIndex(
                name: "IX_TeacherProfiles_SubscriptionPlanId_SubscriptionExpiresAt",
                table: "TeacherProfiles");

            migrationBuilder.DropColumn(
                name: "SubscriptionPlanId",
                table: "TeacherProfiles");

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionPlan",
                table: "TeacherProfiles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "monthly");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherProfiles_SubscriptionPlan",
                table: "TeacherProfiles",
                column: "SubscriptionPlan");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherProfiles_SubscriptionPlan_SubscriptionExpiresAt",
                table: "TeacherProfiles",
                columns: new[] { "SubscriptionPlan", "SubscriptionExpiresAt" });
        }
    }
}
