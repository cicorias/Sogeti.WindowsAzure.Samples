using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CloudController
{
    public partial class SignIn : PhoneApplicationPage
    {
        private App _app = App.Current as App;
        public SignIn()
        {
            InitializeComponent();
            Browser.IsScriptEnabled = true;
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string authURL = string.Format(
            "https://login.windows.net/{0}/oauth2/authorize?response_type=code&resource={1}&client_id={2}&redirect_uri={3}&prompt=login&site_id=501358&display=popup",
            _app.DomainName,
            _app.Resource,
            _app.ClientID,
            _app.RedirectUri);
            //navigate to it
            Browser.Navigate(new Uri(authURL));
        }

        private void MyBrowser_OnNavigating(object sender, NavigatingEventArgs e)
        {
            string returnURL = e.Uri.ToString();
            CurrentURL.Text = returnURL;
            if (returnURL.StartsWith(_app.RedirectUri))
            {
                _app.Code = e.Uri.Query.Remove(0, 6);
                e.Cancel = true;
                Browser.Visibility = System.Windows.Visibility.Collapsed;
                GetToken();
            }   
        }

        private void GetToken()
        {
            HttpWebRequest hwr =
                WebRequest.Create(
                    string.Format("https://login.windows.net/{0}/oauth2/token",
                                   _app.DomainName)) as HttpWebRequest;
            hwr.Method = "POST";
            hwr.ContentType = "application/x-www-form-urlencoded";
            hwr.BeginGetRequestStream(new AsyncCallback(SendTokenEndpointRequest), hwr);
        }

        private void SendTokenEndpointRequest(IAsyncResult rez)
        {
            HttpWebRequest hwr = rez.AsyncState as HttpWebRequest;
            byte[] bodyBits = Encoding.UTF8.GetBytes(
                string.Format(
                    "grant_type=authorization_code&code={0}&client_id={1}&redirect_uri={2}&resource={3}",
                    _app.Code,
                    _app.ClientID,
                    HttpUtility.UrlEncode(_app.RedirectUri),
                    HttpUtility.UrlEncode(_app.Resource)));
            Stream st = hwr.EndGetRequestStream(rez);
            st.Write(bodyBits, 0, bodyBits.Length);
            st.Close();
            hwr.BeginGetResponse(new AsyncCallback(RetrieveTokenEndpointResponse), hwr);
        }

        private void RetrieveTokenEndpointResponse(IAsyncResult rez)
        {
            HttpWebRequest hwr = rez.AsyncState as HttpWebRequest;
            HttpWebResponse resp = hwr.EndGetResponse(rez) as HttpWebResponse;

            StreamReader sr = new StreamReader(resp.GetResponseStream());
            string responseString = sr.ReadToEnd();
            JObject jo = JsonConvert.DeserializeObject(responseString) as JObject;
            _app.AccessToken = (string)jo["access_token"];

            Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/Subscriptions.xaml", UriKind.Relative));
            });
        }

        private void MyBrowser_OnNavigated(object sender, NavigationEventArgs e)
        {
            
        }

        private void MyBrowser_OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            
        }
    }
}