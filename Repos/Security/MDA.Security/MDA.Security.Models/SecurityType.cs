namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For SecurityType
    /// </summary>
    [Table("SecurityType")]
    public class SecurityType
    {
        /// <summary>
        /// Gets or sets Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }
    }
}