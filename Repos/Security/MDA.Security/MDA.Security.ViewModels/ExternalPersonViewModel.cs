namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class ExternalPersonViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ExternalPersonViewModel class.
        /// </summary>
        public ExternalPersonViewModel()
        {
            DataAction = DataOperation.CREATE;
            IsActive = true;
        }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets eMail
        /// </summary>
        [Display(Name = "Display_Name_ExternalPerson_eMail", ResourceType = typeof(ResourceStrings))]
        [StringLength(128, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Invalid_Email")]
        public string eMail { get; set; }

        /// <summary>
        /// Gets or sets ExternalCompanyObj
        /// </summary>
        public ExternalCompany ExternalCompanyObj { get; set; }

        /// <summary>
        /// Gets or sets FirstName
        /// </summary>
        [Display(Name = "Display_Name_ExternalPerson_FirstName", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string FirstName { get; set; }

        /// <summary>
        /// FullName for Display purposes only
        /// </summary>
        [Display(Name = "Display_Name_ExternalPerson_FullName", ResourceType = typeof(ResourceStrings))]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_ExternalPerson_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets IsActive
        /// </summary>
        [Display(Name = "Display_Name_ExternalPerson_IsActive", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets LastName
        /// </summary>
        [Display(Name = "Display_Name_ExternalPerson_LastName", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets LnExternalCompanyId
        /// </summary>
        [Display(Name = "Display_Name_ExternalPerson_LnExternalCompanyId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnExternalCompanyId { get; set; }

        /// <summary>
        /// Gets or sets PersonCode
        /// </summary>
        [Display(Name = "Display_Name_ExternalPerson_PersonCode", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Remote("IsCodeDuplicate", "ExternalPerson", AdditionalFields = "Id", ErrorMessageResourceName = "Error_Message_Duplicate_Entry", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string PersonCode { get; set; }
    }
}