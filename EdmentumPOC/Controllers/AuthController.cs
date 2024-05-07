using EdmentumBLL.Utils;
using EdmentumPOC.Models;
using Microsoft.AspNetCore.Mvc;

namespace EdmentumPOC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult GenerateToken([FromBody] AuthRequestModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.AccessKey) || string.IsNullOrEmpty(model.SecretKey))
            {
                return BadRequest("Access key and secret key are required.");
            }

            try
            {
                string authorizationToken = BasicAuthorizationHelper.GenerateAuthorization(model.AccessKey, model.SecretKey);
                return Ok(new { Authorization = authorizationToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to generate authorization token: {ex.Message}");
            }
        }
    }
}