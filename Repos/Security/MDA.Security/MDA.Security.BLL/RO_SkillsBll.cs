namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;

    public class RO_SkillsBll : IRO_SkillsBll
    {
        /// <summary>
        /// Get Skills For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        public RO_Skills GetSkillsForCode(string code)
        {
            var rO_SkillsDal = new RO_SkillsDal();
            return rO_SkillsDal.GetSkillsForCode(code);
        }

        /// <summary>
        /// Get Skills List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<RO_Skills> GetSkillsList()
        {
            var rO_SkillsDal = new RO_SkillsDal();
            return rO_SkillsDal.GetSkillsList();
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
            var rO_SkillsDal = new RO_SkillsDal();
            return rO_SkillsDal.GetSkillsPage(take, skip, sort, filter);
        }
    }
}