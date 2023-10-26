
using Azure.Messaging.ServiceBus;
using MediPortal_AuthService.Services.IService;
using Newtonsoft.Json;
using System.Text;

namespace MediPortal_AuthService.Services
{
    public class MessageBus : IMessageBus
    {
        public string ConnectionString = "Endpoint=sb://mediportal.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=6sliSn8XniDr0QJf9WoqDZD+HpFtsRETs+ASbBDK/3g=";

        public async Task PublishMessage(object message, string queue_topic_name)
        {
            //connect to a string
            var ServiceBus = new ServiceBusClient(ConnectionString);
            //create a sender
            var sender = ServiceBus.CreateSender(queue_topic_name);

            //convert the message to json
            var JsonMessage = JsonConvert.SerializeObject(message);

            var theMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(JsonMessage))
            {
                //give a unique identify to every message
                CorrelationId = Guid.NewGuid().ToString(),
            };
            //send the message
            await sender.SendMessageAsync(theMessage);
            //clean up resources
            await ServiceBus.DisposeAsync();

        }
    }
}
