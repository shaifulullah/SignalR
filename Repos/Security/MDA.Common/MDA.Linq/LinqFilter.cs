//-----------------------------------------------------------------------
// <copyright file="ApplicationFilter.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MDA.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a filter expression of Kendo DataSource.
    /// </summary>
    [DataContract]
    public class LinqFilter
    {
        /// <summary>
        /// Mapping of Kendo DataSource filtering operators to Dynamic Linq
        /// </summary>
        private static readonly IDictionary<string, string> Operators = new Dictionary<string, string>
        {
            { "eq", "=" },
            { "neq", "!=" },
            { "lt", "<" },
            { "lte", "<=" },
            { "gt", ">" },
            { "gte", ">=" },
            { "startswith", "StartsWith" },
            { "endswith", "EndsWith" },
            { "contains", "Contains" },
            { "doesnotcontain", "DoesNotContain" },
            { "isnull", "IsNull" },
            { "isnotnull", "IsNotNull" },
            { "isempty", "IsEmpty" },
            { "isnotempty", "IsNotEmpty" }
        };

        /// <summary>
        /// Gets or sets the name of the sorted field (property). Set to <c>null</c> if the
        /// <c>Filters</c> property is set.
        /// </summary>
        [DataMember(Name = "Field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the child filter expressions. Set to <c>null</c> if there are no child expressions.
        /// </summary>
        [DataMember(Name = "Filters")]
        public IEnumerable<LinqFilter> Filters { get; set; }

        /// <summary>
        /// Gets or sets the filtering logic. Can be set to "or" or "and". Set to <c>null</c> unless
        /// <c>Filters</c> is set.
        /// </summary>
        [DataMember(Name = "Logic")]
        public string Logic { get; set; }

        /// <summary>
        /// Gets or sets the filtering operator. Set to <c>null</c> if the <c>Filters</c> property
        /// is set.
        /// </summary>
        [DataMember(Name = "Operator")]
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the filtering value. Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        [DataMember(Name = "Value")]
        public object Value { get; set; }

        /// <summary>
        /// Get a flattened list of all child filter expressions.
        /// </summary>
        /// <returns>List of Filters</returns>
        public IList<LinqFilter> All()
        {
            var filters = new List<LinqFilter>();
            Collect(filters);

            return filters;
        }

        /// <summary>
        /// Converts the filter expression to a predicate suitable for Dynamic Linq e.g. "Field1 =
        /// @1 and Field2.Contains(@2)"
        /// </summary>
        /// <param name="filters">A list of flattened filters.</param>
        /// <returns>Filter as string</returns>
        public string ToExpression(IList<LinqFilter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                return "(" + string.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression(filters)).ToArray()) + ")";
            }

            var index = filters.IndexOf(this);

            var comparison = Operators[Operator];
            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                return string.Format("{0}.ToLower().{1}(@{2}.ToLower())", Field, comparison, index);
            }
            if (comparison == "DoesNotContain")
            {
                return string.Format("!{0}.ToLower().Contains(@{1}.ToLower())", Field, index);
            }

            if (comparison == "IsNotNull" || comparison == "IsNotEmpty")
            {
                return string.Format("{0}.ToLower() {1} @{2}", Field, "!=", index);
            }
            if (comparison == "IsNull" || comparison == "IsEmpty")
            {
                return string.Format("{0}.ToLower() {1} @{2}", Field, '=', index);
            }

            return string.Format("{0} {1} @{2}", Field, comparison, index);
        }

        /// <summary>
        /// Collect Filters
        /// </summary>
        /// <param name="filters">List of Filters</param>
        private void Collect(IList<LinqFilter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                foreach (var filter in Filters)
                {
                    filters.Add(filter);
                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }
    }
}