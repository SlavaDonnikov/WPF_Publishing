﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Publishing.EntityData
{
    [Table("Tb_Publications")]
    public class Publication
    {
        [Key]
        [Column(Order = 0)]
        public int PublicationId { get; set; }

        [Column(TypeName = "NVARCHAR", Order = 1)]        
        [MaxLength(20)]       
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "INT", Order = 2)]
        [MaxLength(10)]
        [Required]
        public int ISSN { get; set; }

        [Column(TypeName = "NVARCHAR", Order = 3)]
        [MaxLength(20)]
        [Required]
        public string Genre { get; set; }

        [Column(TypeName = "NVARCHAR", Order = 4)]
        [MaxLength(15)]
        [Required]
        public string Language { get; set; }

        [Column(TypeName = "INT", Order = 5)]
        [MaxLength(20)]
        [Required]
        public int NumberOfCopies { get; set; }

        [Column(TypeName = "INT", Order = 6)]
        [MaxLength(20)]
        [Required]
        public int NumberOfPages { get; set; }

        [Column(TypeName = "NVARCHAR", Order = 7)]
        [MaxLength(15)]
        [Required]
        public string Format { get; set; }

        [Column(TypeName = "VARCHAR(MAX)", Order = 8)]
        [Required]
        public string DownloadLink { get; set; }


        [ForeignKey("Publisher")]
        [Column(Order = 9)]
        public int PublisherRefId { get; set; }
        public virtual Publisher Publisher { get; set; }


        [Column(TypeName = "VARBINARY(MAX)", Order = 10)]   // Cover_Image
        public byte[] Cover { get; set; }

        [Column(TypeName = "VARCHAR", Order = 11)]
        [MaxLength(10)]
        [Required]
        public string PublicationDate { get; set; }

        [Column(TypeName = "DateTime2", Order = 12)]
        [Required]
        public DateTime AddingInDBDate { get; set; }

        [Timestamp]
        [Column(Order = 13)]
        public byte[] RowVersion { get; set; }
    }
}
