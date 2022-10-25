using MediatR;

namespace ApiGateway.Services
{
    public class handle1Message : INotificationHandler<message> 
    {
        public Task Handle(message notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(notification.text);

            return Task.CompletedTask;
        }
    }

    public class handle1Message2 : INotificationHandler<message>
    {
        public Task Handle(message notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(notification.text + " desde un segundo handle");

            return Task.CompletedTask;
        }
    }

    public class message : INotification
    {
        public string text { get; set; }
    }
}
