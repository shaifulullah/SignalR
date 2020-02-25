namespace MDA.Security.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For RO_EmployeeDetails
    /// </summary>
    [Table("HRMS.vw_RO_EmployeeDetails")]
    public class RO_EmployeeDetails
    {
        /// <summary>
        /// Gets or sets ClassOne
        /// </summary>
        public string ClassOne { get; set; }

        /// <summary>
        /// Gets or sets CompanyNumber
        /// </summary>
        public string CompanyNumber { get; set; }

        /// <summary>
        /// Gets or sets Department
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// Gets or sets DepartmentIdNo
        /// </summary>
        public int DepartmentIdNo { get; set; }

        /// <summary>
        /// Gets or sets DirectIndirect
        /// </summary>
        public string DirectIndirect { get; set; }

        /// <summary>
        /// Gets or sets EmployeeNumber
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// Gets or sets EmployeeStatus
        /// </summary>
        public string EmployeeStatus { get; set; }

        /// <summary>
        /// Gets or sets FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets GradeCode
        /// </summary>
        public string GradeCode { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets LocationId
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Gets or sets ManagerFirstName
        /// </summary>
        public string ManagerFirstName { get; set; }

        /// <summary>
        /// Gets or sets ManagerLastName
        /// </summary>
        public string ManagerLastName { get; set; }

        /// <summary>
        /// Gets or sets PayGroupDescription
        /// </summary>
        public string PayGroupDescription { get; set; }

        /// <summary>
        /// Gets or sets RateCode
        /// </summary>
        public string RateCode { get; set; }

        /// <summary>
        /// Gets or sets SkillCode
        /// </summary>
        public string SkillCode { get; set; }

        /// <summary>
        /// Gets or sets SourceId
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// Gets or sets StatusValue
        /// </summary>
        public int StatusValue { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public string Title { get; set; }
    }
}