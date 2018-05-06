using System.IO;
using System.Net;
using System.Text;

namespace SlackMessenger
{
    /// <summary>
    /// Sends you slack messages using web hooks
    /// </summary>
    public class SlackClient
    {
        public string WebHookUrl { get; set; }

        public SlackClient(string webHookUrl)
        {
            WebHookUrl = webHookUrl;
        }

        /// <summary>
        /// Calls the process request method with your message data
        /// </summary>
        /// <param name="message">The message you would like to send to slack</param>
        /// <returns>The response from the server</returns>
        public string Send(Message message)
        {
            return ProcessRequest(WebHookUrl, PreparePostData(message));
        }

        /// <summary>
        /// Gets the JSON data from the message and puts it into a payload parameter
        /// </summary>
        /// <param name="message">The message you would like to send to slack</param>
        /// <returns>A string for the payload containing the slack message</returns>
        private static string PreparePostData(Message message)
        {
            var postData = new StringBuilder();
            postData.Append("payload={");

            if (!string.IsNullOrEmpty(message.Text))
            {
                postData.Append("\"text\":\"" + message.Text + "\"");
            }

            if (!string.IsNullOrEmpty(message.Channel))
            {
                postData.Append(",\"channel\":\"" + message.Channel + "\"");
            }

            if (!string.IsNullOrEmpty(message.Icon))
            {
                if (message.Icon.StartsWith(":"))
                    postData.Append(",\"icon_emoji\":\"" + message.Icon + "\"");
                else
                    postData.Append(",\"icon_url\":\"" + message.Icon + "\"");
            }

            if (!string.IsNullOrEmpty(message.UserName))
            {
                postData.Append(",\"username\": \"" + message.UserName + "\"");
            }

            postData.Append("}");
            return postData.ToString();
        }

        /// <summary>
        /// Calls a web request using the webhook URL and the data from the message
        /// </summary>
        /// <param name="webhookUrl">The webhook URL to send the message to</param>
        /// <param name="sbPostData">A string for the payload containing the slack message</param>
        /// <returns>The response from the server</returns>
        private static string ProcessRequest(string webhookUrl, string sbPostData)
        {
            var request = WebRequest.Create(webhookUrl);
            request.Method = "POST";
            var byteArray = Encoding.UTF8.GetBytes(sbPostData);
            request.ContentType = @"application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            using (var reqStream = request.GetRequestStream())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
                using (var response = request.GetResponse())
                using (var respStream = response.GetResponseStream())
                {
                    if (respStream != null)
                    {
                        using (var reader = new StreamReader(respStream))
                        {
                            var responseFromServer = reader.ReadToEnd();
                            reader.Close();
                            return responseFromServer;
                        }
                    }

                    return null;
                }
            }
        }
    }
}