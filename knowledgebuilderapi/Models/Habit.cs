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
        GoodHabit = 1,
        BadHabit = 2,
    }

    public enum HabitFrequency: Int16
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }

    public class HabitItem
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

        public ICollection<HabitItemRule> Rules { get; set; }

        public HabitItem()
        {
            Rules = new HashSet<HabitItemRule>();
        }
    }

    public class HabitItemRule
    {
        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Column("TimesFrom", TypeName = "INT")]
        public Int32 MeetTimesFrom { get; set; }

        [Column("TimesTo", TypeName = "INT")]
        public Int32 MeetTimesTo { get; set; }

        [Column("Point", TypeName = "INT")]
        public Int32 Point { get; set; }

        public HabitItem CurrentHabit { get; set; }
    }
}

