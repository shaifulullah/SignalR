namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For TreeMenu
    /// </summary>
    [Table("TreeMenu")]
    public class TreeMenu
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
        /// Gets or sets HasChildren
        /// </summary>
        public bool HasChildren { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Image
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// Gets or sets ItemNumber
        /// </summary>
        public int ItemNumber { get; set; }

        /// <summary>
        /// Gets or sets LnParentId
        /// </summary>
        public int LnParentId { get; set; }

        /// <summary>
        /// Gets or sets TreeMenuObj
        /// Foreign Key LnParentId (TreeMenu)
        /// </summary>
        [ForeignKey("LnParentId")]
        public TreeMenu TreeMenuObj { get; set; }

        /// <summary>
        /// Gets or sets URL
        /// </summary>
        public string URL { get; set; }
    }
}