namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserApplicationFavourites
    /// </summary>
    [Table("UserApplicationFavourites")]
    public class UserApplicationFavourites
    {
        /// <summary>
        /// Gets or sets ApplicationObj
        /// Foreign Key LnApplicationId (Application)
        /// </summary>
        [ForeignKey("LnApplicationId")]
        public Application ApplicationObj { get; set; }

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
        /// Gets or sets LnUserAccountId
        /// </summary>
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets UserAccountObj
        /// Foreign Key LnUserAccountId (UserAccount)
        /// </summary>
        [ForeignKey("LnUserAccountId")]
        public UserAccount UserAccountObj { get; set; }
    }
}