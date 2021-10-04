using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace knowledgebuilderapi.Models
{
    public enum AwardRuleType: Int16
    {
        GoToBedTime             = 1,
        SchoolWorkTime          = 2,
        HomeWorkCount           = 3,
        BodyExerciseCount       = 4,
        ErrorCollectionHabit    = 5,
        CleanDeakHabit          = 6,
        HouseKeepingCount       = 7,
        PoliteBehavior          = 8,
        HandWritingHabit        = 9,
    }

    [Table("AwardRuleGroup")]
    public sealed class AwardRuleGroup
    {
        public AwardRuleGroup()
        {
            Rules = new HashSet<AwardRule>();
        }

        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Column("RuleType", TypeName = "SMALLINT")]
        public AwardRuleType RuleType { get; set; }

        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Column("DESP", TypeName = "NVARCHAR(50)")]
        public String Desp { get; set; }

        [Column("ValidFrom", TypeName = "DATETIME")]
        public DateTime ValidFrom { get; set; }

        [Column("ValidTo", TypeName = "DATETIME")]
        public DateTime ValidTo { get; set; }

        public ICollection<AwardRule> Rules { get; set; }

        public bool IsValid()
        {
            if (String.IsNullOrEmpty(TargetUser))
                return false;
            if (String.IsNullOrEmpty(Desp))
                return false;

            if (this.Rules.Count <= 0)
                return false;

            foreach(var rule in this.Rules)
            {
                if (!rule.IsValid(this.RuleType))
                    return false;
            }

            return true;
        }

        public void UpdateData(AwardRuleGroup update)
        {
            this.RuleType = update.RuleType;
            this.TargetUser = update.TargetUser;
            this.Desp = update.Desp;
            this.ValidFrom = update.ValidFrom;
            this.ValidTo = update.ValidTo;
        }
    }

    [Table("AwardRule")]
    public sealed class AwardRule
    {
        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Required]
        [Column("GroupID", TypeName = "INT")]
        public Int32 GroupID { get; set; }

        [Column("CountOfFactLow", TypeName = "INT")]
        public Int32? CountOfFactLow { get; set; }

        [Column("CountOfFactHigh", TypeName = "INT")]
        public Int32? CountOfFactHigh { get; set; }

        [Column("DoneOfFact", TypeName = "BIT")]
        public Boolean? DoneOfFact { get; set; }

        [Column("TimeStart", TypeName = "DECIMAL")]
        public Decimal? TimeStart { get; set; }

        [Column("TimeEnd", TypeName = "DECIMAL")]
        public Decimal? TimeEnd { get; set; }

        [Column("DaysFrom", TypeName = "INT")]
        public Int32? DaysFrom { get; set; }

        [Column("DaysTo", TypeName = "INT")]
        public Int32? DaysTo { get; set; }

        [Column("Point", TypeName = "INT")]
        public Int32 Point { get; set; }

        public AwardRuleGroup CurrentGroup { get; set; }

        public bool IsValid(AwardRuleType rtype)
        {
            switch(rtype)
            {
                case AwardRuleType.GoToBedTime:
                case AwardRuleType.SchoolWorkTime:
                    if (!this.TimeStart.HasValue || !this.TimeEnd.HasValue)
                        return false;
                    break;

                case AwardRuleType.HomeWorkCount:
                case AwardRuleType.BodyExerciseCount:
                case AwardRuleType.HouseKeepingCount:
                case AwardRuleType.PoliteBehavior:
                    if (!this.CountOfFactLow.HasValue)
                        return false;
                    if (!this.CountOfFactHigh.HasValue)
                        return false;
                    if (this.CountOfFactLow.Value > this.CountOfFactHigh.Value)
                        return false;
                    break;

                case AwardRuleType.ErrorCollectionHabit:
                case AwardRuleType.CleanDeakHabit:
                    if (!this.DoneOfFact.HasValue)
                        return false;
                    break;

                default:
                    return false;
            }

            return true;
        }

        public void UpdateData(AwardRule update)
        {
            this.CountOfFactLow = update.CountOfFactLow;
            this.CountOfFactHigh = update.CountOfFactHigh;
            this.DaysFrom = update.DaysFrom;
            this.DaysTo = update.DaysTo;
            this.DoneOfFact = update.DoneOfFact;
            this.Point = update.Point;
            this.TimeEnd = update.TimeEnd;
            this.TimeStart = update.TimeStart;
        }
    }

    [Table("AwardUser")]
    public sealed class AwardUser
    {
        [Key]
        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Key]
        [Column("Supervisor", TypeName = "NVARCHAR(50)")]
        public String Supervisor { get; set; }
    }

    [Table("DailyTrace")]
    public sealed class DailyTrace
    {
        [Key]
        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Key]
        [Column("RecordDate", TypeName = "DATE")]
        public DateTime RecordDate { get; set; }

        [Column("SchoolWorkTime", TypeName = "DECIMAL")]
        public Decimal? SchoolWorkTime { get; set; }

        [Column("GoToBedTime", TypeName = "DECIMAL")]
        public Decimal? GoToBedTime { get; set; }

        [Column("HomeWorkCount", TypeName = "SMALLINT")]
        public Int16? HomeWorkCount { get; set; }

        [Column("BodyExerciseCount", TypeName = "SMALLINT")]
        public Int16? BodyExerciseCount { get; set; }

        [Column("ErrorsCollection", TypeName = "BIT")]
        public Boolean? ErrorsCollection { get; set; }

        [Column("HandWriting", TypeName = "BIT")]
        public Boolean? HandWriting { get; set; }

        [Column("CleanDesk", TypeName = "BIT")]
        public Boolean? CleanDesk { get; set; }

        [Column("HouseKeepingCount", TypeName = "SMALLINT")]
        public Int16? HouseKeepingCount { get; set; }

        [Column("PoliteBehavior", TypeName = "SMALLINT")]
        public Int16? PoliteBehavior { get; set; }

        [Column("COMMENT", TypeName = "NVARCHAR(50)")]
        public String Comment { get; set; }

        public void UpdateData(DailyTrace other)
        {
            this.SchoolWorkTime = other.SchoolWorkTime;
            this.GoToBedTime = other.GoToBedTime;
            this.HomeWorkCount = other.HomeWorkCount;
            this.BodyExerciseCount = other.BodyExerciseCount;
            this.ErrorsCollection = other.ErrorsCollection;
            this.HandWriting = other.HandWriting;
            this.CleanDesk = other.CleanDesk;
            this.HouseKeepingCount = other.HouseKeepingCount;
            this.PoliteBehavior = other.PoliteBehavior;
            this.Comment = other.Comment;
        }
    }

    [Table("AwardPoint")]
    public sealed class AwardPoint
    {
        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Column("RecordDate", TypeName = "DATE")]
        public DateTime RecordDate { get; set; }

        [Column("MatchedRuleID", TypeName = "INT")]
        public Int32? MatchedRuleID { get; set; }

        [Column("CountOfDay", TypeName = "INT")]
        public Int32? CountOfDay { get; set; }

        [Column("Point", TypeName = "INT")]
        public Int32 Point { get; set; }

        [Column("COMMENT", TypeName = "NVARCHAR(50)")]
        public String Comment { get; set; }

        public void UpdateData(AwardPoint update)
        {
            this.TargetUser = update.TargetUser;
            this.RecordDate = update.RecordDate;
            this.MatchedRuleID = update.MatchedRuleID;
            this.Point = update.Point;
            this.Comment = update.Comment;
        }
    }

    public sealed class AwardPointReport
    {
        [Key]
        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Column("RecordDate", TypeName = "DATE")]
        public DateTime RecordDate { get; set; }

        [Column("Point", TypeName = "INT")]
        public Int32 Point { get; set; }

        [Column("AggPoint", TypeName = "INT")]
        public Int32 AggPoint { get; set; }
    }
}
