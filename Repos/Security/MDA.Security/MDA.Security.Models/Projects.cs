namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For Projects
    /// </summary>
    [Table("LN.vw_Projects")]
    public class Projects
    {
        /// <summary>
        /// Gets or sets BusinessSector
        /// </summary>
        public string BusinessSector { get; set; }

        /// <summary>
        /// Gets or sets Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets Customer
        /// </summary>
        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets EndDate
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets Group
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets Program
        /// </summary>
        public string Program { get; set; }

        /// <summary>
        /// Gets or sets ProjectManager
        /// </summary>
        public string ProjectManager { get; set; }

        /// <summary>
        /// Gets or sets ProjectNumber
        /// </summary>
        [Key]
        public string ProjectNumber { get; set; }

        /// <summary>
        /// Gets or sets ProjectStatus
        /// </summary>
        public string ProjectStatus { get; set; }

        /// <summary>
        /// Gets or sets StartDate
        /// </summary>
        public DateTime StartDate { get; set; }
    }
}