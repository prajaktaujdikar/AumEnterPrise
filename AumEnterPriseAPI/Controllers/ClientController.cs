using AumEnterPriseAPI.Interface;
using AumEnterPriseAPI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AumEnterPriseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        public IConfiguration _configuration;
        public IClientManager _iClientManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserViewModel user;
        public ClientController(IConfiguration config, IClientManager clientManager, IHttpContextAccessor httpContext)
        {
            _configuration = config;
            _iClientManager = clientManager;
            _httpContextAccessor = httpContext;
            user = _httpContextAccessor.HttpContext.Items["User"] as UserViewModel;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetClients()
        {
            try
            {
                List<ClientViewModel> clientViewModels = _iClientManager.GetClients();
                return Ok(clientViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while getting clients. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        [Route("{clientId:long}")]
        public IActionResult GetClientById(long clientId)
        {
            try
            {
                ClientViewModel clientViewModels = _iClientManager.GetClientById(clientId);
                return Ok(clientViewModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while getting a client by id. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromForm] ClientViewModel clientViewModel)
        {
            try
            {
                bool isAdded = _iClientManager.AddClient(clientViewModel, Convert.ToInt32(user.UserID));
                return isAdded ? Ok("Successfully Added") : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while adding client. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{clientId:long}")]
        public IActionResult Delete(long clientId)
        {
            try
            {
                bool isDeleted = _iClientManager.DeleteClientById(clientId);
                return isDeleted ? Ok("Successfully Deleted") : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while deleting client. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update/{clientId:long}")]
        public IActionResult Update(long clientId,[FromForm] ClientViewModel clientViewModel)
        {
            try
            {
                bool isUpdated = _iClientManager.UpdateClientById(clientViewModel, Convert.ToInt32(user.UserID));
                return isUpdated ? Ok("Successfully Deleted") : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while updating client. Exception:\r\n{ex.Message}");
                throw;
            }
        }
    }
}
