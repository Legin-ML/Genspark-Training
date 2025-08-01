using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShopMigrationAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    categoryid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.categoryid);
                });

            migrationBuilder.CreateTable(
                name: "color",
                columns: table => new
                {
                    colorid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_color", x => x.colorid);
                });

            migrationBuilder.CreateTable(
                name: "contactus",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    content = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_contactus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "model",
                columns: table => new
                {
                    modelid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_model", x => x.modelid);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    orderid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ordername = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    orderdate = table.Column<DateOnly>(type: "date", nullable: true),
                    paymenttype = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    customername = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    customerphone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    customeremail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    customeraddress = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.orderid);
                });

            migrationBuilder.CreateTable(
                name: "storage",
                columns: table => new
                {
                    storageid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    storage = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_storage", x => x.storageid);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    userid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    password = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "news",
                columns: table => new
                {
                    newsid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    shortdescription = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    image = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    createddate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_news", x => x.newsid);
                    table.ForeignKey(
                        name: "fk_news_user",
                        column: x => x.userid,
                        principalTable: "user",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    productid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productname = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    image = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    price = table.Column<double>(type: "double precision", nullable: true),
                    userid = table.Column<int>(type: "integer", nullable: true),
                    categoryid = table.Column<int>(type: "integer", nullable: true),
                    colorid = table.Column<int>(type: "integer", nullable: true),
                    modelid = table.Column<int>(type: "integer", nullable: true),
                    storageid = table.Column<int>(type: "integer", nullable: true),
                    sellstartdate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true),
                    sellenddate = table.Column<DateTime>(type: "timestamp(3) without time zone", nullable: true),
                    isnew = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.productid);
                    table.ForeignKey(
                        name: "fk_product_category",
                        column: x => x.categoryid,
                        principalTable: "category",
                        principalColumn: "categoryid");
                    table.ForeignKey(
                        name: "fk_product_color",
                        column: x => x.colorid,
                        principalTable: "color",
                        principalColumn: "colorid");
                    table.ForeignKey(
                        name: "fk_product_model",
                        column: x => x.modelid,
                        principalTable: "model",
                        principalColumn: "modelid");
                    table.ForeignKey(
                        name: "fk_product_storage",
                        column: x => x.storageid,
                        principalTable: "storage",
                        principalColumn: "storageid");
                    table.ForeignKey(
                        name: "fk_product_user",
                        column: x => x.userid,
                        principalTable: "user",
                        principalColumn: "userid");
                });

            migrationBuilder.CreateTable(
                name: "orderdetail",
                columns: table => new
                {
                    orderid = table.Column<int>(type: "integer", nullable: false),
                    productid = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<double>(type: "double precision", nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orderdetail", x => new { x.orderid, x.productid });
                    table.ForeignKey(
                        name: "fk_orderdetail_order",
                        column: x => x.orderid,
                        principalTable: "order",
                        principalColumn: "orderid");
                    table.ForeignKey(
                        name: "fk_orderdetail_product",
                        column: x => x.productid,
                        principalTable: "product",
                        principalColumn: "productid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_news_userid",
                table: "news",
                column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_orderdetail_productid",
                table: "orderdetail",
                column: "productid");

            migrationBuilder.CreateIndex(
                name: "IX_product_categoryid",
                table: "product",
                column: "categoryid");

            migrationBuilder.CreateIndex(
                name: "IX_product_colorid",
                table: "product",
                column: "colorid");

            migrationBuilder.CreateIndex(
                name: "IX_product_modelid",
                table: "product",
                column: "modelid");

            migrationBuilder.CreateIndex(
                name: "IX_product_storageid",
                table: "product",
                column: "storageid");

            migrationBuilder.CreateIndex(
                name: "IX_product_userid",
                table: "product",
                column: "userid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contactus");

            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "orderdetail");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "color");

            migrationBuilder.DropTable(
                name: "model");

            migrationBuilder.DropTable(
                name: "storage");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
