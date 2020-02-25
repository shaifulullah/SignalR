namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class UserInCompanyInApplicationViewModel
    {
        /// <summary>
        /// Gets or sets CompanyInApplicationObj
        /// </summary>
        public CompanyInApplication CompanyInApplicationObj { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_UserInCompanyInApplication_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyInApplicationId
        /// </summary>
        [Display(Name = "Display_Name_UserInCompanyInApplication_LnCompanyInApplicationId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        [Display(Name = "Display_Name_UserInCompanyInApplication_LnUserAccountId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets UserAccountDetailsObj
        /// </summary>
        public UserAccountDetails UserAccountDetailsObj { get; set; }
    }
}