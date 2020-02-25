namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class UserSettingsViewModel
    {
        /// <summary>
        /// Initializes a new instance of the UserSettingsViewModel class.
        /// </summary>
        public UserSettingsViewModel()
        {
            DataAction = DataOperation.CREATE;
        }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_UserSettings_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets KeyName
        /// </summary>
        [Display(Name = "Display_Name_UserSettings_KeyName", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Remote("IsKeyNameDuplicate", "UserSettings", AdditionalFields = "Id", ErrorMessageResourceName = "Error_Message_Duplicate_Entry", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string KeyName { get; set; }

        /// <summary>
        /// Gets or sets LnUserInCompanyInApplicationId
        /// </summary>
        [Display(Name = "Display_Name_UserSettings_LnUserInCompanyInApplicationId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnUserInCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets Separator
        /// </summary>
        [Display(Name = "Display_Name_UserSettings_Separator", ResourceType = typeof(ResourceStrings))]
        [StringLength(8, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Separator { get; set; }

        /// <summary>
        /// Gets or sets UserInCompanyInApplicationObj
        /// </summary>
        public UserInCompanyInApplication UserInCompanyInApplicationObj { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [Display(Name = "Display_Name_UserSettings_Value", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Value { get; set; }
    }
}