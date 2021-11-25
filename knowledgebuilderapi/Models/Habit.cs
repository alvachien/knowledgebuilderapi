using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace knowledgebuilderapi.Models
{
    public enum HabitCategory : Int16
    {
        Positive = 0,
        Negative = 1,
    }

    public enum HabitFrequency: Int16
    {
        Daily       = 0,
        Weekly      = 1,
        Monthly     = 2
    }

    public enum HabitCompleteCategory: Int16
    {
        NumberOfTimes   = 0,
        NumberOfCount   = 1,
    }

    [Table("UserHabit")]
    public class UserHabit
    {
        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Required]
        [Column("Category", TypeName = "SMALLINT")]
        public HabitCategory Category { get; set; }

        [Required]
        [Column("Name", TypeName = "NVARCHAR(50)")]
        public String Name { get; set; }

        [Column("Comment", TypeName = "NVARCHAR(50)")]
        public String Comment { get; set; }

        [Required]
        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Required]
        [Column("ValidFrom", TypeName = "DATETIME")]
        public DateTime ValidFrom { get; set; }

        [Required]
        [Column("ValidTo", TypeName = "DATETIME")]
        public DateTime ValidTo { get; set; }

        [Column("Frequency", TypeName = "SMALLINT")]
        public HabitFrequency Frequency { get; set; }

        [Column("CompleteCategory", TypeName = "SMALLINT")]
        public HabitCompleteCategory CompleteCategory { get; set; }

        /// <summary>
        /// Complete Condition
        /// 
        /// Depends on the Complete Category & Frequency.
        /// If CompleteCategory is Number Of Times:
        ///     Frequency is Weekly.
        ///         Count of the record per week.
        ///     Frequency is Monthly.
        ///         Count of the record per month.
        ///     Frequency is Daily.
        ///         Not relevant.
        ///     
        /// If CompleteCategory is Number Of Count:
        ///     Frequency is Weekly.
        ///         Sum of record's CompleteFact per week.
        ///     Frequency is Monthly.
        ///         Sum of record's CompleteFact per month.
        ///     Frequency is Daily.
        ///         Sum of record's CompleteFact per day.
        /// 
        /// </summary>
        [Column("CompleteCondition", TypeName = "INT")]
        public Int32 CompleteCondition { get; set; }

        /// <summary>
        /// Start Date.
        ///     Only valid for Weekly and Montly.
        /// </summary>
        [Column("StartDate", TypeName = "INT")]
        public Int32? StartDate { get; set; }

        public ICollection<UserHabitRule> Rules { get; set; }
        public ICollection<UserHabitRecord> Records { get; set; }

        public UserHabit()
        {
            Rules = new HashSet<UserHabitRule>();
            Records = new HashSet<UserHabitRecord>();
            Frequency = HabitFrequency.Daily;
            Category = HabitCategory.Positive;
            CompleteCategory = HabitCompleteCategory.NumberOfTimes;
        }
    }

    [Table("UserHabitRule")]
    public class UserHabitRule
    {
        [Key]
        [Column("HabitID", TypeName = "INT")]
        public Int32 HabitID { get; set; }

        [Key]
        [Column("RuleID", TypeName = "INT")]
        public Int32 RuleID { get; set; }

        [Column("ContinuousRecordFrom", TypeName = "INT")]
        public Int32? ContinuousRecordFrom { get; set; }

        [Column("ContinuousRecordTo", TypeName = "INT")]
        public Int32? ContinuousRecordTo { get; set; }

        [Column("Point", TypeName = "INT")]
        public Int32 Point { get; set; }

        public UserHabit CurrentHabit { get; set; }
    }

    [Table("UserHabitRecord")]
    public class UserHabitRecord
    {
        [Key]
        [Column("HabitID", TypeName = "INT")]
        public Int32 HabitID { get; set; }

        [Key]
        [Column("RecordDate", TypeName = "DATE")]
        public DateTime RecordDate { get; set; }

        [Key]
        [Column("SubID", TypeName = "INT")]
        // Sub. ID: for same Habit occurs several times in same date
        public Int32 SubID { get; set; }

        [Column("CompleteFact", TypeName = "INT")]
        public Int32? CompleteFact { get; set; }

        [Column("RuleID", TypeName = "INT")]
        public Int32? RuleID { get; set; }

        [Column("ContinuousCount", TypeName = "INT")]
        public Int32 ContinuousCount { get; set; }

        [Column("COMMENT", TypeName = "NVARCHAR(50)")]
        public String Comment { get; set; }

        public UserHabit CurrentHabit { get; set; }
    }
}

