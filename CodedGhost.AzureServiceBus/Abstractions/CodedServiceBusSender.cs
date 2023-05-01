using System;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using CodedGhost.AzureServiceBus.Helpers;
using CoreCodedChatbot.Secrets;
using Newtonsoft.Json;

namespace CodedGhost.AzureServiceBus.Abstractions
{
    public class CodedServiceBusSender<T> : ServiceBusSender
    {
        public CodedServiceBusSender(ISecretService secretService)
        : base(ServiceBusHelper.GetClient<T>(secretService), ServiceBusHelper.GetQueueName<T>())
        {
        }

        public Task SendMessageAsync(T message, CancellationToken cancellationToken = new CancellationToken())
        {
            var serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(message));

            return base.SendMessageAsync(serviceBusMessage, cancellationToken);
        }
    }

    public class CodedServiceBusReceiver<T> : ServiceBusReceiver
    {
        public CodedServiceBusReceiver(ISecretService secretService, ServiceBusReceiveMode receiveMode = ServiceBusReceiveMode.PeekLock)
            : base(ServiceBusHelper.GetClient<T>(secretService), ServiceBusHelper.GetQueueName<T>(),
                new ServiceBusReceiverOptions {ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete})
        {
        }
    }
}