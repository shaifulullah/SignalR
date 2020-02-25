namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class UserAccountViewModel
    {
        /// <summary>
        /// Initializes a new instance of the UserAccountViewModel class.
        /// </summary>
        public UserAccountViewModel()
        {
            DataAction = DataOperation.CREATE;
            IsRecordDeleted = false;
        }

        /// <summary>
        /// Gets or sets Company
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_Company", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets CompanyObj
        /// </summary>
        public Company CompanyObj { get; set; }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets Domain
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_Domain", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets Domain List
        /// </summary>
        public IEnumerable<SelectListItem> DomainList { get; set; }

        /// <summary>
        /// Gets or Sets Employee Number
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_EmployeeNumber", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [Remote("IsEmployeeNumberDuplicate", "UserAccount", AdditionalFields = "Id,LnEmployeeId", ErrorMessageResourceName = "Error_Message_Duplicate_Entry", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// Gets or sets ExternalPersonObj
        /// </summary>
        public ExternalPerson ExternalPersonObj { get; set; }

        /// <summary>
        /// Gets or Sets Full Name
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_FullName", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets IsRecordDeleted
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_IsRecordDeleted", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public bool IsRecordDeleted { get; set; }

        /// <summary>
        /// Gets or sets LnDefaultCompanyId
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_LnDefaultCompanyId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnDefaultCompanyId { get; set; }

        /// <summary>
        /// Gets or sets LnEmployeeId
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_LnEmployeeId", ResourceType = typeof(ResourceStrings))]
        public int LnEmployeeId { get; set; }

        /// <summary>
        /// Gets or sets LnExternalPersonId
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_LnExternalPersonId", ResourceType = typeof(ResourceStrings))]
        public int LnExternalPersonId { get; set; }

        /// <summary>
        /// Gets or Sets Person Code
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_PersonCode", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [Remote("IsPersonCodeDuplicate", "UserAccount", AdditionalFields = "Id,LnExternalPersonId", ErrorMessageResourceName = "Error_Message_Duplicate_Entry", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string PersonCode { get; set; }

        /// <summary>
        /// User Account Type
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_UserAccountType", ResourceType = typeof(ResourceStrings))]
        public UserAccountType UserAccountType { get; set; }

        /// <summary>
        /// Gets the UserAccountType List
        /// </summary>
        public IEnumerable<SelectListItem> UserAccountTypeList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = ResourceStrings.Text_Internal_User, Value = "1" },
                    new SelectListItem { Text = ResourceStrings.Text_External_User, Value = "2" }
                };
            }
        }

        /// <summary>
        /// Gets or sets UserName
        /// </summary>
        [Display(Name = "Display_Name_UserAccount_UserName", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(64, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Remote("IsCodeDuplicate", "UserAccount", AdditionalFields = "Id", ErrorMessageResourceName = "Error_Message_Duplicate_Entry", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string UserName { get; set; }
    }
}