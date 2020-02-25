namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class BusinessEntityRestrictionByRoleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the BusinessEntityRestrictionByRoleViewModel class.
        /// </summary>
        public BusinessEntityRestrictionByRoleViewModel()
        {
            DataAction = DataOperation.CREATE;
        }

        /// <summary>
        /// Gets or sets Application
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByRole_Application", ResourceType = typeof(ResourceStrings))]
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets Company
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByRole_Company", ResourceType = typeof(ResourceStrings))]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByRole_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityBusinessEntitiesId
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByRole_LnSecurityBusinessEntitiesId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnSecurityBusinessEntitiesId { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityRolesId
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByRole_LnSecurityRolesId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnSecurityRolesId { get; set; }

        /// <summary>
        /// Gets or sets SecurityBusinessEntitiesObj
        /// </summary>
        public SecurityBusinessEntities SecurityBusinessEntitiesObj { get; set; }

        /// <summary>
        /// Gets or sets SecurityRolesCode
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByRole_SecurityRolesCode", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public string SecurityRolesCode { get; set; }

        /// <summary>
        /// Gets or sets SecurityRolesDescription
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByRole_SecurityRolesDescription", ResourceType = typeof(ResourceStrings))]
        public string SecurityRolesDescription { get; set; }

        /// <summary>
        /// Gets or sets SecurityRolesObj
        /// </summary>
        public SecurityRoles SecurityRolesObj { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByRole_Value", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Remote("IsValueDuplicate", "BusinessEntityRestrictionByRole", AdditionalFields = "Id,LnSecurityRolesId", ErrorMessageResourceName = "Error_Message_Duplicate_Entry_Exists_In_Access_By_Role", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string Value { get; set; }
    }
}