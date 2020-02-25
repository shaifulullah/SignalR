namespace MDA.Security.ViewModels
{
    using Resources;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProjectsViewModel
    {
        /// <summary>
        /// Gets or sets BusinessSector
        /// </summary>
        [Display(Name = "Display_Name_Projects_BusinessSector", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(3, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string BusinessSector { get; set; }

        /// <summary>
        /// Gets or sets Category
        /// </summary>
        [Display(Name = "Display_Name_Projects_Category", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(3, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets Customer
        /// </summary>
        [Display(Name = "Display_Name_Projects_Customer", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(15, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [Display(Name = "Display_Name_Projects_Description", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(30, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets EndDate
        /// </summary>
        [Display(Name = "Display_Name_Projects_EndDate", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets Group
        /// </summary>
        [Display(Name = "Display_Name_Projects_Group", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(3, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets Program
        /// </summary>
        [Display(Name = "Display_Name_Projects_Program", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(30, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string Program { get; set; }

        /// <summary>
        /// Gets or sets ProjectManager
        /// </summary>
        [Display(Name = "Display_Name_Projects_ProjectManager", ResourceType = typeof(ResourceStrings))]
        [StringLength(9, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string ProjectManager { get; set; }

        /// <summary>
        /// Gets or sets ProjectNumber
        /// </summary>
        [Display(Name = "Display_Name_Projects_ProjectNumber", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(9, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string ProjectNumber { get; set; }

        /// <summary>
        /// Gets or sets ProjectStatus
        /// </summary>
        [Display(Name = "Display_Name_Projects_ProjectStatus", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        [StringLength(3, ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_String_Length_Max")]
        public string ProjectStatus { get; set; }

        /// <summary>
        /// Gets or sets StartDate
        /// </summary>
        [Display(Name = "Display_Name_Projects_StartDate", ResourceType = typeof(ResourceStrings))]
        [Required(ErrorMessageResourceType = typeof(ResourceStrings), ErrorMessageResourceName = "Error_Message_Required")]
        public DateTime StartDate { get; set; }
    }
}