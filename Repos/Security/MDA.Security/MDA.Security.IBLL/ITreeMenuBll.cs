namespace MDA.Security.IBLL
{
    using Models;
    using System.Collections.Generic;

    public interface ITreeMenuBll
    {
        /// <summary>
        /// Get TreeMenu List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        IEnumerable<TreeMenu> GetTreeMenuList();
    }
}