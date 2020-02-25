namespace MDA.Security.ViewModels
{
    /// <summary>
    /// Data Operation
    /// </summary>
    public enum DataOperation
    {
        /// <summary>
        /// Create Operation
        /// </summary>
        CREATE = 1,

        /// <summary>
        /// Read Operation
        /// </summary>
        READ,

        /// <summary>
        /// Update Operation
        /// </summary>
        UPDATE,

        /// <summary>
        /// Delete Operation
        /// </summary>
        DELETE,

        /// <summary>
        /// Execute Operation
        /// </summary>
        EXECUTE,

        /// <summary>
        /// Access Operation
        /// </summary>
        ACCESS,

        /// <summary>
        /// Edit or View Operation
        /// </summary>
        EDIT_VIEW
    }

    /// <summary>
    /// User Account Type
    /// </summary>
    public enum UserAccountType
    {
        /// <summary>
        /// Internal User Account
        /// </summary>
        INTERNAL = 1,

        /// <summary>
        /// External User Account
        /// </summary>
        EXTERNAL
    }
}