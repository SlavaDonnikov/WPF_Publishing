using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Publishing.EntityData
{
    [Table("Tb_PUBLICATION")]
    public class Publication
    {
        [Key]
        public int PublicationId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "INT")]
        [MaxLength(10)]
        [Required]
        public int ISSN { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(20)]
        [Required]
        public string Genre { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(15)]
        [Required]
        public string Language { get; set; }

        [Column(TypeName = "INT")]
        [MaxLength(20)]
        [Required]
        public int NumberOfCopies { get; set; }

        [Column(TypeName = "INT")]
        [MaxLength(20)]
        [Required]
        public int NumberOfPages { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(15)]
        [Required]
        public string Format { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(10)]
        [Required]
        public string PublicationDate { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]        
        [Required]
        public string DownloadLink { get; set; }

        [ForeignKey("Publisher")]
        public int PublisherRefID { get; set; }
        public virtual Publisher Publisher { get; set; }

        [Column(TypeName = "VARBINARY(MAX)")]   // Cover_Image
        public byte[] Cover { get; set; }

        [Column(TypeName = "DateTime2")]
        [Required]
        public DateTime AddingInDBDate { get; set; }
    }
}
