namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For CompanyInApplication
    /// </summary>
    [Table("CompanyInApplication")]
    public class CompanyInApplication
    {
        /// <summary>
        /// Gets or sets ApplicationObj
        /// Foreign Key LnApplicationId (Application)
        /// </summary>
        [ForeignKey("LnApplicationId")]
        public Application ApplicationObj { get; set; }

        /// <summary>
        /// Gets or sets CompanyObj
        /// Foreign Key LnCompanyId (Company)
        /// </summary>
        [ForeignKey("LnCompanyId")]
        public Company CompanyObj { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnApplicationId
        /// </summary>
        public int LnApplicationId { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyId
        /// </summary>
        public int LnCompanyId { get; set; }
    }
}