namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For RO_EmployeeAddInfo
    /// </summary>
    [Table("HRMS.vw_RO_EmployeeAddInfo")]
    public class RO_EmployeeAddInfo
    {
        /// <summary>
        /// Gets or sets ClassificationCode
        /// </summary>
        public string ClassificationCode { get; set; }

        /// <summary>
        /// Gets or sets DirectIndirect
        /// </summary>
        public string DirectIndirect { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets PayGroupDescription
        /// </summary>
        public string PayGroupDescription { get; set; }

        /// <summary>
        /// Gets or sets Rate
        /// </summary>
        public string Rate { get; set; }

        /// <summary>
        /// Gets or sets Skill
        /// </summary>
        public string Skill { get; set; }

        /// <summary>
        /// Gets or sets SourceId
        /// </summary>
        public int SourceId { get; set; }
    }
}