using Microsoft.AspNetCore.Mvc;
using SimpleEmailApp.Models;
using SimpleEmailApp.Services;

namespace SimpleEmailApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;
        public EmailController(IEmailService emailService)
        {
            this.emailService= emailService;
        }

        [HttpPost]
        public IActionResult PostAsync(string body)
        {
            EmailResponse emailResponse=new ();
            var result=emailService.SendEmail(body);
 
            if (result.Equals("Success"))
            {
                emailResponse.Status = true;
                emailResponse.Message = "Success";
                emailResponse.MessageDetail = "Message Send Succesfully";
                return Ok(emailResponse);
            }
            return BadRequest();
        }
    }
}
