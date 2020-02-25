namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserSettings
    /// </summary>
    [Table("UserSettings")]
    public class UserSettings
    {
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
        /// Gets or sets LnUserInCompanyInApplicationId
        /// </summary>
        public int LnUserInCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets Separator
        /// </summary>
        public string Separator { get; set; }

        /// <summary>
        /// Gets or sets UserInCompanyInApplicationObj
        /// Foreign Key LnUserInCompanyInApplicationId (UserInCompanyInApplication)
        /// </summary>
        [ForeignKey("LnUserInCompanyInApplicationId")]
        public UserInCompanyInApplication UserInCompanyInApplicationObj { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public string Value { get; set; }
    }
}