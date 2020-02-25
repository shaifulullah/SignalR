namespace MDA.Security.BLL
{
    using DAL;
    using IBLL;
    using Linq;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    public class UserAttributeBll : IUserAttributeBll
    {
        public UserAttributes GetUserAttribute(int userAccountId)
        {
            var userAttributeDal = new UserAttributeDal();
            return userAttributeDal.GetUserAttributeForId(userAccountId);
        }
    }
}
