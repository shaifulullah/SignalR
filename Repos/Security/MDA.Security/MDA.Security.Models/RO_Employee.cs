namespace MDA.Security.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For RO_Employee
    /// </summary>
    [Table("HRMS.vw_RO_Employee")]
    public class RO_Employee
    {
        /// <summary>
        /// Gets or sets Company
        /// </summary>
        public string Company { get; set; }

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
        /// Gets or sets Division
        /// </summary>
        public string Division { get; set; }

        /// <summary>
        /// Gets or sets eMail
        /// </summary>
        public string eMail { get; set; }

        /// <summary>
        /// Gets or sets EmployeeStatus
        /// </summary>
        public string EmployeeStatus { get; set; }

        /// <summary>
        /// Gets or sets EmployeeStatusIdNo
        /// </summary>
        public int EmployeeStatusIdNo { get; set; }

        /// <summary>
        /// Gets or sets EmpNo
        /// </summary>
        public string EmpNo { get; set; }

        /// <summary>
        /// Gets or sets Fax
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Gets or sets FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets FullName
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets FunctionIdNo
        /// </summary>
        public Nullable<int> FunctionIdNo { get; set; }

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets JobCodeIdNo
        /// </summary>
        public Nullable<int> JobCodeIdNo { get; set; }

        /// <summary>
        /// Gets or sets LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets LineManagerIdNo
        /// </summary>
        public Nullable<int> LineManagerIdNo { get; set; }

        /// <summary>
        /// Gets or sets ManagerFirstName
        /// </summary>
        public string ManagerFirstName { get; set; }

        /// <summary>
        /// Gets or sets ManagerLastName
        /// </summary>
        public string ManagerLastName { get; set; }

        /// <summary>
        /// Gets or sets NickName
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Gets or sets Organization
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Gets or sets PayGroupDescription
        /// </summary>
        public string PayGroupDescription { get; set; }

        /// <summary>
        /// Gets or sets PositionIdNo
        /// </summary>
        public Nullable<int> PositionIdNo { get; set; }

        /// <summary>
        /// Gets or sets ProgramManagerName
        /// </summary>
        public string ProgramManagerName { get; set; }

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

        /// <summary>
        /// Gets or sets WorkAddress
        /// </summary>
        public string WorkAddress { get; set; }

        /// <summary>
        /// Gets or sets WorkCountry
        /// </summary>
        public string WorkCountry { get; set; }

        /// <summary>
        /// Gets or sets WorkExt
        /// </summary>
        public string WorkExt { get; set; }

        /// <summary>
        /// Gets or sets WorkLocation
        /// </summary>
        public string WorkLocation { get; set; }

        /// <summary>
        /// Gets or sets WorkPostalCode
        /// </summary>
        public string WorkPostalCode { get; set; }

        /// <summary>
        /// Gets or sets WorkTelephone
        /// </summary>
        public string WorkTelephone { get; set; }
    }
}