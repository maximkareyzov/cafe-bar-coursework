using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CafeBar.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "varchar", nullable: false),
                    Role = table.Column<string>(type: "varchar", nullable: false),
                    Login = table.Column<string>(type: "varchar", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar", nullable: false),
                    SalesPercent = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "Ingridients",
                columns: table => new
                {
                    IngridientID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar", nullable: false),
                    Unit = table.Column<string>(type: "varchar", nullable: false),
                    CurrentStock = table.Column<string>(type: "varchar", nullable: false),
                    MinStockThresold = table.Column<decimal>(type: "decimal", nullable: false),
                    AvgPurchasePrice = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingridients", x => x.IngridientID);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "varchar", nullable: false),
                    INN = table.Column<string>(type: "varchar", nullable: false),
                    ContactInfo = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.SupplierID);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    MenuItemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "varchar", nullable: false),
                    Price = table.Column<decimal>(type: "decimal", nullable: false),
                    Unit = table.Column<string>(type: "varchar", nullable: false),
                    Description = table.Column<string>(type: "varchar", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.MenuItemID);
                    table.ForeignKey(
                        name: "FK_MenuItems_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeID = table.Column<int>(type: "INTEGER", nullable: false),
                    Action = table.Column<string>(type: "varchar", nullable: false),
                    Details = table.Column<string>(type: "varchar", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeID = table.Column<int>(type: "INTEGER", nullable: false),
                    TableNumber = table.Column<string>(type: "varchar", nullable: true),
                    Status = table.Column<string>(type: "varchar", nullable: false),
                    PaymentType = table.Column<string>(type: "varchar", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "decimal", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    ShiftID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeID = table.Column<int>(type: "INTEGER", nullable: false),
                    ClockIn = table.Column<DateTime>(type: "datetime", nullable: false),
                    ClockOut = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.ShiftID);
                    table.ForeignKey(
                        name: "FK_Shifts_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryWriteOffs",
                columns: table => new
                {
                    WriteOffID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeID = table.Column<int>(type: "INTEGER", nullable: false),
                    IngridientID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal", nullable: false),
                    Reason = table.Column<string>(type: "varchar", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryWriteOffs", x => x.WriteOffID);
                    table.ForeignKey(
                        name: "FK_InventoryWriteOffs_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryWriteOffs_Ingridients_IngridientID",
                        column: x => x.IngridientID,
                        principalTable: "Ingridients",
                        principalColumn: "IngridientID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplyDocuments",
                columns: table => new
                {
                    SupplyDocumentID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SupplierID = table.Column<int>(type: "INTEGER", nullable: false),
                    EmployeeID = table.Column<int>(type: "INTEGER", nullable: false),
                    SupplyDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyDocuments", x => x.SupplyDocumentID);
                    table.ForeignKey(
                        name: "FK_SupplyDocuments_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplyDocuments_Suppliers_SupplierID",
                        column: x => x.SupplierID,
                        principalTable: "Suppliers",
                        principalColumn: "SupplierID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Modifiers",
                columns: table => new
                {
                    ModifierID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MenuItemID = table.Column<int>(type: "INTEGER", nullable: false),
                    GroupName = table.Column<string>(type: "varchar", nullable: false),
                    Name = table.Column<string>(type: "varchar", nullable: false),
                    PriceExtra = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modifiers", x => x.ModifierID);
                    table.ForeignKey(
                        name: "FK_Modifiers_MenuItems_MenuItemID",
                        column: x => x.MenuItemID,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecipeItems",
                columns: table => new
                {
                    RecipeItemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MenuItemID = table.Column<int>(type: "INTEGER", nullable: false),
                    IngridientID = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeItems", x => x.RecipeItemID);
                    table.ForeignKey(
                        name: "FK_RecipeItems_Ingridients_IngridientID",
                        column: x => x.IngridientID,
                        principalTable: "Ingridients",
                        principalColumn: "IngridientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeItems_MenuItems_MenuItemID",
                        column: x => x.MenuItemID,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderID = table.Column<int>(type: "INTEGER", nullable: false),
                    MenuItemID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemID);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuItems_MenuItemID",
                        column: x => x.MenuItemID,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplyItems",
                columns: table => new
                {
                    SupplyItemID = table.Column<string>(type: "varchar", nullable: false),
                    SupplyDocumentID = table.Column<int>(type: "INTEGER", nullable: false),
                    IngridientID = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplyItems", x => x.SupplyItemID);
                    table.ForeignKey(
                        name: "FK_SupplyItems_Ingridients_IngridientID",
                        column: x => x.IngridientID,
                        principalTable: "Ingridients",
                        principalColumn: "IngridientID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplyItems_SupplyDocuments_SupplyDocumentID",
                        column: x => x.SupplyDocumentID,
                        principalTable: "SupplyDocuments",
                        principalColumn: "SupplyDocumentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItemModifiers",
                columns: table => new
                {
                    OrderItemModifierID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderItemID = table.Column<int>(type: "INTEGER", nullable: false),
                    ModifierID = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceExtra = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItemModifiers", x => x.OrderItemModifierID);
                    table.ForeignKey(
                        name: "FK_OrderItemModifiers_Modifiers_ModifierID",
                        column: x => x.ModifierID,
                        principalTable: "Modifiers",
                        principalColumn: "ModifierID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItemModifiers_OrderItems_OrderItemID",
                        column: x => x.OrderItemID,
                        principalTable: "OrderItems",
                        principalColumn: "OrderItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EmployeeID",
                table: "AuditLogs",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryWriteOffs_EmployeeID",
                table: "InventoryWriteOffs",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryWriteOffs_IngridientID",
                table: "InventoryWriteOffs",
                column: "IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_CategoryID",
                table: "MenuItems",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_MenuItemID",
                table: "Modifiers",
                column: "MenuItemID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemModifiers_ModifierID",
                table: "OrderItemModifiers",
                column: "ModifierID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItemModifiers_OrderItemID",
                table: "OrderItemModifiers",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuItemID",
                table: "OrderItems",
                column: "MenuItemID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderID",
                table: "OrderItems",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeID",
                table: "Orders",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_IngridientID",
                table: "RecipeItems",
                column: "IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeItems_MenuItemID",
                table: "RecipeItems",
                column: "MenuItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_EmployeeID",
                table: "Shifts",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyDocuments_EmployeeID",
                table: "SupplyDocuments",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyDocuments_SupplierID",
                table: "SupplyDocuments",
                column: "SupplierID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyItems_IngridientID",
                table: "SupplyItems",
                column: "IngridientID");

            migrationBuilder.CreateIndex(
                name: "IX_SupplyItems_SupplyDocumentID",
                table: "SupplyItems",
                column: "SupplyDocumentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "InventoryWriteOffs");

            migrationBuilder.DropTable(
                name: "OrderItemModifiers");

            migrationBuilder.DropTable(
                name: "RecipeItems");

            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "SupplyItems");

            migrationBuilder.DropTable(
                name: "Modifiers");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Ingridients");

            migrationBuilder.DropTable(
                name: "SupplyDocuments");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
