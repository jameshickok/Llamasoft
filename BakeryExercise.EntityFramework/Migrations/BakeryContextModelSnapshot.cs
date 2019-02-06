﻿// <auto-generated />
using System;
using BakeryExercise.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BakeryExercise.EntityFramework.Migrations
{
    [DbContext(typeof(BakeryContext))]
    partial class BakeryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity("BakeryExercise.EntityFramework.Food", b =>
                {
                    b.Property<Guid>("FoodId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsVegan");

                    b.Property<string>("Name");

                    b.HasKey("FoodId");

                    b.HasIndex("Name");

                    b.ToTable("Food");

                    b.HasData(
                        new
                        {
                            FoodId = new Guid("21def0fd-9364-45df-b619-11ee8a1303f8"),
                            IsVegan = false,
                            Name = "Doughnut"
                        },
                        new
                        {
                            FoodId = new Guid("f01d1324-27c6-4b3b-ba43-e757d0dcd1b2"),
                            IsVegan = false,
                            Name = "Muffin"
                        },
                        new
                        {
                            FoodId = new Guid("525f356f-eb7f-4edb-9975-542d7f58f41a"),
                            IsVegan = false,
                            Name = "Bagel"
                        });
                });

            modelBuilder.Entity("BakeryExercise.EntityFramework.Menu", b =>
                {
                    b.Property<Guid>("MenuId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Category")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("MenuId");

                    b.HasIndex("Name");

                    b.ToTable("Menus");

                    b.HasData(
                        new
                        {
                            MenuId = new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"),
                            Category = "Breakfast",
                            Name = "Everyday Breakfast"
                        });
                });

            modelBuilder.Entity("BakeryExercise.EntityFramework.MenuItem", b =>
                {
                    b.Property<Guid>("MenuItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("MenuId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<double>("Price");

                    b.HasKey("MenuItemId");

                    b.HasIndex("MenuId");

                    b.ToTable("MenuItems");

                    b.HasData(
                        new
                        {
                            MenuItemId = new Guid("b6f080c0-a045-4ffb-b2a8-9551dde7229e"),
                            MenuId = new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"),
                            Name = "Single Doughnut",
                            Price = 1.99
                        },
                        new
                        {
                            MenuItemId = new Guid("78ff1e8c-975b-4ee7-b91b-e63b62014793"),
                            MenuId = new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"),
                            Name = "Dozen Doughnuts",
                            Price = 10.99
                        },
                        new
                        {
                            MenuItemId = new Guid("27861181-006d-480c-be1e-537b01743dc5"),
                            MenuId = new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"),
                            Name = "Single Muffin",
                            Price = 2.9900000000000002
                        },
                        new
                        {
                            MenuItemId = new Guid("343165a7-985e-4f65-9f91-2cfd9332de34"),
                            MenuId = new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"),
                            Name = "Dozen Muffins",
                            Price = 13.99
                        },
                        new
                        {
                            MenuItemId = new Guid("7aa83e66-eb4e-4bad-adca-564c964f679a"),
                            MenuId = new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"),
                            Name = "Single Bagel",
                            Price = 0.98999999999999999
                        },
                        new
                        {
                            MenuItemId = new Guid("8c333bcd-ebed-4f73-b651-c05bed8e98dc"),
                            MenuId = new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"),
                            Name = "Dozen Bagels",
                            Price = 9.9900000000000002
                        },
                        new
                        {
                            MenuItemId = new Guid("70c26879-2630-4ea3-ae1a-b124e96bf7c5"),
                            MenuId = new Guid("dcf5932f-20f0-4677-9d5c-048dd0c48a85"),
                            Name = "Huge Spread",
                            Price = 39.990000000000002
                        });
                });

            modelBuilder.Entity("BakeryExercise.EntityFramework.MenuItemPortion", b =>
                {
                    b.Property<Guid>("MenuItemId");

                    b.Property<Guid>("PortionId");

                    b.HasKey("MenuItemId", "PortionId");

                    b.HasIndex("PortionId");

                    b.ToTable("MenuItemPortions");

                    b.HasData(
                        new
                        {
                            MenuItemId = new Guid("b6f080c0-a045-4ffb-b2a8-9551dde7229e"),
                            PortionId = new Guid("7bdb6536-b211-41ca-ac4b-7c596e5305de")
                        },
                        new
                        {
                            MenuItemId = new Guid("78ff1e8c-975b-4ee7-b91b-e63b62014793"),
                            PortionId = new Guid("f1d66681-3fb5-4595-9b20-2100f499ce55")
                        },
                        new
                        {
                            MenuItemId = new Guid("27861181-006d-480c-be1e-537b01743dc5"),
                            PortionId = new Guid("200f4f9e-dc4c-46aa-8cbc-3b6e4f835ee0")
                        },
                        new
                        {
                            MenuItemId = new Guid("343165a7-985e-4f65-9f91-2cfd9332de34"),
                            PortionId = new Guid("29332f74-fdd1-42f7-ba79-fe44b45fa577")
                        },
                        new
                        {
                            MenuItemId = new Guid("7aa83e66-eb4e-4bad-adca-564c964f679a"),
                            PortionId = new Guid("66d79753-14e3-42e1-98a6-1cf42988ac0e")
                        },
                        new
                        {
                            MenuItemId = new Guid("8c333bcd-ebed-4f73-b651-c05bed8e98dc"),
                            PortionId = new Guid("f96e701c-cfd7-4163-8b5b-c2494a58b942")
                        },
                        new
                        {
                            MenuItemId = new Guid("70c26879-2630-4ea3-ae1a-b124e96bf7c5"),
                            PortionId = new Guid("f1d66681-3fb5-4595-9b20-2100f499ce55")
                        },
                        new
                        {
                            MenuItemId = new Guid("70c26879-2630-4ea3-ae1a-b124e96bf7c5"),
                            PortionId = new Guid("29332f74-fdd1-42f7-ba79-fe44b45fa577")
                        },
                        new
                        {
                            MenuItemId = new Guid("70c26879-2630-4ea3-ae1a-b124e96bf7c5"),
                            PortionId = new Guid("f96e701c-cfd7-4163-8b5b-c2494a58b942")
                        });
                });

            modelBuilder.Entity("BakeryExercise.EntityFramework.Portion", b =>
                {
                    b.Property<Guid>("PortionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<Guid?>("FoodId");

                    b.HasKey("PortionId");

                    b.HasIndex("FoodId");

                    b.ToTable("Portions");

                    b.HasData(
                        new
                        {
                            PortionId = new Guid("7bdb6536-b211-41ca-ac4b-7c596e5305de"),
                            Amount = 1,
                            FoodId = new Guid("21def0fd-9364-45df-b619-11ee8a1303f8")
                        },
                        new
                        {
                            PortionId = new Guid("f1d66681-3fb5-4595-9b20-2100f499ce55"),
                            Amount = 12,
                            FoodId = new Guid("21def0fd-9364-45df-b619-11ee8a1303f8")
                        },
                        new
                        {
                            PortionId = new Guid("200f4f9e-dc4c-46aa-8cbc-3b6e4f835ee0"),
                            Amount = 1,
                            FoodId = new Guid("f01d1324-27c6-4b3b-ba43-e757d0dcd1b2")
                        },
                        new
                        {
                            PortionId = new Guid("29332f74-fdd1-42f7-ba79-fe44b45fa577"),
                            Amount = 12,
                            FoodId = new Guid("f01d1324-27c6-4b3b-ba43-e757d0dcd1b2")
                        },
                        new
                        {
                            PortionId = new Guid("66d79753-14e3-42e1-98a6-1cf42988ac0e"),
                            Amount = 1,
                            FoodId = new Guid("525f356f-eb7f-4edb-9975-542d7f58f41a")
                        },
                        new
                        {
                            PortionId = new Guid("f96e701c-cfd7-4163-8b5b-c2494a58b942"),
                            Amount = 12,
                            FoodId = new Guid("525f356f-eb7f-4edb-9975-542d7f58f41a")
                        });
                });

            modelBuilder.Entity("BakeryExercise.EntityFramework.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsAdmin");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("UserId");

                    b.HasIndex("UserName");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = new Guid("2533ef76-5860-43c6-b0d5-147160a530e6"),
                            IsAdmin = true,
                            Password = "password",
                            UserName = "admin@bakery.com"
                        });
                });

            modelBuilder.Entity("BakeryExercise.EntityFramework.MenuItem", b =>
                {
                    b.HasOne("BakeryExercise.EntityFramework.Menu", "Menu")
                        .WithMany("Items")
                        .HasForeignKey("MenuId");
                });

            modelBuilder.Entity("BakeryExercise.EntityFramework.MenuItemPortion", b =>
                {
                    b.HasOne("BakeryExercise.EntityFramework.MenuItem", "MenuItem")
                        .WithMany("MenuItemPortions")
                        .HasForeignKey("MenuItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BakeryExercise.EntityFramework.Portion", "Portion")
                        .WithMany("MenuItemPortions")
                        .HasForeignKey("PortionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BakeryExercise.EntityFramework.Portion", b =>
                {
                    b.HasOne("BakeryExercise.EntityFramework.Food", "Food")
                        .WithMany()
                        .HasForeignKey("FoodId");
                });
#pragma warning restore 612, 618
        }
    }
}
