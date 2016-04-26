using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using WX.Api;
using WX.Model;
using WX.Model.ApiRequests;

namespace WX
{
    public class MyAccessToken
    {
        private IApiClient m_client = new DefaultApiClient();
        private AppIdentication m_appIdent = new AppIdentication(
             ConfigurationManager.AppSettings["wxappid"],
            ConfigurationManager.AppSettings["wxappsecret"]);
        public void GetAccessToken()
        {
            var request = new SnsOAuthAccessTokenRequest
            {
                AppID = m_appIdent.AppID,
                AppSecret = m_appIdent.AppSecret,
                Code = "json"
            };
            var response = m_client.Execute(request);

        }
    }
}
