namespace SlackMessenger.Test
{
    class Program
    {
        static void Main()
        {
            var client = new SlackClient("https://hooks.slack.com/services/Your/WebHook/Url");
            var msg = new Message($"Test Message for slack channel", "channel name", "username", "https://a.slack-edge.com/9c217/img/loading_hash_animation.gif");
            client.Send(msg);
        }
    }
}
