namespace MDA.Security.DAL
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserInCompanyInApplicationDetailsDal
    {
        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Application Code And Is Hide Terminated
        /// </summary>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForApplicationCodeAndIsHideTerminated(string applicationCode, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var userInCompanyInApplicationDetailsList = db.UserInCompanyInApplicationDetailsSet.AsNoTracking().Where(x => x.ApplicationCode == applicationCode).ToList();

                return userInCompanyInApplicationDetailsList != null && isHideTerminated ?
                    userInCompanyInApplicationDetailsList.Where(x => x.StatusValue != -10133).Where(x => x.StatusValue != 0).ToList() :
                    userInCompanyInApplicationDetailsList;
            }
        }

        /// <summary>
        /// Get UserInCompanyInApplicationDetails List For Company Code And Application Code
        /// </summary>
        /// <param name="companyCode">Company Code</param>
        /// <param name="applicationCode">Application Code</param>
        /// <param name="isHideTerminated">Is Hide Terminated</param>
        /// <returns>Interface to Model Collection Generic of the results</returns>
        public IEnumerable<UserInCompanyInApplicationDetails> GetUserInCompanyInApplicationDetailsListForCompanyCodeAndApplicationCode(string companyCode, string applicationCode, bool isHideTerminated)
        {
            using (var db = new ApplicationDBContext())
            {
                var userInCompanyInApplicationDetailsList = db.UserInCompanyInApplicationDetailsSet.AsNoTracking().Where(x => x.CompanyCode == companyCode).Where(x => x.ApplicationCode == applicationCode).ToList();

                return userInCompanyInApplicationDetailsList != null && isHideTerminated ?
                    userInCompanyInApplicationDetailsList.Where(x => x.StatusValue != -10133).Where(x => x.StatusValue != 0).ToList() :
                    userInCompanyInApplicationDetailsList;
            }
        }
    }
}