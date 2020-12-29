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
        public DbSet<ExerciseItem> ExerciseItems { get; set; }
        public DbSet<ExerciseItemAnswer> ExerciseItemAnswers { get; set; }
        public DbSet<KnowledgeTag> KnowledgeTags { get; set; }
        public DbSet<ExerciseTag> ExerciseTags { get; set; }
        public DbSet<Tag> Tags { get; set; }

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

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();
                }
                else
                {
                    // Testing mode: Sqlite
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                    entity.Property(b => b.ModifiedAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                }

                entity.Property(b => b.Category)
                    .HasConversion(
                        v => (Int16)v,
                        v => (KnowledgeItemCategory)v);
            });

            modelBuilder.Entity<ExerciseItem>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("GETDATE()");
                    entity.Property(b => b.ModifiedAt)
                        .HasDefaultValueSql("GETDATE()");

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();
                }
                else
                {
                    // Testing mode: Sqlite
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                    entity.Property(b => b.ModifiedAt)
                        .HasDefaultValueSql("CURRENT_DATE");

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                }

                entity.Property(b => b.ExerciseType)
                    .HasConversion(
                        v => (Int16)v,
                        v => (ExerciseItemType)v);

                entity.HasOne(d => d.CurrentKnowledgeItem)
                    .WithMany(p => p.Exercises)
                    .HasForeignKey(d => d.KnowledgeItemID)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_EXECITEM_KITEM");
            });

            modelBuilder.Entity<ExerciseItemAnswer>(entity =>
            {
                entity.HasKey(e => new { e.ItemID });

                entity.HasOne(d => d.ExerciseItem)
                    .WithOne(p => p.Answer)
                    .HasForeignKey<ExerciseItem>(prop => prop.ID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EXECAWR_EXECITEM");
            });

            modelBuilder.Entity<KnowledgeTag>(entity =>
            {
                entity.HasOne(d => d.CurrentKnowledgeItem)
                    .WithMany(d => d.Tags)
                    .HasForeignKey(d => d.RefID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_KNOWLEDGETAG_ID");
            });

            modelBuilder.Entity<ExerciseTag>(entity =>
            {
                entity.HasOne(d => d.CurrentExerciseItem)
                    .WithMany(d => d.Tags)
                    .HasForeignKey(d => d.RefID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EXERCISETAG_ID");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToView("Tag");
                entity.HasNoKey();
                entity.Property(b => b.RefType)
                    .HasConversion(
                        v => (Int32)v,
                        v => (TagRefType)v);
            });
        }
    }
}
