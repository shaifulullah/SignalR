namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class SecurityRolesViewModel
    {
        /// <summary>
        /// Initializes a new instance of the SecurityRolesViewModel class.
        /// </summary>
        public SecurityRolesViewModel()
        {
            DataAction = DataOperation.CREATE;
        }

        /// <summary>
        /// Gets or sets Code
        /// </summary>
        [Display(Name = "Display_Name_SecurityRoles_Code", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Remote("IsCodeDuplicate", "SecurityRoles", AdditionalFields = "Id", ErrorMessageResourceName = "Error_Message_Duplicate_Entry", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets CompanyInApplicationObj
        /// </summary>
        public CompanyInApplication CompanyInApplicationObj { get; set; }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [Display(Name = "Display_Name_SecurityRoles_Description", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_SecurityRoles_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets IsDeleted
        /// </summary>
        [Display(Name = "Display_Name_SecurityRoles_IsDeleted", ResourceType = typeof(ResourceStrings))]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets LnActiveDirectoryGroupName
        /// </summary>
        [Display(Name = "Display_Name_SecurityRoles_LnActiveDirectoryGroupName", ResourceType = typeof(ResourceStrings))]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string LnActiveDirectoryGroupName { get; set; }

        /// <summary>
        /// Gets or sets LnCompanyInApplicationId
        /// </summary>
        [Display(Name = "Display_Name_SecurityRoles_LnCompanyInApplicationId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnCompanyInApplicationId { get; set; }

        /// <summary>
        /// Gets or sets LnSkillCode
        /// </summary>
        [Display(Name = "Display_Name_SecurityRoles_LnSkillCode", ResourceType = typeof(ResourceStrings))]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string LnSkillCode { get; set; }
    }
}