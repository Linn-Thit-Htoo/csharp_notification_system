using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace notification_system.notification.Migrations;

/// <inheritdoc />
public partial class MigrateToNewVolume : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Tbl_Email_Templates",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                TemplateName = table.Column<string>(
                    type: "nvarchar(150)",
                    maxLength: 150,
                    nullable: false
                ),
                Subject = table.Column<string>(
                    type: "nvarchar(150)",
                    maxLength: 150,
                    nullable: false
                ),
                ContentType = table.Column<string>(
                    type: "nvarchar(10)",
                    maxLength: 10,
                    nullable: false,
                    comment: "Rich Text, HTML"
                ),
                BodyContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                CcEmailList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                BCcEmailList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(
                    type: "datetime",
                    nullable: false,
                    defaultValueSql: "(getdate())"
                ),
                UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tbl_Email_Templates", x => x.Id);
            }
        );

        migrationBuilder.CreateTable(
            name: "Tbl_Notification_Logs",
            columns: table => new
            {
                LogId = table.Column<string>(
                    type: "nvarchar(50)",
                    maxLength: 50,
                    nullable: false
                ),
                LogType = table.Column<string>(
                    type: "nvarchar(10)",
                    maxLength: 10,
                    nullable: false
                ),
                ToEmailList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CcEmailList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                BCcEmailList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ToPhoneList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatedAt = table.Column<DateTime>(
                    type: "datetime",
                    nullable: false,
                    defaultValueSql: "(getdate())"
                ),
                ResponseAt = table.Column<DateTime>(type: "datetime", nullable: true),
                IsSuccess = table.Column<bool>(type: "bit", nullable: true),
                ResponseMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tbl_Notification_Logs", x => x.LogId);
            }
        );

        migrationBuilder.CreateTable(
            name: "Tbl_Otp",
            columns: table => new
            {
                OtpId = table.Column<string>(
                    type: "nvarchar(50)",
                    maxLength: 50,
                    nullable: false
                ),
                OtpValue = table.Column<int>(type: "int", nullable: false),
                Email = table.Column<string>(
                    type: "nvarchar(50)",
                    maxLength: 50,
                    nullable: false
                ),
                CreatedAt = table.Column<DateTime>(
                    type: "datetime",
                    nullable: false,
                    defaultValueSql: "(getdate())"
                ),
                ExpiredAt = table.Column<DateTime>(type: "datetime", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tbl_Otp", x => x.OtpId);
            }
        );

        migrationBuilder.CreateTable(
            name: "Tbl_SMS_Templates",
            columns: table => new
            {
                Id = table.Column<string>(
                    type: "nchar(10)",
                    fixedLength: true,
                    maxLength: 10,
                    nullable: false
                ),
                BodyContent = table.Column<string>(
                    type: "nvarchar(300)",
                    maxLength: 300,
                    nullable: false
                ),
                IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                CreatedAt = table.Column<DateTime>(
                    type: "datetime",
                    nullable: false,
                    defaultValueSql: "(getdate())"
                ),
                UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                DeletedAt = table.Column<DateTime>(type: "datetime", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Tbl_SMS_Templates", x => x.Id);
            }
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Tbl_Email_Templates");

        migrationBuilder.DropTable(name: "Tbl_Notification_Logs");

        migrationBuilder.DropTable(name: "Tbl_Otp");

        migrationBuilder.DropTable(name: "Tbl_SMS_Templates");
    }
}
