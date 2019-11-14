using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace knowledgebuilderapi.Models {
    public abstract class BaseModel {

        [Column("CreatedAt")]
        [DataType(DataType.Date)]
        public DateTime? CreatedAt { get; set; }
        [Column("ModifiedAt")]
        [DataType(DataType.Date)]
        public DateTime? ModifiedAt { get; set; }
    }
}
