using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Chnage.Migrations
{
    public partial class InitialProjectSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    iId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sName = table.Column<string>(nullable: true),
                    sEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.iId);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: true),
                    Action = table.Column<string>(maxLength: 50, nullable: false),
                    TableName = table.Column<string>(maxLength: 100, nullable: false),
                    EntityId = table.Column<string>(type: "varchar(100)", nullable: true),
                    ChangedColumns = table.Column<string>(type: "varchar(max)", nullable: true),
                    OldData = table.Column<string>(type: "varchar(max)", nullable: true),
                    NewData = table.Column<string>(type: "varchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UpdateWebAppLogs",
                columns: table => new
                {
                    iId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    sWebAppversion = table.Column<string>(nullable: true),
                    dtUploadDate = table.Column<DateTime>(nullable: false),
                    sReason = table.Column<string>(nullable: true),
                    sUserEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateWebAppLogs", x => x.iId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ECNs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModelName = table.Column<string>(nullable: false),
                    ModelNumber = table.Column<string>(nullable: false),
                    DateOfNotice = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ClosedDate = table.Column<DateTime>(nullable: true),
                    OriginatorId = table.Column<int>(nullable: false),
                    ChangeTypeId = table.Column<int>(nullable: false),
                    PTCRBResubmissionRequired = table.Column<bool>(nullable: false),
                    CurrentFirmwareVersion = table.Column<string>(nullable: false),
                    NewFirmwareVersion = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    ImpactMissingReqApprovalDate = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    RejectReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECNs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECNs_RequestTypes_ChangeTypeId",
                        column: x => x.ChangeTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ECNs_Users_OriginatorId",
                        column: x => x.OriginatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ECOs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PermanentChange = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ReasonForChange = table.Column<string>(nullable: true),
                    ChangeTypeId = table.Column<int>(nullable: false),
                    BOMRequired = table.Column<bool>(nullable: false),
                    ProductValidationTestingRequired = table.Column<bool>(nullable: false),
                    PlannedImplementationDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ClosedDate = table.Column<DateTime>(nullable: true),
                    CustomerApproval = table.Column<bool>(nullable: false),
                    PriorityLevel = table.Column<int>(nullable: false),
                    OriginatorId = table.Column<int>(nullable: false),
                    ImplementationType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    NotesForApprover = table.Column<string>(maxLength: 4000, nullable: true),
                    NotesForValidator = table.Column<string>(maxLength: 4000, nullable: true),
                    PreviousRevision = table.Column<string>(nullable: true),
                    NewRevision = table.Column<string>(nullable: true),
                    LinkUrls = table.Column<string>(maxLength: 4000, nullable: true),
                    RejectReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECOs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECOs_RequestTypes_ChangeTypeId",
                        column: x => x.ChangeTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ECOs_Users_OriginatorId",
                        column: x => x.OriginatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ECRs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PermanentChange = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ReasonForChange = table.Column<string>(nullable: true),
                    ChangeTypeId = table.Column<int>(nullable: false),
                    BOMRequired = table.Column<bool>(nullable: false),
                    ProductValidationTestingRequired = table.Column<bool>(nullable: false),
                    PlannedImplementationDate = table.Column<DateTime>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ClosedDate = table.Column<DateTime>(nullable: true),
                    CustomerImpact = table.Column<bool>(nullable: false),
                    PriorityLevel = table.Column<int>(nullable: false),
                    OriginatorId = table.Column<int>(nullable: false),
                    ImplementationType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ECOsCompleted = table.Column<bool>(nullable: false),
                    PreviousRevision = table.Column<string>(nullable: true),
                    NewRevision = table.Column<string>(nullable: true),
                    LinkUrls = table.Column<string>(maxLength: 4000, nullable: true),
                    RejectReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECRs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ECRs_RequestTypes_ChangeTypeId",
                        column: x => x.ChangeTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ECRs_Users_OriginatorId",
                        column: x => x.OriginatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    RequestTypeId = table.Column<int>(nullable: false),
                    RoleInt = table.Column<int>(nullable: false),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_RequestTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ECNHasECOs",
                columns: table => new
                {
                    ECOId = table.Column<int>(nullable: false),
                    ECNId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECNHasECOs", x => new { x.ECOId, x.ECNId });
                    table.ForeignKey(
                        name: "FK_ECNHasECOs_ECNs_ECNId",
                        column: x => x.ECNId,
                        principalTable: "ECNs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ECNHasECOs_ECOs_ECOId",
                        column: x => x.ECOId,
                        principalTable: "ECOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductECOs",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    ECOId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductECOs", x => new { x.ProductId, x.ECOId });
                    table.ForeignKey(
                        name: "FK_ProductECOs_ECOs_ECOId",
                        column: x => x.ECOId,
                        principalTable: "ECOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductECOs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestTypeECOs",
                columns: table => new
                {
                    RequestTypeId = table.Column<int>(nullable: false),
                    ECOId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypeECOs", x => new { x.RequestTypeId, x.ECOId });
                    table.ForeignKey(
                        name: "FK_RequestTypeECOs_ECOs_ECOId",
                        column: x => x.ECOId,
                        principalTable: "ECOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestTypeECOs_RequestTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChangeUserId = table.Column<int>(nullable: false),
                    ChangeDateTime = table.Column<DateTime>(nullable: false),
                    PermanentChange = table.Column<bool>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ReasonForChange = table.Column<string>(nullable: true),
                    ChangeTypeId = table.Column<int>(nullable: true),
                    BOMRequired = table.Column<bool>(nullable: true),
                    ProductValidationTestingRequired = table.Column<bool>(nullable: true),
                    PlannedImplementationDate = table.Column<DateTime>(nullable: true),
                    ClosedDate = table.Column<DateTime>(nullable: true),
                    CustomerImpact = table.Column<bool>(nullable: true),
                    CustomerApproval = table.Column<bool>(nullable: true),
                    PriorityLevel = table.Column<int>(nullable: false),
                    ImplementationType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    PreviousRevision = table.Column<string>(nullable: true),
                    NewRevision = table.Column<string>(nullable: true),
                    LinkUrls = table.Column<string>(maxLength: 4000, nullable: true),
                    NotesForApprovers = table.Column<string>(nullable: true),
                    NotesForValidators = table.Column<string>(nullable: true),
                    AffectedProducts = table.Column<string>(nullable: true),
                    AreasAffected = table.Column<string>(nullable: true),
                    RelatedECOs = table.Column<string>(nullable: true),
                    RelatedECRs = table.Column<string>(nullable: true),
                    Approvers = table.Column<string>(nullable: true),
                    Notifications = table.Column<string>(nullable: true),
                    ECRId = table.Column<int>(nullable: true),
                    ECOId = table.Column<int>(nullable: true),
                    ECNId = table.Column<int>(nullable: true),
                    ModelName = table.Column<string>(nullable: true),
                    ModelNumber = table.Column<string>(nullable: true),
                    DateOfNotice = table.Column<DateTime>(nullable: true),
                    PTCRBResubmissionRequired = table.Column<bool>(nullable: true),
                    CurrentFirmwareVersion = table.Column<string>(nullable: true),
                    NewFirmwareVersion = table.Column<string>(nullable: true),
                    ImpactMissingReqApprovalDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditLogs_RequestTypes_ChangeTypeId",
                        column: x => x.ChangeTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditLogs_Users_ChangeUserId",
                        column: x => x.ChangeUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditLogs_ECNs_ECNId",
                        column: x => x.ECNId,
                        principalTable: "ECNs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditLogs_ECOs_ECOId",
                        column: x => x.ECOId,
                        principalTable: "ECOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AuditLogs_ECRs_ECRId",
                        column: x => x.ECRId,
                        principalTable: "ECRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ECRHasECOs",
                columns: table => new
                {
                    ECRId = table.Column<int>(nullable: false),
                    ECOId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ECRHasECOs", x => new { x.ECRId, x.ECOId });
                    table.ForeignKey(
                        name: "FK_ECRHasECOs_ECOs_ECOId",
                        column: x => x.ECOId,
                        principalTable: "ECOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ECRHasECOs_ECRs_ECRId",
                        column: x => x.ECRId,
                        principalTable: "ECRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    ECRId = table.Column<int>(nullable: true),
                    ECOId = table.Column<int>(nullable: true),
                    ECNId = table.Column<int>(nullable: true),
                    Option = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_ECNs_ECNId",
                        column: x => x.ECNId,
                        principalTable: "ECNs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_ECOs_ECOId",
                        column: x => x.ECOId,
                        principalTable: "ECOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_ECRs_ECRId",
                        column: x => x.ECRId,
                        principalTable: "ECRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductECRs",
                columns: table => new
                {
                    ProductId = table.Column<int>(nullable: false),
                    ECRId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductECRs", x => new { x.ProductId, x.ECRId });
                    table.ForeignKey(
                        name: "FK_ProductECRs_ECRs_ECRId",
                        column: x => x.ECRId,
                        principalTable: "ECRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductECRs_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestTypeECRs",
                columns: table => new
                {
                    RequestTypeId = table.Column<int>(nullable: false),
                    ECRId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypeECRs", x => new { x.RequestTypeId, x.ECRId });
                    table.ForeignKey(
                        name: "FK_RequestTypeECRs_ECRs_ECRId",
                        column: x => x.ECRId,
                        principalTable: "ECRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestTypeECRs_RequestTypes_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleECNs",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(nullable: false),
                    ECNId = table.Column<int>(nullable: false),
                    Approval = table.Column<bool>(nullable: true),
                    AprovedDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleECNs", x => new { x.UserRoleId, x.ECNId });
                    table.ForeignKey(
                        name: "FK_UserRoleECNs_ECNs_ECNId",
                        column: x => x.ECNId,
                        principalTable: "ECNs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoleECNs_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleECOs",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(nullable: false),
                    ECOId = table.Column<int>(nullable: false),
                    Approval = table.Column<bool>(nullable: true),
                    AprovedDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleECOs", x => new { x.UserRoleId, x.ECOId });
                    table.ForeignKey(
                        name: "FK_UserRoleECOs_ECOs_ECOId",
                        column: x => x.ECOId,
                        principalTable: "ECOs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoleECOs_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleECRs",
                columns: table => new
                {
                    UserRoleId = table.Column<int>(nullable: false),
                    ECRId = table.Column<int>(nullable: false),
                    Approval = table.Column<bool>(nullable: true),
                    AprovedDate = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleECRs", x => new { x.UserRoleId, x.ECRId });
                    table.ForeignKey(
                        name: "FK_UserRoleECRs_ECRs_ECRId",
                        column: x => x.ECRId,
                        principalTable: "ECRs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoleECRs_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ChangeTypeId",
                table: "AuditLogs",
                column: "ChangeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ChangeUserId",
                table: "AuditLogs",
                column: "ChangeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ECNId",
                table: "AuditLogs",
                column: "ECNId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ECOId",
                table: "AuditLogs",
                column: "ECOId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_ECRId",
                table: "AuditLogs",
                column: "ECRId");

            migrationBuilder.CreateIndex(
                name: "IX_ECNHasECOs_ECNId",
                table: "ECNHasECOs",
                column: "ECNId");

            migrationBuilder.CreateIndex(
                name: "IX_ECNs_ChangeTypeId",
                table: "ECNs",
                column: "ChangeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ECNs_OriginatorId",
                table: "ECNs",
                column: "OriginatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ECOs_ChangeTypeId",
                table: "ECOs",
                column: "ChangeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ECOs_OriginatorId",
                table: "ECOs",
                column: "OriginatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ECRHasECOs_ECOId",
                table: "ECRHasECOs",
                column: "ECOId");

            migrationBuilder.CreateIndex(
                name: "IX_ECRs_ChangeTypeId",
                table: "ECRs",
                column: "ChangeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ECRs_OriginatorId",
                table: "ECRs",
                column: "OriginatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ECNId",
                table: "Notifications",
                column: "ECNId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ECOId",
                table: "Notifications",
                column: "ECOId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ECRId",
                table: "Notifications",
                column: "ECRId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductECOs_ECOId",
                table: "ProductECOs",
                column: "ECOId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductECRs_ECRId",
                table: "ProductECRs",
                column: "ECRId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTypeECOs_ECOId",
                table: "RequestTypeECOs",
                column: "ECOId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTypeECRs_ECRId",
                table: "RequestTypeECRs",
                column: "ECRId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleECNs_ECNId",
                table: "UserRoleECNs",
                column: "ECNId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleECOs_ECOId",
                table: "UserRoleECOs",
                column: "ECOId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleECRs_ECRId",
                table: "UserRoleECRs",
                column: "ECRId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RequestTypeId",
                table: "UserRoles",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Audits");

            migrationBuilder.DropTable(
                name: "ECNHasECOs");

            migrationBuilder.DropTable(
                name: "ECRHasECOs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ProductECOs");

            migrationBuilder.DropTable(
                name: "ProductECRs");

            migrationBuilder.DropTable(
                name: "RequestTypeECOs");

            migrationBuilder.DropTable(
                name: "RequestTypeECRs");

            migrationBuilder.DropTable(
                name: "UpdateWebAppLogs");

            migrationBuilder.DropTable(
                name: "UserRoleECNs");

            migrationBuilder.DropTable(
                name: "UserRoleECOs");

            migrationBuilder.DropTable(
                name: "UserRoleECRs");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ECNs");

            migrationBuilder.DropTable(
                name: "ECOs");

            migrationBuilder.DropTable(
                name: "ECRs");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "RequestTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
