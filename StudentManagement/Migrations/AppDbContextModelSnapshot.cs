using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StudentManagement.Data;

#nullable disable

namespace StudentManagement.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        Microsoft.EntityFrameworkCore.SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("StudentManagement.Models.Student", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");
            Microsoft.EntityFrameworkCore.SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<int>("Age").HasColumnType("int");
            b.Property<string>("Course").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");
            b.Property<DateTime>("CreatedDate").HasColumnType("datetime2");
            b.Property<string>("Email").IsRequired().HasMaxLength(150).HasColumnType("nvarchar(150)");
            b.Property<string>("Name").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");

            b.HasKey("Id");
            b.HasIndex("Email").IsUnique();
            b.ToTable("Students");
        });

        modelBuilder.Entity("StudentManagement.Models.User", b =>
        {
            b.Property<int>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("int");
            Microsoft.EntityFrameworkCore.SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

            b.Property<string>("PasswordHash").IsRequired().HasColumnType("nvarchar(max)");
            b.Property<string>("Role").IsRequired().HasMaxLength(50).HasColumnType("nvarchar(50)").HasDefaultValue("User");
            b.Property<string>("Username").IsRequired().HasMaxLength(100).HasColumnType("nvarchar(100)");

            b.HasKey("Id");
            b.HasIndex("Username").IsUnique();
            b.ToTable("Users");
        });
#pragma warning restore 612, 618
    }
}
