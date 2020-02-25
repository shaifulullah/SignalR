namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IProjectsBll
    {
        /// <summary>
        /// Get Projects For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        Projects GetProjectsForCode(string code);

        /// <summary>
        /// Get Projects Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<Projects> GetProjectsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter);
    }
}