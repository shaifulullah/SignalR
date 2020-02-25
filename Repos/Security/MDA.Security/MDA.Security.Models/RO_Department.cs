namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For RO_Department
    /// </summary>
    [Table("HRMS.vw_RO_Department")]
    public class RO_Department
    {
        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets SourceId
        /// </summary>
        public int SourceId { get; set; }
    }
}