﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Publishing.EntityData
{
    [Table("Tb_PUBLISHER")]
    public class Publisher
    {
        [Key]
        [Column(Order = 0)]
        public int PublisherId { get; set; }

        [Column(TypeName = "NVARCHAR", Order = 1)]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "NVARCHAR", Order = 2)]
        [MaxLength(300)]
        [Required]
        public string Addres { get; set; }

        [Column(TypeName = "VARCHAR", Order = 3)]
        [MaxLength(30)]
        [Required]
        public string Email { get; set; }

        [Column(TypeName = "DateTime2", Order = 4)]
        [Required]
        public DateTime AddingInDBDate { get; set; }

        [Timestamp]
        [Column(Order = 5)]
        public byte[] RowVersion { get; set; }

        public List<Publication> Publications { get; set; }
    }
}