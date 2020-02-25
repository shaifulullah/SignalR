namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Models;
    using System.Collections.Generic;

    public class TreeMenuBll : ITreeMenuBll
    {
        /// <summary>
        /// Get TreeMenu List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<TreeMenu> GetTreeMenuList()
        {
            var treeMenuDal = new TreeMenuDal();
            return treeMenuDal.GetTreeMenuList();
        }
    }
}