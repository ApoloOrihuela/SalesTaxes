using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SalesTaxes;
using System;
using System.Collections.Generic;

namespace SalesTaxesTests
{
    [TestClass]
    public class TestShoppingBaskets
    {        
        [TestMethod]
        public void TestWithFourElements()
        {
            //Arrange
            ShoppingBasket ShoppingBasketInput = new ShoppingBasket();

            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Book\", \"quantity\": 1, \"price\": 12.49, \"typeOfProduct\": \"book\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Book\", \"quantity\": 1, \"price\": 12.49, \"typeOfProduct\": \"book\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Music CD\", \"quantity\": 1, \"price\": 14.99, \"typeOfProduct\": \"music\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Chocolate bar\", \"quantity\": 1, \"price\": 0.85, \"typeOfProduct\": \"book\", \"isImported\": false }"));

            List<string> expectedResult = new List<string>();
            expectedResult.Add("Book: 24.98 (2 @ 12.49)");
            expectedResult.Add("Music CD: 16.49");
            expectedResult.Add("Chocolate bar: 0.85");
            expectedResult.Add("Sales Taxes: 1.50");
            expectedResult.Add("Total: 42.32");

            //Act
            ShoppingBasketResponse receiptToPrint = Program.CalculateTotal(ShoppingBasketInput);
            List<string> processResult = ConvertShoppingBasketResponseToList(receiptToPrint);

            //Assert
            Assert.AreEqual("1.50", receiptToPrint.salesTaxes.ToString("0.00"));
            Assert.AreEqual("42.32", receiptToPrint.total.ToString("0.00"));
            CollectionAssert.AreEqual(expectedResult, processResult);
        }

        [TestMethod]
        public void TestWithTwoElements()
        {
            //Arrange
            ShoppingBasket ShoppingBasketInput = new ShoppingBasket();

            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"box of chocolates\", \"quantity\": 1, \"price\": 10.00, \"typeOfProduct\": \"food\", \"isImported\": true }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"bottle of perfume\", \"quantity\": 1, \"price\": 47.50, \"typeOfProduct\": \"perfume\", \"isImported\": true }"));

            List<string> expectedResult = new List<string>();
            expectedResult.Add("Imported box of chocolates: 10.50");
            expectedResult.Add("Imported bottle of perfume: 54.65");
            expectedResult.Add("Sales Taxes: 7.65");
            expectedResult.Add("Total: 65.15");

            //Act
            ShoppingBasketResponse receiptToPrint = Program.CalculateTotal(ShoppingBasketInput);
            List<string> processResult = ConvertShoppingBasketResponseToList(receiptToPrint);

            //Assert            
            Assert.AreEqual("7.65", receiptToPrint.salesTaxes.ToString("0.00"));
            Assert.AreEqual("65.15", receiptToPrint.total.ToString("0.00"));
            CollectionAssert.AreEqual(expectedResult, processResult);
        }

        [TestMethod]
        public void TestWithFiveElements()
        {
            //Arrange
            ShoppingBasket ShoppingBasketInput = new ShoppingBasket();

            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"bottle of perfume\", \"quantity\": 1, \"price\": 27.99, \"typeOfProduct\": \"perfume\", \"isImported\": true }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Bottle of perfume\", \"quantity\": 1, \"price\": 18.99, \"typeOfProduct\": \"perfume\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Packet of headache pills\", \"quantity\": 1, \"price\": 9.75, \"typeOfProduct\": \"medical product\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"box of chocolates\", \"quantity\": 1, \"price\": 11.25, \"typeOfProduct\": \"food\", \"isImported\": true }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"box of chocolates\", \"quantity\": 1, \"price\": 11.25, \"typeOfProduct\": \"food\", \"isImported\": true }"));

            List<string> expectedResult = new List<string>();
            expectedResult.Add("Imported bottle of perfume: 32.19");
            expectedResult.Add("Bottle of perfume: 20.89");
            expectedResult.Add("Packet of headache pills: 9.75");
            expectedResult.Add("Imported box of chocolates: 23.70 (2 @ 11.85)");
            expectedResult.Add("Sales Taxes: 7.30");
            expectedResult.Add("Total: 86.53");

            //Act
            ShoppingBasketResponse receiptToPrint = Program.CalculateTotal(ShoppingBasketInput);
            List<string> processResult = ConvertShoppingBasketResponseToList(receiptToPrint);

            //Assert
            Assert.AreEqual("7.30", receiptToPrint.salesTaxes.ToString("0.00"));
            Assert.AreEqual("86.53", receiptToPrint.total.ToString("0.00"));
            CollectionAssert.AreEqual(expectedResult, processResult);
        }

        [TestMethod]
        public void TestWithElevenElements()
        {
            //Arrange
            ShoppingBasket ShoppingBasketInput = new ShoppingBasket();

            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Book\", \"quantity\": 1, \"price\": 12.49, \"typeOfProduct\": \"book\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Book\", \"quantity\": 1, \"price\": 12.49, \"typeOfProduct\": \"book\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Music CD\", \"quantity\": 1, \"price\": 14.99, \"typeOfProduct\": \"music\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Chocolate bar\", \"quantity\": 1, \"price\": 0.85, \"typeOfProduct\": \"book\", \"isImported\": false }"));

            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"box of chocolates\", \"quantity\": 1, \"price\": 10.00, \"typeOfProduct\": \"food\", \"isImported\": true }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"bottle of perfume\", \"quantity\": 1, \"price\": 47.50, \"typeOfProduct\": \"perfume\", \"isImported\": true }"));

            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"bottle of perfume\", \"quantity\": 1, \"price\": 27.99, \"typeOfProduct\": \"perfume\", \"isImported\": true }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Bottle of perfume\", \"quantity\": 1, \"price\": 18.99, \"typeOfProduct\": \"perfume\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"Packet of headache pills\", \"quantity\": 1, \"price\": 9.75, \"typeOfProduct\": \"medical product\", \"isImported\": false }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"box of chocolates\", \"quantity\": 1, \"price\": 11.25, \"typeOfProduct\": \"food\", \"isImported\": true }"));
            ShoppingBasketInput.ItemList.Add(JsonConvert.DeserializeObject<Item>("{\"productDescription\": \"box of chocolates\", \"quantity\": 1, \"price\": 11.25, \"typeOfProduct\": \"food\", \"isImported\": true }"));


            List<string> expectedResult = new List<string>();
            expectedResult.Add("Book: 24.98 (2 @ 12.49)");
            expectedResult.Add("Music CD: 16.49");
            expectedResult.Add("Chocolate bar: 0.85");
            expectedResult.Add("Imported box of chocolates: 10.50");
            expectedResult.Add("Imported bottle of perfume: 54.65");
            expectedResult.Add("Imported bottle of perfume: 32.19");
            expectedResult.Add("Bottle of perfume: 20.89");
            expectedResult.Add("Packet of headache pills: 9.75");
            expectedResult.Add("Imported box of chocolates: 23.70 (2 @ 11.85)");
            expectedResult.Add("Sales Taxes: 16.45");
            expectedResult.Add("Total: 194.00");

            //Act
            ShoppingBasketResponse receiptToPrint = Program.CalculateTotal(ShoppingBasketInput);
            List<string> processResult = ConvertShoppingBasketResponseToList(receiptToPrint);

            //Assert
            Assert.AreEqual("16.45", receiptToPrint.salesTaxes.ToString("0.00"));
            Assert.AreEqual("194.00", receiptToPrint.total.ToString("0.00"));
            CollectionAssert.AreEqual(expectedResult, processResult);
        }

        private List<string> ConvertShoppingBasketResponseToList(ShoppingBasketResponse receiptToPrint)
        {
            List<string> listToReturn = new List<string>();

            //Same functionality of PrintReceipt() but the output is assigned to a String
            //For each item in the receipt we format the output checking if it is imported and if quantity is greater than 1, then print the Sales Taxes and Total
            foreach (ItemResponse receiptItem in receiptToPrint.ItemList)
            {
                string formatReceiptItemDescription;
                if (receiptItem.isImported)
                {
                    formatReceiptItemDescription = "Imported " + receiptItem.productDescription;
                }
                else
                {
                    formatReceiptItemDescription = receiptItem.productDescription;
                }
                if (receiptItem.quantity > 1)
                {
                    listToReturn.Add(String.Format("{0}: {1:0.00} ({2} @ {3:0.00})", formatReceiptItemDescription, receiptItem.subtotal, receiptItem.quantity, receiptItem.unitPrice));
                }
                else
                {
                    listToReturn.Add(String.Format("{0}: {1:0.00}", formatReceiptItemDescription, receiptItem.subtotal));
                }
            }
            listToReturn.Add(String.Format("Sales Taxes: {0:0.00}", receiptToPrint.salesTaxes));
            listToReturn.Add(String.Format("Total: {0:0.00}", receiptToPrint.total));

            return listToReturn;
        }
    }
}
