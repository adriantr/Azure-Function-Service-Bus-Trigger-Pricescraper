using System;
using Microsoft.Extensions.Configuration;
using AzFunc1.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Adrian.Test
{
    public static class AdrianTesterQueueTrigger
    {
        [FunctionName("AdrianTesterQueueTrigger")]
        public static async void Run([ServiceBusTrigger("prices", Connection = "adriantrzeciak_SERVICEBUS")] string myQueueItem, ILogger log, ExecutionContext context)
        {
            var data = JsonConvert.DeserializeObject<MessageContent>(myQueueItem);

            var sqlString = getSqlConnString(getConfig(context));

            using (SqlConnection conn = new SqlConnection(sqlString))
            {
                conn.Open();

                foreach (var item in data.Products)
                {
                    item.Name = item.Name.Replace("'", "''");

                    var text = $@"update [dbo].[PRODUCTS] set PRICE='{item.Price}' where EAN='{item.Ean}' and CHAIN='{data.Chain}' and NAME='{item.Name}'
                            IF @@ROWCOUNT=0
                            insert into [dbo].[PRODUCTS] values('{item.Ean}','{item.Name}','{item.Price}','{item.Img}','{data.Chain}')";

                    using (SqlCommand cmd = new SqlCommand(text, conn))
                    {
                        try
                        {
                            var rows = await cmd.ExecuteNonQueryAsync();
                        }
                        catch (Exception e)
                        {
                            var msg = e.Message;
                        }
                    }
                }

                conn.Close();
            }
        }

        private static IConfigurationRoot getConfig(ExecutionContext context)
        {

            return new ConfigurationBuilder()
                    .SetBasePath(context.FunctionAppDirectory)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();
        }

        private static string getSqlConnString(IConfigurationRoot config)
        {
            return config.GetConnectionString("SQLConnectionString");
        }
    }
}
