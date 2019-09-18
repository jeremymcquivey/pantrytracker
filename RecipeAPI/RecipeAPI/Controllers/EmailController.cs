using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RecipeAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class EmailController : BaseController
    {
        private readonly AppSettings _options;
        private readonly SendGridClient _sgClient;
             
        public EmailController(IOptions<AppSettings> options)
        {
            _options = options.Value;
            _sgClient = new SendGridClient(Environment.GetEnvironmentVariable("SendGridAPIKey", EnvironmentVariableTarget.Process));
        }

        /// <summary>
        /// Sends an email to specified address.
        /// </summary>
        [HttpGet]
        [Route("{address}")]
        public async Task<IActionResult> SendTestEmail([FromRoute]string address)
        {
            if(string.IsNullOrEmpty(address))
            {
                return BadRequest("You must specify a destination address");
            }

            try
            {
                var templateId = _options.EmailConfig.SampleTemplateId;
                var msg = new SendGridMessage
                {
                    TemplateId = templateId,
                    From = new EmailAddress
                    {
                        Email = Environment.GetEnvironmentVariable("EmailFromAddress", EnvironmentVariableTarget.Process),
                        Name = _options.EmailConfig.EmailFromName
                    }
                };

                //TODO: Log email sending. Who is using the sample template?

                msg.SetTemplateData(new
                {
                    name = "Sample User",
                    url = "https://www.amazon.com/Epson-Cinema-Wireless-Miracast-projector/dp/B074FLKWSY/ref=sr_1_5?keywords=epson+projector+4k&qid=1568827078&s=gateway&sr=8-5"
                });
                msg.AddTo(address);

                await _sgClient.SendEmailAsync(msg);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
