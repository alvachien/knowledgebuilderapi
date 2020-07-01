using System;
using Microsoft.EntityFrameworkCore;

namespace knowledgebuilderapi.Models
{
    public class kbdataContext : DbContext
    {
        public kbdataContext(DbContextOptions<kbdataContext> options) : base(options)
        { 
        }

        public DbSet<KnowledgeItem> KnowledgeItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnowledgeItem>()
                .Property(b => b.CreatedAt);
                // .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<KnowledgeItem>()
                .Property(b => b.ModifiedAt);
                // .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<KnowledgeItem>()
                .Property(e => e.Category)
                .HasConversion(
                    v => (Int16)v,
                    v => (KnowledgeItemCategory)v);
        }
    }
}