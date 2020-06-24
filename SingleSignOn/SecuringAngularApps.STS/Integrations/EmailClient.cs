using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SecuringAngularApps.STS.Integrations
{
    public class EmailClient
    {
        public const string SendGridMailEndpoint = "https://api.sendgrid.com";
        private readonly IHttpClientFactory _factory;

        public EmailClient(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        public async Task SendEmail<T>(T model, string recipient) where T : SendGridEmailModel
        {
            using(var client = _factory.CreateClient("SendGridClient"))
            {
                var obj = new
                {
                    from = new { email = Environment.GetEnvironmentVariable("FromEmailAddress") },
                    personalizations = new List<object>
                    {
                        new 
                        { 
                            to = new List<object> { new { email = recipient } },
                            dynamic_template_data = model 
                        } 
                    },
                    template_id = model.templateId
                };

                var jsonContent = JsonConvert.SerializeObject(obj);
                var result = await client.PostAsync("v3/mail/send", new StringContent(jsonContent, Encoding.UTF8, "application/json")); 
            }
        }
    }

    public static class SendGridTemplates
    {
        public static string PasswordRecoveryEmailTemplate = "d-9bcd4db3b3274c66a111c14dff368736";
    }

    public interface SendGridEmailModel
    {
        string templateId { get; }

        string clientHomeUrl { get; set; }
    }

    public class RecoverEmailModel : SendGridEmailModel
    {
        public string templateId => SendGridTemplates.PasswordRecoveryEmailTemplate;

        public string username { get; set; }

        public string clientHomeUrl { get; set; }

        public string linkUrl { get; set; }
    }
}
