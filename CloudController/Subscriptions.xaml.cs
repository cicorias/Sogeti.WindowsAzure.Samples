using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Common;
using Microsoft.WindowsAzure.Management;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Subscriptions;
using Microsoft.WindowsAzure.Subscriptions.Models;

namespace CloudController
{
    public partial class Subscriptions : PhoneApplicationPage
    {
        private App _app;
        public Subscriptions()
        {
            InitializeComponent();

            _app = Application.Current as App;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                var cred = TokenCloudCredentials.Create(new Dictionary<string, object> {{"Token", _app.AccessToken}});
                using(var client = CloudContext.Clients.CreateManagementClient(cred))
                {
                    SubscriptionGetResponse subscriptionGetResponse = await client.Subscriptions.GetAsync();

                }
            }
            catch (Exception exception)
            {
                WriteException(exception);
            }
//            await managementClient.Subscriptions.GetAsync();
//            foreach (var subscription in )
//            {
//                MessageBox.Show(subscription.SubscriptionName);
//            }
        }

        private void WriteException(Exception exception)
        {
            Console.Write(exception);
            if (exception.InnerException != null)
            {
                WriteException(exception.InnerException);
            }
        }

        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}