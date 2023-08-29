using AumEnterPriseAPI.Interface;
using AumEnterPriseAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AumEnterPriseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubClientController : ControllerBase
    {
        public IConfiguration _configuration;
        public ISubClientManager _iSubClientManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserViewModel user;
        public SubClientController(IConfiguration config, ISubClientManager subclientManager, IHttpContextAccessor httpContext)
        {
            _configuration = config;
            _iSubClientManager = subclientManager;
            _httpContextAccessor = httpContext;
            user = _httpContextAccessor.HttpContext.Items["User"] as UserViewModel;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetSubClients()
        {
            try
            {
                List<SubClientViewModel> clientViewModels = _iSubClientManager.GetSubClients();
                return Ok(clientViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while getting sub clients. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("GetSubClientByClientId/{clientId:long}")]
        public IActionResult GetSubClientByClientId(long clientId)
        {
            try
            {
                List<SubClientViewModel> clientViewModels = _iSubClientManager.GetSubClientByClientId(clientId);
                return Ok(clientViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while getting sub clients. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("{subClientId:long}")]
        public IActionResult GetSubClientById(long subClientId)
        {
            try
            {
                SubClientViewModel clientViewModels = _iSubClientManager.GetSubClientById(subClientId);
                return Ok(clientViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while getting a sub client by id. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromForm] SubClientViewModel clientViewModel)
        {
            try
            {
                bool isAdded = _iSubClientManager.AddSubClient(clientViewModel, Convert.ToInt32(user.UserID));
                return isAdded ? Ok("Successfully Added") : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while adding sub client. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{subClientId:long}")]
        public IActionResult Delete(long subClientId)
        {
            try
            {
                bool isDeleted = _iSubClientManager.DeleteSubClientById(subClientId);
                return isDeleted ? Ok("Successfully Deleted") : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while deleting sub client. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update/{subClientId:long}")]
        public IActionResult Update(long subClientId, [FromForm] SubClientViewModel clientViewModel)
        {
            try
            {
                bool isUpdated = _iSubClientManager.UpdateSubClientById(clientViewModel, Convert.ToInt32(user.UserID));
                return isUpdated ? Ok("Successfully Deleted") : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while updating sub client. Exception:\r\n{ex.Message}");
                throw;
            }
        }
    }
}
