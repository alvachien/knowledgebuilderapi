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
        Daily = 0,
        Weekly = 1,
        Monthly = 2
    }

    [Table("UserHabit")]
    public class UserHabit
    {
        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Column("Category", TypeName = "SMALLINT")]
        public HabitCategory Category { get; set; }


        [Column("Name", TypeName = "NVARCHAR(50)")]
        public String Name { get; set; }
        [Column("Comment", TypeName = "NVARCHAR(50)")]
        public String Comment { get; set; }

        [Column("TargetUser", TypeName = "NVARCHAR(50)")]
        public String TargetUser { get; set; }

        [Column("ValidFrom", TypeName = "DATETIME")]
        public DateTime ValidFrom { get; set; }

        [Column("ValidTo", TypeName = "DATETIME")]
        public DateTime ValidTo { get; set; }

        [Column("Frequency", TypeName = "SMALLINT")]
        public HabitFrequency Frequency { get; set; }

        [Column("DoneCriteria", TypeName = "INT")]
        public Int32 DoneCriteria { get; set; }

        [Column("StartDate", TypeName = "SMALLINT")]
        public Int32? StartDate { get; set; }

        public ICollection<UserHabitRule> Rules { get; set; }
        public ICollection<UserHabitRecord> Records { get; set; }

        public UserHabit()
        {
            Rules = new HashSet<UserHabitRule>();
            Records = new HashSet<UserHabitRecord>();
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
        public Int32 ContinuousRecordFrom { get; set; }

        [Column("ContinuousRecordTo", TypeName = "INT")]
        public Int32 ContinuousRecordTo { get; set; }

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

        [Column("RuleID", TypeName = "INT")]
        public Int32 RuleID { get; set; }

        [Key]
        [Column("RecordDate", TypeName = "DATE")]
        public DateTime RecordDate { get; set; }

        [Column("COMMENT", TypeName = "NVARCHAR(50)")]
        public String Comment { get; set; }

        public UserHabit CurrentHabit { get; set; }
    }
}

