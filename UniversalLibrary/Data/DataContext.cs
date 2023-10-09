using UniversalLibrary.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using UniversalLibrary.Models;
using System.Reflection.Emit;
using System;

namespace UniversalLibrary.Data
{
    //responsável pela ligação à BD - herda de um identity e recebe um objecto user
    public class DataContext : IdentityDbContext<User>
    {
        //Criar tabelas
        public DbSet<Author> authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookOnline> BookOnlines { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<BookingDetail> BookingDetails { get; set; }

        public DbSet<BookingDetailTemp> BookingDetailTemps { get; set; }

        public DbSet<BookPublisher> BookPublishers { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Country> Countries { get; set; } 

        public DbSet<Loan> Loans { get; set; }

        public DbSet<LoanDetail> LoanDetails { get; set; }

        public DbSet<LoanDetailTemp> LoanDetailTemps { get; set; }

        public DbSet<Penalty> Penalties { get; set; }

        public DbSet<PhisicalLibrary> PhisicalLibraries { get; set;}

        public DbSet<Proof> Proofs { get; set; }

        public DbSet<Recomendation> Recomendations { get; set;}

        public DbSet<ReturnBook> ReturnBooks { get; set; }

        public DbSet<ImageViewModel> imageViewModels { get; set; }

        public DbSet<LoanOnline> LoanOnlines { get; set; }

        public DbSet<LoanOnlineDetail> LoanOnlineDetails { get; set; }

        public DbSet<LoanOnlineDetailTemp> LoanOnlineDetailTemps { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<LibraryFeedback> LibraryFeedbacks { get; set; }

        public DbSet<Reader> Readers { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Country>()
                .HasIndex(c => c.CountryName)
                .IsUnique();


            modelbuilder.Entity<Penalty>()
                .Property(s => s.FineAmount)
                .HasColumnType("decimal(18, 2)"); // Exemplo: decimal com 18 dígitos no total e 2 casas decimais

            modelbuilder.Entity<Book>()
                .HasIndex(b => b.Isbn)   
                .IsUnique();

            modelbuilder.Entity<BookOnline>()  //tirar e passar a ISBN -> e adicionar a propriedade
                .HasIndex(b => b.Isbn)
                .IsUnique();

            modelbuilder.Entity<Author>()  //DUVIDA: tiramos o firstÑame e LastName -> e passamos para um nif?
                .HasIndex(a => a.Nif)
                .IsUnique();                       


            modelbuilder.Entity<BookPublisher>()
                .HasIndex(b => b.PublisherName)
                .IsUnique();

            modelbuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            modelbuilder.Entity<PhisicalLibrary>()
                .HasIndex(p => p.LibraryName)
                .IsUnique();



            modelbuilder.Entity<User>() 
                .HasIndex(u => u.Email)
                .IsUnique();

            modelbuilder.Entity<User>()
                .HasIndex(u => u.Nif)
                .IsUnique();

            modelbuilder.Entity<Reader>()
                .HasIndex(u => u.Nif)
                .IsUnique();



            base.OnModelCreating(modelbuilder);
        }

        

        
    }
}
