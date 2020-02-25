namespace MDA.Security.ViewModels
{
    using Core.Helpers;
    using Models;
    using Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class UserApplicationSettingsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the UserApplicationSettingsViewModel class.
        /// </summary>
        public UserApplicationSettingsViewModel()
        {
            DataAction = DataOperation.CREATE;
        }

        /// <summary>
        /// Gets or sets Company List
        /// </summary>
        public IEnumerable<SelectListItem> CompanyList { get; set; }

        /// <summary>
        /// Gets or sets CompanyObj
        /// </summary>
        public Company CompanyObj { get; set; }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_UserApplicationSettings_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets Language
        /// </summary>
        [Display(Name = "Display_Name_UserApplicationSettings_Language", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Language { get; set; }

        /// <summary>
        /// Gets Language List
        /// </summary>
        public IEnumerable<SelectListItem> LanguageList
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem { Text = ResourceStrings.Text_Language_English, Value = LanguageCode.English },
                    new SelectListItem { Text = ResourceStrings.Text_Language_French, Value = LanguageCode.French } };
            }
        }

        /// <summary>
        /// Gets or sets LnCompanyId
        /// </summary>
        [Display(Name = "Display_Name_UserApplicationSettings_LnCompanyId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnCompanyId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        [Display(Name = "Display_Name_UserApplicationSettings_LnUserAccountId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets Project
        /// </summary>
        [Display(Name = "Display_Name_UserApplicationSettings_Project", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets UserAccountObj
        /// </summary>
        public UserAccount UserAccountObj { get; set; }

        /// <summary>
        /// Gets or sets UserMenuLocation
        /// </summary>
        [Display(Name = "Display_Name_UserApplicationSettings_UserMenuLocation", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string UserMenuLocation { get; set; }

        /// <summary>
        /// Gets UserMenuLocation List
        /// </summary>
        public IEnumerable<SelectListItem> UserMenuLocationList
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem { Text = ResourceStrings.Text_UserMenuLocation_Vertical, Value =  TreeMenuLocation.Vertical },
                    new SelectListItem { Text = ResourceStrings.Text_UserMenuLocation_Horizontal, Value = TreeMenuLocation.Horizontal } };
            }
        }
    }
}