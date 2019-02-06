using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BakeryExercise.EntityFramework.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    FoodId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsVegan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.FoodId);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    MenuId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Category = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.MenuId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Portions",
                columns: table => new
                {
                    PortionId = table.Column<Guid>(nullable: false),
                    FoodId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portions", x => x.PortionId);
                    table.ForeignKey(
                        name: "FK_Portions_Food_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "FoodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    MenuItemId = table.Column<Guid>(nullable: false),
                    MenuId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.MenuItemId);
                    table.ForeignKey(
                        name: "FK_MenuItems_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "MenuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemPortions",
                columns: table => new
                {
                    MenuItemId = table.Column<Guid>(nullable: false),
                    PortionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemPortions", x => new { x.MenuItemId, x.PortionId });
                    table.ForeignKey(
                        name: "FK_MenuItemPortions_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemPortions_Portions_PortionId",
                        column: x => x.PortionId,
                        principalTable: "Portions",
                        principalColumn: "PortionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Food",
                columns: new[] { "FoodId", "IsVegan", "Name" },
                values: new object[] { new Guid("21def0fd-9364-45df-b619-11ee8a1303f8"), false, "Doughnut" });

            migrationBuilder.InsertData(
                table: "Food",
                columns: new[] { "FoodId", "IsVegan", "Name" },
                values: new object[] { new Guid("f01d1324-27c6-4b3b-ba43-e757d0dcd1b2"), false, "Muffin" });

            migrationBuilder.InsertData(
                table: "Food",
                columns: new[] { "FoodId", "IsVegan", "Name" },
                values: new object[] { new Guid("525f356f-eb7f-4edb-9975-542d7f58f41a"), false, "Bagel" });

            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "MenuId", "Category", "Name" },
                values: new object[] { new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"), "Breakfast", "Everyday Breakfast" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "IsAdmin", "Password", "UserName" },
                values: new object[] { new Guid("2533ef76-5860-43c6-b0d5-147160a530e6"), true, "password", "admin@bakery.com" });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "MenuId", "Name", "Price" },
                values: new object[] { new Guid("b6f080c0-a045-4ffb-b2a8-9551dde7229e"), new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"), "Single Doughnut", 1.99 });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "MenuId", "Name", "Price" },
                values: new object[] { new Guid("78ff1e8c-975b-4ee7-b91b-e63b62014793"), new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"), "Dozen Doughnuts", 10.99 });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "MenuId", "Name", "Price" },
                values: new object[] { new Guid("27861181-006d-480c-be1e-537b01743dc5"), new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"), "Single Muffin", 2.9900000000000002 });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "MenuId", "Name", "Price" },
                values: new object[] { new Guid("343165a7-985e-4f65-9f91-2cfd9332de34"), new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"), "Dozen Muffins", 13.99 });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "MenuId", "Name", "Price" },
                values: new object[] { new Guid("7aa83e66-eb4e-4bad-adca-564c964f679a"), new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"), "Single Bagel", 0.98999999999999999 });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "MenuId", "Name", "Price" },
                values: new object[] { new Guid("8c333bcd-ebed-4f73-b651-c05bed8e98dc"), new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"), "Dozen Bagels", 9.9900000000000002 });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "MenuItemId", "MenuId", "Name", "Price" },
                values: new object[] { new Guid("70c26879-2630-4ea3-ae1a-b124e96bf7c5"), new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"), "Huge Spread", 39.990000000000002 });

            migrationBuilder.InsertData(
                table: "Portions",
                columns: new[] { "PortionId", "Amount", "FoodId" },
                values: new object[] { new Guid("7bdb6536-b211-41ca-ac4b-7c596e5305de"), 1, new Guid("21def0fd-9364-45df-b619-11ee8a1303f8") });

            migrationBuilder.InsertData(
                table: "Portions",
                columns: new[] { "PortionId", "Amount", "FoodId" },
                values: new object[] { new Guid("f1d66681-3fb5-4595-9b20-2100f499ce55"), 12, new Guid("21def0fd-9364-45df-b619-11ee8a1303f8") });

            migrationBuilder.InsertData(
                table: "Portions",
                columns: new[] { "PortionId", "Amount", "FoodId" },
                values: new object[] { new Guid("200f4f9e-dc4c-46aa-8cbc-3b6e4f835ee0"), 1, new Guid("f01d1324-27c6-4b3b-ba43-e757d0dcd1b2") });

            migrationBuilder.InsertData(
                table: "Portions",
                columns: new[] { "PortionId", "Amount", "FoodId" },
                values: new object[] { new Guid("29332f74-fdd1-42f7-ba79-fe44b45fa577"), 12, new Guid("f01d1324-27c6-4b3b-ba43-e757d0dcd1b2") });

            migrationBuilder.InsertData(
                table: "Portions",
                columns: new[] { "PortionId", "Amount", "FoodId" },
                values: new object[] { new Guid("66d79753-14e3-42e1-98a6-1cf42988ac0e"), 1, new Guid("525f356f-eb7f-4edb-9975-542d7f58f41a") });

            migrationBuilder.InsertData(
                table: "Portions",
                columns: new[] { "PortionId", "Amount", "FoodId" },
                values: new object[] { new Guid("f96e701c-cfd7-4163-8b5b-c2494a58b942"), 12, new Guid("525f356f-eb7f-4edb-9975-542d7f58f41a") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("b6f080c0-a045-4ffb-b2a8-9551dde7229e"), new Guid("7bdb6536-b211-41ca-ac4b-7c596e5305de") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("78ff1e8c-975b-4ee7-b91b-e63b62014793"), new Guid("f1d66681-3fb5-4595-9b20-2100f499ce55") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("27861181-006d-480c-be1e-537b01743dc5"), new Guid("200f4f9e-dc4c-46aa-8cbc-3b6e4f835ee0") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("343165a7-985e-4f65-9f91-2cfd9332de34"), new Guid("29332f74-fdd1-42f7-ba79-fe44b45fa577") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("7aa83e66-eb4e-4bad-adca-564c964f679a"), new Guid("66d79753-14e3-42e1-98a6-1cf42988ac0e") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("8c333bcd-ebed-4f73-b651-c05bed8e98dc"), new Guid("f96e701c-cfd7-4163-8b5b-c2494a58b942") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("70c26879-2630-4ea3-ae1a-b124e96bf7c5"), new Guid("f1d66681-3fb5-4595-9b20-2100f499ce55") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("70c26879-2630-4ea3-ae1a-b124e96bf7c5"), new Guid("29332f74-fdd1-42f7-ba79-fe44b45fa577") });

            migrationBuilder.InsertData(
                table: "MenuItemPortions",
                columns: new[] { "MenuItemId", "PortionId" },
                values: new object[] { new Guid("70c26879-2630-4ea3-ae1a-b124e96bf7c5"), new Guid("f96e701c-cfd7-4163-8b5b-c2494a58b942") });

            migrationBuilder.CreateIndex(
                name: "IX_Food_Name",
                table: "Food",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemPortions_PortionId",
                table: "MenuItemPortions",
                column: "PortionId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuId",
                table: "MenuItems",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_Name",
                table: "Menus",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Portions_FoodId",
                table: "Portions",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItemPortions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Portions");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Food");
        }
    }
}
