using LegitBank.Models;
using Microsoft.EntityFrameworkCore;

namespace LegitBank.Contexts;

public class LegitBankContext : DbContext
{
    public LegitBankContext(DbContextOptions options) : base(options)
    {
        
    }
    
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.AccountId).HasName("PK_AccountId");
            entity.Property(a => a.AccountId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(a => a.TransactionId).HasName("PK_TransactionId");
            entity.Property(a => a.TransactionId).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.SenderAccount)
            .WithMany(a => a.SentTransactions)
            .HasForeignKey(t => t.SenderAccountId);
        
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ReceiverAccount)
            .WithMany(a => a.ReceivedTransactions)  
            .HasForeignKey(t => t.ReceiverAccountId);
    }
}