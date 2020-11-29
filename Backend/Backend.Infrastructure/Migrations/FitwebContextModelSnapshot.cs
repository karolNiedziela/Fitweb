﻿// <auto-generated />
using System;
using Backend.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Infrastructure.Migrations
{
    [DbContext(typeof(FitwebContext))]
    partial class FitwebContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend.Core.Domain.Day", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Days");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Name = "Monday"
                        },
                        new
                        {
                            Id = 1,
                            Name = "Tuesday"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Wendnesday"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Thursday"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Friday"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Saturday"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Sunday"
                        });
                });

            modelBuilder.Entity("Backend.Core.Domain.DietGoals", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Carbohydrates")
                        .HasColumnType("float");

                    b.Property<double>("Fats")
                        .HasColumnType("float");

                    b.Property<double>("Proteins")
                        .HasColumnType("float");

                    b.Property<double>("TotalCalories")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DietGoals");
                });

            modelBuilder.Entity("Backend.Core.Domain.Exercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PartOfBody")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Exercises");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Crunch",
                            PartOfBody = "Abs"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Side Plank",
                            PartOfBody = "Abs"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Resisted Crunch",
                            PartOfBody = "Abs"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Trunk Rotation",
                            PartOfBody = "Abs"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Pull-up",
                            PartOfBody = "Back"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Back fly",
                            PartOfBody = "Back"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Lateral pulldown",
                            PartOfBody = "Back"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Parallel bar dips",
                            PartOfBody = "Chest"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Incline bench press",
                            PartOfBody = "Chest"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Jumping squat",
                            PartOfBody = "Legs"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Squat",
                            PartOfBody = "Legs"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Biceps curl",
                            PartOfBody = "Biceps"
                        },
                        new
                        {
                            Id = 14,
                            Name = "Chin-up",
                            PartOfBody = "Biceps"
                        },
                        new
                        {
                            Id = 15,
                            Name = "Lying biceps curl",
                            PartOfBody = "Biceps"
                        },
                        new
                        {
                            Id = 16,
                            Name = "Triceps extension",
                            PartOfBody = "Triceps"
                        },
                        new
                        {
                            Id = 17,
                            Name = "Prone triceps extension",
                            PartOfBody = "Triceps"
                        },
                        new
                        {
                            Id = 18,
                            Name = "Kneeling triceps extension",
                            PartOfBody = "Triceps"
                        });
                });

            modelBuilder.Entity("Backend.Core.Domain.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Calories")
                        .HasColumnType("float");

                    b.Property<double>("Carbohydrates")
                        .HasColumnType("float");

                    b.Property<double>("Fats")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<double>("Proteins")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Calories = 95.0,
                            Carbohydrates = 23.5,
                            Fats = 0.29999999999999999,
                            Name = "Banana",
                            Proteins = 1.0
                        },
                        new
                        {
                            Id = 2,
                            Calories = 46.0,
                            Carbohydrates = 11.9,
                            Fats = 0.20000000000000001,
                            Name = "Peach",
                            Proteins = 1.0
                        },
                        new
                        {
                            Id = 3,
                            Calories = 405.0,
                            Carbohydrates = 99.799999999999997,
                            Fats = 0.0,
                            Name = "Sugar",
                            Proteins = 0.0
                        },
                        new
                        {
                            Id = 4,
                            Calories = 380.0,
                            Carbohydrates = 98.0,
                            Fats = 0.0,
                            Name = "Brown sugar",
                            Proteins = 0.10000000000000001
                        },
                        new
                        {
                            Id = 5,
                            Calories = 36.0,
                            Carbohydrates = 9.5,
                            Fats = 0.29999999999999999,
                            Name = "Lemon",
                            Proteins = 0.80000000000000004
                        },
                        new
                        {
                            Id = 6,
                            Calories = 92.0,
                            Carbohydrates = 1.0,
                            Fats = 0.0,
                            Name = "Tiger shrimp",
                            Proteins = 22.0
                        },
                        new
                        {
                            Id = 7,
                            Calories = 162.0,
                            Carbohydrates = 0.0,
                            Fats = 8.4000000000000004,
                            Name = "Smoked salmon",
                            Proteins = 21.5
                        },
                        new
                        {
                            Id = 8,
                            Calories = 201.0,
                            Carbohydrates = 0.0,
                            Fats = 13.6,
                            Name = "Raw salmon",
                            Proteins = 19.899999999999999
                        },
                        new
                        {
                            Id = 9,
                            Calories = 105.0,
                            Carbohydrates = 0.10000000000000001,
                            Fats = 4.5999999999999996,
                            Name = "Turkey tenderloin",
                            Proteins = 15.800000000000001
                        },
                        new
                        {
                            Id = 10,
                            Calories = 93.0,
                            Carbohydrates = 0.0,
                            Fats = 1.2,
                            Name = "Chicken breast sirloin",
                            Proteins = 20.399999999999999
                        },
                        new
                        {
                            Id = 11,
                            Calories = 15.0,
                            Carbohydrates = 3.6000000000000001,
                            Fats = 0.20000000000000001,
                            Name = "Tomato",
                            Proteins = 0.90000000000000002
                        },
                        new
                        {
                            Id = 12,
                            Calories = 17.0,
                            Carbohydrates = 2.6000000000000001,
                            Fats = 0.40000000000000002,
                            Name = "Mushrooms",
                            Proteins = 2.7000000000000002
                        },
                        new
                        {
                            Id = 13,
                            Calories = 344.0,
                            Carbohydrates = 78.900000000000006,
                            Fats = 0.69999999999999996,
                            Name = "Rice",
                            Proteins = 6.7000000000000002
                        },
                        new
                        {
                            Id = 14,
                            Calories = 540.0,
                            Carbohydrates = 0.90000000000000002,
                            Fats = 50.600000000000001,
                            Name = "Salami",
                            Proteins = 21.899999999999999
                        },
                        new
                        {
                            Id = 15,
                            Calories = 14.0,
                            Carbohydrates = 2.8999999999999999,
                            Fats = 0.20000000000000001,
                            Name = "Lettuce",
                            Proteins = 1.3999999999999999
                        },
                        new
                        {
                            Id = 16,
                            Calories = 291.0,
                            Carbohydrates = 0.69999999999999996,
                            Fats = 18.699999999999999,
                            Name = "Roasted pork loin",
                            Proteins = 30.399999999999999
                        },
                        new
                        {
                            Id = 17,
                            Calories = 391.0,
                            Carbohydrates = 0.10000000000000001,
                            Fats = 31.699999999999999,
                            Name = "Cheddar cheese",
                            Proteins = 27.100000000000001
                        });
                });

            modelBuilder.Entity("Backend.Core.Domain.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 1,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("Backend.Core.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<int?>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("date");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.Core.Domain.UserExercise", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("date");

                    b.Property<int?>("DayId")
                        .HasColumnType("int");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfReps")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfSets")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("DayId");

                    b.HasIndex("ExerciseId");

                    b.HasIndex("UserId");

                    b.ToTable("UserExercises");
                });

            modelBuilder.Entity("Backend.Core.Domain.UserProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("date");

                    b.Property<double>("Calories")
                        .HasColumnType("float");

                    b.Property<double>("Carbohydrates")
                        .HasColumnType("float");

                    b.Property<double>("Fats")
                        .HasColumnType("float");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<double>("Proteins")
                        .HasColumnType("float");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("UserProducts");
                });

            modelBuilder.Entity("Backend.Core.Domain.DietGoals", b =>
                {
                    b.HasOne("Backend.Core.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Core.Domain.User", b =>
                {
                    b.HasOne("Backend.Core.Domain.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Backend.Core.Domain.UserExercise", b =>
                {
                    b.HasOne("Backend.Core.Domain.Day", "Day")
                        .WithMany()
                        .HasForeignKey("DayId");

                    b.HasOne("Backend.Core.Domain.Exercise", "Exercise")
                        .WithMany("UserExercises")
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Core.Domain.User", "User")
                        .WithMany("Exercises")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Core.Domain.UserProduct", b =>
                {
                    b.HasOne("Backend.Core.Domain.Product", "Product")
                        .WithMany("Users")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Core.Domain.User", "User")
                        .WithMany("Products")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
