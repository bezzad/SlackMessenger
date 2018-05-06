namespace SlackMessenger.Test
{
    class Program
    {
        static void Main()
        {
            var client = new SlackClient("https://hooks.slack.com/services/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");
            var msg = new Message($"Test Message for slack channel", "channel name", "username", "https://a.slack-edge.com/9c217/img/loading_hash_animation.gif");
            client.Send(msg);
        }
    }
}
