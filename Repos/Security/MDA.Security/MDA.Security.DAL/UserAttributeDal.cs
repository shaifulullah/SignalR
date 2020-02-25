namespace MDA.Security.DAL
{
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class UserAttributeDal
    {
        /// <summary>
        /// Get UserAttribute For UserId
        /// </summary>
        /// <param name="userAccountId">Code</param>
        /// <returns>Business Entity</returns>
        public UserAttributes GetUserAttributeForId(int userAccountId )
        {
            using (var db = new ApplicationDBContext())
            {
                var result = db.UserAttributeSet.FirstOrDefault(x => x.LnUserId == userAccountId);
                return result;
            }
        }
    }
}
