using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models
{
    public class Tag
    {
        public Tag()
        {
        }

        public Tag(Tag other): this()
        {
            this.TagTerm = other.TagTerm;
            this.RefType = other.RefType;
            this.RefID = other.RefID;
        }

        [Key]
        [Column("Tag")]
        public String TagTerm { get; set; }

        [Column("RefType")]
        public TagRefType RefType { get; set; }

        [Column("RefID")]
        public Int32 RefID { get; set; }
    }
}
