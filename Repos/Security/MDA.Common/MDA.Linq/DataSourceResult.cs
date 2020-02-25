//-----------------------------------------------------------------------
// <copyright file="DataSourceResult.cs" company="MDA Corporation">
//     Copyright (c) MDA Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace MDA.Linq
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes the result of DataSource read operation.
    /// </summary>
    /// <typeparam name="T">DataSourceResult Type</typeparam>
    public class DataSourceResult<T>
    {
        /// <summary>
        /// Gets or sets Data Represents a single page of processed data.
        /// </summary>
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Gets or sets Total number of rows The total number of records available.
        /// </summary>
        public int Total { get; set; }
    }
}