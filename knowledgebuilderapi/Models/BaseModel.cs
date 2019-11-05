using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models {
    public abstract class BaseModel {

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("ModifiedAt")]
        public DateTime ModifiedAt { get; set; }
    }
}
