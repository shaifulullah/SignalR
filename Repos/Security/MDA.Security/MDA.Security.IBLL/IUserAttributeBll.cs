namespace MDA.Security.IBLL
{
    using Linq;
    using Models;
    using System.Collections.Generic;

    public interface IUserAttributeBll
    {
        /// <summary>
        /// Get UserAttribute For Id
        /// </summary>
        /// <param name="userAccountId">Code</param>
        /// <returns>Business Entity</returns>
        UserAttributes GetUserAttribute(int userAccountId);
    }
}
