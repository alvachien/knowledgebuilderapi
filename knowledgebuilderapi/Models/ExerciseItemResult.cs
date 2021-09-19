using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    public class ExerciseItemResult
    {
        [Key]
        [Column("ID", TypeName = "INT")]
        public Int32 ID { get; set; }

        [Required]
        [Column("User", TypeName = "NVARHCAR(50)")]
        public string User { get; set; }

        [Required]
        [Column("ResultDate")]
        [DataType(DataType.Date)]
        public DateTime ResultDate { get; set; }

        [Required]
        [Column("ResultScore", TypeName = "INT")]
        public int ResultScore { get; set; }
    }
}
