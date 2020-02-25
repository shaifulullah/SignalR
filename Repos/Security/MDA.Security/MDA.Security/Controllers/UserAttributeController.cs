namespace MDA.Security.Controllers
{
    using AutoMapper;
    using BLL;
    using Core.Exception;
    using Core.Filters;
    using Core.Helpers;
    using Filters;
    using Globals;
    using Helpers;
    using IBLL;
    using Linq;
    using Models;
    using Newtonsoft.Json;
    using Reports;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.UI;
    using ViewModels;
    public class UserAttributeController : Controller
    {
        /// <summary>
        /// Bind UserAttributes
        /// <param name="id">Id</param>
        /// </summary>
        /// <returns>MaintainUserAttribute View</returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [AuthorizeUser(Operation = DataOperation.UPDATE, SecurityItemCode = PageId.PAGE_USERATTRIBUTE)]
        public ActionResult BindUserAttribute(int id)
        {
            IUserAttributeBll iUserAttributeBll = new UserAttributeBll();
            var userAttribute = iUserAttributeBll.GetUserAttribute(id);

            UserAttributeViewModel userAttributeViewModel = new UserAttributeViewModel();
            userAttributeViewModel.UserAttribute = userAttribute;
            return PartialView("MaintainUserAttribute", userAttributeViewModel);
        }
    }
}