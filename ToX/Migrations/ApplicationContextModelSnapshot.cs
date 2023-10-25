﻿// <auto-generated />
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ToX.Models;

#nullable disable

namespace ToX.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ToX.Models.Answer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<List<string>>("Additions")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("additions");

                    b.Property<List<string>>("AnswerTarget")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("answertarget");

                    b.Property<double>("Distance")
                        .HasColumnType("double precision")
                        .HasColumnName("distance");

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("playername");

                    b.Property<long>("Points")
                        .HasColumnType("bigint")
                        .HasColumnName("points");

                    b.Property<long>("QuizId")
                        .HasColumnType("bigint")
                        .HasColumnName("quizid");

                    b.Property<long>("RoundId")
                        .HasColumnType("bigint")
                        .HasColumnName("roundid");

                    b.Property<List<string>>("Subtractions")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("subtractions");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("answer");
                });

            modelBuilder.Entity("ToX.Models.Host", b =>
                {
                    b.Property<long>("hostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("hostid");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("hostId"));

                    b.Property<string>("hostName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hostname");

                    b.Property<string>("hostPassword")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("hostpassword");

                    b.HasKey("hostId");

                    b.HasIndex("hostId")
                        .IsUnique();

                    b.ToTable("host");
                });

            modelBuilder.Entity("ToX.Models.Player", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("PlayerName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("playername");

                    b.Property<long>("QuizId")
                        .HasColumnType("bigint")
                        .HasColumnName("quizid");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("player");
                });

            modelBuilder.Entity("ToX.Models.Quiz", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("HostId")
                        .HasColumnType("bigint")
                        .HasColumnName("hostid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("quiz");
                });

            modelBuilder.Entity("ToX.Models.Round", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<List<string>>("ForbiddenWords")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("forbiddenwords");

                    b.Property<long>("QuizId")
                        .HasColumnType("bigint")
                        .HasColumnName("quizid");

                    b.Property<string>("RoundTarget")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("roundtarget");

                    b.Property<float[]>("RoundTargetVector")
                        .IsRequired()
                        .HasColumnType("real[]")
                        .HasColumnName("roundtargetvector");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("round");
                });

            modelBuilder.Entity("ToX.Models.TodoItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsComplete")
                        .HasColumnType("boolean")
                        .HasColumnName("iscomplete");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("todoitems");
                });

            modelBuilder.Entity("ToX.Models.WordVector", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("MetricLength")
                        .HasColumnType("double precision")
                        .HasColumnName("metriclength");

                    b.Property<float[]>("NumericVector")
                        .IsRequired()
                        .HasColumnType("real[]")
                        .HasColumnName("numericvector");

                    b.Property<string>("WordOrNull")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("wordornull");

                    b.HasKey("Id");

                    b.HasIndex("WordOrNull")
                        .IsUnique();

                    b.ToTable("wordvector");
                });
#pragma warning restore 612, 618
        }
    }
}
