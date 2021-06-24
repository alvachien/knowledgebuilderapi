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
        PoliteBehavior          = 8
    }

    [Table("AwardRule")]
    public sealed class AwardRule
    {
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

        [Column("CountOfFact", TypeName = "INT")]
        public Int32? CountOfFact { get; set; }

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

        public Boolean IsValid()
        {
            if (String.IsNullOrEmpty(TargetUser))
                return false;
            if (String.IsNullOrEmpty(Desp))
                return false;

            switch(RuleType)
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
                    if (!this.CountOfFact.HasValue)
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
        public Int16 HouseKeepingCount { get; set; }

        [Column("PoliteBehavior", TypeName = "SMALLINT")]
        public Int16 PoliteBehavior { get; set; }

        [Column("COMMENT", TypeName = "NVARCHAR(50)")]
        public String Comment { get; set; }
    }

    [Table("AwardPoints")]
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

        [Column("Point", TypeName = "INT")]
        public Int32 Point { get; set; }
    }
}
