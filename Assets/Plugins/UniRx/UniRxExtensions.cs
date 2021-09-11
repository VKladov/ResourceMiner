using UniRx;

public static class UniRxExtensions
{
    public static void Publish<Message>(this Message message)
    {
        MessageBroker.Default.Publish(message);
    }
}
