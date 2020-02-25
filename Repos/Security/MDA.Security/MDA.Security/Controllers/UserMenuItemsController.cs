namespace MDA.Security.Controllers
{
    using Core.Filters;
    using Core.Helpers;
    using Filters;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    [SessionTimeout]
    [HandleAndLogError(ExceptionType = typeof(Exception), View = "_Error")]
    public class UserMenuItemsController : Controller
    {
        /// <summary>
        /// Get UserMenuItems List
        /// </summary>
        /// <returns>UserMenuItemsPartial View</returns>
        [ChildActionOnly]
        public ActionResult UserMenuItemsList()
        {
            var userMenuItemsList = new List<UserMenuItems> {
                new UserMenuItems { Id = 1, Code = ResourceStrings.User_Menu_Item_Toggle_Menu_Location, Description = ResourceStrings.Message_Toggle_Menu_Vertical_Horizontal, ActionName = string.Empty, ControllerName = string.Empty, ParentId = 0, HtmlAttributeCode = string.Empty , Image = @Url.Content("~/Images/toggle_menu_location_icon.png") },
                new UserMenuItems { Id = 2, Code = ResourceStrings.User_Menu_Item_Toggle_Language, Description = ResourceStrings.Message_Toggle_French_English,ActionName = string.Empty, ControllerName = string.Empty, ParentId = 0, HtmlAttributeCode = string.Empty, Image = @Url.Content("~/Images/toggle_language_icon.png") },
                new UserMenuItems { Id = 3, Code = ResourceStrings.User_Menu_Item_Sign_Different_User, Description = ResourceStrings.Message_Login_Different_Account,ActionName = "SignInAsDifferentUser", ControllerName = "Home",ParentId = 0, HtmlAttributeCode = string.Empty, Image = @Url.Content("~/Images/sign_in_as_different_user_icon.png") },
                new UserMenuItems { Id = 4, Code = ResourceStrings.User_Menu_Item_Sign_Out, Description = ResourceStrings.Message_Logout_Site, ActionName = "SignOut", ControllerName = "Home", ParentId = 0 , HtmlAttributeCode = string.Empty, Image = @Url.Content("~/Images/sign_out_icon.png") },
                new UserMenuItems { Id = 5, Code = ResourceStrings.User_Menu_Item_French, Description = ResourceStrings.User_Menu_Item_French, ActionName = "SetCulture", ControllerName = "Culture", ParentId = 2, HtmlAttributeCode = LanguageCode.French },
                new UserMenuItems { Id = 6, Code = ResourceStrings.User_Menu_Item_English, Description = ResourceStrings.User_Menu_Item_English, ActionName = "SetCulture", ControllerName = "Culture", ParentId = 2, HtmlAttributeCode = LanguageCode.English },
                new UserMenuItems { Id = 7, Code = ResourceStrings.User_Menu_Item_Vertical, Description = ResourceStrings.User_Menu_Item_Vertical, ActionName = "SetTreeMenuLocation", ControllerName = "Home", ParentId = 1, HtmlAttributeCode =  TreeMenuLocation.Vertical },
                new UserMenuItems { Id = 8, Code = ResourceStrings.User_Menu_Item_Horizontal, Description = ResourceStrings.User_Menu_Item_Horizontal, ActionName = "SetTreeMenuLocation", ControllerName = "Home", ParentId = 1, HtmlAttributeCode = TreeMenuLocation.Horizontal } };

            return PartialView("_UserMenuItemsPartial", userMenuItemsList);
        }

        /// <summary>
        /// Handle Unknown Action
        /// </summary>
        /// <param name="actionName">Action Name</param>
        protected override void HandleUnknownAction(string actionName)
        {
            throw new UnauthorizedAccessException(string.Format(ResourceStrings.Message_Unknown_Action, actionName));
        }
    }
}