namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class BusinessEntityRestrictionByUserViewModel
    {
        /// <summary>
        /// Initializes a new instance of the BusinessEntityRestrictionByUserViewModel class.
        /// </summary>
        public BusinessEntityRestrictionByUserViewModel()
        {
            DataAction = DataOperation.CREATE;
        }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets FullName
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByUser_FullName", ResourceType = typeof(ResourceStrings))]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByUser_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityBusinessEntitiesId
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByUser_LnSecurityBusinessEntitiesId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnSecurityBusinessEntitiesId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByUser_LnUserAccountId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets SecurityBusinessEntitiesObj
        /// </summary>
        public SecurityBusinessEntities SecurityBusinessEntitiesObj { get; set; }

        /// <summary>
        /// Gets or sets UserAccountObj
        /// </summary>
        public UserAccount UserAccountObj { get; set; }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByUser_UserName", ResourceType = typeof(ResourceStrings))]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByUser_Value", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Remote("IsValueDuplicate", "BusinessEntityRestrictionByUser", AdditionalFields = "Id,LnUserAccountId", ErrorMessageResourceName = "Error_Message_Duplicate_Entry_Exists_In_Access_By_User", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string Value { get; set; }
    }
}