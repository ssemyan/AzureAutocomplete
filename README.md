# Azure Autocomplete
This project shows how to create a .net core webapi backend to suggest addresses in a textbox using [JQuery UI Autocomplete](https://jqueryui.com/autocomplete/)

There are three backend data stores:
*  Microsoft SQL
*  CosmosDB using the SQL API
*  CosmosDB using the Mongo API

The settings in *appsettings.Development.json* contain settings to use a localdb for SQL and the Azure CosmosDB Emulator. 

There is sample data in the *Data* directory. 

1. To load the SQL data, open a query window for the localdb (e.g. with Visual Studio Data Explorer) create a database called Addresses and then run the *AddressData.sql* file to create the table and sample data.
1. To load data into CosmosDB with the SQL API, open the CosmosDB data explorer, create a new container called *StreetAddresses* in a database called *StreetAddressDb* and then click the items under the new container, choose *Upload Item* and upload the *AddressesData.json* file from the *Data* directory.
1. To load the data into CosmosDB with the Mongo API, navigate to *api/AddressValidator/cmongo* in the web app. For localhost in IIS this would be *https://localhost:44316/api/AddressValidator/cmongo*

To deploy to Azure, 
1. Create CosmosDB SQL, CosmosDB Mongo, and Azure SQL databases and load the data as above.
1. Create a new App Service and then add App Settings in the Configuration settings for the connection strings to each of the datasources:
  1. SqlConnectionString
  1. CosmosSqlConnectionString
  1. CosmosMongoConnectionString
1. Deploy the code to the App Service (e.g. using Visual Studio)