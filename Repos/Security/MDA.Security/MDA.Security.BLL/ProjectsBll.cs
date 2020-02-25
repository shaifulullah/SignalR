namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class ProjectsBll : IProjectsBll
    {
        /// <summary>
        /// Get Projects For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public Projects GetProjectsForCode(string code)
        {
            var projectsDal = new ProjectsDal();
            return projectsDal.GetProjectsForCode(code);
        }

        /// <summary>
        /// Get Projects Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<Projects> GetProjectsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            var projectsDal = new ProjectsDal();
            return projectsDal.GetProjectsPage(take, skip, sort, filter);
        }
    }
}