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
        public DbSet<TagCount> TagCounts { get; set; }
        public DbSet<TagCountByRefType> TagCountByRefTypes { get; set; }
        public DbSet<OverviewInfo> OverviewInfos { get; set; }
        public DbSet<ExerciseItemWithTagView> ExerciseItemWithTagViews { get; set; }
        public DbSet<KnowledgeItemWithTagView> KnowledgeItemWithTagViews { get; set; }
        public DbSet<AwardRule> AwardRules { get; set; }
        public DbSet<DailyTrace> DailyTraces { get; set; }
        public DbSet<AwardPoint> AwardPoints { get; set; }
        public DbSet<AwardPointReport> AwardPointReports { get; set; }

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

                entity.HasOne(d => d.Answer)
                    .WithOne(p => p.ExerciseItem)
                    .HasForeignKey<ExerciseItemAnswer>(d => d.ID)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);
                    // .HasConstraintName("FK_EXECAWR_EXECITEM");
            });

            modelBuilder.Entity<ExerciseItemAnswer>(entity =>
            {
                entity.HasKey(e => new { e.ID });
            });

            modelBuilder.Entity<KnowledgeTag>(entity =>
            {
                entity.HasKey(d => new { d.TagTerm, d.RefID });

                entity.HasOne(d => d.CurrentKnowledgeItem)
                    .WithMany(d => d.Tags)
                    .HasForeignKey(d => d.RefID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_KNOWLEDGETAG_ID");
            });

            modelBuilder.Entity<ExerciseTag>(entity =>
            {
                entity.HasKey(d => new { d.TagTerm, d.RefID });

                entity.HasOne(d => d.CurrentExerciseItem)
                    .WithMany(d => d.Tags)
                    .HasForeignKey(d => d.RefID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EXERCISETAG_ID");
            });

            modelBuilder.Entity<AwardRule>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(b => b.ValidFrom)
                        .HasDefaultValueSql("GETDATE()");
                    entity.Property(b => b.ValidTo)
                        .HasDefaultValueSql("GETDATE()");

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();
                }
                else
                {
                    // Testing mode: Sqlite
                    entity.Property(b => b.ValidFrom)
                        .HasDefaultValueSql("CURRENT_DATE");
                    entity.Property(b => b.ValidTo)
                        .HasDefaultValueSql("CURRENT_DATE");

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                }

                entity.Property(b => b.RuleType)
                    .HasConversion(
                        v => (Int16)v,
                        v => (AwardRuleType)v);
            });

            modelBuilder.Entity<DailyTrace>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(b => b.RecordDate)
                        .HasDefaultValueSql("GETDATE()");
                }
                else
                {
                    // Testing mode: Sqlite
                    entity.Property(b => b.RecordDate)
                        .HasDefaultValueSql("CURRENT_DATE");
                }
                entity.HasKey(d => new { d.TargetUser, d.RecordDate });
            });

            modelBuilder.Entity<AwardPoint>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(b => b.RecordDate)
                        .HasDefaultValueSql("GETDATE()");

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();
                }
                else
                {
                    // Testing mode: Sqlite
                    entity.Property(b => b.RecordDate)
                        .HasDefaultValueSql("CURRENT_DATE");

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                }
            });

            modelBuilder.Entity<AwardPointReport>(entity =>
            {
                entity.ToView("AwardPointReport");
                entity.HasNoKey();
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

            modelBuilder.Entity<TagCountByRefType>(entity =>
            {
                entity.ToView("TagCountByRefType");
                entity.HasNoKey();
                entity.Property(b => b.RefType)
                    .HasConversion(
                        v => (Int32)v,
                        v => (TagRefType)v);
            });

            modelBuilder.Entity<TagCount>(entity =>
            {
                entity.ToView("TagCount");
                entity.HasNoKey();
                entity.Property(b => b.RefType)
                    .HasConversion(
                        v => (Int32)v,
                        v => (TagRefType)v);
            });

            modelBuilder.Entity<OverviewInfo>(entity =>
            {
                entity.ToView("OverviewInfo");
                entity.HasNoKey();
                entity.Property(b => b.RefType)
                    .HasConversion(
                        v => (Int32)v,
                        v => (TagRefType)v);
            });

            modelBuilder.Entity<ExerciseItemWithTagView>(entity =>
            {
                entity.ToView("ExerciseItemWithTagView");
                entity.HasNoKey();
            });

            modelBuilder.Entity<KnowledgeItemWithTagView>(entity =>
            {
                entity.ToView("KnowledgeItemWithTagView");
                entity.HasNoKey();
            });
        }
    }
}
