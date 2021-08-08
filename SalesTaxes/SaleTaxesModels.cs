using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTaxes
{
    class SaleTaxesModels
    {
    }

    public class ShoppingBasket
    {
        public List<Item> ItemList { get; set; } = new List<Item>();
    }

    public class Item
    {
        public string productDescription { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
        public string typeOfProduct { get; set; }
        public bool isImported { get; set; }
    }

    public class ShoppingBasketResponse
    {
        public List<ItemResponse> ItemList { get; set; } = new List<ItemResponse>();
        public float salesTaxes { get; set; }
        public float total { get; set; }
    }

    public class ItemResponse
    {
        public string productDescription { get; set; }
        public float subtotal { get; set; }
        public int quantity { get; set; }
        public float unitPrice { get; set; }
        public float basicSalesTax { get; set; } = 0; // Is initialize with 0 in case we don't apply any tax for this item
        public float importTax { get; set; } = 0; // Is initialize with 0 in case we don't apply any tax for this item
        public string typeOfProduct { get; set; }
        public bool isImported { get; set; }
    }
}
