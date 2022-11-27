using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiDemo.Models
{
    public class ProductsModel
    {
        
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public string ProductAvaibility { get; set; }
        public string ProductSupplier { get; set; }
    }
}