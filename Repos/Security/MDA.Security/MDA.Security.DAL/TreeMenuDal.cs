namespace MDA.Security.DAL
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class TreeMenuDal
    {
        /// <summary>
        /// Get TreeMenu List
        /// </summary>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<TreeMenu> GetTreeMenuList()
        {
            using (var db = new ApplicationDBContext())
            {
                return db.TreeMenuSet.Where(x => x.Id != 0).ToList();
            }
        }
    }
}