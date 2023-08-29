using AumEnterPriseAPI.Interface;
using AumEnterPriseAPI.MiddleWare;
using AumEnterPriseAPI.ViewModel;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace AumEnterPriseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpoolTransactionController : ControllerBase
    {
        public IConfiguration _configuration;
        public ISpoolTransactionManager _iSpoolTransactionManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserViewModel user;
        public SpoolTransactionController(IConfiguration config, ISpoolTransactionManager iSpoolTransactionManager, IHttpContextAccessor httpContext)
        {
            _configuration = config;
            _iSpoolTransactionManager = iSpoolTransactionManager;
            _httpContextAccessor = httpContext;
            user = _httpContextAccessor.HttpContext.Items["User"] as UserViewModel;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromForm] SpoolTransactionViewModel spoolTransactionViewModel)
        {
            try
            {
                bool isAdded = _iSpoolTransactionManager.AddSpoolTransaction(spoolTransactionViewModel, Convert.ToInt32(user.UserID));
                return isAdded ? Ok("Successfully Added") : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while adding spool transactions. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<SpoolTransactionViewModel> spoolTransactionViewModels = _iSpoolTransactionManager.GetAllSpoolTransactions();
                return spoolTransactionViewModels.Count > 0 ? Ok(spoolTransactionViewModels) : NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while adding spool transactions. Exception:\r\n{ex.Message}");
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("uploadSpoolXML")]
        public async Task<bool> UploadFile(IFormFile file, [FromQuery] string jobNo)
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create);
                    await file.CopyToAsync(fileStream);
                    fileStream.Dispose();

                    XDocument xDoc = XDocument.Load(Path.Combine(path, file.FileName));
                    string jobNoFile = xDoc.Descendants("Job_No").FirstOrDefault().Value;
                    if(jobNo == jobNoFile)
                    {
                        return true;
                    }
                    else
                    {
                        System.IO.File.Delete(Path.Combine(path, file.FileName));
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }
    }
}
