using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    public class Tag
    {
        [Key]
        [Column("Tag")]
        public String TagTerm { get; set; }

        [Column("RefType")]
        public TagRefType RefType { get; set; }

        [Column("RefID")]
        public Int32 RefID { get; set; }
    }
}
