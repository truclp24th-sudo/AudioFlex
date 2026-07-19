using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioFlex.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* 
               GHI CHÚ: Các lệnh AlterColumn và AddColumn cho bảng Orders dưới đây 
               đã được vô hiệu hóa vì các cột này đã tồn tại trong Database của bạn.
            */

            /*
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
            */

            // CHỈ THỰC HIỆN LỆNH NÀY: Thêm cột phản hồi của Admin vào bảng Contacts
            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Tương tự hàm Up, chúng ta bỏ qua việc xóa cột Note ở đây nếu nó đã có từ trước
            /*
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Orders");
            */

            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "Contacts");

            /*
            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Orders",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);
            */
        }
    }
}