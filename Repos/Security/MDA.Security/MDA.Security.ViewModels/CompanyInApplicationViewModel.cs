namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class CompanyInApplicationViewModel
    {
        /// <summary>
        /// Gets or sets ApplicationObj
        /// </summary>
        public Application ApplicationObj { get; set; }

        /// <summary>
        /// Gets or sets CompanyObj
        /// </summary>
        public Company CompanyObj { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_CompanyInApplication_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnApplicationId
        /// </summary>
        [Display(Name = "Display_Name_CompanyInApplication_LnApplicationId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnApplicationId { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyId
        /// </summary>
        [Display(Name = "Display_Name_CompanyInApplication_LnCompanyId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnCompanyId { get; set; }
    }
}