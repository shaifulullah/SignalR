namespace MDA.Security.ViewModels
{
    using Resources;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class ExternalCompanyViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ExternalCompanyViewModel class.
        /// </summary>
        public ExternalCompanyViewModel()
        {
            DataAction = DataOperation.CREATE;
        }

        /// <summary>
        /// Gets or sets Code
        /// </summary>
        [Display(Name = "Display_Name_ExternalCompany_Code", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Remote("IsCodeDuplicate", "ExternalCompany", AdditionalFields = "Id", ErrorMessageResourceName = "Error_Message_Duplicate_Entry", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [Display(Name = "Display_Name_ExternalCompany_Description", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_ExternalCompany_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }
    }
}