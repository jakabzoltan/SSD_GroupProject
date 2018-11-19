using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GroupProject.Data.Migrations
{
    public partial class creditcardcvc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "CreditCard",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "CvcCode",
                table: "CreditCard",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CreditCard",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreditCard_UserId",
                table: "CreditCard",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCard_AspNetUsers_UserId",
                table: "CreditCard",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreditCard_AspNetUsers_UserId",
                table: "CreditCard");

            migrationBuilder.DropIndex(
                name: "IX_CreditCard_UserId",
                table: "CreditCard");

            migrationBuilder.DropColumn(
                name: "CvcCode",
                table: "CreditCard");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CreditCard");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "CreditCard",
                nullable: false,
                oldClrType: typeof(Guid))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}
