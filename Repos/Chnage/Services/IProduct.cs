using Chnage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chnage.Services
{
    public interface IProduct
    {
        IEnumerable<Product> GetProducts { get; }
        Product GetProduct(int? ID);
        void Add(Product _Product);
        void Remove(int? ID);
        void Update(Product _Product);
        //IEnumerable<Product> FilterProducts(string currentManufacturerFilter, string currentPartNumberFilter,
        //                        Status? currentStatusFilter, CriticalLevel? criticalLevel, int? idFilter);
    }
}
