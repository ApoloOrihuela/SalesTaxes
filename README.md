
# Sales Taxes
_Implement an application that takes input for shopping baskets and returns receipts in the given format_

## Technical and architectural decisions 

_The application was created using Visual Studio 2019, as a command-line console application that can run over .Net Framework on windows.  The target framework is 4.5 this for a Long-term support, for the sake of simplicity a third party library is used, the Newtonsoft Json.NET was selected and is used to easly convert string to object an viceversa, is light, free, High Performance, Run Anywhere (supports Windows, MacOS, Linux, Mono, and Xamarin.) and can be added to the solution  as an embedded resource so we don't need to export it among the deployment files._
### System Requirements 
```
1.- Windows Operating System
2.- .Net Framework 4.5
3.- Visual Studio 2019 (for compiling and debugging purposes)
```

### Installation

_To install the application you only need to copy the file SalesTaxes.exe and set the location of ShoppingCart.txt properly in the code._

### Deployment

_For deployment you must to open the solution (SalesTaxes.sln) in Visual Studio 2019, then click in “start debugging” button in order to create the executable file for deploying (or if you want to build to release version, change settings to release), once you have done this, in the DEBUG folder (“SalesTaxes\bin\Debug”) you’ll find the file "SalesTaxes.exe" the only necessary to export the application to another location and be executed, you must to set the location of the ShoppingCart.txt file properly._

## Full Documentation

Full documentation for this project is located inside of the solution under SalesTaxes project inside of the Documentation folder with the name "Sonatafy_SalesTaxes_1.0.0_Software_Documentation.pdf".

### Tools 

_Tools used for this project_

* [.Net Core 4.5](https://dotnet.microsoft.com/download/dotnet-framework/net45) - The ASP.NET Core Runtime enables you to run existing web/server applications.
* [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) - Write code using code completions, debugging, testing, Git management, and cloud deployments with Visual Studio.
* [Newtonsoft.Json](https://www.newtonsoft.com/json) - Serialize and deserialize any .NET object with Json.NET's powerful JSON serializer.
