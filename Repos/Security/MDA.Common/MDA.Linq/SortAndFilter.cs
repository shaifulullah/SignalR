namespace MDA.Linq
{
    using System.Collections.Generic;

    public class SortAndFilter
    {
        public LinqFilter Filter { get; set; }
        public IEnumerable<Sort> SortList { get; set; }
    }
}