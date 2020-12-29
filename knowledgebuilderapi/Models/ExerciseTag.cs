using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace knowledgebuilderapi.Models
{
    [Table("ExerciseTag")]
    public class ExerciseTag
    {
        [Key]
        [MaxLength(20)]
        [Column("Tag", TypeName = "NVARCHAR(20)")]
        public String TagTerm { get; set; }

        [Required]
        [Column("RefID", TypeName = "INT")]
        public Int32 RefID { get; set; }

        public ExerciseItem CurrentExerciseItem { get; set; }
    }
}
