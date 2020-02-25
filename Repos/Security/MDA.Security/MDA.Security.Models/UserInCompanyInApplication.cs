namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For UserInCompanyInApplication
    /// </summary>
    [Table("UserInCompanyInApplication")]
    public class UserInCompanyInApplication
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
        /// Gets or sets LnCompanyInApplicationId
        /// </summary>
        public int LnCompanyInApplicationId { get; set; }

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