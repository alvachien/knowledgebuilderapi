using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace knowledgebuilderapi.Models
{
    [Table("ExerciseTag")]
    public class ExerciseTag
    {
        public ExerciseTag()
        {
        }
        public ExerciseTag(ExerciseTag other)
        {
            this.TagTerm = other.TagTerm;
            this.RefID = other.RefID;
        }

        [Key]
        [MaxLength(20)]
        [Column("Tag", TypeName = "NVARCHAR(20)")]
        public String TagTerm { get; set; }

        [Key]
        [Required]
        [Column("RefID", TypeName = "INT")]
        public Int32 RefID { get; set; }

        public ExerciseItem CurrentExerciseItem { get; set; }
    }
}
