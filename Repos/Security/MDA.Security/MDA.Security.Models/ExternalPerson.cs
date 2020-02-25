namespace MDA.Security.Models
{
    using Microsoft.Linq.Translations;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model For ExternalPerson
    /// </summary>
    [Table("ExternalPerson")]
    public class ExternalPerson
    {
        /// <summary>
        /// Full Name Expression
        /// </summary>
        private static readonly CompiledExpression<ExternalPerson, string> FullNameExpression =
            DefaultTranslationOf<ExternalPerson>.Property(x => x.FullName).Is(x => x.FirstName.Trim() + " " + x.LastName.Trim());

        /// <summary>
        /// Gets or sets eMail
        /// </summary>
        public string eMail { get; set; }

        /// <summary>
        /// Gets or sets ExternalCompanyObj
        /// Foreign Key LnExternalCompanyId (ExternalCompany)
        /// </summary>
        [ForeignKey("LnExternalCompanyId")]
        public ExternalCompany ExternalCompanyObj { get; set; }

        /// <summary>
        /// Gets or sets FirstName
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// FullName for Display purposes only
        /// </summary>
        [NotMapped]
        public string FullName { get { return FullNameExpression.Evaluate(this); } set { } }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets IsActive
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets LastName
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets LnExternalCompanyId
        /// </summary>
        public int LnExternalCompanyId { get; set; }

        /// <summary>
        /// Gets or sets PersonCode
        /// </summary>
        public string PersonCode { get; set; }
    }
}