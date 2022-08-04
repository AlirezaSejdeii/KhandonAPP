﻿// <auto-generated />
using System;
using Khandon.Infrastructure.Book.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Khandon.Infrastructure.Book.Migrations
{
    [DbContext(typeof(BookContext))]
    [Migration("20220605210805_change_nullbale_for_description")]
    partial class change_nullbale_for_description
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("BookGroupId")
                        .HasColumnType("int");

                    b.Property<int>("BookType")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<byte>("Difficultye")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(10)
                        .HasColumnType("tinyint")
                        .HasDefaultValue((byte)1);

                    b.Property<bool>("IsComplited")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTextual")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("BookGroupId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.BookGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ShortDescription")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("BookGroups");
                });

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.Chapter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("ShortDescription")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.Study", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ChapterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("ShortDescription")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ChapterId");

                    b.ToTable("Studies");
                });

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.Book", b =>
                {
                    b.HasOne("Khandon.Domain.Enitties.Book.BookGroup", "BookGroup")
                        .WithMany()
                        .HasForeignKey("BookGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BookGroup");
                });

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.Chapter", b =>
                {
                    b.HasOne("Khandon.Domain.Enitties.Book.Book", "Book")
                        .WithMany("Chapters")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.Study", b =>
                {
                    b.HasOne("Khandon.Domain.Enitties.Book.Chapter", "Chapter")
                        .WithMany("Studies")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chapter");
                });

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.Book", b =>
                {
                    b.Navigation("Chapters");
                });

            modelBuilder.Entity("Khandon.Domain.Enitties.Book.Chapter", b =>
                {
                    b.Navigation("Studies");
                });
#pragma warning restore 612, 618
        }
    }
}
