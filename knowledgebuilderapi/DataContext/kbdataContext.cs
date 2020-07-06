using System;
using Microsoft.EntityFrameworkCore;
using knowledgebuilderapi.Models;

namespace knowledgebuilderapi
{
    public class kbdataContext : DbContext
    {
        public kbdataContext(DbContextOptions<kbdataContext> options) : base(options)
        {
            TestingMode = false;
        }

        public kbdataContext(DbContextOptions<kbdataContext> options, bool testingMode = false) : base(options)
        {
            TestingMode = testingMode;
        }

        // Testing mode
        public Boolean TestingMode { get; private set; }

        public DbSet<KnowledgeItem> KnowledgeItems { get; set; }
        public DbSet<QuestionBankItem> QuestionBankItems { get; set; }
        public DbSet<QuestionBankSubItem> QuestionBankSubItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnowledgeItem>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("GETDATE()");
                    entity.Property(b => b.ModifiedAt)
                        .HasDefaultValueSql("GETDATE()");
                }
                else
                {
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                    entity.Property(b => b.ModifiedAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                }
                entity.Property(b => b.Category)
                    .HasConversion(
                        v => (Int16)v,
                        v => (KnowledgeItemCategory)v);
            });

            modelBuilder.Entity<QuestionBankItem>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("GETDATE()");
                    entity.Property(b => b.ModifiedAt)
                        .HasDefaultValueSql("GETDATE()");
                }
                else
                {
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                    entity.Property(b => b.ModifiedAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                }

                entity.HasOne(d => d.CurrentKnowledgeItem)
                    .WithMany(p => p.QuestionBankItems)
                    .HasForeignKey(d => d.KnowledgeItemID)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_QBITEM_KITEM");
            });

            modelBuilder.Entity<QuestionBankSubItem>(entity =>
            {
                entity.HasKey(e => new { e.ItemID, e.SubID });

                entity.HasOne(d => d.CurrentQuestionBankItem)
                    .WithMany(p => p.SubItems)
                    .HasForeignKey(d => d.ItemID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_QBSUBITEM_QBITEM");
            });
        }
    }
}
