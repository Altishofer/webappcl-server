﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ToX.Models;

#nullable disable

namespace ToX.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230930072428_createSchema")]
    partial class createSchema
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ToX.Models.TodoItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsComplete")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("todoitems");
                });

            modelBuilder.Entity("ToX.Models.User", b =>
                {
                    b.Property<long>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("userId"));

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("userPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("userId");

                    b.HasIndex("token")
                        .IsUnique();

                    b.HasIndex("userId")
                        .IsUnique();

                    b.HasIndex("userName")
                        .IsUnique();

                    b.ToTable("user");
                });
#pragma warning restore 612, 618
        }
    }
}
