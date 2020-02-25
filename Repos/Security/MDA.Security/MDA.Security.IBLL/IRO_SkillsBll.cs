namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IRO_SkillsBll
    {
        /// <summary>
        /// Get Skills For Code
        /// </summary>
        /// <param name="code">Code</param>
        /// <returns>Business Entity</returns>
        RO_Skills GetSkillsForCode(string code);

        /// <summary>
        /// Get Skills List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<RO_Skills> GetSkillsList();

        /// <summary>
        /// Get Skills Page
        /// </summary>
        /// <param name="take">Number of rows to take (return)</param>
        /// <param name="skip">Number of rows to skip before take</param>
        /// <param name="sort">Sort</param>
        /// <param name="filter">Filter</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        DataSourceResult<RO_Skills> GetSkillsPage(int take, int skip, IEnumerable<Sort> sort, LinqFilter filter);
    }
}