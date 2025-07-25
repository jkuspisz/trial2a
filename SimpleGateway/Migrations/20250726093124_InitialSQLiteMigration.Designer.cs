﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleGateway.Models;
using SimpleGateway.Data;

#nullable disable

namespace SimpleGateway.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250726093124_InitialSQLiteMigration")]
    partial class InitialSQLiteMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.7");

            modelBuilder.Entity("SimpleGateway.Models.AssignmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AdvisorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Notes")
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<int>("PerformerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SupervisorId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AdvisorId");

                    b.HasIndex("PerformerId");

                    b.HasIndex("SupervisorId");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("SimpleGateway.Models.FileUploadEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("FileId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<long>("FileSize")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FileUploadModelId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UploadedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FileUploadModelId");

                    b.ToTable("FileUploadEntries");
                });

            modelBuilder.Entity("SimpleGateway.Models.FileUploadModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasIndemnityEvidence")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("FileUploads");
                });

            modelBuilder.Entity("SimpleGateway.Models.PerformerDetailsModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOfDentalQualification")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateOfUKRegistration")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("GDCNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("PracticeAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("PracticePostCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("SupportingDentist")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("SupportingDentistContactNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("UniversityCountryOfQualification")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("PerformerDetails");
                });

            modelBuilder.Entity("SimpleGateway.Models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Department")
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "John Smith",
                            Email = "john.smith@example.com",
                            FirstName = "John",
                            IsActive = true,
                            LastName = "Smith",
                            Password = "password123",
                            Role = "performer",
                            Username = "performer1"
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Jane Johnson",
                            Email = "jane.johnson@example.com",
                            FirstName = "Jane",
                            IsActive = true,
                            LastName = "Johnson",
                            Password = "password123",
                            Role = "performer",
                            Username = "performer2"
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Mike Wilson",
                            Email = "mike.wilson@example.com",
                            FirstName = "Mike",
                            IsActive = true,
                            LastName = "Wilson",
                            Password = "password123",
                            Role = "performer",
                            Username = "performer3"
                        },
                        new
                        {
                            Id = 4,
                            CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Dr. Sarah Davis",
                            Email = "sarah.davis@example.com",
                            FirstName = "Dr. Sarah",
                            IsActive = true,
                            LastName = "Davis",
                            Password = "password123",
                            Role = "advisor",
                            Username = "advisor1"
                        },
                        new
                        {
                            Id = 5,
                            CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Prof. Robert Brown",
                            Email = "robert.brown@example.com",
                            FirstName = "Prof. Robert",
                            IsActive = true,
                            LastName = "Brown",
                            Password = "password123",
                            Role = "supervisor",
                            Username = "supervisor1"
                        },
                        new
                        {
                            Id = 6,
                            CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Admin User",
                            Email = "admin@example.com",
                            FirstName = "Admin",
                            IsActive = true,
                            LastName = "User",
                            Password = "password123",
                            Role = "admin",
                            Username = "admin1"
                        },
                        new
                        {
                            Id = 7,
                            CreatedDate = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            DisplayName = "Super User",
                            Email = "superuser@example.com",
                            FirstName = "Super",
                            IsActive = true,
                            LastName = "User",
                            Password = "password123",
                            Role = "superuser",
                            Username = "superuser"
                        });
                });

            modelBuilder.Entity("SimpleGateway.Models.AssignmentModel", b =>
                {
                    b.HasOne("SimpleGateway.Models.UserModel", "Advisor")
                        .WithMany()
                        .HasForeignKey("AdvisorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("SimpleGateway.Models.UserModel", "Performer")
                        .WithMany()
                        .HasForeignKey("PerformerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimpleGateway.Models.UserModel", "Supervisor")
                        .WithMany()
                        .HasForeignKey("SupervisorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Advisor");

                    b.Navigation("Performer");

                    b.Navigation("Supervisor");
                });

            modelBuilder.Entity("SimpleGateway.Models.FileUploadEntry", b =>
                {
                    b.HasOne("SimpleGateway.Models.FileUploadModel", null)
                        .WithMany("UploadedFiles")
                        .HasForeignKey("FileUploadModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SimpleGateway.Models.FileUploadModel", b =>
                {
                    b.Navigation("UploadedFiles");
                });
#pragma warning restore 612, 618
        }
    }
}
