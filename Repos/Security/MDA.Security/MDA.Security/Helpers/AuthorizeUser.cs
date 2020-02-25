namespace MDA.Security.Helpers
{
    using Globals;
    using ViewModels;

    /// <summary>
    /// Authorize User
    /// </summary>
    public static class AuthorizeUser
    {
        /// <summary>
        /// Can Perform Action
        /// </summary>
        /// <param name="securityItemCode">Security Item Code</param>
        /// <param name="dataOperation">
        /// Action (INSERT/EDIT/READ/UPDATE/DELETE/EXECUTE/ACCESS)
        /// </param>
        /// <returns>True or False</returns>
        public static bool CanPerformAction(string securityItemCode, DataOperation dataOperation)
        {
            var userSecurityRightsList = ApplicationGlobals.GetUserSecurityRightsListForSecurityItemCode(securityItemCode);
            try
            {
                var canPerformAction = false;
                foreach (var userSecurityRights in userSecurityRightsList)
                {
                    switch (dataOperation)
                    {
                        case DataOperation.CREATE:
                            canPerformAction = userSecurityRights.Create;
                            break;

                        case DataOperation.READ:
                            canPerformAction = userSecurityRights.Read;
                            break;

                        case DataOperation.UPDATE:
                            canPerformAction = userSecurityRights.Update;
                            break;

                        case DataOperation.DELETE:
                            canPerformAction = userSecurityRights.Delete;
                            break;

                        case DataOperation.EXECUTE:
                            canPerformAction = userSecurityRights.Execute;
                            break;

                        case DataOperation.ACCESS:
                            canPerformAction = userSecurityRights.Access;
                            break;

                        case DataOperation.EDIT_VIEW:
                            canPerformAction = userSecurityRights.Update || userSecurityRights.Read;
                            break;

                        default:
                            canPerformAction = false;
                            break;
                    }
                }

                return canPerformAction;
            }
            catch
            {
                return false;
            }
        }
    }
}