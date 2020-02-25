namespace MDA.Security.ViewModels
{
    using Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class SecurityReportViewModel
    {
        /// <summary>
        /// Gets or sets Application List
        /// </summary>
        public IEnumerable<SelectListItem> ApplicationList { get; set; }

        /// <summary>
        /// Gets or sets Company List
        /// </summary>
        public IEnumerable<SelectListItem> CompanyList { get; set; }

        /// <summary>
        /// Gets or sets LnApplicationId
        /// </summary>
        [Display(Name = "Display_Name_SecurityReport_LnApplicationId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnApplicationId { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyId
        /// </summary>
        [Display(Name = "Display_Name_SecurityReport_LnCompanyId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnCompanyId { get; set; }
    }
}