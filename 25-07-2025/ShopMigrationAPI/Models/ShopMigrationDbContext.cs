using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShopMigrationAPI.Models;

public partial class ShopMigrationDbContext : DbContext
{
    public ShopMigrationDbContext()
    {
    }

    public ShopMigrationDbContext(DbContextOptions<ShopMigrationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Model> Models { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Storage> Storages { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("pk_category");

            entity.ToTable("category");

            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.Colorid).HasName("pk_color");

            entity.ToTable("color");

            entity.Property(e => e.Colorid).HasColumnName("colorid");
            entity.Property(e => e.Color1)
                .HasMaxLength(50)
                .HasColumnName("color");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_contactus");

            entity.ToTable("contactus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content)
                .HasMaxLength(2000)
                .HasColumnName("content");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Model>(entity =>
        {
            entity.HasKey(e => e.Modelid).HasName("pk_model");

            entity.ToTable("model");

            entity.Property(e => e.Modelid).HasColumnName("modelid");
            entity.Property(e => e.Model1)
                .HasMaxLength(50)
                .HasColumnName("model");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Newsid).HasName("pk_news");

            entity.ToTable("news");

            entity.Property(e => e.Newsid).HasColumnName("newsid");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Createddate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createddate");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .HasColumnName("image");
            entity.Property(e => e.Shortdescription)
                .HasMaxLength(500)
                .HasColumnName("shortdescription");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .HasColumnName("title");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.News)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_news_user");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("pk_order");

            entity.ToTable("order");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Customeraddress)
                .HasMaxLength(250)
                .HasColumnName("customeraddress");
            entity.Property(e => e.Customeremail)
                .HasMaxLength(100)
                .HasColumnName("customeremail");
            entity.Property(e => e.Customername)
                .HasMaxLength(50)
                .HasColumnName("customername");
            entity.Property(e => e.Customerphone)
                .HasMaxLength(15)
                .HasColumnName("customerphone");
            entity.Property(e => e.Orderdate).HasColumnName("orderdate");
            entity.Property(e => e.Ordername)
                .HasMaxLength(50)
                .HasColumnName("ordername");
            entity.Property(e => e.Paymenttype)
                .HasMaxLength(50)
                .HasColumnName("paymenttype");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => new { e.Orderid, e.Productid }).HasName("pk_orderdetail");

            entity.ToTable("orderdetail");

            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderdetail_order");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_orderdetail_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("pk_product");

            entity.ToTable("product");

            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Colorid).HasColumnName("colorid");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .HasColumnName("image");
            entity.Property(e => e.Isnew).HasColumnName("isnew");
            entity.Property(e => e.Modelid).HasColumnName("modelid");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Productname)
                .HasMaxLength(250)
                .HasColumnName("productname");
            entity.Property(e => e.Sellenddate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("sellenddate");
            entity.Property(e => e.Sellstartdate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("sellstartdate");
            entity.Property(e => e.Storageid).HasColumnName("storageid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("fk_product_category");

            entity.HasOne(d => d.Color).WithMany(p => p.Products)
                .HasForeignKey(d => d.Colorid)
                .HasConstraintName("fk_product_color");

            entity.HasOne(d => d.Model).WithMany(p => p.Products)
                .HasForeignKey(d => d.Modelid)
                .HasConstraintName("fk_product_model");

            entity.HasOne(d => d.Storage).WithMany(p => p.Products)
                .HasForeignKey(d => d.Storageid)
                .HasConstraintName("fk_product_storage");

            entity.HasOne(d => d.User).WithMany(p => p.Products)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_product_user");
        });

        modelBuilder.Entity<Storage>(entity =>
        {
            entity.HasKey(e => e.Storageid).HasName("pk_storage");

            entity.ToTable("storage");

            entity.Property(e => e.Storageid).HasColumnName("storageid");
            entity.Property(e => e.Storage1)
                .HasMaxLength(50)
                .HasColumnName("storage");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("pk_user");

            entity.ToTable("user");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
