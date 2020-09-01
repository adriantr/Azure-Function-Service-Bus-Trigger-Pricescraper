# Azure-Function-Service-Bus-Trigger-Pricescraper

In order to run the Azure function, you'll need to add local.settings.json file.

It should include following:

```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "Your azure web jobs storage endpoint",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "Azure_SERVICEBUS": "Your Azure Service Bus endpoint"
  }, 
  "ConnectionStrings": {
    "SQLConnectionString": "Your SQL connection string"
  }
}
```
