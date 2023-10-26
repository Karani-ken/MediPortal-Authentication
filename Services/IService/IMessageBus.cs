namespace MediPortal_AuthService.Services.IService
{
    public interface IMessageBus
    {
        Task PublishMessage(object message, string queue_topic_name);
    }
}