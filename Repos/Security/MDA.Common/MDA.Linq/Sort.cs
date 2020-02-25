//-----------------------------------------------------------------------
// <copyright file="Sort.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MDA.Linq
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a sort expression of Kendo DataSource.
    /// </summary>
    [DataContract]
    public class Sort
    {
        /// <summary>
        /// Gets or sets the sort direction. Should be either "asc" or "desc".
        /// </summary>
        [DataMember(Name = "Dir")]
        public string Dir { get; set; }

        /// <summary>
        /// Gets or sets the name of the sorted field (property).
        /// </summary>
        [DataMember(Name = "Field")]
        public string Field { get; set; }

        /// <summary>
        /// Converts to form required by Dynamic Linq e.g. "Field1 desc"
        /// </summary>
        /// <returns>Sort as string</returns>
        public string ToExpression()
        {
            return string.Format("{0} {1}", Field, Dir);
        }
    }
}