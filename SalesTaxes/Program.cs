using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;

namespace SalesTaxes
{
    public class Program
    {
        //Global Variables
        private static IDictionary<string, Assembly> additional = new Dictionary<string, Assembly>();
        //location of the file to feed the Authorizer
        static readonly string ShoppingCartTextFile = @"ShoppingCart.txt";

        static void Main(string[] args)
        {
            //Load external dlls inside of the deliverable EXE file
            LoadAssemblies();

            //start the process to calculate all taxes and totals
            StartProcessToGettingTotal();
        }

        private static void StartProcessToGettingTotal()
        {
            try
            {                
                ShoppingBasket ShoppingBasketInput = new ShoppingBasket();

                //Load the Shopping Basket items from txt file
                if (File.Exists(ShoppingCartTextFile))
                {
                    int lineCounter = 0;                      
                    string[] lines = File.ReadAllLines(ShoppingCartTextFile);
                    foreach (string line in lines)
                    {
                        lineCounter++;
                        if (line != "")
                        {
                            //Convert txt line into a valid Item Object
                            Item LineTransaction = ConvertJsonTextToObject(line);
                            if (LineTransaction != null)
                            {
                                ShoppingBasketInput.ItemList.Add(LineTransaction);
                            }
                            else
                            {
                                Console.WriteLine("Json Format Error at line: {0}, (still processing other rows)", lineCounter.ToString());
                            }
                        }
                    }
                    //Once we have all valid items in the Shopping Basket, we proccess it
                    ShoppingBasketResponse receiptToPrint = CalculateTotal(ShoppingBasketInput);

                    //We print to console the receipt object 
                    PrintReceipt(receiptToPrint);
                }
                else
                {
                    Console.WriteLine("No file (ShoppingCart.txt) was found.");
                }
                //To avoid the console to be closed before seeing the results we add ReadKey()
                Console.ReadKey();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        private static void PrintReceipt(ShoppingBasketResponse totalToPrint)
        {
            //For each item in the receipt we format the output checking if it is imported and if quantity is greater than 1, then print the Sales Taxes and Total
            foreach (ItemResponse receiptItem in totalToPrint.ItemList)
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
                    Console.WriteLine("{0}: {1:0.00} ({2} @ {3:0.00})", formatReceiptItemDescription, receiptItem.subtotal, receiptItem.quantity, receiptItem.unitPrice);
                }
                else
                {
                    Console.WriteLine("{0}: {1:0.00}", formatReceiptItemDescription, receiptItem.subtotal);
                }
            }
            Console.WriteLine("Sales Taxes: {0:0.00}", totalToPrint.salesTaxes);
            Console.WriteLine("Total: {0:0.00}", totalToPrint.total);
        }


        /// <summary>
        /// Method to calculate all taxes and totals for a given Shopping Basket
        /// </summary>
        /// <param name="shoppingBasketInput"></param>
        /// <returns>ShoppingBasketResponse Object that contains the items, Sales Taxes and Total</returns>
        public static ShoppingBasketResponse CalculateTotal(ShoppingBasket shoppingBasketInput)
        {
            ShoppingBasketResponse ShoppingBasketResponseTemp = new ShoppingBasketResponse();
            float salesTaxes = 0;
            float total = 0; ;

            foreach (Item item in shoppingBasketInput.ItemList)
            {
                ItemResponse responseItem = new ItemResponse();
                bool alreadyInReceipt = false;

                //If this is the first time we add a new item to the receipt
                if (ShoppingBasketResponseTemp.ItemList.Count == 0)
                {
                    responseItem.productDescription = item.productDescription;
                    responseItem.quantity = item.quantity;
                    responseItem.isImported = item.isImported;
                    responseItem.unitPrice = item.price;
                    responseItem.typeOfProduct = item.typeOfProduct;

                    ShoppingBasketResponseTemp.ItemList.Add(responseItem);
                }
                else
                {
                    //Check if the Item is already in the receipt, if exists we add 1 to the quantity for that item if not, we add it to the receipt
                    foreach (ItemResponse ItemResponseTemp in ShoppingBasketResponseTemp.ItemList)
                    {                        
                        if (ItemResponseTemp.productDescription.ToLower() == item.productDescription.ToLower() && ItemResponseTemp.isImported == item.isImported && ItemResponseTemp.unitPrice == item.price)
                        {
                            ItemResponseTemp.quantity = ItemResponseTemp.quantity + item.quantity;
                            alreadyInReceipt = true;
                        }
                    }

                    if (alreadyInReceipt == false)
                    {
                        responseItem.productDescription = item.productDescription;
                        responseItem.quantity = item.quantity;
                        responseItem.isImported = item.isImported;
                        responseItem.unitPrice = item.price;
                        responseItem.typeOfProduct = item.typeOfProduct;

                        ShoppingBasketResponseTemp.ItemList.Add(responseItem);
                    }
                }
            }

            //Now we process each item in the receipt
            foreach (ItemResponse ItemResponseTemp in ShoppingBasketResponseTemp.ItemList)
            {
                float tempTax = 0;
                //Check if Item's category is book, food or medical product to avoid 10% taxes
                if (ItemResponseTemp.typeOfProduct != "book" && ItemResponseTemp.typeOfProduct != "food" && ItemResponseTemp.typeOfProduct != "medical product")
                {
                    tempTax = RoundToNearestFiveCents((float)(ItemResponseTemp.unitPrice * .10));
                    ItemResponseTemp.basicSalesTax = tempTax;
                }

                //Check if the Item is imported to apply the 5% of taxes
                if (ItemResponseTemp.isImported)
                {
                    float importTax = RoundToNearestFiveCents((float)(ItemResponseTemp.unitPrice * .05));
                    tempTax += importTax;
                    ItemResponseTemp.importTax = importTax;
                }

                //If Item generates any type of taxes we add it to the subtotal updating the final price and if quantity is greater than 1 we update the unit price with taxes, else we calculate the subtotal normally
                if (tempTax > 0)
                {
                    ItemResponseTemp.subtotal = (float)((ItemResponseTemp.unitPrice + tempTax) * ItemResponseTemp.quantity);
                    salesTaxes += (tempTax * ItemResponseTemp.quantity);
                    if (ItemResponseTemp.quantity > 1)
                    {
                        ItemResponseTemp.unitPrice = (float)(ItemResponseTemp.unitPrice + tempTax);                        
                    }
                }
                else
                {
                    ItemResponseTemp.subtotal = (ItemResponseTemp.unitPrice * ItemResponseTemp.quantity);
                }

                //Add the subtotal of this Item to the total 
                total += ItemResponseTemp.subtotal;

            }

            //Assign the sum of the Sales Taxes generated and the grand Total to the response object
            ShoppingBasketResponseTemp.salesTaxes = salesTaxes;
            ShoppingBasketResponseTemp.total = total;

            return ShoppingBasketResponseTemp;
        }

        /// <summary>
        /// Method to round the value up to the nearest 5 cents (When calculating the sales tax)
        /// </summary>
        /// <param name="float_number"></param>
        /// <returns>float number rounded to the nearest 5 cents</returns>
        public static float RoundToNearestFiveCents(float float_number)
        {
            //Extract the decimal part only to round
            var decimalPart = float_number - Math.Truncate(float_number);
            //Convert to an integer number
            int decimalPartConverted = (int)(((decimal)float_number % 1) * 100);

            if (decimalPartConverted > 0)
            {
                //Get the remainder of dividing into 5
                var divideCentsInFive = (decimalPartConverted % 5);
                if (divideCentsInFive > 0 && divideCentsInFive != 5)
                {
                    if (divideCentsInFive > 5)
                    {
                        //If the remainder is greater than 5, add the remainder to the integer number to get the nearest ten
                        decimalPart = (decimalPartConverted + divideCentsInFive);
                    }
                    else
                    {
                        //If the remainder is less than 5, subtract the remainder to the integer number to get the nearest ten and then add 5
                        decimalPart = (decimalPartConverted - divideCentsInFive) + 5;
                    }
                    //Convert the integer number to a decimal value
                    decimalPart = (decimalPart / 100);
                }
            }

            //Return the integer plus the decimal value calculated
            return (float)(Math.Truncate(float_number) + decimalPart);
        }

        /// <summary>
        /// Method to Convert Json Text To C# Object (Account/Trasaction), if any error ocurred returns null
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Account/Trasaction Object</returns>
        private static Item ConvertJsonTextToObject(string line)
        {
            Item LineToObject = new Item();
            try
            {
                //try to convert the text line to a valid Item object
                LineToObject = JsonConvert.DeserializeObject<Item>(line);
                if (LineToObject.productDescription == null)
                {
                    return null;
                }
                return LineToObject;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Method to load external assemblies
        /// </summary>
        static private void LoadAssemblies()
        {
            try
            {
                string[] Assemblies = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

                foreach (string AssablyToLoad in Assemblies)
                {
                    using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(AssablyToLoad))
                    {
                        Byte[] assemblyData = new Byte[stream.Length];
                        stream.Read(assemblyData, 0, assemblyData.Length);
                        Assembly.Load(assemblyData);
                        var assembly = Assembly.Load(assemblyData);
                        additional.Add(assembly.FullName, assembly);
                    }
                }

                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += ResolveAssembly;
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            }
            catch { }

        }

        /// <summary>
        /// Method to resolve an assembly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        static private Assembly ResolveAssembly(Object sender, ResolveEventArgs e)
        {
            Assembly res;
            additional.TryGetValue(e.Name, out res);
            return res;
        }
    }
}
