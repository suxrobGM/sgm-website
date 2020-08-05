using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuxrobGM.Sdk.Utils;
using SuxrobGM_Website.Web.Utils;

namespace SuxrobGM_Website.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok();
        }

        [HttpPost("[action]")]
        [Consumes("multipart/form-data")]
        public ActionResult SaveImage([FromForm] IList<IFormFile> uploadFiles)
        {
            foreach (var file in uploadFiles)
            {
                if (file == null) 
                    continue;

                var fileName = $"{GeneratorId.GenerateLong()}_content.jpg";
                var fileNameAbsPath = Path.Combine(_env.WebRootPath, "db_files", "img", fileName);
                ImageHelper.SaveImage(file.OpenReadStream(), fileNameAbsPath);
            }

            return Ok();
        }
    }
}