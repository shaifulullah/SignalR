namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For ApplicationSettings
    /// </summary>
    [Table("ApplicationSettings")]
    public class ApplicationSettings
    {
        /// <summary>
        /// Gets or sets CompanyInApplicationObj
        /// Foreign Key LnCompanyInApplicationId (CompanyInApplication)
        /// </summary>
        [ForeignKey("LnCompanyInApplicationId")]
        public CompanyInApplication CompanyInApplicationObj { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets KeyName
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyInApplicationId
        /// </summary>
        public int LnCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets Separator
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
}