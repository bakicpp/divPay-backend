using System;
using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
	public class DivPayDBContext : DbContext
	{
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost,1433;User Id=NewSA;Password=Password@1234;Database=DivPayDB;Trusted_Connection=false");
            //optionsBuilder.UseSqlServer(@"Server=localhost,1433;User Id=NewSA;Password=Password@1234;Database=deneme;Trusted_Connection=false");

        }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<CardDetail> KartDetay { get; set; }
        public DbSet<Card> Kart { get; set; }
        public DbSet<Client> Musteri { get; set; }
        public DbSet<Account> Hesap { get; set; }
        public DbSet<Expense> Harcama { get; set; }
        public DbSet<PaymentRequest> OdemeIstekleri { get; set; }
        public DbSet<MusteriOperationClaim> MusteriOperationClaims { get; set; }
    }
}

