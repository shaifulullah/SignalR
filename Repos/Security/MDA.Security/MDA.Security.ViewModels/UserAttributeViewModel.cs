namespace MDA.Security.ViewModels
{
    using Models;

    public class UserAttributeViewModel
    {
        public UserAttributeViewModel()
        {
            DataAction = DataOperation.UPDATE;
        }
        /// <summary>
        /// Gets or sets DataAction (CREATE/UPDATE/DELETE)
        /// </summary>
        public DataOperation DataAction { get; set; }
        public UserAttributes UserAttribute { get; set; }
    }
}
