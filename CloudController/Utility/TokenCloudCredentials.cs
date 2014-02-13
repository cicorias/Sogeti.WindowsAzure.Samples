using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Common;

namespace CloudController.Utility
{
    public partial class TokenCloudCredentials : SubscriptionCloudCredentials
    {
        private const string DefaultAuthorizationScheme = "Bearer";
        private string _subscriptionId;

        public virtual string AuthorizationScheme { get; set; }

        public virtual string Token { get; set; }

        public TokenCloudCredentials()
        {
            AuthorizationScheme = DefaultAuthorizationScheme;
        }

        public TokenCloudCredentials(string token, string subscriptionId) : this()
        {
            Token = token;
            _subscriptionId = subscriptionId;
        }

        public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(AuthorizationScheme, Token);
            }

            return base.ProcessHttpRequestAsync(request, cancellationToken);
        }

        public override string SubscriptionId
        {
            get { return _subscriptionId; }
        }
    }
}
