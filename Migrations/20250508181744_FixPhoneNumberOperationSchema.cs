using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITBaza.Migrations
{
    /// <inheritdoc />
    public partial class FixPhoneNumberOperationSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileOperator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Corporation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IdCountry = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileOperator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MobileOperator_Country",
                        column: x => x.IdCountry,
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Placement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Office = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Placement_Country",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Division",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Division", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Division_Department",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    InfoAccessRights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastPasswordChangeDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemUser_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OperatorId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SimSerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Pin1 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Pin2 = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Puk1 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Puk2 = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumber_MobileOperator",
                        column: x => x.OperatorId,
                        principalTable: "MobileOperator",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vacation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vacation_Department",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vacation_Division",
                        column: x => x.DivisionId,
                        principalTable: "Division",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DismissalDate = table.Column<DateOnly>(type: "date", nullable: true),
                    PersonalPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VacationId = table.Column<int>(type: "int", nullable: true),
                    WorkType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PlacementId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Placement",
                        column: x => x.PlacementId,
                        principalTable: "Placement",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Person_Vacation",
                        column: x => x.VacationId,
                        principalTable: "Vacation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumberOperation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumberId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExecutorId = table.Column<int>(type: "int", nullable: false),
                    //RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumberOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumberOperation_Person",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PhoneNumberOperation_PhoneNumber",
                        column: x => x.PhoneNumberId,
                        principalTable: "PhoneNumber",
                        principalColumn: "Id");
                    //table.ForeignKey(
                    //    name: "FK_PhoneNumberOperation_Role_RoleId",
                    //    column: x => x.RoleId,
                    //    principalTable: "Role",
                    //    principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PhoneNumberOperation_SystemUser",
                        column: x => x.ExecutorId,
                        principalTable: "SystemUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Resource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AltLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ResponsiblePersonId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resource_Person",
                        column: x => x.ResponsiblePersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Resource_ResourceType",
                        column: x => x.ResourceTypeId,
                        principalTable: "ResourceType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ResourceRole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceRole_Resource",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceId = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ResourceRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Access_Resource",
                        column: x => x.ResourceId,
                        principalTable: "Resource",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Access_ResourceRole",
                        column: x => x.ResourceRoleId,
                        principalTable: "ResourceRole",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccessOperation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccessId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ExecutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessOperation_Access",
                        column: x => x.AccessId,
                        principalTable: "Access",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccessOperation_Person",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AccessOperation_SystemUser",
                        column: x => x.ExecutorId,
                        principalTable: "SystemUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Access_ResourceId",
                table: "Access",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Access_ResourceRoleId",
                table: "Access",
                column: "ResourceRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessOperation_AccessId",
                table: "AccessOperation",
                column: "AccessId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessOperation_ExecutorId",
                table: "AccessOperation",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessOperation_PersonId",
                table: "AccessOperation",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Division_DepartmentId",
                table: "Division",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MobileOperator_IdCountry",
                table: "MobileOperator",
                column: "IdCountry");

            migrationBuilder.CreateIndex(
                name: "IX_Person_PlacementId",
                table: "Person",
                column: "PlacementId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_VacationId",
                table: "Person",
                column: "VacationId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumber_OperatorId",
                table: "PhoneNumber",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumberOperation_ExecutorId",
                table: "PhoneNumberOperation",
                column: "ExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumberOperation_PersonId",
                table: "PhoneNumberOperation",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumberOperation_PhoneNumberId",
                table: "PhoneNumberOperation",
                column: "PhoneNumberId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumberOperation_RoleId",
                table: "PhoneNumberOperation",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Placement_CountryId",
                table: "Placement",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_ResourceTypeId",
                table: "Resource",
                column: "ResourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Resource_ResponsiblePersonId",
                table: "Resource",
                column: "ResponsiblePersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceRole_ResourceId",
                table: "ResourceRole",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemUser_RoleId",
                table: "SystemUser",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UQ__SystemUs__5E55825BAEE17D73",
                table: "SystemUser",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_DepartmentId",
                table: "Vacation",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacation_DivisionId",
                table: "Vacation",
                column: "DivisionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessOperation");

            migrationBuilder.DropTable(
                name: "PhoneNumberOperation");

            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.DropTable(
                name: "PhoneNumber");

            migrationBuilder.DropTable(
                name: "SystemUser");

            migrationBuilder.DropTable(
                name: "ResourceRole");

            migrationBuilder.DropTable(
                name: "MobileOperator");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Resource");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "ResourceType");

            migrationBuilder.DropTable(
                name: "Placement");

            migrationBuilder.DropTable(
                name: "Vacation");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Division");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
