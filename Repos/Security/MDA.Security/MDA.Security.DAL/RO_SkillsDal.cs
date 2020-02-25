namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class RO_SkillsDal
    {
        /// <summary>
        /// Get Skills For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public RO_Skills GetSkillsForCode(string code)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.RO_SkillsSet.FirstOrDefault(x => x.Skill == code);
            }
        }

        /// <summary>
        /// Get Skills List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_Skills> GetSkillsList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.RO_SkillsSet.ToList();
            }
        }

        /// <summary>
        /// Get Skills Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public DataSourceResult<RO_Skills> GetSkillsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter)
        {
            using (var db = new ApplicationDBContext())
            {
                return db.RO_SkillsSet.ToDataSourceResult(take, skip, sort, filter);
            }
        }
    }
}