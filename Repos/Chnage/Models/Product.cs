using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public ICollection<ProductECR> ECRs { get; set; }
        public ICollection<ProductECO> ECOs { get; set; }
    }
    public class ProductECR
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ECRId { get; set; }
        public ECR ECR { get; set; }
    }
    public class ProductECO
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ECOId { get; set; }
        public ECO ECO { get; set; }
    }
}
