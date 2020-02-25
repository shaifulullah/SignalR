namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.ComponentModel.DataAnnotations;

    public class BusinessEntityRestrictionByADViewModel
    {
        /// <summary>
        /// Initializes a new instance of the BusinessEntityRestrictionByADViewModel class.
        /// </summary>
        public BusinessEntityRestrictionByADViewModel()
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
        [Display(Name = "Display_Name_BusinessEntityRestrictionByAD_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnActiveDirectoryGroupName
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByAD_LnActiveDirectoryGroupName", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string LnActiveDirectoryGroupName { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityBusinessEntitiesId
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByAD_LnSecurityBusinessEntitiesId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnSecurityBusinessEntitiesId { get; set; }

        /// <summary>
        /// Gets or sets SecurityBusinessEntitiesObj
        /// </summary>
        public SecurityBusinessEntities SecurityBusinessEntitiesObj { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        [Display(Name = "Display_Name_BusinessEntityRestrictionByAD_Value", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Value { get; set; }
    }
}