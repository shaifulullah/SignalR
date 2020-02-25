//-----------------------------------------------------------------------
// <copyright file="SubmitButtonAttribute.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MDA.Core.Filters
{
    using System;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// Submit Button Attribute Used when there are multiple Submit buttons on a view
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SubmitButtonAttribute : ActionNameSelectorAttribute
    {
        /// <summary>
        /// Gets or sets Match Form Key
        /// </summary>
        public string MatchFormKey { get; set; }

        /// <summary>
        /// Gets or sets Match Form Value
        /// </summary>
        public string MatchFormValue { get; set; }

        /// <summary>
        /// Is Valid Name
        /// </summary>
        /// <param name="controllerContext">Controller Context</param>
        /// <param name="actionName">Action Name</param>
        /// <param name="methodInfo">Method Information</param>
        /// <returns>True on Success, False on Failure</returns>
        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            return controllerContext.HttpContext.Request[MatchFormKey] != null &&
                controllerContext.HttpContext.Request[MatchFormKey] == MatchFormValue;
        }
    }
}