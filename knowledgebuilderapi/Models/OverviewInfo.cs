﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace knowledgebuilderapi.Models
{
    public class OverviewInfo
    {
        [Key]
        [Column("RefType")]
        public TagRefType RefType { get; set; }

        [Column("Count")]
        public int Count { get; set; }
    }
}
