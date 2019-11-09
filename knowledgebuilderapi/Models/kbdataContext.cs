using System;
using Microsoft.EntityFrameworkCore;

namespace knowledgebuilderapi.Models
{
    public class kbdataContext : DbContext
    {
        public kbdataContext(DbContextOptions<kbdataContext> options) : base(options)
        { 
        }

        public DbSet<Knowledge> Knowledges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Knowledge>()
                .Property(b => b.CreatedAt);
                // .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Knowledge>()
                .Property(b => b.ModifiedAt);
                // .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Knowledge>()
                .Property(e => e.Category)
                .HasConversion(
                    v => (Int16)v,
                    v => (KnowledgeCategory)v);
        }
    }
}