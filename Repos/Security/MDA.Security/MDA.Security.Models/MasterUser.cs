namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For MasterUser
    /// </summary>
    [Table("MasterUser")]
    public class MasterUser
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        public int LnUserAccountId { get; set; }
    }
}