using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web;
using Skybound.Gecko;
using System.Windows.Forms.Integration;
using System.Net;
using System.IO;

namespace Lab_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GeckoWebBrowser webBrowser1;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string appId = "51785724"; //vk
            var uriStr = @"https://oauth.vk.com/authorize?client_id=" + appId +
            @"&scope=offline&redirect_uri=https://oauth.vk.com/blank.html&display=page&v=6.3&response_type=token";

            string path = "C:\\Users\\zharn\\Desktop\\subjects\\3 year\\IS Architecture\\net_app_cs\\lab_5\\Lab_5\\Lab_5\\xulrunner";
            Skybound.Gecko.Xpcom.Initialize(path);

            webBrowser1 = new Skybound.Gecko.GeckoWebBrowser();
            webBrowser1.Navigated += WebBrowserNavigated;

            WindowsFormsHost host = new WindowsFormsHost();
            host.Child = webBrowser1;


            MainGrid.Children.Add(host);
            host.HorizontalAlignment = HorizontalAlignment.Stretch;
            host.VerticalAlignment = VerticalAlignment.Stretch;

            webBrowser1.Navigate(uriStr);
        }

        private void WebBrowserNavigated(object sender, Skybound.Gecko.GeckoNavigatedEventArgs e)
        {
            if (e.Uri.AbsoluteUri.Contains(@"oauth.vk.com/blank.html"))
            {
                string url = e.Uri.Fragment;
                url = url.Trim('#');
                string Access_token = HttpUtility.ParseQueryString(url).Get("access_token");
                string UserID = HttpUtility.ParseQueryString(url).Get("user_id");

                if (Access_token != null)
                {
                    ActionWindow actionWindow = new ActionWindow(Access_token, UserID);
                    actionWindow.Show();
                }

                //MessageBox.Show(GET(url, method, Access_token));
            }
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
    }
}
