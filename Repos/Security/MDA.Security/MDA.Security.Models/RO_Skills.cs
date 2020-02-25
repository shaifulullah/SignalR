namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For RO_Skills
    /// </summary>
    [Table("HRMS.vw_RO_Skills")]
    public class RO_Skills
    {
        /// <summary>
        /// Gets or sets Skill
        /// </summary>
        [Key]
        public string Skill { get; set; }
    }
}