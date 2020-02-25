namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ProjectsDal
    {
        /// <summary>
        /// Get Projects For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public Projects GetProjectsForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.ProjectsSet.FirstOrDefault(x => x.ProjectNumber == code);
            }
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
            using (var db = new ApplicationDBContext())
            {
                return db.ProjectsSet.Where(x => !string.IsNullOrEmpty(x.ProjectNumber)).ToDataSourceResult(take, skip, sort, filter);
            }
        }
    }
}