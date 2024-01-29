using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lab_5
{
    /// <summary>
    /// Interaction logic for ActionWindow.xaml
    /// </summary>
    public partial class ActionWindow : Window
    {
        private string _accessToken;
        private string _userID;

        public ActionWindow(string accessToken, string userID)
        {
            _accessToken = accessToken;
            _userID = userID;

            InitializeComponent();
        }

        private void Show_Data_Click(object sender, RoutedEventArgs e)
        {
            string reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=6.3";
            string f = GET(reqStrTemplate, "account.getProfileInfo", _accessToken);
            var user = JsonConvert.DeserializeObject<Response>(f);

            StringBuilder stroka = new StringBuilder();
            stroka.Append(user._user.LastName + " "
            + user._user.FirstName + " "
            + user._user.BDate + " "
            + user._user.ScreenName);
            actionTextBox.Text = stroka.ToString();
        }

        private void Show_Audio_Click(object sender, RoutedEventArgs e)
        {
            string reqStrTemplate = "https://api.vk.com/method/{0}?user_id=" + _userID + "&access_token={1}&v=6.3";
            string f = GET(reqStrTemplate, "groups.get", _accessToken);

            var res = JsonConvert.DeserializeObject(f) as JObject;
            string input = res["response"]["items"].ToString();
            input = input.Replace("[", "").Replace("]", "").Replace("\n", "").Trim();

            string parameters = $"?group_ids={input}&field=name";
            string f1 = GET("https://api.vk.com/method/{0}&access_token={1}&v=5.131", "groups.getById" + parameters.ToString(), _accessToken);

            var user = JsonConvert.DeserializeObject(f1) as JObject;

            actionTextBox.Clear();
            var a = user["response"].ToArray();
            foreach (var item in a)
            {
                actionTextBox.Text += item["name"].ToString() + '\n';
            }
        }

        private void Show_Friends_Click(object sender, RoutedEventArgs e)
        {
            string reqStrTemplate = "https://api.vk.com/method/{0}?access_token={1}&v=6.3";
            string f = GET(reqStrTemplate, "friends.get", _accessToken);
            var res = JsonConvert.DeserializeObject(f) as JObject;

            string input = res.ToString();
            input = input.Replace("[", "").Replace("]", "").Replace("\n", "").Trim();
            string parameters = $"?user_ids={input}&fields=first_name,last_name";
            string f1 = GET("https://api.vk.com/method/{0}&access_token={1}&v=6.3", "users.get" + parameters.ToString(), _accessToken);
            var user = JsonConvert.DeserializeObject(f1) as JObject;

            actionTextBox.Clear();
            var a = user["response"].ToArray();
            foreach (var item in a)
            {
                actionTextBox.Text += item["first_name"].ToString() + "  " + item["last_name"].ToString() + '\n';
            }
        }

        public async Task<string> ConvertTaskToString(Task<string> task)
        {
            string result = await task;
            return result;
        }

        private string GET(string Url, string Method, string Token)
        {
            WebRequest req = WebRequest.Create(String.Format(Url, Method, Token));
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string Out = sr.ReadToEnd();
            return Out;
        }

        public class Response
        {
            [JsonProperty("response")]
            public User _user { get; set; }
        }

        public class User
        {
            [JsonProperty("last_name")]
            public string LastName { get; set; }

            [JsonProperty("first_name")]
            public string FirstName{ get; set; }

            [JsonProperty("bdate")]
            public string BDate { get; set; }

            [JsonProperty("screen_name")]
            public string ScreenName { get; set; }

            [JsonProperty("items")]
            public string Items { get; set; }
        }
    }
}
