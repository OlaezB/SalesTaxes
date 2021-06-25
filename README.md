# SalesTaxesSystem


# Assumptions
  - User is going to interact with the application through the console.
  - User is going to interact only with shown commands and inputs in the console.
  - The solution has json files that contain the products and taxes lists needed for the appliation ('products.json', 'taxes.json').
  - Available products and taxes are not mutable once the application has been executed, 
    only products and taxes loaded during application start will be used,
    only restarting the application will cause them to get updated.
  - To checkout, at least 1 product has to been added to the cart by user. 
    The user can exit from application without this condition.

# Design
This solution incorporates next features:
  - Configuration: Configuration can be incorporated from files ('appsettings.json', 'appsettings.production.json'), and from environment variables as well.
  - Logging: Serilog has been added and configured to log into a text file ('log.text') rolling it every day.
  - Dependency Injection: DI pattern has been implemented to have a loosely coupled design in the solution, making it easier to mantain, extend and test.
  - Test Cases: Unit testing of main functionality has been added using the testing framework xUnit.

The solution has 4 projects on it
  - SalesTaxesSystem: Serves as the view of the application, handles user's input and data outputs. Presentation Layer
  - SalesTaxesSystem.BLL: Coordinates data flow and contains logic related to data manipulation. Business Logic Layer
  - SalesTaxesSystem.DAL: Operates transactions with one or more data stores. Data Access Layer
  - SalesTaxesSystem.Test: Contains Test cases that verify the functioning of services and clasess. Test Project
  
  
# Specifications
  - Target Framework: .NET Core 2.2
  - Type of application: Console
