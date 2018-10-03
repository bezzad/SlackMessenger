using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
            return ProcessRequestAsync(WebHookUrl, PreparePostData(message)).Result;
        }

        /// <summary>
        /// Async implementation of sending a Slack message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<string> SendAsync(Message message)
        {
            return await ProcessRequestAsync(WebHookUrl, PreparePostData(message));
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
        /// Async ProcessRequest implementation
        /// </summary>
        /// <param name="webhookUrl"></param>
        /// <param name="sbPostData"></param>
        /// <returns></returns>
        private static async Task<string> ProcessRequestAsync(string webhookUrl, string sbPostData)
        {
            var request = WebRequest.Create(webhookUrl);
            request.Method = "POST";
            var byteArray = Encoding.UTF8.GetBytes(sbPostData);
            request.ContentType = @"application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            using (var reqStream = await request.GetRequestStreamAsync())
            {
                reqStream.Write(byteArray, 0, byteArray.Length);
                using (var response = await request.GetResponseAsync())
                using (var respStream = response.GetResponseStream())
                {
                    if (respStream != null)
                    {
                        using (var reader = new StreamReader(respStream))
                        {
                            var responseFromServer = await reader.ReadToEndAsync();
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