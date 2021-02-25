﻿// <auto-generated />
using System;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Infrastructure.Migrations
{
    [DbContext(typeof(FitwebContext))]
    [Migration("20210223210747_IndexOnUserNameAndEmail")]
    partial class IndexOnUserNameAndEmail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend.Core.Entities.Athlete", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Athletes");
                });

            modelBuilder.Entity("Backend.Core.Entities.AthleteExercise", b =>
                {
                    b.Property<int>("AthleteId")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<int>("DayId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfReps")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfSets")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("AthleteId", "ExerciseId");

                    b.HasIndex("DayId");

                    b.HasIndex("ExerciseId");

                    b.ToTable("AthleteExercises");
                });

            modelBuilder.Entity("Backend.Core.Entities.AthleteProduct", b =>
                {
                    b.Property<int>("AthleteId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("AthleteId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("AthleteProducts");
                });

            modelBuilder.Entity("Backend.Core.Entities.CategoryOfProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CategoriesOfProduct");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Meat"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Fruits"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Vegetables"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Dairy_and_eggs"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Fish_and_seafood"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Bread"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Drinks"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Grain_Products"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Sweets"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Sugar_and_spices"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Supplements"
                        });
                });

            modelBuilder.Entity("Backend.Core.Entities.Day", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Days");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Monday"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Tuesday"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Wednesday"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Thursday"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Friday"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Saturday"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Sunday"
                        });
                });

            modelBuilder.Entity("Backend.Core.Entities.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("PartOfBodyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PartOfBodyId");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("Backend.Core.Entities.PartOfBody", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PartOfBodies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Chest"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Legs"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Biceps"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Triceps"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Shoulders"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Abdominals"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Back"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Forearm"
                        });
                });

            modelBuilder.Entity("Backend.Core.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Calories")
                        .HasColumnType("float");

                    b.Property<double>("Carbohydrates")
                        .HasColumnType("float");

                    b.Property<int>("CategoryOfProductId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<double>("Fats")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("Proteins")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CategoryOfProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Backend.Core.Entities.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Backend.Core.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            Name = "User"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Instructor"
                        });
                });

            modelBuilder.Entity("Backend.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateUpdated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsExternalLoginProvider")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.Core.Entities.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Backend.Core.Entities.Athlete", b =>
                {
                    b.HasOne("Backend.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Core.Entities.AthleteExercise", b =>
                {
                    b.HasOne("Backend.Core.Entities.Athlete", "Athlete")
                        .WithMany("AthleteExercises")
                        .HasForeignKey("AthleteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Core.Entities.Day", "Day")
                        .WithMany()
                        .HasForeignKey("DayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Core.Entities.Exercise", "Exercise")
                        .WithMany("AthleteExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Athlete");

                    b.Navigation("Day");

                    b.Navigation("Exercise");
                });

            modelBuilder.Entity("Backend.Core.Entities.AthleteProduct", b =>
                {
                    b.HasOne("Backend.Core.Entities.Athlete", "Athlete")
                        .WithMany("AthleteProducts")
                        .HasForeignKey("AthleteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Core.Entities.Product", "Product")
                        .WithMany("AthleteProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Athlete");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Backend.Core.Entities.Exercise", b =>
                {
                    b.HasOne("Backend.Core.Entities.PartOfBody", "PartOfBody")
                        .WithMany()
                        .HasForeignKey("PartOfBodyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PartOfBody");
                });

            modelBuilder.Entity("Backend.Core.Entities.Product", b =>
                {
                    b.HasOne("Backend.Core.Entities.CategoryOfProduct", "CategoryOfProduct")
                        .WithMany()
                        .HasForeignKey("CategoryOfProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CategoryOfProduct");
                });

            modelBuilder.Entity("Backend.Core.Entities.UserRole", b =>
                {
                    b.HasOne("Backend.Core.Entities.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Core.Entities.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Core.Entities.Athlete", b =>
                {
                    b.Navigation("AthleteExercises");

                    b.Navigation("AthleteProducts");
                });

            modelBuilder.Entity("Backend.Core.Entities.Exercise", b =>
                {
                    b.Navigation("AthleteExercises");
                });

            modelBuilder.Entity("Backend.Core.Entities.Product", b =>
                {
                    b.Navigation("AthleteProducts");
                });

            modelBuilder.Entity("Backend.Core.Entities.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("Backend.Core.Entities.User", b =>
                {
                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
