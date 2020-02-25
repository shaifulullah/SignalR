namespace MDA.Security.ViewModels
{
    using Resources;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class ConfigurationViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ConfigurationViewModel class.
        /// </summary>
        public ConfigurationViewModel()
        {
            DataAction = DataOperation.CREATE;
            ConfigurationTypeName = string.Empty;
        }

        /// <summary>
        /// Gets or sets Code
        /// </summary>
        [StringLength(63, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [Display(Name = "Display_Name_Configuration_Code", ResourceType = typeof(ResourceStrings))]
        [Remote("IsCodeDuplicate", "Configuration", AdditionalFields = "Id", ErrorMessageResourceName = "Error_Message_Duplicate_Entry", ErrorMessageResourceType = typeof(ResourceStrings))]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets Configuration Type Name
        /// </summary>
        public string ConfigurationTypeName { get; set; }

        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [StringLength(255, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [Display(Name = "Display_Name_Configuration_Id", ResourceType = typeof(ResourceStrings))]
        public int Id { get; set; }
    }
}