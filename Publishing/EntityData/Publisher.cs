using System;
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
        public int PublisherId { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [MaxLength(300)]
        [Required]
        public string Addres { get; set; }

        [Column(TypeName = "VARCHAR")]
        [MaxLength(30)]
        [Required]
        public string Email { get; set; }        

        //[ForeignKey("Publication")]
        //public int PublicationRefID { get; set; }

        public List<Publication> Publications { get; set; }

        [Column(TypeName = "DateTime2")]
        [Required]
        public DateTime AddingInDBDate { get; set; }
    }
}
