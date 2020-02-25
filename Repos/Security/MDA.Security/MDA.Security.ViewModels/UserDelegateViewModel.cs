namespace MDA.Security.ViewModels
{
    using Models;
    using Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserDelegateViewModel
    {
        /// <summary>
        /// Initializes a new instance of the UserDelegateViewModel class.
        /// </summary>
        public UserDelegateViewModel()
        {
            DataAction = DataOperation.CREATE;
        }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets DateTimeEnd
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_DateTimeEnd", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        //[GreaterThanDate("DateTimeStart", false)]
        public Nullable<DateTime> DateTimeEnd { get; set; }

        /// <summary>
        /// Gets or sets DateTimeStart
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_DateTimeStart", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public Nullable<DateTime> DateTimeStart { get; set; }

        /// <summary>
        /// Gets or sets DelegateFlagIsApprover
        /// </summary>
        public bool DelegateFlagIsApprover { get; set; }

        /// <summary>
        /// Gets or sets DelegateUserAccountObj
        /// </summary>
        public UserAccount DelegateUserAccountObj { get; set; }

        /// <summary>
        /// Gets or sets DelegateUserName
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_DelegateUserName", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public string DelegateUserName { get; set; }

        /// <summary>
        /// Gets or sets FlagIsApprover
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_FlagIsApprover", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public bool FlagIsApprover { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_Id", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LnDelegateUserAccountId
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_LnDelegateUserAccountId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnDelegateUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets LnSecurityRolesId
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_LnSecurityRolesId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnSecurityRolesId { get; set; }

        /// <summary>
        /// Gets or sets LnUserAccountId
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_LnUserAccountId", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public int LnUserAccountId { get; set; }

        /// <summary>
        /// Gets or sets SecurityRoles
        /// </summary>
        [Display(Name = "Display_Name_UserDelegate_SecurityRoles", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public string SecurityRoles { get; set; }

        /// <summary>
        /// Gets or sets SecurityRolesObj
        /// </summary>
        public SecurityRoles SecurityRolesObj { get; set; }

        /// <summary>
        /// Gets or sets UserAccountObj
        /// </summary>
        public UserAccount UserAccountObj { get; set; }
    }
}