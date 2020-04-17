# Azure Autocomplete
This project shows how to create a .net core webapi backend to suggest addresses in a textbox using [JQuery UI Autocomplete](https://jqueryui.com/autocomplete/)

There are three backend data stores:
*  Microsoft SQL
*  CosmosDB using the SQL API
*  CosmosDB using the Mongo API

The settings in *appsettings.Development.json* contain settings run locally using *localdb* for SQL and the [Azure CosmosDB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) for the SQL and Mongo versions of CosmosDB. 

There is sample data in the *Data* directory. To load into each of the data stores:

1. For MS SQL, connect to a localdb (e.g. with Visual Studio SQL Server Object Explorer) and create a database called Addresses. Then in a query window, run the *AddressData.sql* file to create the table and add the sample data.
1. For CosmosDB with the SQL API, open the CosmosDB data explorer from the emulator, create a new container called *StreetAddresses* in a database called *StreetAddressDb* and then click the items under the new container, choose *Upload Item* and upload the *AddressesData.json* file from the *Data* directory.
1. For CosmosDB with the Mongo API, navigate to *api/AddressValidator/cmongo* in the web app which will create the database and table and then load the data found in *AddressData.txt* in the *Data* directory. For localhost in IIS this would be *https://localhost:44316/api/AddressValidator/cmongo*

To deploy to Azure, 
1. Create the CosmosDB SQL, CosmosDB Mongo, and Azure SQL databases
1. Create a new App Service and then add App Settings in the Configuration settings for the connection strings to each of the datasources:
  1. SqlConnectionString
  1. CosmosSqlConnectionString
  1. CosmosMongoConnectionString
1. Deploy the code to the App Service (e.g. using Visual Studio)
1. Finally, load the sample data as explained above
