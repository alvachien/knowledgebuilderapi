using System;
using Microsoft.EntityFrameworkCore;
using knowledgebuilderapi.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        public DbSet<AwardRuleGroup> AwardRuleGroups { get; set; }
        public DbSet<AwardRule> AwardRules { get; set; }
        public DbSet<AwardUser> AwardUsers { get; set; }
        public DbSet<DailyTrace> DailyTraces { get; set; }
        public DbSet<AwardPoint> AwardPoints { get; set; }
        public DbSet<AwardPointReport> AwardPointReports { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<UserCollectionItem> UserCollectionItems { get; set; }
        public DbSet<ExerciseItemUserScore> ExerciseItemUserScores { get; set; }
        public DbSet<InvitedUser> InvitedUsers { get; set; }
        public DbSet<AwardUserView> AwardUserViews { get; set; }
        public DbSet<UserHabit> UserHabits { get; set; }
        public DbSet<UserHabitRule> UserHabitRules { get; set; }
        public DbSet<UserHabitRecord> UserHabitRecords { get; set; }
        public DbSet<UserHabitPointsByUserDate> UserHabitPointsByUserDates { get; set; }
        public DbSet<UserHabitPointsByUserHabitDate> UserHabitPointsByUserHabitDates { get; set; }

        private readonly ValueConverter habitCategoryConverter = new ValueConverter<HabitCategory, Int16>(v => (short)v, v => (HabitCategory)v);
        private readonly ValueConverter habitFrequencyConverter = new ValueConverter<HabitFrequency, Int16>(v => (short)v, v => (HabitFrequency)v);
        private readonly ValueConverter habitCompleteFactoryConverter = new ValueConverter<HabitCompleteCategory, Int16>(v => (short)v, v => (HabitCompleteCategory)v);

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

                entity.HasMany(d => d.UserScores)
                    .WithOne(p => p.ReferenceItem)
                    .HasForeignKey(d => d.RefID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_EXERCISEITEM_USRSCORE_ID");
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

            modelBuilder.Entity<AwardRuleGroup>(entity =>
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

                entity.HasMany(d => d.Rules)
                    .WithOne(p => p.CurrentGroup)
                    .HasForeignKey(d => d.GroupID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_AWARDRULE_GROUPID");
            });

            modelBuilder.Entity<AwardRule>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();
                }
                else
                {
                    // Testing mode: Sqlite
                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                }
            });

            modelBuilder.Entity<AwardUser>(entity =>
            {
                entity.HasKey(d => new { d.TargetUser, d.Supervisor });
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

            modelBuilder.Entity<UserCollection>(entity =>
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

                entity.HasMany(d => d.Items)
                    .WithOne(p => p.Collection)
                    .HasForeignKey(d => d.ID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USERCOLL_ITEM_ID");
            });

            modelBuilder.Entity<UserCollectionItem>(entity =>
            {
                entity.HasKey(d => new { d.ID, d.RefType, d.RefID });

                entity.Property(b => b.RefType)
                    .HasConversion(
                        v => (Int16)v,
                        v => (TagRefType)v);
            });

            modelBuilder.Entity<ExerciseItemUserScore>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(b => b.TakenDate)
                        .HasDefaultValueSql("GETDATE()");

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd()
                        .UseIdentityColumn();
                }
                else
                {
                    // Testing mode: Sqlite
                    entity.Property(b => b.TakenDate)
                        .HasDefaultValueSql("CURRENT_DATE");

                    entity.Property(e => e.ID)
                        .ValueGeneratedOnAdd();
                }
            });

            modelBuilder.Entity<InvitedUser>(entity =>
            {
                if (!TestingMode)
                {
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("GETDATE()");
                    entity.Property(b => b.LastLoginAt)
                        .HasDefaultValueSql("GETDATE()");
                }
                else
                {
                    // Testing mode: Sqlite
                    entity.Property(b => b.CreatedAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                    entity.Property(b => b.LastLoginAt)
                        .HasDefaultValueSql("CURRENT_DATE");
                }

                entity.HasIndex(p => p.InvitationCode).IsUnique();
                entity.HasIndex(p => p.DisplayAs).IsUnique();
            });

            modelBuilder.Entity<AwardUserView>(entity =>
            {
                entity.ToView("AwardUserView");
                // entity.HasNoKey();
                entity.HasKey(d => new { d.TargetUser, d.Supervisor });

                entity.HasOne(d => d.CurrentUser)
                    .WithMany(d => d.AwardUsers)
                    .HasForeignKey(d => d.Supervisor);
            });

            modelBuilder.Entity<UserHabit>(entity =>
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

                entity.Property(b => b.Category)
                    .HasConversion(habitCategoryConverter);
                entity.Property(b => b.Frequency)
                    .HasConversion(habitFrequencyConverter);
                entity.Property(b => b.CompleteCategory)
                    .HasConversion(habitCompleteFactoryConverter);

                entity.HasMany(d => d.Rules)
                    .WithOne(p => p.CurrentHabit)
                    .HasForeignKey(d => d.HabitID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USERHABITRULE_HABIT");
                entity.HasMany(d => d.Records)
                    .WithOne(p => p.CurrentHabit)
                    .HasForeignKey(d => d.HabitID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_USERHABITRECORD_HABIT");
            });

            modelBuilder.Entity<UserHabitRule>(entity =>
            {
                entity.HasKey(d => new { d.HabitID, d.RuleID });
            });

            modelBuilder.Entity<UserHabitRecord>(entity =>
            {
                entity.HasKey(d => new { d.HabitID, d.RecordDate, d.SubID });

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
            });

            modelBuilder.Entity<UserHabitPointsByUserDate>(entity =>
            {
                entity.ToView("HabitUserDatePointView");
                entity.HasNoKey();
            });

            modelBuilder.Entity<UserHabitPointsByUserHabitDate>(entity =>
            {
                entity.ToView("HabitUserHabitDatePointView");
                entity.HasNoKey();
            });
        }
    }
}
