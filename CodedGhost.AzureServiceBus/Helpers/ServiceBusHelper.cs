using Azure.Messaging.ServiceBus;
using CoreCodedChatbot.Secrets;
using System.Threading.Tasks;
using System;
using Azure.Messaging.ServiceBus.Administration;

namespace CodedGhost.AzureServiceBus.Helpers
{
    public static class ServiceBusHelper
    {
        public static ServiceBusClient GetClient<T>(ISecretService secretService)
        {
            var queueName = GetQueueName<T>();

            var asyncClient = new Lazy<Task<ServiceBusClient>>(async () =>
            {
                var connectionString = secretService.GetSecret<string>("ServiceBusConnectionString");
                var adminClient =
                    new ServiceBusAdministrationClient(connectionString);

                var exists = await adminClient.QueueExistsAsync(queueName);

                if (!exists.Value)
                {
                    await adminClient.CreateQueueAsync(queueName);
                }

                return new ServiceBusClient($"{connectionString}");
            });

            return asyncClient.Value.Result;
        }

        public static string GetQueueName<T>() => $"{typeof(T).Name}Queue";
    }
}