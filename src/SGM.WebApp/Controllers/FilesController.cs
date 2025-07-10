using Microsoft.AspNetCore.Mvc;
using SGM.Application.Services;

namespace SGM.WebApp.Controllers;

[ApiController]
public class FilesController : ControllerBase
{
    [HttpGet("cv")]
    public IActionResult GetCv()
    {
        return File("/resume.pdf", "application/pdf");
    }

    [HttpGet("resume")]
    public IActionResult GetResume()
    {
        return File("/resume.pdf", "application/pdf");
    }

    [HttpGet("sitemap")]
    public IActionResult GetSitemap()
    {
        return File("/sitemap.xml", "application/xml");
    }
}
